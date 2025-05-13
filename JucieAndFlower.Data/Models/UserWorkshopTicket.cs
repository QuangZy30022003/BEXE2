using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JucieAndFlower.Data.Models;

public partial class UserWorkshopTicket
{
    [Key]
    public int UserTicketId { get; set; }

    public int? UserId { get; set; }

    public int? TicketId { get; set; }

    public int? Quantity { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PurchaseDate { get; set; }

    [ForeignKey("TicketId")]
    [InverseProperty("UserWorkshopTickets")]
    public virtual WorkshopTicket? Ticket { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserWorkshopTickets")]
    public virtual User? User { get; set; }
}
