using JucieAndFlower.Data.Enities.Revenue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IRevenueService
    {
        Task<List<DailyRevenueDto>> GetDailyRevenueAsync(DateTime startDate, DateTime endDate);
        Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year);
        Task<RevenueStatsDto> GetRevenueStatsAsync();

        Task<List<TopProductDto>> GetTopProductsAsync(int limit, DateTime startDate, DateTime endDate);

        Task<List<OrderStatusStatsDto>> GetOrderStatusStatsAsync(DateTime startDate, DateTime endDate);


    }
}
