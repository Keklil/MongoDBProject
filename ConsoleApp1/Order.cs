using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public List<ProductUnit> ProductUnits { get; set; }
    public float TotalPrice { get; set; }
    public DateTime OrderTime { get; set; }
    public OrderStatus OrderStatus { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string VendorId { get; set; }
}

public class ProductUnit
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderStatus
{
    public bool Completness { get; set; }
    public string Status { get; set; }
    public string OrderDescription { get; set; }
}