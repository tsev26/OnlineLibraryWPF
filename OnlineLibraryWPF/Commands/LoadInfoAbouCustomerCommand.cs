using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class LoadInfoAbouCustomerCommand : AsyncCommandBase
    {
        private readonly MongoDBService _mongoDBService;
        private readonly MessageStore _messageStore;
        private readonly UserStore _userStore;

        public LoadInfoAbouCustomerCommand(MongoDBService mongoDBService, UserStore userStore, MessageStore messageStore)
        {
            _mongoDBService = mongoDBService;
            _messageStore = messageStore;
            _userStore = userStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            long numberOfRentalsCustomers = await _mongoDBService.GetNumberOfRentalsCustomerAsync(_userStore.Customer.Id);
            _messageStore.Message = "Currently rented " + numberOfRentalsCustomers + " books!";
        }
    }
}
