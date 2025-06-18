using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Models
{
    public class FlowerComponent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [StringLength(50)]
        public string? Unit { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        public virtual ICollection<CustomFlowerItem> CustomFlowerItems { get; set; } = new List<CustomFlowerItem>();
}
}
