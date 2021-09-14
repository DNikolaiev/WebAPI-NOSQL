using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DataAccess.Models;
using MongoDB.Driver;

namespace DataAccess.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ICatalog<Product> _context;

        public ProductRepository(ICatalog<Product> context)
        {
            _context = context??throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Collection.Find(p => true).ToListAsync();
        }

        public async Task<Product> Get(string id)
        {
            return await _context.Collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);
            return await _context.Collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            var filter = Builders<Product>.Filter.ElemMatch(p => p.Category, categoryName);
            return await _context.Collection.Find(filter).ToListAsync();
        }

        public async Task Create(Product product)
        {
            await _context.Collection.InsertOneAsync(product);
        }

        public async Task<bool> Update(Product product)
        {
            var updateResult =  await _context
                .Collection
                .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context.Collection.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
