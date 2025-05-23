using JucieAndFlower.Data.Enities.Promotion;
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
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _repo;

        public PromotionService(IPromotionRepository repo)
        {
            _repo = repo;
        }
    
        public async Task<PromotionDTO> CreateAsync(PromotionCreateUpdateDTO dto)
        {
            var p = new Promotion
            {
                Code = dto.Code,
                Description = dto.Description,
                DiscountPercent = dto.DiscountPercent,
                MaxDiscount = dto.MaxDiscount,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive
            };
            var result = await _repo.AddAsync(p);
            return await GetByIdAsync(result.PromotionId) ?? throw new Exception("Fail To creat"); 
        }

        public async Task DeleteAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) throw new Exception("Not Found");
            await _repo.DeleteAsync(p);
        }

        public async Task<List<PromotionDTO>> GetAllAsync()
        {
           var list  = await _repo.GetAllAsync();
            return list.Select(p => new PromotionDTO
            {
                PromotionId = p.PromotionId,
                Code = p.Code,
                Description = p.Description,
                DiscountPercent = p.DiscountPercent,
                MaxDiscount = p.MaxDiscount,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                IsActive = p.IsActive
            }).ToList();
        }

        public async Task<PromotionDTO?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;
            return new PromotionDTO
            {
                PromotionId = p.PromotionId,
                Code = p.Code,
                Description = p.Description,
                DiscountPercent = p.DiscountPercent,
                MaxDiscount = p.MaxDiscount,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                IsActive = p.IsActive
            };
        }

        public async Task<Promotion?> GetValidPromotionByCodeAsync(string code)
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            var p = await _repo.GetByCodeAsync(code);
            if (p != null && p.IsActive == true && p.StartDate <= now && p.EndDate >= now)
            {
                return p;
            }
            return null;
        }

        public async Task UpdateAsync(int id, PromotionCreateUpdateDTO dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) throw new Exception("Promotion not found");

            existing.Code = dto.Code;
            existing.Description = dto.Description;
            existing.DiscountPercent = dto.DiscountPercent;
            existing.MaxDiscount = dto.MaxDiscount;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.IsActive = dto.IsActive;

            await _repo.UpdateAsync(existing);
        }
    }
}
