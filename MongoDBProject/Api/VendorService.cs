using MongoDB.Driver;
using MongoDBProject.Models;
using MongoDBProject.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDBProject.Api
{
    public class VendorService
    {
        private readonly IMongoCollection<Vendor> vendors;
        public VendorService(IMarketplaceDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            vendors = database.GetCollection<Vendor>(settings.VendorsCollectionName);
        }

        public async Task<List<Vendor>> GetAllVendorsAsync()
        {
            return await vendors.Find(x => true).ToListAsync();
        }

        public async Task<Vendor> GetVendorByIdAsync(string id)
        {
            return await vendors.Find<Vendor>(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Vendor> CreateVendorAsync(Vendor vendor)
        {
            await vendors.InsertOneAsync(vendor);
            return vendor;
        }

        public async Task<Vendor> GetVendorLoginAsync(string login, string password)
        {
            return await vendors.Find<Vendor>(x => x.Login == login && x.Password == password).FirstOrDefaultAsync();
        }

        public async Task UpdateVendorAsync(string id, Vendor vendor)
        {
            await vendors.ReplaceOneAsync(new BsonDocument("_id", id), vendor);
        }
        public async Task DeleteVendorAsync(string id)
        {
            await vendors.DeleteOneAsync(x => x.Id == id);
        }
    }
}
