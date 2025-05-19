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
        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
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
                // TODO: Xử lý logic mã giảm giá
            }

            var order = new Order
            {
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

            // Xóa các cart item đã đặt hàng
            await _cartRepository.DeleteRangeAsync(cartItems);

            return await _orderRepository.AddAsync(order);
        }


    }

}
