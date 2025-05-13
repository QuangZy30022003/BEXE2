using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JucieAndFlower.Data.Models;

public partial class WorkshopTicket
{
    [Key]
    public int TicketId { get; set; }

    public int? WorkshopId { get; set; }

    [StringLength(50)]
    public string? TicketType { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    [InverseProperty("Ticket")]
    public virtual ICollection<UserWorkshopTicket> UserWorkshopTickets { get; set; } = new List<UserWorkshopTicket>();

    [ForeignKey("WorkshopId")]
    [InverseProperty("WorkshopTickets")]
    public virtual Workshop? Workshop { get; set; }
}
