using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Vendor
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public Contacts Contacts { get; set; }
    public List<Address> Address { get; set; }
}
public class Address
{
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string Zip { get; set; }
}
public class Contacts
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
