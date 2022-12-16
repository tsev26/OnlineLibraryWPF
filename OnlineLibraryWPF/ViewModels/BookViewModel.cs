using OnlineLibraryWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.ViewModels
{
    public class BookViewModel : ViewModelBase
    {
        private Book _book;
        public Book Book
        {
            get
            {
                return _book;
            }
            set
            {
                _book = value;
                OnPropertyChanged(nameof(Book));
            }
        }

        public BookViewModel(Book book)
        {
            Book = book;
        }
    }
}
