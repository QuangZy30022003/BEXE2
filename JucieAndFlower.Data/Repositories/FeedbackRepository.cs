using JucieAndFlower.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ApplicationDbContext _context;

        public FeedbackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks
                .Include(f => f.Product)
                .Include(f => f.Workshop)
                .Include(f => f.User)
                .ToListAsync();
        }

        public async Task<Feedback?> GetByIdAsync(int id)
        {
            return await _context.Feedbacks
                .Include(f => f.Product)
                .Include(f => f.Workshop)
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.FeedbackId == id);
        }

        public async Task AddAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
        }

        public async Task UpdateAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
        }

        public async Task DeleteAsync(Feedback feedback)
        {
            _context.Feedbacks.Remove(feedback);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }


}
