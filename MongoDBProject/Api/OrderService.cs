using MongoDB.Driver;
using MongoDBProject.Models;
using MongoDBProject.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDBProject.Api
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> orders;

        public OrderService(IMarketplaceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            orders = database.GetCollection<Order>(settings.OrdersCollectionName);
        }
        public async Task<Order> GetOrderByIdAsync(string id)
        {
            return await orders.Find<Order>(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task UpdateOrderAsync(string id, Order order)
        {
            await orders.ReplaceOneAsync(x => x.Id == id, order);
        }
        public async Task<List<Order>> GetVendorOrdersAsync(string id)
        {
            //var filter = Builders<Order>.Filter.Eq(x => x.VendorId, id);
            return await orders.Find<Order>(x => x.VendorId == id).ToListAsync();
        }
        public async Task<List<Order>> GetUserOrdersAsync(string id)
        {
            return await orders.Find<Order>(x => x.UserId == id).ToListAsync();
        }
        public async Task<Order> CreateOrderAsync(Order order)
        {
            await orders.InsertOneAsync(order);
            return order;
        }
    }
}
