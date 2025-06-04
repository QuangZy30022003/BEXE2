using JucieAndFlower.Data.Enities.Revenue;
using JucieAndFlower.Data.Repositories;
using JucieAndFlower.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Service
{
    public class RevenueService : IRevenueService
    {
        private readonly IRevenueRepository _revenueRepository;

        public RevenueService(IRevenueRepository revenueRepository)
        {
            _revenueRepository = revenueRepository;
        }

        public async Task<List<DailyRevenueDto>> GetDailyRevenueAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _revenueRepository.GetOrdersAsync(startDate, endDate);

            return orders
                .GroupBy(o => o.OrderDate!.Value.Date)
                .Select(g => new DailyRevenueDto
                {
                    Date = g.Key,
                    Revenue = g.Where(o => o.Status == "Complete").Sum(o => o.FinalAmount ?? 0),
                    OrderCount = g.Count(),
                    AverageOrderValue = g.Where(o => o.Status == "Complete").Any()
                        ? g.Where(o => o.Status == "Complete").Average(o => o.FinalAmount ?? 0)
                        : 0,
                    CompletedOrders = g.Count(o => o.Status == "Complete"),
                    PendingOrders = g.Count(o => o.Status == "Pending"),
                    CancelledOrders = g.Count(o => o.Status == "Cancelled")
                })
                .OrderBy(x => x.Date)
                .ToList();
        }

        public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year)
        {
            var orders = await _revenueRepository.GetOrdersAsync(
                new DateTime(year, 1, 1),
                new DateTime(year, 12, 31));

            var monthlyData = orders
                .GroupBy(o => new { o.OrderDate!.Value.Year, o.OrderDate!.Value.Month })
                .Select(g => new MonthlyRevenueDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                    Revenue = g.Where(o => o.Status == "Complete").Sum(o => o.FinalAmount ?? 0),
                    OrderCount = g.Count(),
                    AverageOrderValue = g.Where(o => o.Status == "Complete").Any()
                        ? g.Where(o => o.Status == "Complete").Average(o => o.FinalAmount ?? 0)
                        : 0
                })
                .OrderBy(x => x.Month)
                .ToList();

            for (int i = 1; i < monthlyData.Count; i++)
            {
                var current = monthlyData[i];
                var previous = monthlyData[i - 1];
                if (previous.Revenue > 0)
                {
                    current.GrowthPercentage = ((current.Revenue - previous.Revenue) / previous.Revenue) * 100;
                }
            }

            return monthlyData;
        }

        public async Task<RevenueStatsDto> GetRevenueStatsAsync()
        {
            var allOrders = await _revenueRepository.GetAllOrdersAsync();
            var completedOrders = allOrders.Where(o => o.Status == "Complete").ToList();
            var now = DateTime.Now;
            var today = now.Date;
            var weekStart = today.AddDays(-(int)today.DayOfWeek);
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = monthStart.AddMonths(-1);
            var lastMonthEnd = monthStart.AddDays(-1);

            var stats = new RevenueStatsDto
            {
                TotalRevenue = completedOrders.Sum(o => o.FinalAmount ?? 0),
                TodayRevenue = completedOrders.Where(o => o.OrderDate?.Date == today).Sum(o => o.FinalAmount ?? 0),
                ThisWeekRevenue = completedOrders.Where(o => o.OrderDate >= weekStart && o.OrderDate <= today.AddDays(1)).Sum(o => o.FinalAmount ?? 0),
                ThisMonthRevenue = completedOrders.Where(o => o.OrderDate >= monthStart).Sum(o => o.FinalAmount ?? 0),
                LastMonthRevenue = completedOrders.Where(o => o.OrderDate >= lastMonthStart && o.OrderDate <= lastMonthEnd).Sum(o => o.FinalAmount ?? 0),
                TotalOrders = allOrders.Count,
                TodayOrders = allOrders.Count(o => o.OrderDate?.Date == today),
                ThisWeekOrders = allOrders.Count(o => o.OrderDate >= weekStart && o.OrderDate <= today.AddDays(1)),
                ThisMonthOrders = allOrders.Count(o => o.OrderDate >= monthStart),
                LastMonthOrders = allOrders.Count(o => o.OrderDate >= lastMonthStart && o.OrderDate <= lastMonthEnd),
                PendingOrders = allOrders.Count(o => o.Status == "Pending"),
                CompletedOrders = allOrders.Count(o => o.Status == "Complete"),
                CancelledOrders = allOrders.Count(o => o.Status == "Cancelled"),
                AverageOrderValue = completedOrders.Any() ? completedOrders.Average(o => o.FinalAmount ?? 0) : 0
            };

            if (stats.LastMonthRevenue > 0)
            {
                stats.MonthlyGrowthPercentage = ((stats.ThisMonthRevenue - stats.LastMonthRevenue) / stats.LastMonthRevenue) * 100;
            }

            var bestDay = completedOrders
                .GroupBy(o => o.OrderDate!.Value.Date)
                .Select(g => new { Date = g.Key, Revenue = g.Sum(o => o.FinalAmount ?? 0) })
                .OrderByDescending(g => g.Revenue)
                .FirstOrDefault();

            if (bestDay != null)
            {
                stats.HighestRevenueDate = bestDay.Date;
                stats.HighestDailyRevenue = bestDay.Revenue;
            }

            return stats;
        }

        public async Task<List<TopProductDto>> GetTopProductsAsync(int limit, DateTime startDate, DateTime endDate)
        {
            var orderDetails = await _revenueRepository.GetCompletedOrderDetailsAsync(startDate, endDate);

            var grouped = orderDetails
                .GroupBy(od => new { od.ProductId, od.Product!.Name, od.Product.ImageUrl, od.Product.Price })
                .Select(g => new TopProductDto
                {
                    ProductId = (int)g.Key.ProductId,
                    ProductName = g.Key.Name,
                    ImageUrl = g.Key.ImageUrl ?? "",
                    TotalSold = (int)g.Sum(x => x.Quantity),
                    TotalRevenue = (decimal)g.Sum(x => x.Quantity * x.UnitPrice),
                    Price = g.Key.Price ?? 0
                })
                .OrderByDescending(p => p.TotalRevenue)
                .Take(limit)
                .ToList();

            var totalRevenue = grouped.Sum(p => p.TotalRevenue);
            if (totalRevenue > 0)
            {
                foreach (var item in grouped)
                {
                    item.RevenuePercentage = (item.TotalRevenue / totalRevenue) * 100;
                }
            }

            return grouped;
        }

        public async Task<List<OrderStatusStatsDto>> GetOrderStatusStatsAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _revenueRepository.GetOrdersAsync(startDate, endDate);

            var statusStats = orders
                .GroupBy(o => o.Status)
                .Select(g => new OrderStatusStatsDto
                {
                    Status = g.Key ?? "Unknown",
                    Count = g.Count(),
                    TotalAmount = g.Sum(o => o.FinalAmount ?? 0)
                })
                .ToList();

            var totalOrders = statusStats.Sum(s => s.Count);
            var statusColors = new Dictionary<string, string>
            {
                { "Pending", "#ff9800" },
                { "Complete", "#4caf50" },
                { "Cancelled", "#f44336" },
                { "Shipping", "#2196f3" },
                { "Unknown", "#9e9e9e" }
            };

            foreach (var stat in statusStats)
            {
                stat.Percentage = totalOrders > 0 ? (decimal)stat.Count / totalOrders * 100 : 0;
                stat.Color = statusColors.ContainsKey(stat.Status) ? statusColors[stat.Status] : "#9e9e9e";
            }

            return statusStats.OrderByDescending(s => s.Count).ToList();
        }
    }
}
