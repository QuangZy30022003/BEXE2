using JucieAndFlower.Data.Enities.Worshop;
using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IWorkshopService
    {
        Task<IEnumerable<Workshop>> GetAllAsync();
        Task<Workshop?> GetByIdAsync(int id);
        Task<Workshop> CreateAsync(WorkshopDTO dto);
        Task<bool> UpdateAsync(int id, WorkshopDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
