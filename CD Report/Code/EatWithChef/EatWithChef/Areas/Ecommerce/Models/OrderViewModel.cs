using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entity;

namespace EatWithChef.Areas.Ecommerce.Models
{
    public class BillViewModel {
        public string BillCode { get; set; }
        public List<OrderViewModel> OrderView { get; set; }

        public BillViewModel() {
            OrderView = new List<OrderViewModel>();
        }
    }

    public class OrderViewModel {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int TotalPayment { get; set; }
        public int OrderStatus { get; set; }
        public int PaymentStatus { get; set; }
        public List<OrderDetailViewModel> OrderDetailView { get; set; }

        public OrderViewModel() {
            OrderDetailView = new List<OrderDetailViewModel>();
        }
    }

    public class OrderDetailViewModel {
        public int OrderDetailId { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string DishName { get; set; }
        public string DishUrl { get; set; }
        public string DishImageUrl { get; set; }
        public List<OrderDetailDishItem> OrderDetailDishItemModel { get; set; }

        public OrderDetailViewModel()
        {
            OrderDetailDishItemModel = new List<OrderDetailDishItem>();
        }
    }
}