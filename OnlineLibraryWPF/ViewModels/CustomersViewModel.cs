using OnlineLibraryWPF.Commands;
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
using System.Windows.Input;

namespace OnlineLibraryWPF.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        public ICommand NavigateLibrarianMenuCommand { get; }
        public ICommand NavigateEditCommand { get; }
        public ICommand NavigateRegisterCommand { get; }
        public ICommand NavigateRentalsCommand { get; }
        public ICommand ApproveUserCommand { get; }
        public ICommand BanUserCommand { get; }
        public ICommand LoadCustomersCommand { get; }

        public ICommand NavigateRentBookCommand { get; }

        public bool IsLoading { get; set; }
        public UserStore UserStore { get; set; }
        public MessageStore MessageStore { get; set; }

        private string _searchString;
        public string SearchString
        {
            get
            {
                return _searchString;
            }
            set
            {
                _searchString = value;
                LoadCustomersCommand.Execute(_searchString);
                OnPropertyChanged(nameof(SearchString));
            }
        }

        public bool IsCustomerSelected => UserStore.Customer != null;

        public CustomersViewModel(UserStore userStore,
                                  MongoDBService mongoDBService, 
                                  MessageStore messageStore, 
                                  INavigationService navigateLibrarianCommand,
                                  INavigationService navigateRegisterCommand,
                                  INavigationService navigateRentalsCommand,
                                  INavigationService navigateBooksCommand)
        {
            UserStore = userStore;
            MessageStore = messageStore;

            NavigateLibrarianMenuCommand = new NavigateCommand(navigateLibrarianCommand);
            NavigateEditCommand = new NavigateCommand(navigateRegisterCommand);
            NavigateRegisterCommand = new NavigateRegisterCommand(navigateRegisterCommand, this);
            NavigateRentalsCommand = new NavigateCommand(navigateRentalsCommand);
            ApproveUserCommand = new ApproveUserCommand(userStore, mongoDBService, messageStore, this);
            BanUserCommand = new BanUserCommand(userStore, mongoDBService, messageStore, this);
            LoadCustomersCommand = new LoadCustomersCommand(this, mongoDBService);
            NavigateRentBookCommand = new NavigateRentBookCommand(navigateBooksCommand, userStore);

            MessageStore.MessageChanged += MessageStore_MessageChanged;
            UserStore.CustomerChanged += UserStore_CustomerChanged;
        }

        private void UserStore_CustomerChanged()
        {
            OnPropertyChanged(nameof(IsCustomerSelected));
        }

        public override void Dispose()
        {
            MessageStore.MessageChanged -= MessageStore_MessageChanged; 
            UserStore.CustomerChanged -= UserStore_CustomerChanged;
            base.Dispose();
        }

        private void MessageStore_MessageChanged()
        {
            OnPropertyChanged(nameof(MessageStore));
        }

        public static CustomersViewModel LoadViewModel(UserStore userStore,
                                                       MongoDBService mongoDBService, 
                                                       MessageStore messageStore, 
                                                       INavigationService navigateLibrarianCommand,
                                                       INavigationService navigateRegisterCommand,
                                                       INavigationService navigateRentalsCommand,
                                                       INavigationService navigateBooksCommand)
        {
            CustomersViewModel viewModel = new CustomersViewModel(userStore,
                                                                  mongoDBService, 
                                                                  messageStore, 
                                                                  navigateLibrarianCommand, 
                                                                  navigateRegisterCommand, 
                                                                  navigateRentalsCommand,
                                                                  navigateBooksCommand);

            viewModel.Customers = new ObservableCollection<User>();

            viewModel.LoadCustomersCommand.Execute(null);

            return viewModel;
        }

        public ObservableCollection<User> Customers { get; set; }

    }
}
