using MongoDB.Driver;
using MongoDBProject.Models;
using MongoDBProject.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDBProject.Api
{
    public class UserService
    {
        private readonly IMongoCollection<User> users;

        public UserService(IMarketplaceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await users.Find(x => true).ToListAsync();
        }

        public async Task<User> GetUserLoginAsync(string login, string password)
        {
            return await users.Find<User>(x => x.Login == login && x.Password == password).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await users.Find<User>(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByLoginAsync(string login)
        {
            return await users.Find<User>(x => x.Login == login).FirstOrDefaultAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await users.InsertOneAsync(user);
            return user;
        }

        public async Task DeleteUserAsync(string id)
        {
            await users.DeleteOneAsync(x => x.Id == id);
        }

        public async Task UpdateUserAsync(string id, User user)
        {
            await users.ReplaceOneAsync(new BsonDocument("_id", id), user);
        }

        public async Task<List<BsonDocument>> GetUserShoppingCartByIdAsync(string id)
        {
            var aggregation = users.Aggregate()
                .Match(x => x.Id == id)
                .Project(new BsonDocument
                {
                    {"ShoppingCart", 1}
                });
            return await aggregation.ToListAsync();
        }

        public async Task AddProductToShoppingCart(string id) 
        {

        }
    }
}
