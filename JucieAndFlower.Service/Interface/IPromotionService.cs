using JucieAndFlower.Data.Enities.Promotion;
using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IPromotionService
    {
        Task<List<PromotionDTO>> GetAllAsync();
        Task<PromotionDTO?> GetByIdAsync(int id);
        Task<PromotionDTO> CreateAsync(PromotionCreateUpdateDTO dto);
        Task UpdateAsync(int id, PromotionCreateUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<Promotion?> GetValidPromotionByCodeAsync(string code);
    }

}
