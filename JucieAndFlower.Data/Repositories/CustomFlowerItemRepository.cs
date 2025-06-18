using JucieAndFlower.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public class CustomFlowerItemRepository : ICustomFlowerItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomFlowerItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomFlowerItem> AddAsync(CustomFlowerItem item)
        {
            _context.CustomFlowerItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<List<CustomFlowerItem>> GetByUserIdAsync(int userId)
        {
            return await _context.CustomFlowerItems
                .Include(c => c.Component)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<CustomFlowerItem?> GetByIdAsync(int id)
        {
            return await _context.CustomFlowerItems.FirstOrDefaultAsync(c => c.CustomFlowerItemId == id);
        }
    }
}
