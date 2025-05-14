using JucieAndFlower.Data.Enities.ProductDetails;
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
    public class ProductDetailService : IProductDetailService
    {
        private readonly IProductDetailRepository _repo;

        public ProductDetailService(IProductDetailRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ProductDetailDTO>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(x => new ProductDetailDTO
            {
                ProductDetailId = x.ProductDetailId,
                ProductId = x.ProductId,
                Size = x.Size,
                Color = x.Color,
                FlowerType = x.FlowerType,
                ExtraPrice = x.ExtraPrice
            }).ToList();
        }

        public async Task<ProductDetailDTO?> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return null;

            return new ProductDetailDTO
            {
                ProductDetailId = x.ProductDetailId,
                ProductId = x.ProductId,
                Size = x.Size,
                Color = x.Color,
                FlowerType = x.FlowerType,
                ExtraPrice = x.ExtraPrice
            };
        }

        public async Task<ProductDetailDTO> AddAsync(ProductDetailCreateDTO dto)
        {
            var entity = new ProductDetail
            {
                ProductId = dto.ProductId,
                Size = dto.Size,
                Color = dto.Color,
                FlowerType = dto.FlowerType,
                ExtraPrice = dto.ExtraPrice
            };
            var added = await _repo.AddAsync(entity);
            return new ProductDetailDTO
            {
                ProductDetailId = added.ProductDetailId,
                ProductId = added.ProductId,
                Size = added.Size,
                Color = added.Color,
                FlowerType = added.FlowerType,
                ExtraPrice = added.ExtraPrice
            };
        }

        public async Task<ProductDetailDTO?> UpdateAsync(int id, ProductDetailCreateDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            entity.ProductId = dto.ProductId;
            entity.Size = dto.Size;
            entity.Color = dto.Color;
            entity.FlowerType = dto.FlowerType;
            entity.ExtraPrice = dto.ExtraPrice;

            await _repo.UpdateAsync(entity);

            return new ProductDetailDTO
            {
                ProductDetailId = entity.ProductDetailId,
                ProductId = entity.ProductId,
                Size = entity.Size,
                Color = entity.Color,
                FlowerType = entity.FlowerType,
                ExtraPrice = entity.ExtraPrice
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            await _repo.DeleteAsync(entity);
            return true;
        }
    }
}
