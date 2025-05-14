using JucieAndFlower.Data.Enities.ProductDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IProductDetailService
    {
        Task<List<ProductDetailDTO>> GetAllAsync();
        Task<ProductDetailDTO?> GetByIdAsync(int id);
        Task<ProductDetailDTO> AddAsync(ProductDetailCreateDTO dto);
        Task<ProductDetailDTO?> UpdateAsync(int id, ProductDetailCreateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
