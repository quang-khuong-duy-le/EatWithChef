using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.DataAccess;
using Domain.Entity;
using Domain.Utility;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;
using EatWithChef.Areas.Admin.Models;



namespace EatWithChef.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;
        private IIngredientRepository _ingredientRepository;

        public SupplierController()
        {
            _supplierRepository = new SupplierRepository();
            _ingredientRepository = new IngredientRepository();
        }

        public ActionResult Index()
        {
            //Get all supplier.           
            SupplierModel supplier = new SupplierModel();
            supplier.ListSupCategory = _supplierRepository.GetAllCategory();
            if (supplier.ListSupCategory == null ) return View("Error");
            return View(supplier);
        }

        //Get create supplier form.
        [HttpPost]
        public int Create(string Name, string Address, string Phone, double Latitude, double Longitude, int SupplierCategory)
        {
            
            bool result = _supplierRepository.Insert(Name,Address,Phone,Latitude,Longitude,SupplierCategory);
            if (!result) return 0;
            return 1;
        }
        [HttpPost]
        public JsonResult GetSupplier(string keyword, int categoryID, string sortBy, string sortDirection, int page)
        {
            int maxPage = 0;
            List<Supplier> listsupplier = _supplierRepository.GetSupplier(keyword, categoryID, page, sortBy, sortDirection, out maxPage);

            if (listsupplier == null || listsupplier.Count == 0)
            {
                return Json(new { MaxPage = 1 }, JsonRequestBehavior.AllowGet);
            }

            List<object> supplierJson = new List<object>();
            foreach (Supplier supplier in listsupplier)
            {
                List<Ingredient> list = _supplierRepository.GetIngredientbySupplierId(supplier.Id);
                var model = new { ID = supplier.Id, Name = supplier.Name, Phone = supplier.Phone,Address = supplier.Address, Category = supplier.SupplierCategory, Latitude = supplier.Latitude, Longitude = supplier.Longitude, IsAvailable = supplier.IsAvailable, Count = list.Count };
                supplierJson.Add(model);
            }
            var result = new { Listsupplier = supplierJson, MaxPage = maxPage };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateSupplier(int id)
        {
            SupplierModel supplierlist = new SupplierModel();
            supplierlist.Supplier = _supplierRepository.GetSupplierById(id);
            supplierlist.ListSupCategory = _supplierRepository.GetAllCategory();
            return PartialView(supplierlist);
        }
        [HttpPost]
        public int Update(int Id, string Name, string Address, string Phone, double Latitude, double Longitude, int SupplierCategory)
        {
            bool result = _supplierRepository.Update(Id, Name, Address, Phone, Latitude, Longitude, SupplierCategory);
            if (result)
            {
                return 1;
            }
            return 0;
        }
        [HttpPost]
        public int ActiveSupplier(int supid)
        {
            bool result = _supplierRepository.ActiveSupplier(supid);
            if (!result) return 0;
            return 1;
        }
        [HttpPost]
        public ActionResult CheckDefaultSupplier(int supid)
        {
            List<Ingredient> listingredient = _supplierRepository.getIngredientbyDefaultSupplier(supid);                     
            var result = new { Count = listingredient.Count()};
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetMarkerbyCategory(string listSearch)
        {
            var listId = new List<int>();
            foreach (var lid in listSearch.Split(','))
            {
                    listId.Add(int.Parse(lid));
                }
            IEnumerable<Supplier> list = _supplierRepository.GetSupplierByCategory(listId);
            List<object> supplierJson = new List<object>();
            foreach (Supplier supplier in list)
            {
                var model = new { Name = supplier.Name, Phone = supplier.Phone, Latitude = supplier.Latitude, Longitude = supplier.Longitude, Address  = supplier.Address, Category = supplier.SupplierCategory};
                supplierJson.Add(model);
            }
            var result = new {Listsupplier = supplierJson};
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIngredientIem(int id, string keyword, int categoryID, string sortBy, string sortDirection, string except_ingredient)
        {
            int maxPage = 0;
            string[] except_ingredients = except_ingredient.Split(',');
            List<Ingredient> listIngredient = _ingredientRepository.GetIngredient(keyword, categoryID, 0, sortBy, sortDirection, except_ingredients, out maxPage);
            if (listIngredient == null || listIngredient.Count == 0)
            {
                return Json(new { MaxPage = 1 }, JsonRequestBehavior.AllowGet);
            }

            List<object> ingredientsJson = new List<object>();
            foreach (Ingredient ingredient in listIngredient)
            {
                IngredientItem ingredientItem = new IngredientItem();
                ingredientItem = _ingredientRepository.getIngredientItem(ingredient.Id);
                var model = new { ID = ingredient.Id, Name = ingredient.Name, Image = ingredient.ImageUrl, CategoryID = ingredient.Category };
                ingredientsJson.Add(model);
            }

            var result = new { ListIngredient = ingredientsJson };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateIngreOfSupplier(int id)
        {
            SupplierModel supplierlist = new SupplierModel();
            
            supplierlist.ListIngrebySupId = _supplierRepository.GetIngredientbySupplierId(id);
            supplierlist.Supplier = _supplierRepository.GetSupplierById(id);
            //supplierlist.AllIngreNotInSup = _supplierRepository.GetIngredientNotInSup(id);
            supplierlist.ListIngredientCategory = _ingredientRepository.GetAllCategory();
            return PartialView(supplierlist);
        }
        [HttpPost]
        public int GetDefaultIngredientItem(int ingreid) {
            IngredientItem ingredientItem = _ingredientRepository.getIngredientItem(ingreid);
            if (ingredientItem == null) return 0;
            return ingredientItem.SupplierID;
        }
        public ActionResult UpdateIngredientItemDefault(int supid, int ingreid) {
            SupplierModel supplierlist = new SupplierModel();
            supplierlist.AllSupplier = _supplierRepository.GetAllSupplierAvaible();
            supplierlist.Supplier = _supplierRepository.GetSupplierById(supid);
            supplierlist.IngredientbyId = _ingredientRepository.GetIngredientByID(ingreid);
            return PartialView(supplierlist);
        }
        [HttpPost]
        public string UpdateIngredientOfSup(string info, int SupId) {
            try
            {
                List<SupplierModel.IngredientSupplier> listIngredient = JsonHelper.JsonDeserialize<List<SupplierModel.IngredientSupplier>>(info);
                _ingredientRepository.DeleteAllIngredientInSupplier(SupId);
                foreach (var ingredient in listIngredient)
                {
                    bool check = _ingredientRepository.IsExist(SupId, ingredient.Id);
                    if (check)
                    {
                        bool update = _ingredientRepository.UpdateIsAvailableIngredientItem(SupId, ingredient.Id, true);
                        if (!update)
                        {
                            return "Fail";
                        }
                    }
                    else {
                        _ingredientRepository.InsertIngredientItem(SupId, ingredient.Id, true, false);
                    }                    
                }
                return "Success";
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message); ;
            }
            return "Fail";
        }
        [HttpPost]
        public int NoSupplyIngredientItem(int oldsupId, int ingreid, int newsupid)
        {
            bool updateIsdefault = _ingredientRepository.UpdateIngredientItem(oldsupId, ingreid, true, false);
            if (!updateIsdefault)
            {
                return 0;
            }
            bool isExist = _ingredientRepository.IsExist(newsupid, ingreid);
            if (isExist)
            {
                bool updatenew = _ingredientRepository.UpdateIngredientItem(newsupid, ingreid, true, true);
                if (!updatenew)
                {
                    return 0;
                }
            }
            else
            {
                bool addnew = _ingredientRepository.InsertIngredientItem(newsupid, ingreid, true, true);
                if (!addnew)
                {
                    return 0;
                }
            }
            return 1;
        }
        //[HttpPost]
        //public string DeleteIngredientItem(int SupId, int ingredid)
        //{
        //    bool result = _ingredientRepository.UpdateIsAvailableIngredientItem(SupId, ingredid, false);
        //    if (!result) return "Fail";
        //    return "Success";
        //}
        public ActionResult UpdateAllIngredientItemDefault(int supid) {
            SupplierModel supplierlist = new SupplierModel();
            supplierlist.ListIngrebySupId = _supplierRepository.getIngredientbyDefaultSupplier(supid);
            supplierlist.AllSupplier = _supplierRepository.GetAllSupplierAvaible();
            supplierlist.Supplier = _supplierRepository.GetSupplierById(supid);
            return PartialView(supplierlist);
        }
        [HttpPost]
        public string UpdateAllIngredientDefault(string info, int OldSupid)
        {
            List<SupplierModel.SupplierDefaultOfIngredient> list = JsonHelper.JsonDeserialize<List<SupplierModel.SupplierDefaultOfIngredient>>(info);
            bool result = _ingredientRepository.DeleteAllIngredientItemDefaulSupplier(OldSupid);
            if (!result) return "Fail";
            foreach (var ingredient in list) {
                bool check = _ingredientRepository.IsExist(ingredient.SupId, ingredient.Id);
                if (check)
                {
                    bool updatenew = _ingredientRepository.UpdateIngredientItem(ingredient.SupId, ingredient.Id, true, true);
                    if (!updatenew)
                    {
                        return "Fail";
                    }
                }
                else
                {
                    bool addnew = _ingredientRepository.InsertIngredientItem(ingredient.SupId, ingredient.Id, true, true);
                    if (!addnew)
                    {
                        return "Fail";
                    }
                }               
            }
            return "Success";
        }
        [HttpPost]
        public string DeleteSupplier(int id){
            bool result = _supplierRepository.Delete(id);
            bool result2 = _ingredientRepository.DeleteAllIngredientItemDefaulSupplier(id);
            if (!result || !result2) return "Fail";
            return "Success";
        }
        [HttpPost]
        public JsonResult checkName(int id , string name) {
            bool result = _supplierRepository.checkName(id, name);
            if (!result) return Json("Success", JsonRequestBehavior.AllowGet);
            else return Json("Error", JsonRequestBehavior.AllowGet);
        }
    }
}
