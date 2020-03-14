using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Contracts
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object Id);
        Task<long> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(object Id);
        Task<bool> ExistAsync(object Id);
    }

    public interface IReadOnlyRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object Id);        
        Task<bool> ExistAsync(object Id);
    }
}
