using JucieAndFlower.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly ApplicationDbContext _context;

        public PromotionRepository ( ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Promotion> AddAsync(Promotion promotion)
        {
                _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
            return promotion;
        }

        public async Task DeleteAsync(Promotion promotion)
        {
            _context?.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Promotion>> GetAllAsync() => await _context.Promotions.ToListAsync();

        public async Task<Promotion?> GetByCodeAsync(string code) => await _context.Promotions.FirstOrDefaultAsync(p => p.Code == code);
       

        public async Task<Promotion?> GetByIdAsync(int id) => await _context.Promotions.FindAsync(id);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
        }
    }
}
