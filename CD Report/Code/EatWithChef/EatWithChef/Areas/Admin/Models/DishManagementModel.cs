using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entity;

namespace EatWithChef.Areas.Admin.Models
{
    public class DishManagementModel
    {
        public List<DishCategory> ListDishCategory;
        public List<IngredientCategory> ListIngredientCategory;
        public Dish Dish;

        public DishManagementModel()
        {
            ListDishCategory = new List<DishCategory>();
            ListIngredientCategory = new List<IngredientCategory>();
            Dish = new Dish();
        }
    }
}