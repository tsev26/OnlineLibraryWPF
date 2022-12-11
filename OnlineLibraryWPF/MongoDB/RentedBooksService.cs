using Microsoft.Extensions.Options;
using MongoDB.Bson;
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
        private readonly IMongoCollection<RentedBook> _rentedBooksCollection;

        public RentedBooksService(DatabaseSettings databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.DatabaseName);

            _rentedBooksCollection = mongoDatabase.GetCollection<RentedBook>(
                databaseSettings.RentedBooksCollectionName);
        }

        public async Task<List<RentedBook>> GetAsync() =>
            await _rentedBooksCollection.Find(_ => true).ToListAsync();

        /*
        public async Task<RentedBook?> GetAsync(ObjectId id) =>
            await _rentedBooksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(RentedBook newRental) =>
            await _rentedBooksCollection.InsertOneAsync(newRental);

        public async Task UpdateAsync(ObjectId id, RentedBook updatedRental) =>
            await _rentedBooksCollection.ReplaceOneAsync(x => x.Id == id, updatedRental);

        public async Task RemoveAsync(ObjectId id) =>
            await _rentedBooksCollection.DeleteOneAsync(x => x.Id == id);
        */
    }
}
