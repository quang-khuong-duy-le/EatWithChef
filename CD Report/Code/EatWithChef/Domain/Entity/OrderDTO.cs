using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Entity
{
    public class OrderDTO
    {
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverEmail { get; set; }
        public string Note { get; set; }
        public int[] ListDishID { get; set; }
        public int[] MenuID { get; set; }
        public int[] Quantity { get; set; }
    }

}
