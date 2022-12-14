using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Models
{
    [BsonIgnoreExtraElements]
    public class Book
    {
        public Book() 
        {
            //RentedBooks = new List<RentedBook>();
        }
        public Book(string title, string author, int numberOfPages, int yearPublished, byte[] picture, int totalLicences) : base()
        {
            Title = title;
            Author = author;
            NumberOfPages = numberOfPages;
            YearPublished = yearPublished;
            Picture = picture;
            TotalLicences = totalLicences;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int NumberOfPages { get; set; }
        public int YearPublished { get; set; }
        public byte[] Picture { get; set; } //GridFS
        public int TotalLicences { get; set; }
    }
}
