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
        public int Id { get; set; }

        [Required]
        public int CartItemId { get; set; }

        [Required]
        public int ComponentId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [ForeignKey("CartItemId")]
        public virtual CartItem CartItem { get; set; } = null!;

        [ForeignKey("ComponentId")]
        public virtual FlowerComponent Component { get; set; } = null!;
    }

}
