using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.ProductDetails
{
    public class ProductDetailCreateDTO
    {
        public int? ProductId { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? FlowerType { get; set; }
        public decimal? ExtraPrice { get; set; }
    }
    
}
