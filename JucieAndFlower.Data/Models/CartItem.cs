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

    public int? ProductId { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; } = null!;


    public int? CustomFlowerItemId { get; set; }
    [ForeignKey("CustomFlowerItemId")]
    public virtual CustomFlowerItem? CustomFlowerItem { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

}
