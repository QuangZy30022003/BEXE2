using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public interface ICartRepository
    {
        Task<CartItem?> GetByUserAndProductAsync(int userId, int productId);
        Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId);
        Task<CartItem> AddAsync(CartItem item);
        Task UpdateAsync(CartItem item);
        Task DeleteAsync(CartItem item);
        Task SaveChangesAsync();

        Task<List<CartItem>> GetCartItemsByIdsAsync(List<int> ids);
        Task DeleteRangeAsync(List<CartItem> items);

    }
}
