using JucieAndFlower.Data.Enities.Product;
using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(ProductCreateUpdateDto dto);
        Task<bool> UpdateAsync(int id, ProductCreateUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }

}
