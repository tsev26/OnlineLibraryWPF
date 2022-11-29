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
    public class ApproveUserCommand : AsyncCommandBase
    {
        private readonly UserStore _userStore;
        private readonly UsersService _usersService;
        private readonly MessageStore _messageStore;
        private readonly CustomersViewModel _customersViewModel;

        public ApproveUserCommand(UserStore userStore, UsersService usersService, MessageStore messageStore, CustomersViewModel customersViewModel)
        {
            _userStore = userStore;
            _usersService = usersService;
            _messageStore = messageStore;
            _customersViewModel = customersViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            if (_userStore.Customer == null)
            {
                _messageStore.Message = "First select customer to approve!";
                return;
            }
            await _usersService.ApproveUser(_userStore.Customer.Id, !_userStore.Customer.IsApproved);
            
            _messageStore.Message = "Customer " + _userStore.Customer.LoginName + (!_userStore.Customer.IsApproved ? " approved" : " unapproved") + "!";
            //_userStore.Customer.IsApproved = !_userStore.Customer.IsApproved;
            _customersViewModel.LoadCustomersCommand.Execute(null);

        }
    }
}
