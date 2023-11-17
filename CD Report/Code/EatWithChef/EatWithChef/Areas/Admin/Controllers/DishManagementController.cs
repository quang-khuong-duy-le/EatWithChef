using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EatWithChef.Areas.Admin.Models;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;
using System.IO;
using Domain.Utility;
using Domain.Entity;
using Newtonsoft.Json;

namespace EatWithChef.Areas.Admin.Controllers
{
    public class DishManagementController : Controller
    {
        
        private const string DEFAULT_DISH_IMAGE = "/Images/Dish/default-image.png";

        private IDishRepository dishRepository;
        // GET: /DishManagement/
        public DishManagementController()
        {
            dishRepository = new DishRepository();
        }

        public ActionResult Index()
        {
            ListDishCategoryModel model = new ListDishCategoryModel();
            IngredientRepository ingredientRepository = new IngredientRepository();

            model.ListDishCategory = dishRepository.GetAllCategory();
            model.ListIngredientCategory = ingredientRepository.GetAllCategory();
            if (model.ListIngredientCategory == null || model.ListDishCategory == null)
            {
                return View("Error");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult GetDishes(string keyword, int categoryID, string sortBy, string sortDirection, int page)
        {
            int maxPage = 0;
            List<Dish> listDish = dishRepository.GetDish(keyword, categoryID, page, sortBy, sortDirection, out maxPage);

            if (listDish == null || listDish.Count == 0)
            {
                return Json(new {MaxPage = 1 }, JsonRequestBehavior.AllowGet);
            }

            List<object> dishesJson = new List<object>(); 
            foreach (Dish dish in listDish)
            {
                var model = new { ID = dish.Id, Name = dish.Name, Price = dish.Price, Image = dish.Image, Category = dish.DishCategory.Name };
                dishesJson.Add(model);
            }
            var result = new { ListDish = dishesJson, MaxPage = maxPage };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckDishName(string name, int dishid)
        {
            bool result = dishRepository.CheckDishName(name, dishid);
            if (result) return Json("Success", JsonRequestBehavior.AllowGet);
            else return Json("Error", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateDish()
        {
            ListDishCategoryModel model = new ListDishCategoryModel();
            try
            {
                model.ListDishCategory = dishRepository.GetAllCategory();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View("ERROR");
            }

            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult CreateDish(string json)
        {
            try
            {
                var jsonData = JsonConvert.DeserializeObject<dynamic>(json);
                string name = (string)jsonData.name;
                if (!dishRepository.CheckDishName(name, 0)) return Json("Error", JsonRequestBehavior.AllowGet);

                string description = (string)jsonData.description;
                string cookingGuide = (string)jsonData.cookingGuide;
                int categoryID = Int32.Parse((string)jsonData.categoryID);
                int price = Int32.Parse((string)jsonData.price);
                string tagList = (string)jsonData.taglist;
                string image = (string)jsonData.image;
                if (image.Equals("")) image = DEFAULT_DISH_IMAGE;
                string selected_ingredient_str = (string)jsonData.selected_ingredient;
                string[] selected_ingredient = selected_ingredient_str.Split(',');
                if (selected_ingredient.Length == 0 || selected_ingredient[0].Equals(""))
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
                // save dish to db
                bool result = dishRepository.InsertDish(name, categoryID, price, description, cookingGuide, 5, 1, image, tagList, selected_ingredient);

                if (result)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateDish(int DishID)
        {
            DishManagementModel model = new DishManagementModel();
            IngredientRepository ingredientRepository = new IngredientRepository();
            try
            {
                model.Dish = dishRepository.GetDishByID(DishID);
                model.ListDishCategory = dishRepository.GetAllCategory();
                model.ListIngredientCategory = ingredientRepository.GetAllCategory();
            }
            catch (Exception)
            {
                return PartialView(model);
            }
            return PartialView(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult UpdateDish(string json)
        {
            try
            {
                var jsonData = JsonConvert.DeserializeObject<dynamic>(json);
                bool result = false;
                int id = 0;
                result = Int32.TryParse((string)jsonData.id, out id);
                string name = (string)jsonData.name;
                if (!result || !dishRepository.CheckDishName(name, id)) return Json("Error", JsonRequestBehavior.AllowGet);

                string description = (string)jsonData.description;
                string cookingGuide = (string)jsonData.cookingGuide;
                int categoryID = Int32.Parse((string)jsonData.categoryID);
                int price = Int32.Parse((string)jsonData.price);
                string image = (string)jsonData.image;
                if (image.Equals("")) image = DEFAULT_DISH_IMAGE;
                string tagList = (string)jsonData.taglist;
                string selected_ingredient_str = (string)jsonData.selected_ingredient;
                string[] selected_ingredient = selected_ingredient_str.Split(',');
                if (selected_ingredient.Length == 0 || selected_ingredient[0].Equals(""))
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }

                Dish dish = new Dish();
                dish = dishRepository.GetDishByID(id);
                if (dish == null)
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }

                // save category to db
                result = dishRepository.UpdateDish(id, name, categoryID, price, description, cookingGuide, 1, image, tagList, selected_ingredient);

                if (result)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteDish(int id)
        {
            bool result = false;
            result = dishRepository.DeleteDish(id);

            if (result)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult LoadTags(String tagName)
        {
            List<Tag> item = dishRepository.GetTagsByName(tagName);
            return Json(item.Select(tag => new { ID = tag.Id, Name = tag.Name }), JsonRequestBehavior.AllowGet);
        }
    }
}
