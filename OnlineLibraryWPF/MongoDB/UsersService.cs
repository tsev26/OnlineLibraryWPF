using MongoDB.Driver;
using OnlineLibraryWPF.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using DnsClient;
using MongoDB.Bson;

namespace OnlineLibraryWPF.MongoDB
{
    public class UsersService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UsersService(DatabaseSettings databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(
                databaseSettings.UsersCollectionName);
        }

        public async Task<List<User>> GetAsync() =>
            await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(ObjectId id) =>
            await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<User>> GetAllCustomersAsync(string searchString = "") 
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq("_t", "Customer");
            if (searchString.Length >= 3)
            {
                BsonRegularExpression reg = new BsonRegularExpression(searchString, "i");
                filter &= Builders<User>.Filter.Or(
                                    Builders<User>.Filter.Regex("LoginName", reg),
                                    Builders<User>.Filter.Regex("FirstName", reg),
                                    Builders<User>.Filter.Regex("LastName", reg),
                                    Builders<User>.Filter.Regex("PID", reg),
                                    Builders<User>.Filter.Regex("Address.City", reg),
                                    Builders<User>.Filter.Regex("Address.Street", reg),
                                    Builders<User>.Filter.Regex("Address.PostalCode", reg),
                                    Builders<User>.Filter.Regex("Address.Country", reg)
                                    );
            }
            return await _usersCollection.Find(filter).ToListAsync(); 
        }

        public List<User> GetAllCustomers() =>
             _usersCollection.Find(Builders<User>.Filter.Eq("_t", "Customer")).ToList();

        public async Task<User?> GetByLoginAndPassAsync(string login, string password) =>
            await _usersCollection.Find(x => x.LoginName == login && x.Password == password).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _usersCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(ObjectId id, User updatedUser)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Id, id);
            await _usersCollection.ReplaceOneAsync(filter, updatedUser);
        }
            

        public async Task RemoveAsync(ObjectId id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<bool> CheckUserWithLoginExists(string? loginName) => 
            await _usersCollection.Find(x => x.LoginName == loginName ).FirstOrDefaultAsync() != null;

        public async Task BanUser(ObjectId? id, bool value)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Id, id);
            UpdateDefinition<User> update = Builders<User>.Update.Set("IsBanned", value);
            await _usersCollection.UpdateOneAsync(filter, update);
        }



        public async Task ApproveUser(ObjectId? id, bool value)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Id, id);
            UpdateDefinition<User> update = Builders<User>.Update.Set("IsApproved", value);
            await _usersCollection.UpdateOneAsync(filter, update);
        }

        public async Task<long> GetNumberOfUnapprovedCustomersAsync()
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq("_t", "Customer") & Builders<User>.Filter.Eq("IsApproved", false);
            return await _usersCollection.CountDocumentsAsync(filter);
        }

        public async Task<long> GetNumberOfRentalsCustomerAsync(ObjectId? id)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Id, id); 
            ProjectionDefinition<User> projection = Builders<User>.Projection.Include("RentedBooks").Exclude("_id");

            AggregateCountResult result = await _usersCollection.Aggregate()
                            .Match(filter)
                            .Project(projection)
                            .Unwind("RentedBooks")
                            .Count()
                            .FirstOrDefaultAsync();

            long count = 0;
            if (result != null)
            {
                count = result.Count;
            }

            return count;
        }

        public async Task RentBook(ObjectId id, RentedBook rentedBook)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var update = Builders<User>.Update.Push("RentedBooks", rentedBook);
            await _usersCollection.FindOneAndUpdateAsync(filter, update);
        }
    }
}
