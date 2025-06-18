using JucieAndFlower.Data.Enities.CusFlower;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Service
{
    public class FlowerComponentService : IFlowerComponentService
    {
        private readonly IFlowerComponentRepository _repo;

        public FlowerComponentService(IFlowerComponentRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<FlowerComponentDto>> GetAllAsync()
        {
            var components = await _repo.GetAllAsync();
            return components.Select(c => new FlowerComponentDto
            {
                Name = c.Name,
                UnitPrice = c.UnitPrice,
                Unit = c.Unit,
                ImageUrl = c.ImageUrl,
                Color = c.Color
            });
        }

        public async Task<FlowerComponentDto> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;
            return new FlowerComponentDto
            {
                Name = c.Name,
                UnitPrice = c.UnitPrice,
                Unit = c.Unit,
                ImageUrl = c.ImageUrl,
                Color = c.Color
            };
        }

        public async Task AddAsync(FlowerComponentDto dto)
        {
            var component = new FlowerComponent
            {
                Name = dto.Name,
                UnitPrice = dto.UnitPrice,
                Unit = dto.Unit,
                ImageUrl = dto.ImageUrl,
                Color = dto.Color
            };

            await _repo.AddAsync(component);
        }

        public async Task UpdateAsync(int id, FlowerComponentDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return;

            existing.Name = dto.Name;
            existing.UnitPrice = dto.UnitPrice;
            existing.Unit = dto.Unit;
            existing.ImageUrl = dto.ImageUrl;
            existing.Color = dto.Color;

            await _repo.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id) =>
            await _repo.DeleteAsync(id);
    }
}
