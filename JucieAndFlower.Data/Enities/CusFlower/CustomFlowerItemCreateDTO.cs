using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.CusFlower
{
    public class CustomFlowerItemCreateDTO
    {
        public int FlowerComponentId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
    }

    public class CustomFlowerItemDTO
    {
        public int CustomFlowerItemId { get; set; }
        public int FlowerComponentId { get; set; }
        public string FlowerComponentName { get; set; } = "";
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }


}
