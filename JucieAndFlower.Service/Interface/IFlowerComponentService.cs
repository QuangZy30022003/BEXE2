using JucieAndFlower.Data.Enities.CusFlower;
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
        Task<IEnumerable<FlowerComponentDto>> GetAllAsync();
        Task<FlowerComponentDto> GetByIdAsync(int id);
        Task AddAsync(FlowerComponentDto dto);
        Task UpdateAsync(int id, FlowerComponentDto dto);
        Task DeleteAsync(int id);
    }
}
