using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JucieAndFlower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   //[Authorize(Roles = "4")] 
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueService _revenueService;

        public RevenueController(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyRevenue([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                startDate ??= DateTime.Now.AddDays(-30);
                endDate ??= DateTime.Now;

                var result = await _revenueService.GetDailyRevenueAsync(startDate.Value, endDate.Value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyRevenue([FromQuery] int? year = null)
        {
            try
            {
                year ??= DateTime.Now.Year;
                var result = await _revenueService.GetMonthlyRevenueAsync(year.Value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetRevenueStats()
        {
            try
            {
                var result = await _revenueService.GetRevenueStatsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopProducts([FromQuery] int limit = 10, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                startDate ??= DateTime.Now.AddDays(-30);
                endDate ??= DateTime.Now;

                var result = await _revenueService.GetTopProductsAsync(limit, startDate.Value, endDate.Value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("order-status-stats")]
        public async Task<IActionResult> GetOrderStatusStats([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                startDate ??= DateTime.Now.AddDays(-30);
                endDate ??= DateTime.Now;

                var result = await _revenueService.GetOrderStatusStatsAsync(startDate.Value, endDate.Value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
