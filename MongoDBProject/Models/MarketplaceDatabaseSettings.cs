using MongoDBProject.Interfaces;

namespace MongoDBProject.Models
{
    public class MarketplaceDatabaseSettings : IMarketplaceDatabaseSettings
    {
        public string VendorsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string ProductsCollectionName { get; set; }
        public string OrdersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
