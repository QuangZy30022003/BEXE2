using JucieAndFlower.Data.Enities.Product;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Data.Repositories;
using JucieAndFlower.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            return products.OrderByDescending(p => p.OnTop)
.ThenByDescending(p => p.CreatedAt)
.Select(p => new ProductDto
{
    ProductId = p.ProductId,
    Name = p.Name,
    Description = p.Description,
    Price = p.Price ?? 0,
    CategoryId = p.CategoryId ?? 0,
    IsAvailable = p.IsAvailable ?? true,
    CreatedAt = p.CreatedAt ?? DateTime.Now,
    ImageUrl = p.ImageUrl,
    Images = p.Images.Select(img => new ProductImageDto
    {
        Id = img.Id,
        ImageUrl = img.ImageUrl
    }).ToList()
}).ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;

            return new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price ?? 0,
                CategoryId = p.CategoryId ?? 0,
                IsAvailable = p.IsAvailable ?? true,
                CreatedAt = p.CreatedAt ?? DateTime.Now,
                ImageUrl = p.ImageUrl,
                Images = p.Images.Select(img => new ProductImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl
                }).ToList()
            };
        }

        public async Task<bool> CreateAsync(ProductCreateUpdateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                IsAvailable = dto.IsAvailable,
                OnTop = dto.OnTop,
                CreatedAt = DateTime.Now
            };

            await _repo.AddAsync(product);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, ProductCreateUpdateDto dto)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return false;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.ImageUrl = dto.ImageUrl;
            product.CategoryId = dto.CategoryId;
            product.IsAvailable = dto.IsAvailable;
            product.OnTop = dto.OnTop;
            await _repo.UpdateAsync(product);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return false;

            await _repo.DeleteAsync(product);
            return await _repo.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetByCategoryIdAsync(int categoryId)
        {
            var products = await _repo.GetByCategoryIdAsync(categoryId);
            return products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price ?? 0,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId ?? 0,
                IsAvailable = p.IsAvailable ?? true,
                CreatedAt = p.CreatedAt ?? DateTime.Now
            }).ToList();
        }

    }
}
