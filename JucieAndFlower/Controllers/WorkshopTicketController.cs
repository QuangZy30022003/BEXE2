using JucieAndFlower.Data.Enities.Worshop;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JucieAndFlower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopTicketController : Controller
    {
        private readonly IWorkshopTicketService _service;

        public WorkshopTicketController(IWorkshopTicketService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ticket = await _service.GetByIdAsync(id);
            return ticket == null ? NotFound() : Ok(ticket);
        }

        [HttpPost]
        [Authorize(Roles = "2")] 
        public async Task<IActionResult> Create(WorkshopTicketDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.TicketId }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Update(int id, WorkshopTicketDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result ? Ok(dto) : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
