using OnlineLibraryWPF.Models;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Stores;
using OnlineLibraryWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class ReturnBookCommand : AsyncCommandBase
    {
        private readonly RentalsViewModel _rentalsViewModel;
        private readonly MongoDBService _mongoDBService;
        private readonly MessageStore _messageStore;

        public ReturnBookCommand(RentalsViewModel rentalsViewModel, MongoDBService mongoDBService, MessageStore messageStore)
        {
            _rentalsViewModel = rentalsViewModel;
            _mongoDBService = mongoDBService;
            _messageStore = messageStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            _rentalsViewModel.SelectedRental.BookReturned = DateTime.Now;
            RentalViewModel rental = _rentalsViewModel.SelectedRental;

            RentedBook rentedBook = new RentedBook(rental.BookId, rental.CustomerId, rental.BookRented, rental.BookReturned);
            rentedBook.Id = rental.Id;

            await _mongoDBService.UpdateRentedBookAsync(rentedBook.Id, rentedBook);
            _rentalsViewModel.Type = false;
            _rentalsViewModel.LoadRentalsCommand.Execute(null);
            _messageStore.Message = rental.Customer.LoginName + " renturns " + rental.Book.Title;
            //_navigationService.Navigate();
        }
    }
}
