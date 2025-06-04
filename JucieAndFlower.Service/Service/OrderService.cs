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

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            return new OrderDto
            {
                OrderId = order.OrderId,
                UserId = (int)order.UserId,
                OrderDate =(DateTime)order.OrderDate,
                TotalAmount = (Decimal)order.TotalAmount,
                DiscountAmount = (Decimal)order.DiscountAmount,
                FinalAmount = (Decimal)order.FinalAmount,
                Status = order.Status,
                Note = order.Note,
                DeliveryAddress = order.DeliveryAddress,
                PromotionCode = order.PromotionCode,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = (int)od.ProductId,
                    ProductDetailId = od.ProductDetailId,
                    Quantity = (int)od.Quantity,
                    UnitPrice = (Decimal)od.UnitPrice
                }).ToList()
            };
        }


        public async Task<List<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(order => new OrderResponseDTO
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                DiscountAmount = order.DiscountAmount,
                FinalAmount = order.FinalAmount,
                Status = order.Status,
                Note = order.Note,
                DeliveryAddress = order.DeliveryAddress,
                PromotionCode = order.PromotionCode,
                OrderDetails = order.OrderDetails?.Select(od => new OrderDetailResponseDTO
                {
                    OrderDetailId = od.OrderDetailId,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    ProductDetailId = od.ProductDetailId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    Product = od.Product != null ? new ProductResponseDTO
                    {
                        ProductId = od.Product.ProductId,
                        Name = od.Product.Name,
                        Description = od.Product.Description,
                        Price = od.Product.Price,
                        ImageUrl = od.Product.ImageUrl,
                        CategoryId = od.Product.CategoryId,
                        IsAvailable = od.Product.IsAvailable,
                        CreatedAt = od.Product.CreatedAt
                    } : null
                }).ToList() ?? new List<OrderDetailResponseDTO>()
            }).ToList();
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
