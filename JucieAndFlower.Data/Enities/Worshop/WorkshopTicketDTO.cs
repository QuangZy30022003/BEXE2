using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.Worshop
{
    public class WorkshopTicketDTO
    {
        public int? WorkshopId { get; set; }
        public string? TicketType { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }

}
