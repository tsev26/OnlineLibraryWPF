using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using OnlineLibraryWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OnlineLibraryWPF.MongoDB
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;

        public BooksService(DatabaseSettings databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<Book>(
                databaseSettings.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<Book?> GetAsync(string id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Book newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Book updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Book>> GetAllBooksAsync(string searchString, bool onlyAvailable = false)
        {
            FilterDefinition<Book> filter = Builders<Book>.Filter.Empty;
            if (searchString.Length >= 3)
            {
                BsonRegularExpression reg = new BsonRegularExpression(searchString, "i");
                filter &= Builders<Book>.Filter.Or(
                                    Builders<Book>.Filter.Regex("Title", reg),
                                    Builders<Book>.Filter.Regex("Author", reg),
                                    Builders<Book>.Filter.Regex("YearPublished", reg)
                                    );
            }
            return await _booksCollection.Find(filter).ToListAsync();
        }
    }
}

