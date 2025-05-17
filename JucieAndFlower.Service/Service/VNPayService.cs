using JucieAndFlower.Data.Enities.VnPay;
using JucieAndFlower.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPAY_CS_ASPX;

namespace JucieAndFlower.Service.Service
{
    public class VNPayService : IVNPayService
    {
        private readonly VnPayConfig _vnPayConfig;
        public VNPayService(IOptions<VnPayConfig> vnPayConfig)
        {
            _vnPayConfig = vnPayConfig.Value ?? throw new ArgumentNullException(nameof(vnPayConfig));
        }
        public string CreatePaymentUrl(int orderId, decimal totalAmount, HttpContext context)
        {
            if (_vnPayConfig == null)
                throw new Exception("VnPayConfig is not configured properly.");

            var vnpay = new VnPayLibrary();

            string createDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            string txnRef = $"{orderId}{createDate}"; // Không có ký tự đặc biệt

            vnpay.AddRequestData("vnp_Version", _vnPayConfig.Version);
            vnpay.AddRequestData("vnp_Command", _vnPayConfig.Command);
            vnpay.AddRequestData("vnp_TmnCode", _vnPayConfig.TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((int)(totalAmount * 100)).ToString()); // Nhân 100 vì đơn vị là đồng
            vnpay.AddRequestData("vnp_CurrCode", _vnPayConfig.CurrCode);
            vnpay.AddRequestData("vnp_Locale", _vnPayConfig.Locale);
            vnpay.AddRequestData("vnp_ReturnUrl", _vnPayConfig.ReturnUrl);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_CreateDate", createDate); // ❗ Bắt buộc

            vnpay.AddRequestData("vnp_TxnRef", txnRef);
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang #{orderId}");
            vnpay.AddRequestData("vnp_OrderType", "other");

            // Tạo URL thanh toán
            string paymentUrl = vnpay.CreateRequestUrl(_vnPayConfig.BaseUrl, _vnPayConfig.HashSecret);

            return paymentUrl;
        }


        public VnPayResponseModel GetReturnData(IQueryCollection query)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in query)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value);
                }
            }

            var txnRef = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");

            var checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _vnPayConfig.HashSecret);

            int orderId = 0;

            // ✅ Parse OrderId từ TxnRef (dạng orderId-yyyyMMddHHmmss)
            if (!string.IsNullOrEmpty(txnRef))
            {
                var parts = txnRef.Split('-');
                if (parts.Length > 0 && int.TryParse(parts[0], out int parsedOrderId))
                {
                    orderId = parsedOrderId;
                }
            }

            return new VnPayResponseModel
            {
                OrderId = orderId,
                Success = checkSignature && vnp_ResponseCode == "00"
            };
        }
    }
}