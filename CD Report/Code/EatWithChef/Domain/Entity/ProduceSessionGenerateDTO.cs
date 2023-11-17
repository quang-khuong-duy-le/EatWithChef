using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ProduceSessionGenerateDTO
    {
        public int DishID { get; set; }
        public int Quantity { get; set; }
        public string IngredientsString { get; set; }
    }
}
