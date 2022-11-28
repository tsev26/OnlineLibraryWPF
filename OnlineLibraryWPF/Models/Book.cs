﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Models
{
    public class Book
    {
        public Book(string title, string author, int numberOfPages, int yearPublished, byte[] picture, int totalLicences)
        {
            Title = title;
            Author = author;
            NumberOfPages = numberOfPages;
            YearPublished = yearPublished;
            Picture = picture;
            TotalLicences = totalLicences;
            RentedBooks = new List<RentedBooks>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public int NumberOfPages { get; set; }
        public int YearPublished { get; set; }
        public byte[] Picture { get; set; } //GridFS
        public int TotalLicences { get; set; }
        public virtual List<RentedBooks> RentedBooks { get; set; }
        public int RentedLicences => RentedBooks.Where(o => o.BookReturned == null).Count();
        public int AvaibleLicences => TotalLicences - RentedLicences;
    }
}