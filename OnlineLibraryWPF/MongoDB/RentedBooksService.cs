using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnlineLibraryWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.MongoDB
{
    public class RentedBooksService
    {
        private readonly IMongoCollection<RentedBooks> _rentedBooksCollection;

        public RentedBooksService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _rentedBooksCollection = mongoDatabase.GetCollection<RentedBooks>(
                databaseSettings.Value.RentedBooksCollectionName);
        }

        public async Task<List<RentedBooks>> GetAsync() =>
            await _rentedBooksCollection.Find(_ => true).ToListAsync();

        public async Task<RentedBooks?> GetAsync(string id) =>
            await _rentedBooksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(RentedBooks newRental) =>
            await _rentedBooksCollection.InsertOneAsync(newRental);

        public async Task UpdateAsync(string id, RentedBooks updatedRental) =>
            await _rentedBooksCollection.ReplaceOneAsync(x => x.Id == id, updatedRental);

        public async Task RemoveAsync(string id) =>
            await _rentedBooksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
