using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.CusFlower
{
    public class CreateCustomFlowerItemDto
    {
        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public string Color { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PricePerUnit { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public bool Status { get; set; } = true; 
    }

    public class UpdateCustomFlowerItemDto
    {
        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public string Color { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PricePerUnit { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public bool Status { get; set; }
    }
}
