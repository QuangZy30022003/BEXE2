using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JucieAndFlower.Data.Models;

public partial class Feedback
{
    [Key]
    public int FeedbackId { get; set; }

    public int UserId { get; set; }

    public int? ProductId { get; set; }

    public int? WorkshopId { get; set; }

    public int? Rating { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Feedbacks")]
    public virtual Product? Product { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Feedbacks")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("WorkshopId")]
    [InverseProperty("Feedbacks")]
    public virtual Workshop? Workshop { get; set; }
}
