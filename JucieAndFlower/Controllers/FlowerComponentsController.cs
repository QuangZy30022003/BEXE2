using JucieAndFlower.Data.Enities.CusFlower;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JucieAndFlower.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlowerComponentsController : ControllerBase
    {
        private readonly IFlowerComponentService _service;

        public FlowerComponentsController(IFlowerComponentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FlowerComponentDto dto)
        {
            await _service.AddAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FlowerComponentDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
