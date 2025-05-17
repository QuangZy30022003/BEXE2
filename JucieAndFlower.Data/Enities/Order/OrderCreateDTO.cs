using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.Order
{
    public class OrderCreateDTO
    {
        public int UserId { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? PromotionCode { get; set; }
        public string? Note { get; set; }
        public List<OrderDetailCreateDTO> OrderDetails { get; set; } = new();
    }

}
