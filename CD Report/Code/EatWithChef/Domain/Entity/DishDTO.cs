using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class DishPriceDTO
    {
        public int DishID { get; set; }
        public int Price { get; set; }
        public string DishName { get; set; }
    }
}
