using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models;
using MongoDB.Driver;

namespace DataAccess.Interfaces
{
    public interface ICatalog<T>
    {
        IMongoDatabase Database { get; set; }
        IMongoCollection<T> Collection { get; set; }
    }
}
