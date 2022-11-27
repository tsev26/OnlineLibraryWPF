using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Models
{
    public class RentedBooks
    {
        private static readonly TimeSpan MAXDAYSRENTED = new(6, 0, 0, 0);

        public RentedBooks(Book book, Customer customer, DateTime bookRented)
        {
            Book = book;
            Customer = customer;
            BookRented = bookRented;
        }

        public Book Book { get; set; }
        public Customer Customer { get; set; }
        public DateTime BookRented { get; set; }
        public DateTime? BookReturned { get; set; }

        public DateTime BookRentedTo => BookRented.Add(MAXDAYSRENTED);
    }
}
