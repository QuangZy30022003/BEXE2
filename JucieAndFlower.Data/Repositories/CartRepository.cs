using JucieAndFlower.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem?> GetByUserAndProductAsync(int userId, int productId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        }

        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _context.CartItems.Include(c => c.Product).Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<CartItem> AddAsync(CartItem item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartItem>> GetCartItemsByIdsAsync(List<int> ids)
        {
            return await _context.CartItems
                .Where(c => ids.Contains(c.Id))
                .Include(c => c.Product)
                .ToListAsync();
        }

        public async Task DeleteRangeAsync(List<CartItem> items)
        {
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

    }
}
