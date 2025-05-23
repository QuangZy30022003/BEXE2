using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.Promotion
{
    public class PromotionDTO
    {
        public int PromotionId { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? DiscountPercent { get; set; }
        public decimal? MaxDiscount { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }

}
