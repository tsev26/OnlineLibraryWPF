using MongoDB.Bson;
using OnlineLibraryWPF.Models;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class RentBookCommand : AsyncCommandBase
    {
        private readonly UsersService _usersService;
        private readonly BooksService _booksService;
        private readonly UserStore _userStore;
        private readonly MessageStore _messageStore;
        private readonly INavigationService _navigationService;

        public RentBookCommand(BooksService booksService, 
                               UsersService usersService, 
                               UserStore userStore, 
                               MessageStore messageStore, 
                               INavigationService navigateBooksNavigationService) 
        {
            _usersService = usersService;
            _booksService = booksService;
            _userStore = userStore;
            _messageStore = messageStore;
            _navigationService = navigateBooksNavigationService;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            Book book = _userStore.Book;
            User user = _userStore.Customer;
            if (user == null)
            {
                _messageStore.Message = "Select customer!";
                return;
            }
            if (book == null)
            {
                _messageStore.Message = "Select book to rent!";
                return;
            }
            if (_userStore.Customer.RentedBooks.Count > 6)
            {
                _messageStore.Message = _userStore.Customer.LoginName + " has already rented max of 6 books!";
                return;
            }
            
            foreach (var rent in _userStore.Customer.RentedBooks)
            {
                if (rent.BookId == book.Id)
                {
                    _messageStore.Message = _userStore.Customer.LoginName + " have already rented this book!";
                    return;
                }
            }

            RentedBook rentedBook = new RentedBook(book.Id, DateTime.Now);

            
            await _usersService.RentBook(user.Id, rentedBook);
            _userStore.Customer.RentedBooks.Add(rentedBook);

            ++book.RentedLicences;
            await _booksService.UpdateAsync(book.Id, book);

            _navigationService.Navigate(user.LoginName + " rents " + book.Title);
        }
    }
}
