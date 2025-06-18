using JucieAndFlower.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IPaymentService
    {
        Task AddPaymentAsync(Payment payment);

        Task<List<Payment>> GetAllPaymentsAsync();

        Task<List<Payment>> GetPaymentsByUserIdAsync(int userId);

        Task<Payment?> GetPaymentByOrderIdAsync(int orderId);

    }

}
