//using JucieAndFlower.Data.Enities.CusFlower;
//using JucieAndFlower.Data.Models;
//using JucieAndFlower.Data.Repositories;
//using JucieAndFlower.Service.Interface;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JucieAndFlower.Service.Service
//{
//    public class CustomFlowerItemService : ICustomFlowerItemService
//    {
//        private readonly ICustomFlowerItemRepository _repository;

//        public CustomFlowerItemService(ICustomFlowerItemRepository repository)
//        {
//            _repository = repository;
//        }

//        public Task<IEnumerable<CustomFlowerItem>> GetAllAsync() => _repository.GetAllAsync();

//        public Task<CustomFlowerItem?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

//        public async Task<CustomFlowerItem> AddAsync(CreateCustomFlowerItemDto dto)
//        {
//            var item = new CustomFlowerItem
//            {
//                Type = dto.Type,
//                Color = dto.Color,
//                PricePerUnit = dto.PricePerUnit,
//                Quantity = dto.Quantity,
//                Status = dto.Status
//            };

//            await _repository.AddAsync(item);
//            return item;
//        }

//        public async Task<bool> UpdateAsync(int id, UpdateCustomFlowerItemDto dto)
//        {
//            var existingItem = await _repository.GetByIdAsync(id);
//            if (existingItem == null)
//                return false;

//            existingItem.Type = dto.Type;
//            existingItem.Color = dto.Color;
//            existingItem.PricePerUnit = dto.PricePerUnit;
//            existingItem.Quantity = dto.Quantity;
//            existingItem.Status = dto.Status;

//            await _repository.UpdateAsync(existingItem);
//            return true;
//        }


//        public async Task<bool> DeleteAsync(int id)
//        {
//            if (!await _repository.ExistsAsync(id)) return false;
//            await _repository.DeleteAsync(id);
//            return true;
//        }
//    }
//}
