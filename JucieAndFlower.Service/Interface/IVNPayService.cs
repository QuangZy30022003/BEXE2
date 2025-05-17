using JucieAndFlower.Data.Enities.VnPay;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Service.Interface
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(int orderId, decimal totalAmount, HttpContext context);
        VnPayResponseModel GetReturnData(IQueryCollection query); // thêm dòng này
    }
}
