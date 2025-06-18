using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.VnPay
{
    public class PayOSConfig
    {
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string ChecksumKey { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }


    public class PayOSResponseModel
    {
        public int PayOSOrderCode { get; set; }  // ✅ Đây là cái bạn lấy từ PayOS
        public bool Success { get; set; }
    }


    public class PayOSResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("data")]
        public PayOSData? Data { get; set; }
    }

    public class PayOSData
    {
        [JsonProperty("checkoutUrl")]
        public string CheckoutUrl { get; set; }

    }

}
