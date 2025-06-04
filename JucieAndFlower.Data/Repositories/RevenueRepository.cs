using JucieAndFlower.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public class RevenueRepository : IRevenueRepository
    {
        private readonly ApplicationDbContext _context;

        public RevenueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            return await _context.Orders
                .Where(o => (!startDate.HasValue || o.OrderDate >= startDate) &&
                            (!endDate.HasValue || o.OrderDate <= endDate))
                .ToListAsync();
        }

        public async Task<List<OrderDetail>> GetCompletedOrderDetailsAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Product)
                .Where(od => od.Order != null &&
                             od.Order.OrderDate >= startDate &&
                             od.Order.OrderDate <= endDate &&
                             od.Order.Status == "Complete")
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }
    }
}
