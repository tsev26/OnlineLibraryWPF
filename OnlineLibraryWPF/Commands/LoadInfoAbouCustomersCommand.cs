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
    public class LoadInfoAbouCustomersCommand : AsyncCommandBase
    {
        private readonly MongoDBService _mongoDBService;
        private readonly MessageStore _messageStore;

        public LoadInfoAbouCustomersCommand(MongoDBService mongoDBService, MessageStore messageStore)
        {
            _mongoDBService = mongoDBService;
            _messageStore = messageStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            long numberOfUnapprovedCustomers = await _mongoDBService.GetNumberOfUnapprovedCustomersAsync();
            _messageStore.Message = "There are " + numberOfUnapprovedCustomers + " customers waiting to be approved!";
        }
    }
}
