using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public interface IProductImageRepository
    {
        Task AddAsync(ProductImage image);
        Task<bool> SaveChangesAsync();
    }

}
