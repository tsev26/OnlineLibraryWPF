using MongoDB.Bson;
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
    public class RentBookCommand : AsyncCommandBase
    {
        private readonly MongoDBService _mongoDBService;
        private readonly UserStore _userStore;
        private readonly MessageStore _messageStore;
        private readonly INavigationService _navigationService;

        public RentBookCommand(MongoDBService mongoDBService, 
                               UserStore userStore, 
                               MessageStore messageStore, 
                               INavigationService navigateBooksNavigationService) 
        {
            _mongoDBService = mongoDBService;
            _userStore = userStore;
            _messageStore = messageStore;
            _navigationService = navigateBooksNavigationService;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            BookViewModel book = _userStore.Book;
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

            if (user is Customer customer && !customer.IsApproved)
            {
                _messageStore.Message = customer.LoginName + " is not approve yet!";
                return;
            }

            if (user is Customer cus && cus.IsBanned)
            {
                _messageStore.Message = cus.LoginName + " is banned!";
                return;
            }

            if (book.AvaibleLicences <= 0)
            {
                _messageStore.Message = book.Title + " has no available licence!";
                return;
            }

            List<RentalViewModel> rentedBooks = await _mongoDBService.GetRentalsCustomerAsync(_userStore.Customer.Id);
            foreach (var rent in rentedBooks)
            {
                if (rent.Book.Id == book.Id)
                {
                    _messageStore.Message = _userStore.Customer.LoginName + " have already rented this book!";
                    return;
                }
            }

            if (rentedBooks.Count >= 6)
            {
                _messageStore.Message = _userStore.Customer.LoginName + " has already rented max of 6 books!";
                return;
            }

            RentedBook rentedBook = new RentedBook(book.Id, user.Id, DateTime.Now);

            
            await _mongoDBService.CreateRentedBookAsync(rentedBook);

            _navigationService.Navigate(user.LoginName + " rents " + book.Title);
        }
    }
}
