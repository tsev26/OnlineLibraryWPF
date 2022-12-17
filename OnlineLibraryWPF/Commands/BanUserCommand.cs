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
    public class BanUserCommand : AsyncCommandBase
    {
        private readonly UserStore _userStore;
        private readonly MongoDBService _mongoDBService;
        private readonly MessageStore _messageStore;
        private readonly CustomersViewModel _customersViewModel;

        public BanUserCommand(UserStore userStore, MongoDBService mongoDBService, MessageStore messageStore, CustomersViewModel customersViewModel)
        {
            _userStore = userStore;
            _mongoDBService = mongoDBService;
            _messageStore = messageStore;
            _customersViewModel = customersViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            if (_userStore.Customer == null)
            {
                _messageStore.Message = "First select customer to ban!";
                return;
            }
            await _mongoDBService.BanUser(_userStore.Customer.Id, !_userStore.Customer.IsBanned);

            _messageStore.Message = "Customer " + _userStore.Customer.LoginName + (!_userStore.Customer.IsBanned ? " banned" : " unbanned") + "!";
            //_userStore.Customer.IsBanned = !_userStore.Customer.IsBanned;
            _customersViewModel.LoadCustomersCommand.Execute(null);
        }
    }
}
