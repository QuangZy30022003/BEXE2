using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IFlowerComponentRepository
    {
        Task<IEnumerable<FlowerComponent>> GetAllAsync();
        Task<FlowerComponent> GetByIdAsync(int id);
        Task AddAsync(FlowerComponent component);
        Task UpdateAsync(FlowerComponent component);
        Task DeleteAsync(int id);
    }
}
