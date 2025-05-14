using JucieAndFlower.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public class WorkshopRepository : IWorkshopRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkshopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Workshop>> GetAllAsync() => await _context.Workshops.ToListAsync();

        public async Task<Workshop?> GetByIdAsync(int id) => await _context.Workshops.FindAsync(id);

        public async Task<Workshop> AddAsync(Workshop workshop)
        {
            _context.Workshops.Add(workshop);
            await _context.SaveChangesAsync();
            return workshop;
        }

        public async Task UpdateAsync(Workshop workshop)
        {
            _context.Workshops.Update(workshop);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Workshops.FindAsync(id);
            if (existing != null)
            {
                _context.Workshops.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }

}
