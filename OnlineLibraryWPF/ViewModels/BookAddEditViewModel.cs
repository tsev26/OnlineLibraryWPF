using OnlineLibraryWPF.Commands;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.Stores;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnlineLibraryWPF.ViewModels
{
    public class BookAddEditViewModel : ViewModelBase
    {
        public ICommand AddOrUpdateBookCommand { get; }
        public ICommand CloseModalCommand { get; }
        public ICommand SelectImgCommand { get; }

        private string _bookTitle;
        public string BookTitle
        {
            get
            {
                return _bookTitle;
            }
            set
            {
                _bookTitle = value;
                OnPropertyChanged(nameof(BookTitle));
            }
        }

        private string _author;
        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private int _numberOfPages;
        public int NumberOfPages
        {
            get
            {
                return _numberOfPages;
            }
            set
            {
                _numberOfPages = value;
                OnPropertyChanged(nameof(NumberOfPages));
            }
        }

        private int _yearPublished;
        public int YearPublished
        {
            get
            {
                return _yearPublished;
            }
            set
            {
                _yearPublished = value;
                OnPropertyChanged(nameof(YearPublished));
            }
        }

        private byte[] _picture;
        public byte[] Picture
        {
            get
            {
                return _picture;
            }
            set
            {
                _picture = value;
                OnPropertyChanged(nameof(Picture));
            }
        }

        private int _totalLicences;
        public int TotalLicences
        {
            get
            {
                return _totalLicences;
            }
            set
            {
                _totalLicences = value;
                OnPropertyChanged(nameof(TotalLicences));
            }
        }

        public string ButtonName { get; set; }

        public string Title { get; set; }

        public MessageStore MessageStore { get; set; }

        public UserStore UserStore { get; set; }

        public BookAddEditViewModel(BooksService booksService, 
                                    MessageStore messageStore,
                                    UserStore userStore,
                                    INavigationService closeModalNavigationService,
                                    INavigationService closeModalAndReloadCustomersNavigationService)
        {
            MessageStore = messageStore;
            UserStore = userStore;

            AddOrUpdateBookCommand = new AddOrUpdateBookCommand(booksService, userStore, messageStore, closeModalAndReloadCustomersNavigationService, this);
            CloseModalCommand = new NavigateCommand(closeModalNavigationService);
            SelectImgCommand = new SelectImgCommand(this);
            MessageStore = messageStore;

            MessageStore.ModalMessageChanged += MessageStore_ModalMessageChanged;
        }

        public static BookAddEditViewModel LoadViewModel(BooksService booksService,
                                    MessageStore messageStore,
                                    UserStore userStore,
                                    INavigationService closeModalNavigationService,
                                    INavigationService closeModalAndReloadBooksNavigationService)
        {
            BookAddEditViewModel view = new BookAddEditViewModel(booksService, messageStore, userStore, closeModalNavigationService, closeModalAndReloadBooksNavigationService);

            if (userStore.Book != null)
            {
                view.BookTitle = userStore.Book.Title;
                view.Author = userStore.Book.Author;
                view.NumberOfPages = userStore.Book.NumberOfPages;
                view.YearPublished = userStore.Book.YearPublished;
                view.Picture = userStore.Book.Picture;
                view.TotalLicences = userStore.Book.TotalLicences;

                view.Title = "Edit book '" + userStore.Book.Title + "'";
                view.ButtonName = "Edit";
            }
            else
            {
                view.Title = "Add new book";
                view.ButtonName = "Add";
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
