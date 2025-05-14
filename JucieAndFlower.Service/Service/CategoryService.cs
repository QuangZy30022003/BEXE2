using JucieAndFlower.Data.Enities.Categories;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();
            return categories.Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<CategoryNoID> AddAsync(CategoryNoID dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var added = await _repo.AddAsync(category);

            return new CategoryNoID
            {
                Name = added.Name,
                Description = added.Description
            };
        }


        public async Task<CategoryDTO?> UpdateAsync(int id, CategoryNoID dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return null;

            entity.Name = dto.Name;
            entity.Description = dto.Description;

            await _repo.UpdateAsync(entity);

            return new CategoryDTO
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }

}
