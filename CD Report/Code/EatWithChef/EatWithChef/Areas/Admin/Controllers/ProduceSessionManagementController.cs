using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using EatWithChef.Areas.Admin.Models;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;
using Domain.Entity;
using Newtonsoft.Json;

namespace EatWithChef.Areas.Admin.Controllers
{
    public class ProduceSessionManagementController : Controller
    {
        private const string QRCODE_FOLDER_PATH = "~/Images/QRCode/";
        //
        // GET: /ProduceSessionManagement/

        public ActionResult TodayMenuProduce()
        {
            ListDishProduceModel model = new ListDishProduceModel();
            IProduceSessionRepository produceSessionRespository = new ProduceSessionRepository();
            try
            {
                // get menu for today
                Menu menu = new Menu();
                menu = produceSessionRespository.GetMenuByDate(DateTime.Today);
                if (menu == null)
                {
                    return View(model);
                }
                // for each dish in today menu get its info 
                List<Dish> listDish = new List<Dish>();
                listDish = produceSessionRespository.LoadDishFromMenu(menu.Id);
                if (listDish == null)
                {
                    return View(model);
                }

                // go through each Dish in Menu
                DishItem dishItem = new DishItem();
                foreach (Dish dish in listDish)
                {
                    DishProduceSessionModel dishProduceModel = new DishProduceSessionModel(dish);
                    dishProduceModel.ID = dish.Id;
                    dishProduceModel.Name = dish.Name;
                    dishProduceModel.Image = dish.Image;
                    // calculate number of produced dish
                    dishProduceModel.NumberProducedDishItem = produceSessionRespository.CalculateNumberProducedDishItemOfMenu(menu, dish.Id);
                    if (dishProduceModel.NumberProducedDishItem == -1)
                    {
                        return View("Error");
                    }
                    // calculate number of ordered dish
                    dishProduceModel.NumberOrderedDishItem = produceSessionRespository.CalculateNumberOrderedDishItem(menu, dish.Id);
                    if (dishProduceModel.NumberOrderedDishItem == -1)
                    {
                        return View("Error");
                    }
                    model.ListDishProduceSession.Add(dishProduceModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View("ERROR");
            }
            return View(model);
        }

        public ActionResult ProduceSessionGenerate()
        {
            ListProduceSessionGenerateModel model = new ListProduceSessionGenerateModel();
            IProduceSessionRepository produceSessionRespository = new ProduceSessionRepository();
            try
            {
                // get menu for today
                Menu menu = new Menu();
                menu = produceSessionRespository.GetMenuByDate(DateTime.Today);
                if (menu == null)
                {
                    return PartialView(model);
                }
                // for each dish in today menu get its info 
                List<Dish> listDish = new List<Dish>();
                listDish = produceSessionRespository.LoadDishFromMenu(menu.Id);
                if (listDish == null)
                {
                    return PartialView(model);
                }

                // go through each Dish in Menu
                foreach (Dish dish in listDish)
                {
                    // calculate number of produced dish
                    int numberProducedDishItem = produceSessionRespository.CalculateNumberProducedDishItemOfMenu(menu, dish.Id);
                    if (numberProducedDishItem == -1)
                    {
                        ViewBag.Result = "Error";
                        model = null;
                        return PartialView(model);
                    }
                    // calculate number of ordered dish
                    int numberOrderedDishItem = produceSessionRespository.CalculateNumberOrderedDishItem(menu, dish.Id);
                    if (numberOrderedDishItem == -1)
                    {
                        ViewBag.Result = "Error";
                        model = null;
                        return PartialView(model);
                    }

                    // auto generate ProduceSession or not
                    if (numberOrderedDishItem > numberProducedDishItem)
                    {
                        CreateProduceSessionModel createProduceSession = new CreateProduceSessionModel();
                        // get list IngredientItem available for Dish
                        List<IngredientItem> listIngredientItem = new List<IngredientItem>();
                        listIngredientItem = produceSessionRespository.GetAvailableIngredientItemForDish(dish.Id);
                        if (listIngredientItem == null)
                        {
                            continue;
                        }

                        // go through each available IngredientItem
                        foreach (IngredientItem ingredientItem in listIngredientItem)
                        {
                            bool found = false;
                            foreach (IngredientWithSupplierModel ingredientWithSupplier in createProduceSession.ListIngredientWithSupplier)
                            {
                                // if found in ListIngredientWithSupplier of Model, then add new Supplier
                                if (ingredientItem.IngredientID == ingredientWithSupplier.ID)
                                {
                                    SupplierForProduceSessionModel supplierModel = new SupplierForProduceSessionModel(ingredientItem);
                                    if (ingredientItem.IsDefaultSupplier) ingredientWithSupplier.DefaultIngredientItem = ingredientItem.Id;
                                    ingredientWithSupplier.ListSupplier.Add(supplierModel);
                                    found = true;
                                    break;
                                }
                            }
                            // if not found in ListIngredientWithSupplier of Model, insert new record of IngredientWithSupplierModel
                            if (!found)
                            {
                                IngredientWithSupplierModel ingredientWithSupplier = new IngredientWithSupplierModel(ingredientItem);
                                if (ingredientItem.IsDefaultSupplier) ingredientWithSupplier.DefaultIngredientItem = ingredientItem.Id;
                                createProduceSession.ListIngredientWithSupplier.Add(ingredientWithSupplier);
                            }
                        }
                        createProduceSession.ProduceQuantity = numberOrderedDishItem - numberProducedDishItem;
                        createProduceSession.Dish = dish;
                        model.ListCreateProduceSession.Add(createProduceSession);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.Result = "Error";
                model = null;
                return PartialView(model);
            }

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ProduceSessionGenerate(string json)
        {
            try
            {
                var jsonData = JsonConvert.DeserializeObject<dynamic>(json);

                List<ProduceSessionGenerateDTO> ListProduceSessionGenerate = new List<ProduceSessionGenerateDTO>();
                for (int i = 0; i < jsonData.ListProduceSession.Count; i++)
                {
                    if (jsonData.ListProduceSession[i].IngredientsString.Equals("")) return Json("Error", JsonRequestBehavior.AllowGet);
                    ProduceSessionGenerateDTO ps = new ProduceSessionGenerateDTO();
                    ps.DishID = jsonData.ListProduceSession[i].DishID;
                    ps.Quantity = jsonData.ListProduceSession[i].Quantity;
                    ps.IngredientsString = jsonData.ListProduceSession[i].IngredientsString;
                    ListProduceSessionGenerate.Add(ps);
                }

                IProduceSessionRepository produceRepository = new ProduceSessionRepository();

                bool result = produceRepository.ProduceSessionGenerate(ListProduceSessionGenerate, Server.MapPath(QRCODE_FOLDER_PATH));

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
        
        // return partial view for producing session
        public ActionResult CreateProduceSession(int DishID)
        {
            CreateProduceSessionModel model = new CreateProduceSessionModel();
            IDishRepository dishRepository = new DishRepository();
            IProduceSessionRepository produceRepository = new ProduceSessionRepository();
            
            // get Dish
            model.Dish = dishRepository.GetDishByID(DishID);
            if (model.Dish == null)
            {
                model = null;
                return PartialView(model);
            }

            // get list IngredientItem available for Dish
            List<IngredientItem> listIngredientItem = new List<IngredientItem>();
            listIngredientItem = produceRepository.GetAvailableIngredientItemForDish(DishID);
            if (listIngredientItem == null)
            {
                model = null;
                return PartialView(model);
            }

            // go through each available IngredientItem
            foreach (IngredientItem ingredientItem in listIngredientItem)
            {
                bool found = false;
                foreach (IngredientWithSupplierModel ingredientWithSupplier in model.ListIngredientWithSupplier)
                {
                    // if found in ListIngredientWithSupplier of Model, then add new Supplier
                    if (ingredientItem.IngredientID == ingredientWithSupplier.ID)
                    {
                        SupplierForProduceSessionModel supplierModel = new SupplierForProduceSessionModel(ingredientItem);
                        if (ingredientItem.IsDefaultSupplier) ingredientWithSupplier.DefaultIngredientItem = ingredientItem.Id;
                        ingredientWithSupplier.ListSupplier.Add(supplierModel);
                        found = true;
                        break;
                    }
                }
                // if not found in ListIngredientWithSupplier of Model, insert new record of IngredientWithSupplierModel
                if (!found)
                {
                    IngredientWithSupplierModel ingredientWithSupplier = new IngredientWithSupplierModel(ingredientItem);
                    if (ingredientItem.IsDefaultSupplier) ingredientWithSupplier.DefaultIngredientItem = ingredientItem.Id;
                    model.ListIngredientWithSupplier.Add(ingredientWithSupplier);
                }
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CreateProduceSessionWithQRCode(int id, int quantity, string ingredients_str)
        {
            IProduceSessionRepository produceRepository = new ProduceSessionRepository();
            
            // create new DishItem 
            DishItem dishItem = produceRepository.ProduceDishItem(id, quantity, ingredients_str);

            // Generate QR Code
            bool result = false;
            if (dishItem != null)
            {
                String guid = System.Guid.NewGuid().ToString();
                string path = Path.Combine(Server.MapPath(QRCODE_FOLDER_PATH), guid);
                result = produceRepository.CreateQRCodeForDishItem(dishItem, path);
            }

            if (result)
            {
                return PartialView(dishItem);
            }
            else if (dishItem != null)
            {
                produceRepository.DeleteDishItemPermanent(dishItem);
                dishItem = null;
                return PartialView(dishItem);
            }
            else
            {
                dishItem = null;
                return PartialView(dishItem);
            }
        }

        public ActionResult ProduceSessionHistory(string date)
        {
            IProduceSessionRepository produceRepository = new ProduceSessionRepository();
            ListDishItemModel model = new ListDishItemModel();

            List<DishItem> listDishItem = new List<DishItem>();
            DateTime dt = new DateTime();
            try
            {
                dt = DateTime.ParseExact(date, "MM/dd/yyyy", null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                model = null;
                return PartialView(model);
            }
            listDishItem = produceRepository.GetDishItemByDate(dt);
            if (listDishItem == null)
            {
                model = null;
                return PartialView(model);
            }

            foreach (DishItem dishItem in listDishItem)
            {
                DishItemModel dishItemModel = new DishItemModel(dishItem);
                model.ListDishItem.Add(dishItemModel);
            }
            
            return PartialView(model);
        }

        public JsonResult DeleteProduceSession(int ID)
        {
            IProduceSessionRepository produceRepository = new ProduceSessionRepository();
            bool result = produceRepository.DeleteDishItem(ID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        //[HttpPost]
        //public ActionResult TodayMenuProduce(bool GenerateProduceSession)
        //{
        //    ListDishProduceModel model = new ListDishProduceModel();
        //    IProduceSessionRepository produceSessionRespository = new ProduceSessionRepository();
        //    try
        //    {
        //        // get menu for today
        //        Menu menu = new Menu();
        //        menu = produceSessionRespository.GetMenuByDate(DateTime.Today);
        //        if (menu == null)
        //        {
        //            return View(model);
        //        }
        //        // for each dish in today menu get its info 
        //        List<Dish> listDish = new List<Dish>();
        //        listDish = produceSessionRespository.LoadDishFromMenu(menu.Id);
        //        if (listDish == null)
        //        {
        //            return View(model);
        //        }

        //        // go through each Dish in Menu
        //        DishItem dishItem = new DishItem();
        //        foreach (Dish dish in listDish) 
        //        {
        //            DishProduceSessionModel dishProduceModel = new DishProduceSessionModel(dish);
        //            dishProduceModel.ID = dish.Id;
        //            dishProduceModel.Name = dish.Name;
        //            dishProduceModel.Image = dish.Image;
        //            // calculate number of produced dish
        //            dishProduceModel.NumberProducedDishItem = produceSessionRespository.CalculateNumberProducedDishItemOfMenu(menu, dish.Id);
        //            if (dishProduceModel.NumberProducedDishItem == -1)
        //            {
        //                return View("Error");
        //            }
        //            // calculate number of ordered dish
        //            dishProduceModel.NumberOrderedDishItem = produceSessionRespository.CalculateNumberOrderedDishItem(dish.Id);
        //            if (dishProduceModel.NumberOrderedDishItem == -1)
        //            {
        //                return View("Error");
        //            }

        //            // auto generate ProduceSession or not
        //            if (GenerateProduceSession && dishProduceModel.NumberOrderedDishItem > dishProduceModel.NumberProducedDishItem)
        //            {
        //                dishItem = produceSessionRespository.ProduceDishItem(dish.Id, dishProduceModel.NumberOrderedDishItem - dishProduceModel.NumberProducedDishItem, "", true);
        //                if (dishItem != null)
        //                {
        //                    String guid = System.Guid.NewGuid().ToString();
        //                    string path = Path.Combine(Server.MapPath(QRCODE_FOLDER_PATH), guid);
        //                    bool result = produceSessionRespository.CreateQRCodeForDishItem(dishItem, path);
        //                    if (result)
        //                    {
        //                        dishProduceModel.NumberProducedDishItem = dishProduceModel.NumberOrderedDishItem;
        //                    }
        //                    else if (dishItem != null)
        //                    {
        //                        produceSessionRespository.DeleteDishItemPermanent(dishItem);
        //                    }
        //                }
        //            }
        //            model.ListDishProduceSession.Add(dishProduceModel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return View("ERROR");
        //    }

        //    ViewBag.GenerateProduceSession = GenerateProduceSession;
        //    return View(model);
        //}
    }
}
