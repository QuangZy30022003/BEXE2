using JucieAndFlower.Data.Enities.CusFlower;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Data.Repositories;
using JucieAndFlower.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Service
{
    public class CustomFlowerItemService : ICustomFlowerItemService
    {
        private readonly ICustomFlowerItemRepository _repository;

        public CustomFlowerItemService(ICustomFlowerItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomFlowerItem> CreateCustomItemAsync(int userId, CustomFlowerItemCreateDTO dto)
        {
            if (dto.Quantity < 1 || dto.Quantity > 100)
                throw new ArgumentException("Quantity must be between 1 and 100.");

            var item = new CustomFlowerItem
            {
                UserId = userId,
                FlowerComponentId = dto.FlowerComponentId,
                Quantity = dto.Quantity,
                CreatedAt = DateTime.UtcNow
            };

            return await _repository.AddAsync(item);
        }

        public async Task<List<CustomFlowerItem>> GetUserCustomItemsAsync(int userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }
    }
}
