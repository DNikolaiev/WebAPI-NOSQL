using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DataAccess.Services
{
    public class ProductCatalog: ICatalog<Product>
    {
        public IMongoDatabase Database { get; set; }
        public IMongoCollection<Product> Collection { get; set; }

        public ProductCatalog(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.IsContained ? settings.Container : settings.ConnectionString);
            Database = client.GetDatabase(settings.Database);

            Collection = Database.GetCollection<Product>("Products");
        }
    }
}
