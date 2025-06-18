using JucieAndFlower.Data.Models;
using JucieAndFlower.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public class FlowerComponentRepository : IFlowerComponentRepository
    {
        private readonly ApplicationDbContext _context;

        public FlowerComponentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlowerComponent>> GetAllAsync() =>
          await _context.FlowerComponents.ToListAsync();

        public async Task<FlowerComponent> GetByIdAsync(int id) =>
            await _context.FlowerComponents.FindAsync(id);

        public async Task AddAsync(FlowerComponent component)
        {
            _context.FlowerComponents.Add(component);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FlowerComponent component)
        {
            _context.FlowerComponents.Update(component);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var component = await _context.FlowerComponents.FindAsync(id);
            if (component != null)
            {
                _context.FlowerComponents.Remove(component);
                await _context.SaveChangesAsync();
            }
        }
    }
    }
