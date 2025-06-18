using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Repositories
{
    public interface IPaymentRepository
    {
        Task AddPaymentAsync(Payment payment);
        Task SaveChangesAsync();
        Task<List<Payment>> GetAllAsync();
        Task<List<Payment>> GetByUserIdAsync(int userId);

        Task<Payment?> GetPaymentByOrderIdAsync(int orderId);
    }

}
