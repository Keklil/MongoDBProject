using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public List<UserAddress> UserAddress { get; set; }
    public List<UserContacts> UserContacts { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> ShoppingCart { get; set; }
    [BsonIgnore]
    public List<Product> ProductList { get; set; }
}

public class UserAddress
{
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string Zip { get; set; }
}

public class UserContacts
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}