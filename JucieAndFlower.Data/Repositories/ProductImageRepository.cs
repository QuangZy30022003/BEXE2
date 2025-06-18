using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProductImage image)
        {
            await _context.ProductImages.AddAsync(image);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
