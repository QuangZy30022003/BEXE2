//using JucieAndFlower.Data.Enities.CusFlower;
//using JucieAndFlower.Data.Models;
//using JucieAndFlower.Service.Interface;
//using Microsoft.AspNetCore.Mvc;

//namespace JucieAndFlower.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CustomFlowerItemController : Controller
//    {
//        private readonly ICustomFlowerItemService _service;

//        public CustomFlowerItemController(ICustomFlowerItemService service)
//        {
//            _service = service;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<CustomFlowerItem>>> GetAll()
//        {
//            var items = await _service.GetAllAsync();
//            return Ok(items);
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<CustomFlowerItem>> GetById(int id)
//        {
//            var item = await _service.GetByIdAsync(id);
//            if (item == null) return NotFound();

//            return Ok(item);
//        }


//        [HttpPost]
//        public async Task<ActionResult> Create([FromBody] CreateCustomFlowerItemDto dto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var createdItem = await _service.AddAsync(dto);

//            return CreatedAtAction(nameof(GetById), new { id = createdItem.CustomItemID }, createdItem);
//        }


//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomFlowerItemDto dto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var success = await _service.UpdateAsync(id, dto);
//            if (!success)
//                return NotFound();

//            return NoContent();
//        }


//        [HttpDelete("{id}")]
//        public async Task<ActionResult> Delete(int id)
//        {
//            var deleted = await _service.DeleteAsync(id);
//            return deleted ? NoContent() : NotFound();
//        }
//    }
//}
