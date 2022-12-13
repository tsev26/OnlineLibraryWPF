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
    public class RentalsViewModel : ViewModelBase
    {
        public ICommand NavigateLibrarianMenuCommand { get; }
        public ICommand NavigateCustomerMenuCommand { get; }
        public ICommand RentBookCommand { get; }
        public ICommand LoadRentalsCommand { get; }

        public bool IsLoading { get; set; }
        public UserStore UserStore { get; set; }
        public MessageStore MessageStore { get; set; }

        public bool IsBookSelected => UserStore.Book != null;

        public RentalsViewModel(UserStore userStore,
                              UsersService usersService,
                              MessageStore messageStore,
                              INavigationService navigateLibrarianCommand,
                              INavigationService navigateCustomerMenuCommand)
        {
            UserStore = userStore;
            MessageStore = messageStore;

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
                                                     UsersService usersService,
                                                     MessageStore messageStore,
                                                     INavigationService navigateLibrarianCommand,
                                                     INavigationService navigateCustomerMenuCommand)
        {
            RentalsViewModel viewModel = new RentalsViewModel(userStore,
                                                          usersService,
                                                          messageStore,
                                                          navigateLibrarianCommand,
                                                          navigateCustomerMenuCommand);

            viewModel.Books = new ObservableCollection<Book>();

            viewModel.LoadBooksCommand.Execute(null);

            return viewModel;
        }


        public ObservableCollection<Book> Books { get; set; }
    }
}
