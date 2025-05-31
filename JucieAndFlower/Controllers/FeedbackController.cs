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
            var result = await _feedbackService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _feedbackService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
    }
