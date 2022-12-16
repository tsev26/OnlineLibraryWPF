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

        public RentedBook(ObjectId bookId, ObjectId customerId, DateTime bookRented, DateTime? bookReturned = null)
        {
            BookId = bookId;
            CustomerId = customerId;
            BookRented = bookRented;
            BookReturned = bookReturned;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public ObjectId CustomerId { get; set; }

        public ObjectId BookId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime BookRented { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? BookReturned { get; set; }

        public DateTime BookRentedTo => BookRented.Add(MAXDAYSRENTED);
    }
}
