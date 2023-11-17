using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entity;

namespace EatWithChef.Areas.Admin.Models
{
    public class DishCategoryModel
    {
    }

    public class ListDishCategoryModel
    {
        public List<DishCategory> ListDishCategory;
        public List<IngredientCategory> ListIngredientCategory;

        public ListDishCategoryModel()
        {
            ListDishCategory = new List<DishCategory>();
            ListIngredientCategory = new List<IngredientCategory>();
        }
    }
}