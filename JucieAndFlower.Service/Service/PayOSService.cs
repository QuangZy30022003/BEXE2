using JucieAndFlower.Data.Enities.VnPay;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Data.Repositories;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
namespace JucieAndFlower.Service.Service
{
    public class PayOSService : IPayOSService
    {
        private readonly PayOSConfig _config;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartItemRepository;
        private readonly PayOS _payOS;

        public PayOSService(IOptions<PayOSConfig> config,
                            IOrderRepository orderRepository,
                            ICartRepository cartItemRepository)
        {
            _config = config.Value;

            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;

            _payOS = new PayOS(_config.ClientId, _config.ApiKey, _config.ChecksumKey);
        }

        public async Task<string> CreatePaymentUrlAsync(int orderId, decimal totalAmount, HttpContext context)
        {
            var order = await _orderRepository.GetOrderWithUserAsync(orderId);
            if (order == null || order.User == null)
                throw new Exception("Không tìm thấy đơn hàng hoặc người dùng");

            int amountInVND = (int)(totalAmount);

            var cartItems = await _cartItemRepository
                .GetAll()
                .Include(c => c.Product)
                .Where(c => c.UserId == order.UserId && c.Product != null)
                .ToListAsync();

            var items = cartItems.Select(ci => new ItemData(
                name: ci.Product.Name,
                quantity: ci.Quantity,
                price: (int)(ci.Product.Price)
            )).ToList();

            int generatedOrderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
            order.PayOSOrderCode = generatedOrderCode;
            await _orderRepository.SaveChangesAsync();

            var paymentData = new PaymentData(
                orderCode: generatedOrderCode,
                amount: amountInVND,
                description: $"Thanh toán đơn hàng #{orderId}",
                items: items,
                cancelUrl: _config.CancelUrl,
                returnUrl: _config.ReturnUrl,
                expiredAt: DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds(),
                buyerName: order.User.FullName,
                buyerEmail: order.User.Email,
                buyerPhone: order.User.Phone,
                buyerAddress: order.User.Address ?? "Việt Nam"
            );

            var result = await _payOS.createPaymentLink(paymentData);

            return result.checkoutUrl;
        }

        public PayOSResponseModel GetReturnData(IQueryCollection query)
        {
            var orderCodeStr = query["orderCode"];
            var status = query["status"];
            int.TryParse(orderCodeStr, out int payOSOrderCode);
            bool success = status.ToString().ToLower() is "success" or "paid";

            return new PayOSResponseModel
            {
                PayOSOrderCode = payOSOrderCode,
                Success = success
            };
        }

    }
}
