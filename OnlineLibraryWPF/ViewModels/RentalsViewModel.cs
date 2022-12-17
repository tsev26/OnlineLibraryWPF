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
using System.Windows.Input;

namespace OnlineLibraryWPF.ViewModels
{
    public class RentalsViewModel : ViewModelBase
    {
        public ICommand NavigateLibrarianMenuCommand { get; }
        public ICommand NavigateCustomerMenuCommand { get; }
        public ICommand LoadRentalsCommand { get; }
        public ICommand ReturnBookCommand { get; }

        public bool IsLoading { get; set; }
        public UserStore UserStore { get; set; }
        public MessageStore MessageStore { get; set; }

        public bool IsBookSelected => SelectedRental != null && Type;

        public string RentalsForCustomer { get; set; }


        private RentalViewModel _selectedRental;
        public RentalViewModel SelectedRental
        {
            get
            {
                return _selectedRental;
            }
            set
            {
                _selectedRental = value;
                OnPropertyChanged(nameof(SelectedRental));
                OnPropertyChanged(nameof(IsBookSelected));
            }
        }



        public string LoadRentalsNameButton => Type ? "Show returned rentals" : "Show current rentals";


        private bool _type;
        public bool Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
                OnPropertyChanged(nameof(LoadRentalsNameButton));
                OnPropertyChanged(nameof(IsBookSelected));
            }
        }

        public string ColumnName => Type ? "Returned" : "Expire";

        public RentalsViewModel(UserStore userStore,
                              MongoDBService mongoDBService,
                              MessageStore messageStore,
                              INavigationService navigateLibrarianCommand,
                              INavigationService navigateCustomerMenuCommand)
        {
            UserStore = userStore;
            MessageStore = messageStore;

            NavigateLibrarianMenuCommand = new NavigateCommand(navigateLibrarianCommand);
            NavigateCustomerMenuCommand = new NavigateCommand(navigateCustomerMenuCommand);

            LoadRentalsCommand = new LoadRentalsCommand(this, mongoDBService);
            ReturnBookCommand = new ReturnBookCommand(this, mongoDBService, messageStore);

            MessageStore.MessageChanged += MessageStore_MessageChanged;
            UserStore.BookChanged += UserStore_BookChanged;
        }

        private void UserStore_BookChanged()
        {
            OnPropertyChanged(nameof(IsBookSelected));
        }

        public override void Dispose()
        {
            MessageStore.MessageChanged -= MessageStore_MessageChanged;
            UserStore.BookChanged -= UserStore_BookChanged;
            base.Dispose();
        }

        private void MessageStore_MessageChanged()
        {
            OnPropertyChanged(nameof(MessageStore));
        }

        public static RentalsViewModel LoadViewModel(UserStore userStore,
                                                     MongoDBService mongoDBService,
                                                     MessageStore messageStore,
                                                     INavigationService navigateLibrarianCommand,
                                                     INavigationService navigateCustomerMenuCommand)
        {
            RentalsViewModel viewModel = new RentalsViewModel(userStore,
                                                     mongoDBService,
                                                     messageStore,
                                                     navigateLibrarianCommand,
                                                     navigateCustomerMenuCommand);

            viewModel.Rentals = new ObservableCollection<RentalViewModel>();
            viewModel.RentalsForCustomer = "Rentals of " + userStore.Customer.LoginName;
            //viewModel.Type = false;
            viewModel.LoadRentalsCommand.Execute(null);

            return viewModel;
        }

        public ObservableCollection<RentalViewModel> Rentals { get; set; }
    }
}
