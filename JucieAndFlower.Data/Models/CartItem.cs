using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JucieAndFlower.Data.Models;

public partial class CartItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Required]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; } = null!;

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    public bool IsCustom { get; set; } = false; // ✅ Đánh dấu là bó hoa custom

    [InverseProperty("CartItem")]
    public virtual ICollection<CustomFlowerItem> CustomFlowerItems { get; set; } = new List<CustomFlowerItem>();
}
