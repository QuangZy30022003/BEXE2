using JucieAndFlower.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public class WorkshopTicketRepository : IWorkshopTicketRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkshopTicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkshopTicket>> GetAllAsync()
            => await _context.WorkshopTickets.ToListAsync();

        public async Task<WorkshopTicket?> GetByIdAsync(int id)
            => await _context.WorkshopTickets.FindAsync(id);

        public async Task<WorkshopTicket> AddAsync(WorkshopTicket entity)
        {
            _context.WorkshopTickets.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(WorkshopTicket entity)
        {
            _context.WorkshopTickets.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.WorkshopTickets.FindAsync(id);
            if (entity != null)
            {
                _context.WorkshopTickets.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
