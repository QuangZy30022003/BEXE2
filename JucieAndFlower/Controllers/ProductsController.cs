using JucieAndFlower.Data.Enities.Product;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JucieAndFlower.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        public ProductsController(IProductService productService, IProductImageService productImageService)
        {
            _productService = productService;
            _productImageService = productImageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }


        [Authorize(Roles = "2,4")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateUpdateDto product)
        {
            await _productService.CreateAsync(product);
            return Ok(product);
        }


        [Authorize(Roles = "2,4")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductCreateUpdateDto product)
        {
            var result = await _productService.UpdateAsync(id, product);
            if (!result) return NotFound();
            return Ok();
        }
        [Authorize(Roles = "2,4")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result) return NotFound();
            return Ok();
        }


        [HttpGet("categoryId")]
        public async Task<IActionResult> GetByCategoryId(int categoryId)
        {
            var products = await _productService.GetByCategoryIdAsync(categoryId);
            return Ok(new { Success = true, Data = products });
        }


        [HttpPost("{productId}/images")]
        [Authorize(Roles = "2,4")]
        public async Task<IActionResult> UploadImage(int productId, IFormFile file)
        {
            try
            {
                var imageUrl = await _productImageService.UploadImageAsync(productId, file);
                return Ok(new { success = true, imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
