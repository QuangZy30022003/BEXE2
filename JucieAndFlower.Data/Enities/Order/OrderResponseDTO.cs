using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.Order
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? FinalAmount { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? PromotionCode { get; set; }
        public List<OrderDetailResponseDTO> OrderDetails { get; set; } = new();
    }

    public class OrderDetailResponseDTO
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? ProductDetailId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public ProductResponseDTO? Product { get; set; }
    }

    public class ProductResponseDTO
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsAvailable { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
