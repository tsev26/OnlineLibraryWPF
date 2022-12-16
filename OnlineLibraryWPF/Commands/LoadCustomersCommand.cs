using OnlineLibraryWPF.Models;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OnlineLibraryWPF.Commands
{
    public class LoadCustomersCommand : AsyncCommandBase
    {
        private readonly CustomersViewModel _viewModel;
        private readonly MongoDBService _usersService;
        public LoadCustomersCommand(CustomersViewModel viewModel, MongoDBService usersService)
        {
            _viewModel = viewModel;
            _usersService = usersService;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            _viewModel.IsLoading = true;

            if (parameter is string searchString)
            {

            }
            else
            {
                searchString = "";
            }

            try
            {
                _viewModel.Customers.Clear();
                List<User> cust = await _usersService.GetAllCustomersAsync(searchString);

                foreach (User user in cust)
                {
                    _viewModel.Customers.Add(user);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load customers.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _viewModel.IsLoading = false;
        }
    }
}
