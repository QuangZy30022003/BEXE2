using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public interface ICustomFlowerItemRepository
    {
        Task<IEnumerable<CustomFlowerItem>> GetAllAsync();
        Task<CustomFlowerItem?> GetByIdAsync(int id);
        Task AddAsync(CustomFlowerItem item);
        Task UpdateAsync(CustomFlowerItem item);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
