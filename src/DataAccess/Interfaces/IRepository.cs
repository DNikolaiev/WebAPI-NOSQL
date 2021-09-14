using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(string id);
        Task Create(T product);
        Task<bool> Update(T product);
        Task<bool> Delete(string id);

    }
}
