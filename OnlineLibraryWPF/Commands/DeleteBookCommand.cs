using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.Stores;
using OnlineLibraryWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class DeleteBookCommand : AsyncCommandBase
    {
        private readonly MongoDBService _mongoDBService;
        private readonly UserStore _userStore;
        private readonly MessageStore _messageStore;
        private readonly BooksViewModel _booksViewModel;

        public DeleteBookCommand(MongoDBService mongoDBService, UserStore userStore, MessageStore messageStore, BooksViewModel booksViewModel)
        {
            _mongoDBService = mongoDBService;
            _userStore = userStore;
            _messageStore = messageStore;
            _booksViewModel = booksViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            bool isRented = await _mongoDBService.CheckIfRented(_userStore.Book.Id);

            if (!isRented)
            {

                await _mongoDBService.RemoveBookAsync(_userStore.Book.Id);

                _messageStore.Message = "Book " + _userStore.Book.Title + " deleted!";

                _booksViewModel.LoadBooksCommand.Execute(null);
            } 
            else
            {
                _messageStore.Message = "Book cant be deleted because is rented!";
            }
        }
    }
}
