using JucieAndFlower.Data.Enities.VnPay;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IPayOSService
    {
        Task<string> CreatePaymentUrlAsync(int orderId, decimal totalAmount, HttpContext context);
        PayOSResponseModel GetReturnData(IQueryCollection query);
    }
}
