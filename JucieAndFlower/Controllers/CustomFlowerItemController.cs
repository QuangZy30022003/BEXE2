using JucieAndFlower.Data.Enities.CusFlower;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JucieAndFlower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomFlowerItemController : Controller
    {
        private readonly ICustomFlowerItemService _service;

        public CustomFlowerItemController(ICustomFlowerItemService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomItem([FromBody] CustomFlowerItemCreateDTO dto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("User ID not found."));

            var result = await _service.CreateCustomItemAsync(userId, dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCustomItems()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("User ID not found."));

            var items = await _service.GetUserCustomItemsAsync(userId);
            return Ok(items);
        }
    }
}
