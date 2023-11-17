using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entity;

namespace EatWithChef.Areas.Admin.Models
{
    public class SupplierModel
    {
        public List<Supplier> AllSupplier;
        public List<SupplierCategory> ListSupCategory;
        public List<IngredientCategory> ListIngredientCategory;
        public Supplier Supplier;
        public List<Ingredient> ListIngrebySupId;
        //public List<Ingredient> AllIngreNotInSup;
        public IngredientItem IngredientItem;
        public Ingredient IngredientbyId;
        public List<IngredientItem> ListIngredientItem;
        public class IngredientSupplier
        {
            public int Id;
        }
        public class SupplierDefaultOfIngredient
        {
            public int Id;
            public int SupId;
        }
        public SupplierModel()
        {
            AllSupplier = new List<Supplier>();
            ListSupCategory = new List<SupplierCategory>();
            Supplier = new Supplier();
            //AllIngreNotInSup = new List<Ingredient>();
            IngredientItem = new IngredientItem();
            IngredientbyId = new Ingredient();
            ListIngredientCategory = new List<IngredientCategory>();
        }
    }
}