using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatWithChef.Areas.Ecommerce.Models
{
    public class DishViewModel
    {
        public int DishID { get; set; }
        public string DishImage { get; set; }
        public string DishName { get; set; }
        public double DishRate { get; set; }
        public string DishDescription { get; set; }
        public int DishPrice { get; set; }
        public int Quota { get; set; }
        public string ChefName { get; set; }
        public string ImageURL { get; set; }
        public string ListTagID { get; set; }
    }

    public class DishCategoryViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<DishViewModel> ListDish { get; set; }

        public DishCategoryViewModel()
        {
            ListDish = new List<DishViewModel>();
        }
    }

    public class DishOrderViewModel {
        public DateTime MenuDate { get; set; }
        public List<DishViewModel> ListDish { get; set; }
        public List<int> ListQuantity { get; set; }

        public DishOrderViewModel() {
            ListDish = new List<DishViewModel>();
            ListQuantity = new List<int>();
        }
    }
}