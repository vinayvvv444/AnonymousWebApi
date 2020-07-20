using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonymousWebApi.Data.Contracts
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(Guid id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(Guid id);
        Task<T> Get(int id);
    }
}
