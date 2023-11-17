using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Utility;
using Domain.Entity;
using EatWithChef.Areas.Admin.Models;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;
using System.IO;

namespace EatWithChef.Areas.Admin.Controllers
{
    public class IngredientManagementController : Controller
    {
        //
        private const string DefaultImage = "/Images/ingredient/noimage.jpg";
        private const string INGREDIENT_FOLDER_PATH = "~/Content/images/ingredient/";
        IIngredientRepository ingreRespository = new IngredientRepository();
        ISupplierRepository supplierRepository = new SupplierRepository();
        // GET: /IngredientManagement/

        public ActionResult Index()
        {

            IngredientModel model = new IngredientModel();
            model.ListIngreCategory = ingreRespository.GetAllCategory();
            model.Listsupplier = supplierRepository.GetAllSupplierAvaible();
            if (model.ListIngreCategory == null || model.Listsupplier == null) return View("Error");
            return View(model);
        }

        [HttpPost]
        public JsonResult GetPickingIngredients(string keyword, int categoryID, string except_ingredient)
        {
            int maxPage = 0;
            string[] except_ingredients = except_ingredient.Split(',');
            List<Ingredient> listIngredient = ingreRespository.GetIngredient(keyword, categoryID, 0, "Name", "ascending", except_ingredients, out maxPage);
            if (listIngredient == null || listIngredient.Count == 0)
            {
                return Json(new { MaxPage = 1 }, JsonRequestBehavior.AllowGet);
            }

            List<object> ingredientsJson = new List<object>();
            foreach (Ingredient ingredient in listIngredient)
            {
                IngredientItem ingredientItem = new IngredientItem();
                ingredientItem = ingreRespository.getIngredientItem(ingredient.Id);
                var model = new { ID = ingredient.Id, Name = ingredient.Name, Image = ingredient.ImageUrl, CategoryID = ingredient.Category };
                ingredientsJson.Add(model);
            }

            var result = new { ListIngredient = ingredientsJson };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetIngredients(string keyword, int categoryID, string sortBy, string sortDirection, int page)
        {
            int maxPage = 0;
            List<Ingredient> listIngredient = ingreRespository.GetIngredient(keyword, categoryID, page, sortBy, sortDirection, new string[] { "" }, out maxPage);
            if (listIngredient == null || listIngredient.Count == 0)
            {
                return Json(new { MaxPage = 1 }, JsonRequestBehavior.AllowGet);
            }

            List<object> ingredientsJson = new List<object>();
            foreach (Ingredient ingredient in listIngredient)
            {
                var model = new { ID = ingredient.Id, Name = ingredient.Name, Image = ingredient.ImageUrl, CategoryID = ingredient.Category, Category = ingredient.IngredientCategory.Name, IsTracibility = ingredient.IsTracibility, IsAvailable = ingredient.IsAvailable};
                ingredientsJson.Add(model);
            }
            var result = new { ListIngredient = ingredientsJson, MaxPage = maxPage };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateIngredient(FormCollection col)
        {            
            string Name = col["Name"];
            int Category = Int32.Parse(col["Category"]);
            int Supplier = Int32.Parse(col["Supplier"]);
            string imageurl = col["ImageUrl"];
            if (imageurl.Equals("")) imageurl = DefaultImage;
            bool IsTracibility = col["IsTracibility"].Contains("true");
            int result = result = ingreRespository.InsertIngredient(Name, Category, imageurl, IsTracibility, Supplier);
            if (result != 0)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(int id)
        {
            IngredientModel model = new IngredientModel();
            model.ListIngreCategory = ingreRespository.GetAllCategory();
            model.Listsupplier = supplierRepository.GetAllSupplierAvaible();
            model.IngredientbyId = ingreRespository.GetIngredientByID(id);
            model.IngredientItem = ingreRespository.getIngredientItem(id);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult UpdateIngredient(FormCollection col)
        {
            int Id = Int32.Parse(col["Id"]);
            string Name = col["Name"];
            int Category = Int32.Parse(col["Category"]);
            //int Supplier = Int32.Parse(col["Supplier"]);
            bool IsTracibility = col["IsTracibility"].Contains("true");
            string image = col["ImageUrl"];
            int supId = Int32.Parse(col["oldsupId"]);
            if (image.Equals("")) image = DefaultImage;
            //bool IsAvailable = col["IsAvailable"].Contains("true");
            if (IsTracibility) {
                bool result2 = ingreRespository.UpdateIngredientItem(supId,Id,true,true);
            }
            else if (!IsTracibility) {
                bool result2 = ingreRespository.UpdateIngredientItem(supId, Id, false, true);
            }
            bool result = ingreRespository.UpdateIngerdient(Id,Name,Category,image, IsTracibility);
            if (result)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public int UpdateIngredientItem(int oldsupId, int ingreid, int newsupid, bool IsAvaible)
        {
            bool updateIsdefault = ingreRespository.UpdateIngredientItem(oldsupId, ingreid, IsAvaible, false);          
            if (!updateIsdefault)
            {
                return 0;
            }
            bool isExist = ingreRespository.IsExist(newsupid, ingreid);
            if (isExist)
            {
                bool updatenew = ingreRespository.UpdateIngredientItem(newsupid, ingreid, true, true);
                if (!updatenew) {
                    return 0;
                }
            }
            else {
                bool addnew = ingreRespository.InsertIngredientItem(newsupid, ingreid, true, true);
                if (!addnew) {
                    return 0;
                }
            }               
            return 1;
        }
    
        [HttpPost]
        public int Delete(int id) {
            bool result = ingreRespository.DeleteIngredient(id);
            if (!result)
                return 0;
            return 1;
        }
        [HttpPost]
        public JsonResult checkName(int id, string name)
        {
            bool result = ingreRespository.checkName(id, name);
            if (!result) return Json("Success", JsonRequestBehavior.AllowGet);
            else return Json("Error", JsonRequestBehavior.AllowGet);
        }
    }
}
