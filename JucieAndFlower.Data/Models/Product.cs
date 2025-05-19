using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JucieAndFlower.Data.Models;

public partial class Product
{
    [Key]
    public int ProductId { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price { get; set; }

    [StringLength(255)]
    public string? ImageUrl { get; set; }


    public int? CategoryId { get; set; }

    public bool? IsAvailable { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

}
