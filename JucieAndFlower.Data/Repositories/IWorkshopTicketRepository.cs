using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public interface IWorkshopTicketRepository
    {
        Task<IEnumerable<WorkshopTicket>> GetAllAsync();
        Task<WorkshopTicket?> GetByIdAsync(int id);
        Task<WorkshopTicket> AddAsync(WorkshopTicket entity);
        Task UpdateAsync(WorkshopTicket entity);
        Task DeleteAsync(int id);
    }

}
