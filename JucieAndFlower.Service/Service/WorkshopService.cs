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
    public class WorkshopService : IWorkshopService
    {
        private readonly IWorkshopRepository _repo;

        public WorkshopService(IWorkshopRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Workshop>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Workshop?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<Workshop> CreateAsync(WorkshopDTO dto)
        {
            var entity = new Workshop
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                EventDate = dto.EventDate,
                MaxAttendees = dto.MaxAttendees,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                CreatedAt = DateTime.Now
            };
            return await _repo.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(int id, WorkshopDTO dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Location = dto.Location;
            existing.EventDate = dto.EventDate;
            existing.MaxAttendees = dto.MaxAttendees;
            existing.Price = dto.Price;
            existing.ImageUrl = dto.ImageUrl;

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
