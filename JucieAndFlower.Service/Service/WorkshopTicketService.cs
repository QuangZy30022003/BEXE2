using JucieAndFlower.Data.Enities.Worshop;
using JucieAndFlower.Data.Models;
using JucieAndFlower.Data.Repositories;
using JucieAndFlower.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Service
{
    public class WorkshopTicketService : IWorkshopTicketService
    {
        private readonly IWorkshopTicketRepository _repo;

        public WorkshopTicketService(IWorkshopTicketRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<WorkshopTicket>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<WorkshopTicket?> GetByIdAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<WorkshopTicket> CreateAsync(WorkshopTicketDTO dto)
        {
            var entity = new WorkshopTicket
            {
                WorkshopId = dto.WorkshopId,
                TicketType = dto.TicketType,
                Price = dto.Price,
                Quantity = dto.Quantity
            };
            return await _repo.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(int id, WorkshopTicketDTO dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.WorkshopId = dto.WorkshopId;
            existing.TicketType = dto.TicketType;
            existing.Price = dto.Price;
            existing.Quantity = dto.Quantity;

            await _repo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }
    }

}
