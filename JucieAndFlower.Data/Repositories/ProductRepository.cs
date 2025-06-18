using JucieAndFlower.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync() =>
await _context.Products
        .Include(p => p.Category)
        .Include(p => p.Images)
        .ToListAsync();
        public async Task<Product?> GetByIdAsync(int id) =>
      await _context.Products
          .Include(p => p.Category)
          .Include(p => p.Images)
          .FirstOrDefaultAsync(p => p.ProductId == id);
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;

        public async Task<List<Product>> GetByCategoryIdAsync(int categoryId)
        {
          return await _context.Products
                  .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }
    }

}
