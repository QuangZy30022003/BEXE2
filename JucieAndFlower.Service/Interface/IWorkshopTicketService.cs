using JucieAndFlower.Data.Enities.Worshop;
using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IWorkshopTicketService
    {
        Task<IEnumerable<WorkshopTicket>> GetAllAsync();
        Task<WorkshopTicket?> GetByIdAsync(int id);
        Task<WorkshopTicket> CreateAsync(WorkshopTicketDTO dto);
        Task<bool> UpdateAsync(int id, WorkshopTicketDTO dto);
        Task<bool> DeleteAsync(int id);
    }

}
