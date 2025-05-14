using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.ProductDetails
{
    public class ProductDetailDTO
    {
        public int ProductDetailId { get; set; }
        public int? ProductId { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? FlowerType { get; set; }
        public decimal? ExtraPrice { get; set; }
    }

}
