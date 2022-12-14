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
    public class BooksViewModel : ViewModelBase
    {
        public ICommand NavigateLibrarianMenuCommand { get; }
        public ICommand NavigateCustomerMenuCommand { get; }
        
        public ICommand NavigateAddBookCommand { get; }
        public ICommand NavigateEditBookCommand { get; }
        public ICommand NavigateRentalsCommand { get; }
        public ICommand RentBookCommand { get; }
        public ICommand LoadBooksCommand { get; }

        public ICommand DeleteBookCommand { get; }

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
                LoadBooksCommand.Execute(_searchString);
                OnPropertyChanged(nameof(SearchString));
            }
        }

        public bool IsBookSelected => UserStore.Book != null;

        public bool IsBookSelectedForLibrarian => IsBookSelected && UserStore.IsLoggedInLibrarian;

        public bool IsBookSelectedForCustomer => IsBookSelected && UserStore.Customer != null;

        public BooksViewModel(UserStore userStore,
                              MongoDBService mongoDBService,
                              MessageStore messageStore,
                              INavigationService navigateLibrarianCommand,
                              INavigationService navigateCustomerMenuCommand,
                              INavigationService navigateRentalsCommand,
                              INavigationService navigateAddUpdateBookCommand,
                              INavigationService navigateBooksNavigationService)
        {
            UserStore = userStore;
            MessageStore = messageStore;

            NavigateLibrarianMenuCommand = new NavigateCommand(navigateLibrarianCommand);
            NavigateCustomerMenuCommand = new NavigateCommand(navigateCustomerMenuCommand);
            NavigateAddBookCommand = new NavigateAddBookCommand(navigateAddUpdateBookCommand, userStore);
            NavigateEditBookCommand = new NavigateCommand(navigateAddUpdateBookCommand);
            NavigateRentalsCommand = new NavigateCommand(navigateRentalsCommand);
            RentBookCommand = new RentBookCommand(mongoDBService, userStore, messageStore, navigateBooksNavigationService);
            LoadBooksCommand = new LoadBooksCommand(this, mongoDBService);
            DeleteBookCommand = new DeleteBookCommand(mongoDBService, userStore, messageStore, this);

            MessageStore.MessageChanged += MessageStore_MessageChanged;
            UserStore.BookChanged += UserStore_BookChanged;
        }

        private void UserStore_BookChanged()
        {
            OnPropertyChanged(nameof(IsBookSelected));
            OnPropertyChanged(nameof(IsBookSelectedForCustomer));
            OnPropertyChanged(nameof(IsBookSelectedForLibrarian));
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

        public static BooksViewModel LoadViewModel(UserStore userStore,
                                               MongoDBService usersService,
                                               MessageStore messageStore,
                                               INavigationService navigateLibrarianCommand,
                                               INavigationService navigateCustomerMenuCommand,
                                               INavigationService navigateRentalsCommand,
                                               INavigationService navigateAddUpdateBookCommand,
                                               INavigationService navigateBooksNavigationService)
        {
            BooksViewModel viewModel = new BooksViewModel(userStore,
                                                          usersService,
                                                          messageStore,
                                                          navigateLibrarianCommand,
                                                          navigateCustomerMenuCommand,
                                                          navigateRentalsCommand,
                                                          navigateAddUpdateBookCommand,
                                                          navigateBooksNavigationService);

            viewModel.Books = new ObservableCollection<BookViewModel>();

            viewModel.LoadBooksCommand.Execute(null);

            return viewModel;
        }


        public ObservableCollection<BookViewModel> Books { get; set; }

    }
}
