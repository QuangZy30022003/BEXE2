using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.Worshop
{
    public class WorkshopDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime? EventDate { get; set; }
        public int? MaxAttendees { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
    }

}
