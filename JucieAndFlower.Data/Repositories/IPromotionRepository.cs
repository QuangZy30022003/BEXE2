using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public interface IPromotionRepository
    {
        Task<List<Promotion>> GetAllAsync();
        Task<Promotion?> GetByIdAsync(int id);
        Task<Promotion?> GetByCodeAsync(string code);
        Task<Promotion> AddAsync(Promotion promotion);
        Task UpdateAsync(Promotion promotion);
        Task DeleteAsync(Promotion promotion);
        Task SaveChangesAsync();
    }

}
