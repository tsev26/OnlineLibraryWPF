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
using static OnlineLibraryWPF.Services.HashFunction;

namespace OnlineLibraryWPF.Commands
{
    public class AddOrUpdateCustomerCommand : AsyncCommandBase
    {
        private readonly UsersService _usersService;
        private readonly MessageStore _messageStore;
        private readonly INavigationService _closeModalNavigationService;
        private readonly RegisterViewModel _registerViewModel;
        private readonly UserStore _userStore;
        public AddOrUpdateCustomerCommand(UsersService usersService, 
                                          UserStore userStore,
                                          MessageStore messageStore, 
                                          INavigationService closeModalNavigationService, 
                                          RegisterViewModel registerViewModel)
        {
            _usersService = usersService;
            _messageStore = messageStore;
            _closeModalNavigationService = closeModalNavigationService;
            _registerViewModel = registerViewModel;
            _userStore = userStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {

            if ((_registerViewModel.FirstName ?? "").Length < 1)
            {
                _messageStore.ModalMessage = "First name should be atleas 1 characters.";
                return;
            }
            if ((_registerViewModel.LastName ?? "").Length < 1)
            {
                _messageStore.ModalMessage = "Last name should be atleas 1 characters.";
                return;
            }
            if ((_registerViewModel.PID ?? "").Length != 10)
            {
                _messageStore.ModalMessage = "PID should be 10 characters.";
                return;
            }
            if ((_registerViewModel.Street ?? "").Length < 3)
            {
                _messageStore.ModalMessage = "Street should be atleas 3 characters.";
                return;
            }
            if ((_registerViewModel.City ?? "").Length < 1)
            {
                _messageStore.ModalMessage = "City should be atleas 1 characters.";
                return;
            }
            if ((_registerViewModel.PostalCode ?? "").Length < 3)
            {
                _messageStore.ModalMessage = "Postal code should be atleas 3 characters.";
                return;
            }
            if ((_registerViewModel.Country ?? "").Length < 2)
            {
                _messageStore.ModalMessage = "Country should be atleas 2 characters.";
                return;
            }

            if (_userStore.Customer == null && !_userStore.IsLoggedInLibrarian)
            {
                if ((_registerViewModel.LoginName ?? "").Length < 3)
                {
                    _messageStore.ModalMessage = "Login name should be atleas 3 characters.";
                    return;
                }
                if ((_registerViewModel.Password ?? "").Length < 3)
                {
                    _messageStore.ModalMessage = "Password should be atleas 3 characters.";
                    return;
                }

                bool userExists = await _usersService.CheckUserWithLoginExists(_registerViewModel.LoginName);
                if (userExists)
                {
                    _messageStore.ModalMessage = "User with this login name already exists!";
                    return;
                }

                Customer customer = new Customer(_registerViewModel.LoginName,
                                                 HashString(_registerViewModel.Password),
                                                 _registerViewModel.FirstName,
                                                 _registerViewModel.LastName,
                                                 _registerViewModel.PID,
                                                 new Address(_registerViewModel.Street, _registerViewModel.City, _registerViewModel.PostalCode, _registerViewModel.Country),
                                                 false);
                await _usersService.CreateAsync(customer);

                _messageStore.Message = "User created!";
            }
            else if (_userStore.IsLoggedInCustomer)
            {
                string newPassword = _registerViewModel.Password?.Length >= 3 ? HashString(_registerViewModel.Password) : _userStore.Customer.Password;

                User customer = new Customer(_userStore.Customer.LoginName,
                                                 newPassword,
                                                 _registerViewModel.FirstName,
                                                 _registerViewModel.LastName,
                                                 _registerViewModel.PID,
                                                 new Address(_registerViewModel.Street, _registerViewModel.City, _registerViewModel.PostalCode, _registerViewModel.Country),
                                                 false);
                customer.Id = _userStore.Customer.Id;

                await _usersService.UpdateAsync(_userStore.Customer.Id, customer);

                _messageStore.Message = "User upadated!";
            }
            else if (_userStore.IsLoggedInLibrarian)
            {
                User customer = new Customer(_registerViewModel.LoginName,
                                                 HashString(_registerViewModel.Password),
                                                 _registerViewModel.FirstName,
                                                 _registerViewModel.LastName,
                                                 _registerViewModel.PID,
                                                 new Address(_registerViewModel.Street, _registerViewModel.City, _registerViewModel.PostalCode, _registerViewModel.Country),
                                                 true);


                await _usersService.CreateAsync(customer);

                _messageStore.Message = "User created!";
            }


            _closeModalNavigationService.Navigate();
        }
    }
}
