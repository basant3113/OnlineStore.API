using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Core.Repositories
{
    public interface IGenaricRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> SearchByNameAsync(string Name);
        Task<T> GetByIdAsync(int id);
        Task<int> AddAsync(T Entity);
        Task<int> UpdateAsync(T Entity);
        Task<int> DeleteAsync(T Entity);
    }
}
