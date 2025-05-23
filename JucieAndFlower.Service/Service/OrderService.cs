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
        private readonly ICartRepository _cartRepository;
        private readonly IPromotionService _promotionService;
        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IPromotionService promotionService)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _promotionService = promotionService;
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

        public async Task MarkOrderAsCanceledAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null && order.Status == "Pending")
            {
                order.Status = "Cancel";
                await _orderRepository.SaveChangesAsync();
            }
        }
        public async Task<Order> CreateOrderFromCartAsync(OrderFromCartDTO dto)
        {
            var cartItems = await _cartRepository.GetCartItemsByIdsAsync(dto.SelectedCartItemIds);

            if (cartItems == null || !cartItems.Any())
                throw new Exception("Không tìm thấy sản phẩm trong giỏ hàng.");

            decimal total = (decimal)cartItems.Sum(c => c.Quantity * c.Product.Price);
            decimal discount = 0;

            // Xử lý mã giảm giá nếu có
            if (!string.IsNullOrEmpty(dto.PromotionCode))
            {
                var promotion = await _promotionService.GetValidPromotionByCodeAsync(dto.PromotionCode);
                if (promotion != null)
                {
                    discount = (total * (promotion.DiscountPercent ?? 0) / 100);
                    if (promotion.MaxDiscount.HasValue)
                        discount = Math.Min(discount, promotion.MaxDiscount.Value);
                }
            }


            var order = new Order
            {
                UserId = dto.UserId ?? throw new Exception("UserId is missing"),
                OrderDate = DateTime.Now,
                TotalAmount = total,
                DiscountAmount = discount,
                FinalAmount = total - discount,
                DeliveryAddress = dto.DeliveryAddress,
                PromotionCode = dto.PromotionCode,
                Note = dto.Note,
                Status = "Pending",
                OrderDetails = cartItems.Select(item => new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price,
                }).ToList()
            };

            return await _orderRepository.AddAsync(order);
        }

        public async Task AutoCancelExpiredPendingOrdersAsync()
        {
            var allPendingOrders = await _orderRepository.GetPendingOrdersAsync(); // cần thêm hàm này
            var expiredOrders = allPendingOrders
      .Where(o => o.OrderDate.HasValue && (DateTime.Now - o.OrderDate.Value).TotalMinutes > 15)
      .ToList();

            foreach (var order in expiredOrders)
            {
                order.Status = "Cancel";
            }

            if (expiredOrders.Any())
            {
                await _orderRepository.SaveChangesAsync();
            }
        }

    }

}
