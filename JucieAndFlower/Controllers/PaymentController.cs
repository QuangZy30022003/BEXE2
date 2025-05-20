using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JucieAndFlower.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPayments()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { Success = false, Message = "Unauthorized" });
            }

            if (role == "4")
            {
                var allPayments = await _paymentService.GetAllPaymentsAsync();
                return Ok(new { Success = true, Data = allPayments });
            }
            else
            {
                var userPayments = await _paymentService.GetPaymentsByUserIdAsync(userId);
                return Ok(new { Success = true, Data = userPayments });
            }
        }

    }
}
