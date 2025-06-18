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
        Task<CustomFlowerItem> AddAsync(CustomFlowerItem item);
        Task<List<CustomFlowerItem>> GetByUserIdAsync(int userId);

        Task<CustomFlowerItem?> GetByIdAsync(int id);
    }
}
