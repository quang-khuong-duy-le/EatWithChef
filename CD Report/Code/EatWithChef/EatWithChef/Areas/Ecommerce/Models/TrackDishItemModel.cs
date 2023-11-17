using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;
using Domain.Entity;

namespace EatWithChef.Areas.Ecommerce.Models
{
    public class TrackDishItemModel
    {
        public DishItemForTrackingModel dishItemModel { get; set; }
        public List<IngredientForTrackingModel> ListIngredientForTracking { get; set; }

        public TrackDishItemModel(DishItem dishItem)
        {
            ListIngredientForTracking = new List<IngredientForTrackingModel>();
            dishItemModel = new DishItemForTrackingModel(dishItem);
        }
    }

    public class DishItemForTrackingModel
    {
        public int Id { get; set; }
        public string DishName { get; set; }
        public string DishImage { get; set; }
        public string DishDescription { get; set; }
        public string ChefName { get; set; }
        public string CookingTime { get; set; }

        public DishItemForTrackingModel(DishItem dishItem)
        {
            IUserProfileRepository userRepository = new UserProfileRepository();

            Id = dishItem.Id;
            DishName = dishItem.Dish.Name;
            DishImage = dishItem.Dish.Image;
            DishDescription = dishItem.Dish.Description;
            ChefName = userRepository.GetUserProfileByID(dishItem.Chef.UserID).FullName;
            CookingTime = dishItem.CookingTime.Value.ToString();
        }
    }

    public class IngredientForTrackingModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public List<SupplierForTrackingModel> ListSupplier { get; set; }

        public IngredientForTrackingModel()
        {
            ListSupplier = new List<SupplierForTrackingModel>();
        }

        public IngredientForTrackingModel(IngredientItem ingredientItem)
        {
            ID = ingredientItem.IngredientID;
            Name = ingredientItem.Ingredient.Name;
            ImageURL = ingredientItem.Ingredient.ImageUrl;
            ListSupplier = new List<SupplierForTrackingModel>();
            SupplierForTrackingModel supplierModel = new SupplierForTrackingModel(ingredientItem);
            ListSupplier.Add(supplierModel);
        }
    }

    public class SupplierForTrackingModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

        public SupplierForTrackingModel() { }

        public SupplierForTrackingModel(IngredientItem ingredientItem)
        {
            ID = ingredientItem.SupplierID;
            Name = ingredientItem.Supplier.Name;
            Address = ingredientItem.Supplier.Address;
            Lat = ingredientItem.Supplier.Latitude;
            Long = ingredientItem.Supplier.Longitude;
        }
    }
}