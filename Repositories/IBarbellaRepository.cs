using System.Collections.Generic;
using System.Threading.Tasks;
using BarbellaInventory.Models;

namespace BarbellaInventory.Repositories
{
    public interface IBarbellaRepository
    {
        Task<IEnumerable<BarbieSet>> GetAllAsync();
        Task<BarbieSet?> GetByIdAsync(string id);
        Task<bool> AddAsync(BarbieSet barbieSet);
        Task<bool> UpdateAsync(BarbieSet barbieSet);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string Name);
    }
}