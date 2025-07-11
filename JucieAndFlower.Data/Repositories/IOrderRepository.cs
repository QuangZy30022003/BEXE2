﻿using JucieAndFlower.Data.Models;

using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<Order?> GetByIdAsync(int id);

        Task SaveChangesAsync();

        Task<List<Order>> GetPendingOrdersAsync();

        Task<List<Order>> GetOrdersByUserIdAsync(int userId);

        Task<Order> GetOrderWithUserAsync(int orderId);

        Task<Order?> GetOrderByPayOSOrderCodeAsync(int payOSOrderCode);
    }
}
