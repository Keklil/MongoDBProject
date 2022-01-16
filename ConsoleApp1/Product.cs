using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ProductName { get; set; }
    public string VendorCode { get; set; }
    public string[] Categories { get; set; }
    public List<Grades> Grades { get; set; }
    public int GradesQuantity { get; set; }
    public float Price { get; set; }
    public ProductInfo ProductInfo { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string VendorId { get; set; }
}

public class Grades
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }
    public int Grade { get; set; }
    public string Comment { get; set; }
    public DateTime Date { get; set; }
}

public class ProductInfo
{
    public string Description { get; set; }
    public int WarrantyPeriod { get; set; }
    public string ProductModel { get; set; }

    [BsonExtraElements]
    public BsonDocument Metadata { get; set; }
}