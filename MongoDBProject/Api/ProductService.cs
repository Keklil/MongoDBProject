using MongoDB.Driver;
using MongoDBProject.Models;
using MongoDBProject.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDBProject.Api
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> products;

        public ProductService(IMarketplaceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            products = database.GetCollection<Product>(settings.ProductsCollectionName);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await products.Find<Product>(x => true).ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await products.Find<Product>(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            await products.InsertOneAsync(product);
            return product;
        }

        public async Task UpdateProductAsync(string id, Product product)
        {
            await products.ReplaceOneAsync(new BsonDocument("_id", id), product);
        }
        public async Task<List<Product>> GetProductsByVendorIdAsync(string id)
        {
            return await products.Find<Product>(x => x.VendorId == id).ToListAsync();
        }
        public async Task DeleteProductAsync(string id)
        {
            await products.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<Product>> FilterByPriceAsync(float minPrice, float maxPrice)
        {
            var builder = new FilterDefinitionBuilder<Product>();
            var filter = builder.Gte("Price", minPrice) & builder.Lte("Price", maxPrice);
            return await products.Find(filter).Limit(50).ToListAsync();

            /*var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument()
                    .Add("Price", new BsonDocument()
                        .Add("$gte", minPrice)
                        .Add("$lte", maxPrice)
                    ))
            };

            return await products.Aggregate<Product>(pipeline).ToListAsync();*/
        }
    }
}
