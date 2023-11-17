using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entity;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;

namespace EatWithChef.Areas.Admin.Models
{
    public class DishProduceSessionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public int NumberProducedDishItem { get; set; }
        public int NumberOrderedDishItem { get; set; }

        public DishProduceSessionModel() { }

        public DishProduceSessionModel(Dish dish)
        {
            ID = dish.Id;
            Name = dish.Name;
            CategoryName = dish.DishCategory.Name;
            Image = dish.Image;
        }
    }

    public class ListDishProduceModel
    {
        public List<DishProduceSessionModel> ListDishProduceSession { get; set; }

        public ListDishProduceModel()
        {
            this.ListDishProduceSession = new List<DishProduceSessionModel>();
        }
    }

    public class CreateProduceSessionModel
    {
        public Dish Dish { get; set; }
        public int ProduceQuantity { get; set; }
        public List<IngredientWithSupplierModel> ListIngredientWithSupplier { get; set; }

        public CreateProduceSessionModel()
        {
            Dish = new Dish();
            ListIngredientWithSupplier = new List<IngredientWithSupplierModel>();
        }
    }

    public class IngredientWithSupplierModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int DefaultIngredientItem { get; set; }
        public List<SupplierForProduceSessionModel> ListSupplier { get; set; }

        public IngredientWithSupplierModel()
        {
            ListSupplier = new List<SupplierForProduceSessionModel>();
        }

        public IngredientWithSupplierModel(IngredientItem ingredientItem)
        {
            ID = ingredientItem.IngredientID;
            Name = ingredientItem.Ingredient.Name;
            ListSupplier = new List<SupplierForProduceSessionModel>();
            SupplierForProduceSessionModel supplierModel = new SupplierForProduceSessionModel(ingredientItem);
            ListSupplier.Add(supplierModel);
        }
    }

    public class SupplierForProduceSessionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int IngredientItemID { get; set; }

        public SupplierForProduceSessionModel() { }

        public SupplierForProduceSessionModel(IngredientItem ingredientItem)
        {
            ID = ingredientItem.SupplierID;
            Name = ingredientItem.Supplier.Name;
            IngredientItemID = ingredientItem.Id;
        }
    }

    public class ListDishItemModel
    {
        public List<DishItemModel> ListDishItem { get; set; }

        public ListDishItemModel()
        {
            ListDishItem = new List<DishItemModel>();
        }
    }

    public class DishItemModel
    {
        public int Id { get; set; }
        public string DishName { get; set; }
        public string DishImage { get; set; }
        public bool Deletable { get; set; }
        public string ChefName { get; set; }
        public string CookingTime { get; set; }
        public int Quantity { get; set; }
        public string QRCode { get; set; }

        public DishItemModel(DishItem dishItem)
        {
            UserProfileRepository userRepository = new UserProfileRepository();

            Id = dishItem.Id;
            DishName = dishItem.Dish.Name;
            DishImage = dishItem.Dish.Image;
            if (dishItem.OrderDetailDishItems.Count == 0) Deletable = true;
            else Deletable = false;

            QRCode = dishItem.QRCode;
            ChefName = userRepository.GetUserProfileByID(dishItem.Chef.UserID).FullName;
            CookingTime = dishItem.CookingTime.Value.ToLongTimeString();
            Quantity = dishItem.Quantity;
        }
    }

    public class ListProduceSessionGenerateModel
    {
        public List<CreateProduceSessionModel> ListCreateProduceSession { get; set; }

        public ListProduceSessionGenerateModel()
        {
            ListCreateProduceSession = new List<CreateProduceSessionModel>();
        }
    }
}