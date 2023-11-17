//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EWC.Core.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            this.DishItems = new HashSet<DishItem>();
        }
    
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int DishID { get; set; }
    
        public virtual Dish Dish { get; set; }
        public virtual ICollection<DishItem> DishItems { get; set; }
        public virtual Order Order { get; set; }
    }
}
