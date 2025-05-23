using JucieAndFlower.Data.Enities.Promotion;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JucieAndFlower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : Controller
    {
        private readonly IPromotionService _service;

        public PromotionController(IPromotionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "2")] // Only Staff
        public async Task<IActionResult> GetAll() =>
       Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Get(int id)
        {
            var promo = await _service.GetByIdAsync(id);
            return promo == null ? NotFound() : Ok(promo);
        }

        [HttpPost]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Create([FromBody] PromotionCreateUpdateDTO dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Update(int id, [FromBody] PromotionCreateUpdateDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
