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
        private readonly UsersService _usersService;
        private readonly MessageStore _messageStore;
        private readonly UserStore _userStore;

        public LoadInfoAbouCustomerCommand(UsersService usersService, UserStore userStore, MessageStore messageStore)
        {
            _usersService = usersService;
            _messageStore = messageStore;
            _userStore = userStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            long numberOfRentalsCustomers = await _usersService.GetNumberOfRentalsCustomerAsync(_userStore.Customer.Id);
            _messageStore.Message = "Currently rented " + numberOfRentalsCustomers + " books!";
        }
    }
}
