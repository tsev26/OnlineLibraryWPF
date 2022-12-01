using OnlineLibraryWPF.Models;
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
    public class AddOrUpdateBookCommand : AsyncCommandBase
    {
        private BooksService _booksService;
        private UserStore _userStore;
        private MessageStore _messageStore;
        private INavigationService _closeModalNavigationService;
        private BookAddEditViewModel _bookAddEditViewModel;

        public AddOrUpdateBookCommand(BooksService booksService, 
                                      UserStore userStore, 
                                      MessageStore messageStore, 
                                      INavigationService closeModalNavigationService,
                                      BookAddEditViewModel bookAddEditViewModel)
        {
            _booksService = booksService;
            _userStore = userStore;
            _messageStore = messageStore;
            _closeModalNavigationService = closeModalNavigationService;
            _bookAddEditViewModel = bookAddEditViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            if ((_bookAddEditViewModel.BookTitle ?? "").Length < 1)
            {
                _messageStore.ModalMessage = "Title should be atleas 1 characters.";
                return;
            }
            if ((_bookAddEditViewModel.Author ?? "").Length < 1)
            {
                _messageStore.ModalMessage = "Author should be atleas 1 characters.";
                return;
            }
            if (!(_bookAddEditViewModel.TotalLicences > 0))
            {
                _messageStore.ModalMessage = "Total Licences should be greater than 0!";
                return;
            }
            if (!(_bookAddEditViewModel.YearPublished > 0))
            {
                _messageStore.ModalMessage = "Year Published should be greater than 0!";
                return;
            }
            if (!(_bookAddEditViewModel.NumberOfPages > 0))
            {
                _messageStore.ModalMessage = "NumberOfPages should be greater than 0!";
                return;
            }
            if (_bookAddEditViewModel.Picture == null)
            {
                _messageStore.ModalMessage = "Select Picture to be uploaded.";
                return;
            }

            Book book = new Book(_bookAddEditViewModel.BookTitle,
                     _bookAddEditViewModel.Author,
                     _bookAddEditViewModel.NumberOfPages,
                     _bookAddEditViewModel.YearPublished,
                     _bookAddEditViewModel.Picture,
                     _bookAddEditViewModel.TotalLicences);

            if (_userStore.Book == null)
            {
                await _booksService.CreateAsync(book);

                _messageStore.Message = "Book added!";
            }
            else
            {
                book.Id = _userStore.Book.Id;

                await _booksService.UpdateAsync(_userStore.Book.Id, book);

                _messageStore.Message = "Book upadated!";
            }


            _closeModalNavigationService.Navigate(_messageStore.Message);
        }
    }
}
