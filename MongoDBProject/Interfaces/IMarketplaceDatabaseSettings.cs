namespace MongoDBProject.Interfaces
{
    public interface IMarketplaceDatabaseSettings
    {
        string VendorsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string ProductsCollectionName { get; set; }
        string OrdersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
