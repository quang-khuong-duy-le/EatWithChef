using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entity;

namespace EatWithChef.Areas.Admin.Models
{
    public class IngredientModel
    {
        public List<IngredientCategory> ListIngreCategory;
        public List<Ingredient> ingredient;
        public List<Supplier> Listsupplier;
        public Ingredient IngredientbyId;
        public IngredientItem IngredientItem;
        public IngredientModel()
        {
            ListIngreCategory = new List<IngredientCategory>();
            ingredient = new List<Ingredient>();
            Listsupplier = new List<Supplier>();
            IngredientbyId = new Ingredient();
            IngredientItem = new IngredientItem();
        }
    }
}