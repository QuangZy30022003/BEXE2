using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.Product
{
    public class ProductDto : ProductCreateUpdateDto
    {
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
