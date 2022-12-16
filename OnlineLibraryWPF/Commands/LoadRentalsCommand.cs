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
            List<RentalViewModel> rentals = new List<RentalViewModel>();
            ObjectId customerId = _rentalsViewModel.UserStore.Customer!.Id;
            bool type = _rentalsViewModel.Type;

            _mongoDBService.LoadRentalsForUser(customerId, type);
            //rentals = 
        }
    }
}
