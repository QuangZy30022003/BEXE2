using JucieAndFlower.Data.Enities.Order;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Data.Repositories;
using JucieAndFlower.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrderAsync(OrderCreateDTO dto)
        {
            decimal total = dto.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);
            decimal discount = 0;

            // (Tùy chọn) kiểm tra mã giảm giá nếu có logic áp dụng
            if (!string.IsNullOrEmpty(dto.PromotionCode))
            {
                // Logic xử lý mã khuyến mãi
                // discount = ...;
            }

            var final = total - discount;

            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                DiscountAmount = discount,
                FinalAmount = final,
                DeliveryAddress = dto.DeliveryAddress,
                PromotionCode = dto.PromotionCode,
                Note = dto.Note,
                Status = "Pending",
                OrderDetails = dto.OrderDetails.Select(d => new OrderDetail
                {
                    ProductId = d.ProductId,
                    ProductDetailId = d.ProductDetailId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };

            return await _orderRepository.AddAsync(order);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }
        public async Task MarkOrderAsCompleteAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null && order.Status == "Pending")
            {
                order.Status = "Complete";
                 await _orderRepository.SaveChangesAsync();
            }
        }

    }

}
