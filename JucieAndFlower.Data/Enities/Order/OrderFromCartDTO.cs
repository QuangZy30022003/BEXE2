using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.Order
{
    public class OrderFromCartDTO
    {
        public string? DeliveryAddress { get; set; }
        public string? PromotionCode { get; set; }
        public string? Note { get; set; }
        public List<int> SelectedCartItemIds { get; set; } = new();
    }

}
