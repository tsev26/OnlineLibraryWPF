using OnlineLibraryWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.ViewModels
{
    public class RentalViewModel : ViewModelBase
    {
        public RentalViewModel(Book book, DateTime rentedFrom, DateTime? returned)
        {
            Book = book;
            RentedFrom = rentedFrom;
            Returned = returned;
        }

        public Book Book { get; set; }
        public DateTime RentedFrom { get; set; }
        public DateTime? Returned { get; set; }
    }
}
