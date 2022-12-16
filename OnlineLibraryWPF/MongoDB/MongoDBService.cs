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
    public class MongoDBService
    {
        private readonly IMongoCollection<Book> _booksCollection;
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<Customer> _customersCollection;
        private readonly IMongoCollection<RentedBook> _rentedBooksCollection;

        public MongoDBService(DatabaseSettings databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.DatabaseName);

            _rentedBooksCollection = mongoDatabase.GetCollection<RentedBook>(databaseSettings.RentedBooksCollectionName);
            _booksCollection = mongoDatabase.GetCollection<Book>(databaseSettings.BooksCollectionName);
            _usersCollection = mongoDatabase.GetCollection<User>(databaseSettings.UsersCollectionName);
            _customersCollection = mongoDatabase.GetCollection<Customer>(databaseSettings.UsersCollectionName);
        }

        public async Task<Book?> GetBookAsync(ObjectId id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateBookAsync(Book newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateBookAsync(ObjectId id, Book updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveBookAsync(ObjectId id) =>
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

        //TODO
        //wrong
        public async Task<bool> CheckIfRented(ObjectId id)
        {
            FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(x => x.Id, id);
            ProjectionDefinition<Book> projection = Builders<Book>.Projection.Include("RentedBooks").Exclude("_id");

            AggregateCountResult result = await _booksCollection.Aggregate()
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
            return count > 0;
        }

        public async Task<RentedBook?> GetRentedBookAsync(ObjectId id) =>
             await _rentedBooksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateRentedBookAsync(RentedBook newRental) =>
            await _rentedBooksCollection.InsertOneAsync(newRental);

        public async Task UpdateRentedBookAsync(ObjectId id, RentedBook updatedRental) =>
            await _rentedBooksCollection.ReplaceOneAsync(x => x.Id == id, updatedRental);

        public async Task RemoveRentedBookAsync(ObjectId id) =>
            await _rentedBooksCollection.DeleteOneAsync(x => x.Id == id);


        public async Task<User?> GetUserAsync(ObjectId id) =>
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

        public async Task<List<User>> GetAllCustomersAsync() =>
             await _usersCollection.Find(Builders<User>.Filter.Eq("_t", "Customer")).ToListAsync();

        public async Task<User?> GetByLoginAndPassAsync(string login, string password) =>
            await _usersCollection.Find(x => x.LoginName == login && x.Password == password).FirstOrDefaultAsync();

        public async Task CreateUserAsync(User newUser) =>
            await _usersCollection.InsertOneAsync(newUser);

        public async Task UpdateUserAsync(ObjectId id, User updatedUser)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Id, id);
            await _usersCollection.ReplaceOneAsync(filter, updatedUser);
        }


        public async Task RemoveUserAsync(ObjectId id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<bool> CheckUserWithLoginExists(string? loginName) =>
            await _usersCollection.Find(x => x.LoginName == loginName).FirstOrDefaultAsync() != null;

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

        //TODO
        //wrong - must take from rentalbookscollection
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

        //TODO
        public async Task LoadRentalsForUser(ObjectId id, bool type)
        {

            try
            {
                FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
                ProjectionDefinition<RentedBook> projection = Builders<RentedBook>.Projection.Include(x => x.BookRented).Exclude("_id");

                var result = await _customersCollection
                                   .Aggregate()
                                   .Match(filter)
                                   .Unwind<Customer, RentedBook>(x => x.RentedBooks)
                                   .Project(key => key.BookId)
                                   .ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }



            //<List<RentalViewModel>>
        }

    }
}
