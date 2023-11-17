using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class DishOfMenuDTO
    {
        public int DishID { get; set; }
        public int MenuID { get; set; }
        public int DishPrice { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Category { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string CookingGuide { get; set; }
        public double Rate { get; set; }
        public int ChefID { get; set; }
        public bool IsAvailable { get; set; }

        public DishMenu dishmenus { get; set; }
        public Dish dishs { get; set; }
    }

    public class MenuDTO
    {
        public int MenuID { get; set; }
        public List<int> ListDishID { get; set; }
        public List<int> ListQuantity { get; set; }

        public MenuDTO()
        {
            ListDishID = new List<int>();
            ListQuantity = new List<int>();
        }
    }

    public class CatDishMenuDTO
    {
        public DishOfMenuDTO DishMenu { get; set; }
        public DishCategory DishCategory { get; set; }
    }

    public class CatOfDishDTO
    {
        public Dish Dish { get; set; }
        public DishCategory DishCategory { get; set; }
    }
}
