using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JucieAndFlower.Data.Models;

public partial class ProductDetail
{
    [Key]
    public int ProductDetailId { get; set; }

    public int? ProductId { get; set; }

    [StringLength(20)]
    public string? Size { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    [StringLength(50)]
    public string? FlowerType { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? ExtraPrice { get; set; }

    [InverseProperty("ProductDetail")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("ProductId")]
    [InverseProperty("ProductDetails")]
    public virtual Product? Product { get; set; }
}
