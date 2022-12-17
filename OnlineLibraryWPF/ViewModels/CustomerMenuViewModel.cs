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
    public class CustomerMenuViewModel : ViewModelBase
    {

        public ICommand LogoutCommand { get; }
        public ICommand NavigateBooksCommand { get; }
        public ICommand NavigateRentalsCommand { get; }
        public ICommand NavigateEditCommand { get; }
        public ICommand LoadInfoAbouCustomerCommand { get; }

        public MessageStore MessageStore { get; set; }
        public UserStore UserStore { get; set; }

        public CustomerMenuViewModel(UserStore userStore,
                                     MessageStore messageStore,
                                     MongoDBService mongoDBService,
                                     INavigationService navigateHomeCommand,
                                     INavigationService navigateBooksCommand,
                                     INavigationService navigateRentalsCommand,
                                     INavigationService navigateRegisterCommand)
        {
            UserStore = userStore;
            MessageStore = messageStore;
            LogoutCommand = new LogoutCommand(navigateHomeCommand, userStore);
            NavigateBooksCommand = new NavigateCommand(navigateBooksCommand);
            NavigateRentalsCommand = new NavigateCommand(navigateRentalsCommand);
            LoadInfoAbouCustomerCommand = new LoadInfoAbouCustomerCommand(mongoDBService, userStore, messageStore);
            NavigateEditCommand = new NavigateCommand(navigateRegisterCommand);

            MessageStore.MessageChanged += MessageStore_MessageChanged;
        }

        private void MessageStore_MessageChanged()
        {
            OnPropertyChanged(nameof(MessageStore));
        }

        public override void Dispose()
        {
            MessageStore.MessageChanged -= MessageStore_MessageChanged;
            base.Dispose();
        }

        public static CustomerMenuViewModel LoadViewModel(UserStore userStore,
                                     MessageStore messageStore,
                                     MongoDBService mongoDBService,
                                     INavigationService navigateHomeCommand,
                                     INavigationService navigateBooksCommand,
                                     INavigationService navigateRentalsCommand,
                                     INavigationService navigateRegisterCommand)
        {
            CustomerMenuViewModel viewModel = new CustomerMenuViewModel(userStore,
                                                                        messageStore,
                                                                        mongoDBService,
                                                                        navigateHomeCommand,
                                                                        navigateBooksCommand,
                                                                        navigateRentalsCommand,
                                                                        navigateRegisterCommand);

            viewModel.LoadInfoAbouCustomerCommand.Execute(null);

            return viewModel;
        }
    }
}
