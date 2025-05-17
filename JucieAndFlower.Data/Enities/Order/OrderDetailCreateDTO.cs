using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.Order
{
    public class OrderDetailCreateDTO
    {
        public int ProductId { get; set; }
        public int? ProductDetailId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
