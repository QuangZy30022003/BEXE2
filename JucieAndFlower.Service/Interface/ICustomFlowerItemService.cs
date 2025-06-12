using JucieAndFlower.Data.Enities.CusFlower;
using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface ICustomFlowerItemService
    {
        Task<IEnumerable<CustomFlowerItem>> GetAllAsync();
        Task<CustomFlowerItem?> GetByIdAsync(int id);
        Task<CustomFlowerItem> AddAsync(CreateCustomFlowerItemDto dto);
        Task<bool> UpdateAsync(int id, UpdateCustomFlowerItemDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
