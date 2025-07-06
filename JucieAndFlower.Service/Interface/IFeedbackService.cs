using JucieAndFlower.Data.Enities.Feedback;
using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackDto>> GetAllAsync();
        Task<FeedbackDto?> GetByIdAsync(int id);
        Task<Feedback> CreateAsync(int userId, FeedbackCreateDto dto);
        Task<bool> UpdateAsync(int id, FeedbackUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }

}
