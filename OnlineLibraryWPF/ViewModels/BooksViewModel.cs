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

        public BooksViewModel(UserStore userStore,
                              BooksService booksService,
                              RentedBooksService rentedBooksService,
                              MessageStore messageStore,
                              INavigationService navigateLibrarianCommand,
                              INavigationService navigateCustomerMenuCommand,
                              INavigationService navigateRentalsCommand,
                              INavigationService navigateAddUpdateBookCommand)
        {
            UserStore = userStore;
            MessageStore = messageStore;

            NavigateLibrarianMenuCommand = new NavigateCommand(navigateLibrarianCommand);
            NavigateCustomerMenuCommand = new NavigateCommand(navigateCustomerMenuCommand);
            NavigateAddBookCommand = new NavigateAddBookCommand(navigateAddUpdateBookCommand, UserStore);
            NavigateEditBookCommand = new NavigateCommand(navigateAddUpdateBookCommand);
            NavigateRentalsCommand = new NavigateCommand(navigateRentalsCommand);
            RentBookCommand = new RentBookCommand(rentedBooksService);
            LoadBooksCommand = new LoadBooksCommand(this, booksService);

            MessageStore.MessageChanged += MessageStore_MessageChanged;
        }

        public override void Dispose()
        {
            MessageStore.MessageChanged -= MessageStore_MessageChanged;
            base.Dispose();
        }

        private void MessageStore_MessageChanged()
        {
            OnPropertyChanged(nameof(MessageStore));
        }

        public static BooksViewModel LoadViewModel(UserStore userStore,
                                               BooksService booksService,
                                               RentedBooksService rentedBooksService,
                                               MessageStore messageStore,
                                               INavigationService navigateLibrarianCommand,
                                               INavigationService navigateCustomerMenuCommand,
                                               INavigationService navigateRentalsCommand,
                                               INavigationService navigateAddUpdateBookCommand)
        {
            BooksViewModel viewModel = new BooksViewModel(userStore,
                                                          booksService,
                                                          rentedBooksService,
                                                          messageStore,
                                                          navigateLibrarianCommand,
                                                          navigateCustomerMenuCommand,
                                                          navigateRentalsCommand,
                                                          navigateAddUpdateBookCommand);

            viewModel.Books = new ObservableCollection<Book>();

            viewModel.LoadBooksCommand.Execute(null);

            return viewModel;
        }


        public ObservableCollection<Book> Books { get; set; }

    }
}
