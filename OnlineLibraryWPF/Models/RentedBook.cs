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

        public RentedBook(ObjectId bookId, DateTime bookRented, DateTime? bookReturned = null)
        {
            BookId = bookId;
            BookRented = bookRented;
            BookReturned = bookReturned;
        }
        public ObjectId _id { get; set; }

        public ObjectId BookId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime BookRented { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? BookReturned { get; set; }

        public DateTime BookRentedTo => BookRented.Add(MAXDAYSRENTED);
    }
}
