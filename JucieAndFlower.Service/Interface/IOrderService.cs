using JucieAndFlower.Data.Enities.Order;
using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderByIdAsync(int id);

        Task MarkOrderAsCompleteAsync(int orderId);

        Task<Order> CreateOrderFromCartAsync(OrderFromCartDTO dto);

        Task MarkOrderAsCanceledAsync(int orderId);

        Task AutoCancelExpiredPendingOrdersAsync();

        Task<List<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId);
    }

}
