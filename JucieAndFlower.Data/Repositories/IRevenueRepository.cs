using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public interface IRevenueRepository
    {
        Task<List<Order>> GetOrdersAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<List<OrderDetail>> GetCompletedOrderDetailsAsync(DateTime startDate, DateTime endDate);
        Task<List<Order>> GetAllOrdersAsync();
    }
}
