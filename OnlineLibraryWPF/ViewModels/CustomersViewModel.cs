using OnlineLibraryWPF.Models;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OnlineLibraryWPF.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        public CustomersViewModel(UsersService usersService, MessageStore messageStore, INavigationService navigateLibrarianCommand)
        {
            
        }

        public static CustomersViewModel LoadViewModel(UsersService usersService, MessageStore messageStore, INavigationService navigateLibrarianCommand)
        {
            CustomersViewModel viewModel = new CustomersViewModel(usersService, messageStore, navigateLibrarianCommand);

            List<User> cust = usersService.GetAllCustomers();

            viewModel.Customers = new ObservableCollection<User>();

            foreach (User user in cust)
            {
                viewModel.Customers.Add(user);
            }

            return viewModel;
        }

        public ObservableCollection<User> Customers { get; set; }

    }
}
