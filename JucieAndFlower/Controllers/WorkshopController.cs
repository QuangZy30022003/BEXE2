using JucieAndFlower.Data.Enities.Worshop;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JucieAndFlower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopController : Controller
    {
        private readonly IWorkshopService _service;

        public WorkshopController(IWorkshopService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [Authorize(Roles = "2")] // RoleId = 2
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkshopDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.WorkshopId }, created);
        }

        [Authorize(Roles = "2")] // RoleId = 2
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WorkshopDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [Authorize(Roles = "2")] // RoleId = 2
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }


    }
}
