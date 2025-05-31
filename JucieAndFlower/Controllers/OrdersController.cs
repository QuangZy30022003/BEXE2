using JucieAndFlower.Data.Enities.Order;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Service.Interface;
using JucieAndFlower.Service.Service;
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
        public OrdersController(IOrderService orderService, IVNPayService vnpayService, IPaymentService paymentService, IEmailService emailService)
        {
            _orderService = orderService;
            _vnpayService = vnpayService;
            _paymentService = paymentService;
            _emailService = emailService;
        }


        [HttpGet]
        public async Task<IActionResult> GetOrderById(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID claim not found"));
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // Dùng cho testing API chứ không redirect nữa
        [HttpGet("payment-return")]
        public async Task<IActionResult> PaymentReturn()
        {
            var response = _vnpayService.GetReturnData(Request.Query);
            Console.WriteLine("VNPay Response OrderId: " + response.OrderId);

            // Lấy thông tin đơn hàng
            var order = await _orderService.GetOrderByIdAsync(response.OrderId);
            if (order == null)
            {
                return NotFound(new { Success = false, Message = "Order not found." });
            }

            // Tạo bản ghi thanh toán
            var payment = new Payment
            {
                OrderId = order.OrderId,
                PaymentMethod = "VNPay",
                PaidAmount = order.FinalAmount,
                PaymentDate = DateTime.Now,
                Status = response.Success ? "Paid" : "Cancel"
            };

            await _paymentService.AddPaymentAsync(payment);
            if (!response.Success)
            {
                await _orderService.MarkOrderAsCanceledAsync(order.OrderId);
            }
            // Nếu thành công, cập nhật trạng thái đơn hàng
            if (response.Success)
            {
                await _orderService.MarkOrderAsCompleteAsync(order.OrderId);

                return Ok(new
                {
                    Success = true,
                    Message = "Payment successful",
                    OrderId = order.OrderId
                });
            }

            // Nếu thất bại, chỉ trả kết quả
            return BadRequest(new
            {
                Success = false,
                Message = "Payment failed or canceled",
                OrderId = order.OrderId
            });
        }

        [HttpPost("from-cart")]
        public async Task<IActionResult> CreateOrderFromCart([FromBody] OrderFromCartDTO dto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID claim not found"));
            dto.UserId = userId;
            var order = await _orderService.CreateOrderFromCartAsync(dto);
            decimal totalAmount = (decimal)order.FinalAmount;
            var paymentUrl = _vnpayService.CreatePaymentUrl(order.OrderId, totalAmount, HttpContext);

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
