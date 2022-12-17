using MongoDB.Bson;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class LoadRentalsCommand : AsyncCommandBase
    {
        private readonly RentalsViewModel _rentalsViewModel;
        private readonly MongoDBService _mongoDBService;

        public LoadRentalsCommand(RentalsViewModel rentalsViewModel, MongoDBService mongoDBService)
        {
            _rentalsViewModel = rentalsViewModel;
            _mongoDBService = mongoDBService;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            ObjectId customerId = _rentalsViewModel.UserStore.Customer!.Id;
            _rentalsViewModel.Type = !_rentalsViewModel.Type;
            bool type = _rentalsViewModel.Type;

            List<RentalViewModel> rentals = await _mongoDBService.GetRentalsCustomerAsync(customerId, type);

            _rentalsViewModel.Rentals.Clear();
            foreach (var rent in rentals)
            {
                _rentalsViewModel.Rentals.Add(rent);
            }
            
        }
    }
}
