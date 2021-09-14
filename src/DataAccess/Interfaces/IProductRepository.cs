using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Interfaces
{
    public interface IProductRepository:IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
    }
}
