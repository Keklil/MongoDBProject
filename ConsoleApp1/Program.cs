using Bogus;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MongoDB.Bson.Serialization;

namespace ConsoleApp1
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //BenchmarkRunner.Run<Program>();

            var program = new Program();

            await program.CreateDataVendors();

            await program.CreateDataUsers();

            for (int i = 0; i < 10; i++)
                await program.CreateDataProducts();

            for (int i = 0; i < 10; i++)
                await program.CreateDataOrders();
        }
        [Benchmark(Description = "CreateDataVendors")]
        public async Task CreateDataVendors()
        {
            var count = 10000;
            Console.WriteLine("Generating data for Vendors...");

            Console.WriteLine("Connect and insert do database...");
            var client = new MongoClient(
                "mongodb://localhost:27017/marketplace"
            );
            var database = client.GetDatabase("marketplace");
            var collection = database.GetCollection<Vendor>("Vendors");

            var contactsFaker = new Faker<Contacts>()
                .RuleFor(x => x.Email, x => x.Person.Email)
                .RuleFor(x => x.PhoneNumber, x => x.Person.Phone);
            var addressFaker = new Faker<Address>()
                .RuleFor(x => x.Country, x => x.Address.Country())
                .RuleFor(x => x.City, x => x.Address.City())
                .RuleFor(x => x.Street, x => x.Address.StreetAddress())
                .RuleFor(x => x.Zip, x => x.Address.ZipCode());
            var vendorFaker = new Faker<Vendor>()
                .RuleFor(x => x.Name, x => x.Person.FullName)
                .RuleFor(x => x.Password, x => x.Internet.Password())
                .RuleFor(x => x.Contacts, x => contactsFaker)
                .RuleFor(x => x.Address, x => addressFaker.Generate(x.Random.Number(1, 3)).ToList());

            var data = vendorFaker.Generate(count);
            await collection.InsertManyAsync(data);

            /*data = vendorFaker.Generate(count);
            foreach (var person in data)
            {
                await collection.InsertOneAsync(person);
            }
            Console.WriteLine("...done!");*/
        }
        [Benchmark(Description = "CreateDataUsers")]
        public async Task CreateDataUsers()
        {
            var count = 10000;
            Console.WriteLine("Generating data for Users...");

            Console.WriteLine("Connect and insert do database...");
            var client = new MongoClient(
                "mongodb://localhost:27017/marketplace"
            );
            var database = client.GetDatabase("marketplace");
            var collection = database.GetCollection<User>("Users");

            var userContactsFaker = new Faker<UserContacts>()
                .RuleFor(x => x.Email, x => x.Person.Email)
                .RuleFor(x => x.PhoneNumber, x => x.Person.Phone);
            var userAddressFaker = new Faker<UserAddress>()
                .RuleFor(x => x.Country, x => x.Address.Country())
                .RuleFor(x => x.City, x => x.Address.City())
                .RuleFor(x => x.Street, x => x.Address.StreetAddress())
                .RuleFor(x => x.Zip, x => x.Address.ZipCode());
            var userFaker = new Faker<User>()
                .RuleFor(x => x.Name, x => x.Person.FullName)
                .RuleFor(x => x.Password, x => x.Internet.Password())
                .RuleFor(x => x.UserContacts, x => userContactsFaker.Generate(x.Random.Number(1, 3)).ToList())
                .RuleFor(x => x.UserAddress, x => userAddressFaker.Generate(x.Random.Number(1, 3)).ToList());

            var data = userFaker.Generate(count);
            await collection.InsertManyAsync(data);

            /*data = userFaker.Generate(10000);
            foreach (var person in data)
            {
                await collection.InsertOneAsync(person);
            }
            Console.WriteLine("...done!");*/
        }

        [Benchmark(Description = "CreateDataProducts")]
        public async Task CreateDataProducts()
        {
            var count = 10000;
            Console.WriteLine("Generating data for Products...");
            var client = new MongoClient(
                "mongodb://localhost:27017/marketplace"
            );
            var database = client.GetDatabase("marketplace");
            var collection = database.GetCollection<Product>("Products");
            var vendors = database.GetCollection<BsonDocument>("Vendors");
            var users = database.GetCollection<BsonDocument>("Users");

            var vendorsIds = await vendors.Find(new BsonDocument()).Project("{_id: 1}").ToListAsync();
            var vendorsIdsArray = vendorsIds.ConvertAll(x => x.ToString()).ToArray();
            var str = new List<string>();
            foreach (var x in vendorsIdsArray)
            {
                var temp = x.Replace("{ \"_id\" : ObjectId(\"", "");
                temp = temp.Replace("\") }", "");
                str.Add(temp);
            }
            vendorsIdsArray = str.ToArray();

            var usersIds = await users.Find(new BsonDocument()).Project("{_id: 1}").ToListAsync();
            var usersIdsArray = usersIds.ConvertAll(x => x.ToString()).ToArray();
            str = new List<string>();
            foreach (var x in usersIdsArray)
            {
                var temp = x.Replace("{ \"_id\" : ObjectId(\"", "");
                temp = temp.Replace("\") }", "");
                str.Add(temp);
            }
            usersIdsArray = str.ToArray();

            var productGradesFaker = new Faker<Grades>()
              .RuleFor(x => x.UserId, x => x.PickRandom(usersIdsArray))
              .RuleFor(x => x.Grade, x => x.Random.Int(0, 5))
              .RuleFor(x => x.Comment, x => x.Lorem.Sentences())
              .RuleFor(x => x.Date, x => x.Date.Recent());
            var productInfoFaker = new Faker<ProductInfo>()
                .RuleFor(x => x.Description, x => x.Commerce.ProductDescription())
                .RuleFor(x => x.WarrantyPeriod, x => x.Random.Number(0, 1000))
                .RuleFor(x => x.ProductModel, x => x.Random.AlphaNumeric(8));
            var productFaker = new Faker<Product>()
                .RuleFor(x => x.ProductName, x => x.Commerce.ProductName())
                .RuleFor(x => x.VendorCode, x => x.Random.AlphaNumeric(8))
                .RuleFor(x => x.Categories, x => x.Commerce.Categories(3))
                .RuleFor(x => x.Grades, x => productGradesFaker.Generate(x.Random.Number(1, 200)).ToList())
                .RuleFor(x => x.GradesQuantity, x => x.Random.Number(3000))
                .RuleFor(x => x.Price, x => x.Random.Float()*100f)
                .RuleFor(x => x.ProductInfo, x => productInfoFaker)
                .RuleFor(x => x.VendorId, x => x.PickRandom(vendorsIdsArray));

            var data = productFaker.Generate(count);
            await collection.InsertManyAsync(data);

            /*data = productFaker.Generate(count);
            foreach (var person in data)
            {
                await collection.InsertOneAsync(person);
            }
            Console.WriteLine("...done!");*/
        }

        [Benchmark(Description = "CreateDataOrders")]
        public async Task CreateDataOrders()
        {
            var count = 1000;
            Console.WriteLine("Generating data for Orders..");
            var client = new MongoClient(
                "mongodb://localhost:27017/marketplace"
            );
            var database = client.GetDatabase("marketplace");
            var collection = database.GetCollection<Order>("Orders");
            var users = database.GetCollection<BsonDocument>("Users");
            var vendors = database.GetCollection<BsonDocument>("Vendors");
            var products = database.GetCollection<BsonDocument>("Products");

            var usersIds = await users.Find(new BsonDocument()).Project("{_id: 1}").ToListAsync();
            var usersIdsArray = usersIds.ConvertAll(x => x.ToString()).ToArray();
            var str = new List<string>();
            foreach (var x in usersIdsArray)
            {
                var temp = x.Replace("{ \"_id\" : ObjectId(\"", "");
                temp = temp.Replace("\") }", "");
                str.Add(temp);
            }
            usersIdsArray = str.ToArray();

            var vendorsIds = await vendors.Find(new BsonDocument()).Project("{_id: 1}").ToListAsync();
            var vendorsIdsArray = vendorsIds.ConvertAll(x => x.ToString()).ToArray();
            str = new List<string>();
            foreach (var x in vendorsIdsArray)
            {
                var temp = x.Replace("{ \"_id\" : ObjectId(\"", "");
                temp = temp.Replace("\") }", "");
                str.Add(temp);
            }
            vendorsIdsArray = str.ToArray();

            var productsIds = await products.Find(new BsonDocument()).Project("{_id: 1}").ToListAsync();
            var productsIdsArray = productsIds.ConvertAll(x => x.ToString()).ToArray();
            str = new List<string>();
            foreach (var x in productsIdsArray)
            {
                var temp = x.Replace("{ \"_id\" : ObjectId(\"", "");
                temp = temp.Replace("\") }", "");
                str.Add(temp);
            }
            productsIdsArray = str.ToArray();

            foreach (var item in productsIdsArray)
                Console.WriteLine(item);

            var status = new[] {"Canceled", "Pending payment", "Awaits delivery", "Delivered"};
            
            var productUnitFaker = new Faker<ProductUnit>()
                .RuleFor(x => x.ProductId, x => x.PickRandom(productsIdsArray))
                .RuleFor(x => x.Quantity, x => x.Random.Int(1, 20));
            var orderStatusFaker = new Faker<OrderStatus>()
                .RuleFor(x => x.Completness, x => x.Random.Bool())
                .RuleFor(x => x.Status, x => x.PickRandom(status))
                .RuleFor(x => x.OrderDescription, x => x.Lorem.Sentences());
            var orderFaker = new Faker<Order>()
                .RuleFor(x => x.ProductUnits, x => productUnitFaker.Generate(x.Random.Number(1, 20)).ToList())
                .RuleFor(x => x.TotalPrice, x => x.Random.Float() * 1000f)
                .RuleFor(x => x.OrderTime, x => x.Date.Recent())
                .RuleFor(x => x.OrderStatus, x => orderStatusFaker)
                .RuleFor(x => x.UserId, x => x.PickRandom(usersIdsArray))
                .RuleFor(x => x.VendorId, x => x.PickRandom(vendorsIdsArray));

            var data = orderFaker.Generate(count);
            await collection.InsertManyAsync(data);

            /*data = orderFaker.Generate(count);
            foreach (var order in data)
            {
                await collection.InsertOneAsync(order);
            }
            Console.WriteLine("...done!");*/
        }

    }   
}
