using JucieAndFlower.Data.Enities.Order;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JucieAndFlower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IVNPayService _vnpayService;

        public OrdersController(IOrderService orderService, IVNPayService vnpayService)
        {
            _orderService = orderService;
            _vnpayService = vnpayService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO dto)
        {
            var order = await _orderService.CreateOrderAsync(dto);
            decimal totalAmount = (decimal)order.TotalAmount;
            var paymentUrl = _vnpayService.CreatePaymentUrl(order.OrderId, totalAmount, HttpContext);

            return Ok(new
            {
                order.OrderId,
                PaymentUrl = paymentUrl
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // Dùng cho testing API chứ không redirect nữa
        [HttpGet("payment-return")]
        public async Task<IActionResult> PaymentReturn()
        {
            var response = _vnpayService.GetReturnData(Request.Query);

            if (response.Success)
            {
                await _orderService.MarkOrderAsCompleteAsync(response.OrderId);
                return Ok(new { Success = true, OrderId = response.OrderId });
            }

            return BadRequest(new { Success = false, OrderId = response.OrderId });
        }

    }
}
