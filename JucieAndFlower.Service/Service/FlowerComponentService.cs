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
        private readonly IFlowerComponentRepository _repository;

        public FlowerComponentService(IFlowerComponentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FlowerComponent>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<FlowerComponent?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<FlowerComponent> CreateAsync(FlowerComponent component)
        {
            return await _repository.AddAsync(component);
        }

        public async Task<bool> UpdateAsync(int id, FlowerComponent component)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = component.Name;
            existing.Description = component.Description;
            existing.Price = component.Price;
            existing.Type = component.Type;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
