using OnlineLibraryWPF.Commands;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnlineLibraryWPF.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public ICommand AddOrUpdateCustomerCommand { get; }
        public ICommand CloseModalCommand { get; }


        private string _loginName;
        public string LoginName
        {
            get
            {
                return _loginName;
            }
            set
            {
                _loginName = value;
                OnPropertyChanged(nameof(LoginName));
            }
        }

        public bool LoginNameVisible { get; set; }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }


        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _PID;
        public string PID
        {
            get
            {
                return _PID;
            }
            set
            {
                _PID = value;
                OnPropertyChanged(nameof(PID));
            }
        }

        private string _street;
        public string Street
        {
            get
            {
                return _street;
            }
            set
            {
                _street = value;
                OnPropertyChanged(nameof(Street));
            }
        }

        private string _city;
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        private string _postalCode;
        public string PostalCode
        {
            get
            {
                return _postalCode;
            }
            set
            {
                _postalCode = value;
                OnPropertyChanged(nameof(PostalCode));
            }
        }

        private string _country;

        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
                OnPropertyChanged(nameof(Country));
            }
        }

        public string ButtonName { get; set; }

        public string Title { get; set; }

        public MessageStore MessageStore { get; set; }

        public RegisterViewModel(UsersService usersService, 
                                 UserStore userStore, 
                                 MessageStore messageStore, 
                                 INavigationService closeModalNavigationService,
                                 INavigationService closeModalAndReloadCustomersNavigationService)
        {
            AddOrUpdateCustomerCommand = new AddOrUpdateCustomerCommand(usersService, userStore, messageStore, closeModalAndReloadCustomersNavigationService, this);
            CloseModalCommand = new NavigateCommand(closeModalNavigationService);
            MessageStore = messageStore;
            
            MessageStore.ModalMessageChanged += MessageStore_ModalMessageChanged;
        }

        public static RegisterViewModel LoadViewModel(UsersService usersService, 
                                                      UserStore userStore, 
                                                      MessageStore messageStore, 
                                                      INavigationService closeModalNavigationService,
                                                      INavigationService closeModalAndReloadCustomersNavigationService)
        {
            RegisterViewModel view = new RegisterViewModel(usersService, userStore, messageStore, closeModalNavigationService, closeModalAndReloadCustomersNavigationService);
            
            if (userStore.Customer != null)
            {
                view.FirstName = userStore.Customer.FirstName;
                view.LastName = userStore.Customer.LastName;
                view.PID = userStore.Customer.PID;
                view.Street = userStore.Customer.Address.Street;
                view.City = userStore.Customer.Address.City;
                view.PostalCode = userStore.Customer.Address.PostalCode;
                view.Country = userStore.Customer.Address.Country;

                view.Title = "Edit customer '" + userStore.Customer.LoginName + "'";
                view.ButtonName = "Edit";
                view.LoginNameVisible = false;
            }
            else
            {
                view.Title = "Register new customer";
                view.ButtonName = "Register";
                view.LoginNameVisible = true;

            }

            return view;
        }

        public override void Dispose()
        {
            MessageStore.ModalMessageChanged -= MessageStore_ModalMessageChanged;
            base.Dispose();
        }

        private void MessageStore_ModalMessageChanged()
        {
            OnPropertyChanged(nameof(MessageStore));
        }
    }
}
