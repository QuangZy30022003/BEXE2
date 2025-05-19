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

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<List<CartItemResponseDTO>> GetUserCartAsync(int userId)
        {
            var items = await _cartRepository.GetCartItemsByUserIdAsync(userId);
            return items.Select(c => new CartItemResponseDTO
            {
                Id = c.Id,
                ProductId = c.ProductId,
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
    }
}
