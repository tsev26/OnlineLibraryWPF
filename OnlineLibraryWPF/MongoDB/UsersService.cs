using MongoDB.Driver;
using OnlineLibraryWPF.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using DnsClient;

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

        public async Task<User?> GetAsync(string id) =>
            await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<User>> GetAllCustomersAsync() =>
            await _usersCollection.Find(Builders<User>.Filter.Eq("_t", "Customer")).ToListAsync();

        public List<User> GetAllCustomers() =>
             _usersCollection.Find(Builders<User>.Filter.Eq("_t", "Customer")).ToList();

        public async Task<User?> GetByLoginAndPassAsync(string login, string password) =>
            await _usersCollection.Find(x => x.LoginName == login && x.Password == password).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _usersCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) =>
            await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<bool> CheckUserWithLoginExists(string? loginName) => 
            await _usersCollection.Find(x => x.LoginName == loginName ).FirstOrDefaultAsync() != null;
    }
}
