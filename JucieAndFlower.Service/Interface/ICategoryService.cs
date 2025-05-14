using JucieAndFlower.Data.Enities.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO?> GetByIdAsync(int id);
        Task<CategoryNoID> AddAsync(CategoryNoID dto);
        Task<CategoryDTO?> UpdateAsync(int id, CategoryNoID dto);
        Task<bool> DeleteAsync(int id);
    }

}
