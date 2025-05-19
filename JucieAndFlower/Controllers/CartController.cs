using JucieAndFlower.Data.Enities.Cart;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JucieAndFlower.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID claim not found"));
            var result = await _cartService.GetUserCartAsync(userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCartItem( [FromBody] CartItemCreateDTO dto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID claim not found"));
            await _cartService.AddOrUpdateCartItemAsync(userId, dto);
            return Ok(new { message = "Item added/updated in cart." });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteItem(int productId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("User ID claim not found"));
            await _cartService.RemoveCartItemAsync(userId, productId);
            return Ok(new { message = "Item removed from cart." });
        }

    }
}
