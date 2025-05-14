using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public interface IProductDetailRepository
    {
        Task<List<ProductDetail>> GetAllAsync();
        Task<ProductDetail?> GetByIdAsync(int id);
        Task<ProductDetail> AddAsync(ProductDetail entity);
        Task UpdateAsync(ProductDetail entity);
        Task DeleteAsync(ProductDetail entity);
    }
}
