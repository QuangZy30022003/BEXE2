using JucieAndFlower.Data.Enities.Order;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Service.Interface;
using JucieAndFlower.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JucieAndFlower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IVNPayService _vnpayService;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        private readonly IPayOSService _payOSService;
        public OrdersController(IOrderService orderService, IVNPayService vnpayService, IPaymentService paymentService, IEmailService emailService, IPayOSService payOSService)
        {
            _orderService = orderService;
            _vnpayService = vnpayService;
            _paymentService = paymentService;
            _emailService = emailService;
            _payOSService = payOSService;
        }

     

        [HttpGet]
        [Authorize(Roles = "4")] 
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { Success = false, Message = "Order not found" });
                }

                return Ok(new { Success = true, Data = order });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting order by ID: {ex.Message}");
                return StatusCode(500, new { Success = false, Message = "Internal server error" });
            }
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserOrders()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { Success = false, Message = "User ID claim not found or invalid" });
                }

                var orders = await _orderService.GetOrdersByUserIdAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user orders: {ex.Message}");
                return StatusCode(500, new { Success = false, Message = "Internal server error" });
            }
        }

        [HttpGet("payment-return")]
        [AllowAnonymous]
        public async Task<IActionResult> PaymentReturn()
        {
            try
            {
                var response = _payOSService.GetReturnData(Request.Query);

                // ✅ Tìm đơn hàng theo PayOSOrderCode chứ không phải OrderId
                var order = await _orderService.GetOrderByPayOSOrderCodeAsync(response.PayOSOrderCode);
                if (order == null)
                    if (order == null)
                {
                    return Redirect($"http://localhost:5173/payment-result?success=false&message=Order not found&orderId=0");
                }

                // Tránh tạo nhiều Payment nếu reload
                var existingPayment = await _paymentService.GetPaymentByOrderIdAsync(order.OrderId);
                if (existingPayment == null)
                {
                    var payment = new Payment
                    {
                        OrderId = order.OrderId,
                        PaymentMethod = "PayOS",
                        PaidAmount = order.FinalAmount,
                        PaymentDate = DateTime.Now,
                        Status = response.Success ? "Paid" : "Cancel"
                    };
                    await _paymentService.AddPaymentAsync(payment);
                }

                // Cập nhật trạng thái đơn hàng
                if (order.Status == "Pending")
                {
                    if (response.Success)
                    {
                        await _orderService.MarkOrderAsCompleteAsync(order.OrderId);
                        return Redirect($"http://localhost:5173/payment-result?success=true&orderId={order.OrderId}&amount={order.FinalAmount}&message=Payment successful");
                    }
                    else
                    {
                        await _orderService.MarkOrderAsCanceledAsync(order.OrderId);
                        return Redirect($"http://localhost:5173/payment-result?success=false&orderId={order.OrderId}&message=Payment failed or canceled");
                    }
                }

                // Nếu đơn hàng đã xử lý rồi
                return Redirect($"http://localhost:5173/payment-result?success=true&orderId={order.OrderId}&message=Order already processed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PaymentReturn: {ex.Message}");
                return Redirect("http://localhost:5173/payment-result?success=false&message=Payment processing error");
            }
        }



        [HttpPost("from-cart")]
        public async Task<IActionResult> CreateOrderFromCart([FromBody] OrderFromCartDTO dto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID claim not found"));
            dto.UserId = userId;

            var order = await _orderService.CreateOrderFromCartAsync(dto);
            decimal totalAmount = (decimal)order.FinalAmount;

            // Gọi PayOS thay vì VNPay
            var paymentUrl = await _payOSService.CreatePaymentUrlAsync(order.OrderId, totalAmount, HttpContext);
            string email = User.FindFirst(ClaimTypes.Email)?.Value ?? "default@example.com";
            await _emailService.SendOrderInvoiceEmailAsync(email, order);

            return Ok(new
            {
                order.OrderId,
                PaymentUrl = paymentUrl
            });
        }


    }
}
