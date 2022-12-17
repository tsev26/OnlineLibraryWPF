using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnlineLibraryWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.ViewModels
{
    [BsonIgnoreExtraElements]
    public class RentalViewModel : ViewModelBase
    {
        public ObjectId Id { get; set; }

        public ObjectId BookId { get; set; }
        public Book Book { get; set; }

        public ObjectId CustomerId { get; set; }
        public Customer Customer { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime BookRented { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? BookReturned { get; set; }

        public DateTime BookReturnedOrExpire => BookReturned ?? BookRented.AddDays(6);
    }
}
