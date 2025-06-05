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

        public ProductsController(IProductService productService)
        {
            _productService = productService;
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
    }
}
