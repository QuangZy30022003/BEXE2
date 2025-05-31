using JucieAndFlower.Data.Enities.Feedback;
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
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repository;

        public FeedbackService(IFeedbackRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Feedback?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Feedback> CreateAsync(int userId, FeedbackCreateDto dto)
        {
            if ((dto.ProductId == null && dto.WorkshopId == null) ||
                (dto.ProductId != null && dto.WorkshopId != null))
            {
                throw new ArgumentException("Chỉ được chọn 1 trong ProductId hoặc WorkshopId.");
            }

            var feedback = new Feedback
            {
                UserId = userId,
                ProductId = dto.ProductId,
                WorkshopId = dto.WorkshopId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(feedback);
            await _repository.SaveChangesAsync();

            return feedback;
        }

        public async Task<bool> UpdateAsync(int id, FeedbackUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Rating = dto.Rating;
            existing.Comment = dto.Comment;

            await _repository.UpdateAsync(existing);
            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(existing);
            return await _repository.SaveChangesAsync();
        }
    }

}
