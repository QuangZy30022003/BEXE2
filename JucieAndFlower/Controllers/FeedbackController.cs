using JucieAndFlower.Data.Enities.Feedback;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JucieAndFlower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var feedbacks = await _feedbackService.GetAllAsync();
            return Ok(feedbacks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var feedback = await _feedbackService.GetByIdAsync(id);
            if (feedback == null) return NotFound();
            return Ok(feedback);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeedbackCreateDto dto)
        {
            // Giả sử userId được lấy từ token hoặc tạm hardcoded (demo):
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                var feedback = await _feedbackService.CreateAsync(userId, dto);
                return CreatedAtAction(nameof(GetById), new { id = feedback.FeedbackId }, feedback);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FeedbackUpdateDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // 2. Lấy feedback hiện tại
            var existing = await _feedbackService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            // 3. Kiểm tra quyền: chỉ chủ feedback mới được update
            if (existing.UserId != userId)
                return Forbid();    // 403

            // 4. Thực hiện update
            var success = await _feedbackService.UpdateAsync(id, dto);
            if (!success)
                return StatusCode(500, "Update failed");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var existing = await _feedbackService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            if (existing.UserId != userId)
                return Forbid();

            var success = await _feedbackService.DeleteAsync(id);
            if (!success)
                return StatusCode(500, "Delete failed");

            return NoContent();
        }
    }
    }
