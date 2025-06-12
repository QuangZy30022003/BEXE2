using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IFlowerComponentService
    {
        Task<IEnumerable<FlowerComponent>> GetAllAsync();
        Task<FlowerComponent?> GetByIdAsync(int id);
        Task<FlowerComponent> CreateAsync(FlowerComponent component);
        Task<bool> UpdateAsync(int id, FlowerComponent component);
        Task<bool> DeleteAsync(int id);
    }
}
