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
    public class BookViewModel : ViewModelBase
    {

        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int NumberOfPages { get; set; }
        public int YearPublished { get; set; }
        public byte[] Picture { get; set; } //GridFS
        public int TotalLicences { get; set; }
        
        public List<RentedBook> RentedBooks { get; set; }

        public int RentedLicences => RentedBooks.Where(x => x.BookReturned == null).Count();
        
        public int AvaibleLicences => TotalLicences - RentedLicences;
    }
}
