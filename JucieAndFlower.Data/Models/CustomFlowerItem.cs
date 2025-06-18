using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Models
{
    public class CustomFlowerItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomFlowerItemId { get; set; }

        [Required]
        public int FlowerComponentId { get; set; } 

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual CartItem? CartItem { get; set; }

        [ForeignKey("FlowerComponentId")]
        public virtual FlowerComponent Component { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

}
