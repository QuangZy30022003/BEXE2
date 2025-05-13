using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JucieAndFlower.Data.Models;

[Index("Code", Name = "UQ__Promotio__A25C5AA770A710DC", IsUnique = true)]
public partial class Promotion
{
    [Key]
    public int PromotionId { get; set; }

    [StringLength(50)]
    public string? Code { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    public int? DiscountPercent { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? MaxDiscount { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? IsActive { get; set; }
}
