using JucieAndFlower.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public class ProductDetailRepository : IProductDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDetail>> GetAllAsync()
        {
            return await _context.ProductDetails.ToListAsync();
        }

        public async Task<ProductDetail?> GetByIdAsync(int id)
        {
            return await _context.ProductDetails.FindAsync(id);
        }

        public async Task<ProductDetail> AddAsync(ProductDetail entity)
        {
            _context.ProductDetails.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(ProductDetail entity)
        {
            _context.ProductDetails.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductDetail entity)
        {
            _context.ProductDetails.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

}
