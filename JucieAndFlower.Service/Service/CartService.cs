using JucieAndFlower.Data.Enities.Cart;
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
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICustomFlowerItemRepository _customFlowerItemRepository;

        public CartService(ICartRepository cartRepository, ICustomFlowerItemRepository customFlowerItemRepository)
        {
            _cartRepository = cartRepository;
            _customFlowerItemRepository = customFlowerItemRepository;
        }

        public async Task<List<CartItemResponseDTO>> GetUserCartAsync(int userId)
        {
            var items = await _cartRepository.GetCartItemsByUserIdAsync(userId);
            return items.Select(c => new CartItemResponseDTO
            {
                Id = c.Id,
                ProductId = c.ProductId.HasValue ? c.ProductId.Value : throw new InvalidOperationException("ProductId cannot be null."),
                ProductName = c.Product.Name,
                Quantity = c.Quantity
            }).ToList();
        }

        public async Task AddOrUpdateCartItemAsync(int userId, CartItemCreateDTO dto)
        {
            var existingItem = await _cartRepository.GetByUserAndProductAsync(userId, dto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
                await _cartRepository.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };
                await _cartRepository.AddAsync(newItem);
            }
        }

        public async Task RemoveCartItemAsync(int userId, int productId)
        {
            var item = await _cartRepository.GetByUserAndProductAsync(userId, productId);
            if (item != null)
            {
                await _cartRepository.DeleteAsync(item);
            }
        }

        public async Task AddCustomItemToCartAsync(int userId, CustomCartItemCreateDTO dto)
        {
            // Lấy custom flower item từ DB
            var customItem = await _customFlowerItemRepository.GetByIdAsync(dto.CustomFlowerItemId);
            if (customItem == null)
                throw new Exception("Custom flower item not found.");

            if (customItem.UserId != userId)
                throw new Exception("You are not allowed to add this item to your cart.");

            if (customItem.Quantity < 1)
                throw new Exception("Invalid quantity for custom flower item.");

            // Kiểm tra giỏ hàng đã có chưa
            var existingItem = await _cartRepository.GetByUserAndCustomItemAsync(userId, dto.CustomFlowerItemId);
            if (existingItem != null)
            {
                existingItem.Quantity += customItem.Quantity;
                await _cartRepository.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    UserId = userId,
                    CustomFlowerItemId = customItem.CustomFlowerItemId,
                    Quantity = customItem.Quantity,
                };
                await _cartRepository.AddAsync(newItem);
            }
        }

    }
}
