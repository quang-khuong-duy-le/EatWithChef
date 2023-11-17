using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWC.Core.Entity
{
    public class MenuDTO
    {
        public int MenuID { get; set; }
        public List<int> ListDishID { get; set; }
        public List<int> ListQuantity { get; set; }

        public MenuDTO() {
            ListDishID = new List<int>();
            ListQuantity = new List<int>();
        }
    }
}
