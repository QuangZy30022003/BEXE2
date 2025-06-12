//using JucieAndFlower.Data.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JucieAndFlower.Data.Repositories
//{
//    public class CustomFlowerItemRepository : ICustomFlowerItemRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public CustomFlowerItemRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<CustomFlowerItem>> GetAllAsync()
//        {
//            return await _context.CustomFlowerItems.ToListAsync();
//        }

//        public async Task<CustomFlowerItem?> GetByIdAsync(int id)
//        {
//            return await _context.CustomFlowerItems.FindAsync(id);
//        }

//        public async Task AddAsync(CustomFlowerItem item)
//        {
//            _context.CustomFlowerItems.Add(item);
//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateAsync(CustomFlowerItem item)
//        {
//            _context.CustomFlowerItems.Update(item);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var item = await _context.CustomFlowerItems.FindAsync(id);
//            if (item != null)
//            {
//                _context.CustomFlowerItems.Remove(item);
//                await _context.SaveChangesAsync();
//            }
//        }

//        public async Task<bool> ExistsAsync(int id)
//        {
//            return await _context.CustomFlowerItems.AnyAsync(e => e.Id == id);
//        }
//    }
//}
