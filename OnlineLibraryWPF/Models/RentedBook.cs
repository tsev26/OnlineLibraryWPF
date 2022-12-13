using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Models
{
    public class RentedBook
    {
        private static readonly TimeSpan MAXDAYSRENTED = new(6, 0, 0, 0);

        public RentedBook(ObjectId bookId, DateTime bookRented)
        {
            BookId = bookId;
            BookRented = bookRented;
        }

        public ObjectId BookId { get; set; }
        public DateTime BookRented { get; set; }
        public DateTime? BookReturned { get; set; }

        public DateTime BookRentedTo => BookRented.Add(MAXDAYSRENTED);
    }
}
