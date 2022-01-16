using System;
using MongoDB.Bson;

class Vendor
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public Contacts Contacts { get; set; }
    public IEnumerable<Address> Address { get; set; }

}