using JucieAndFlower.Data.Enities.Categories;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JucieAndFlower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        // POST: api/Category
        [HttpPost]
        [Authorize(Roles = "2")] // RoleId = 2
        public async Task<IActionResult> Create(CategoryNoID dto)
        {
            var created = await _service.AddAsync(new CategoryNoID
            {
                Name = dto.Name,
                Description = dto.Description
            });
            return Ok(created);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        [Authorize(Roles = "2")] // RoleId = 2
        public async Task<IActionResult> Update(int id, [FromBody] CategoryNoID dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "2")] // RoleId = 2
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
