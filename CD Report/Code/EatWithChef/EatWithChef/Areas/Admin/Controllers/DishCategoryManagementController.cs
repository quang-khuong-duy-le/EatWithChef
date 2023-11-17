using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Utility;
using Domain.DataAccess;
using EatWithChef.Areas.Admin.Models;
using Domain.Entity;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;

namespace EatWithChef.Areas.Admin.Controllers
{
    public class DishCategoryManagementController : Controller
    {
        private readonly IDishRepository _dishRepository;
        private const string DEFAULT_DISH_CATEGORY_IMAGE = "/Images/default-image.png";

        public DishCategoryManagementController() {
            _dishRepository = new DishRepository();
        }

        public ActionResult Index()
        {
            ListDishCategoryModel model = new ListDishCategoryModel();
            try
            {
                model.ListDishCategory = _dishRepository.GetAllDishCategory();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View("ERROR");
            }

            return View(model);
        }

        public ActionResult CreateDishCategory()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateDishCategory(FormCollection col)
        {
            string name = col["name"];
            string description = col["description"];
            //bool isactive = col["isactive"].Contains("true");
            string image = col["image"];
            if (image.Equals("")) image = DEFAULT_DISH_CATEGORY_IMAGE;

            //if (Request.Files.Count != 0 && !Request.Files[0].FileName.Equals("") && !string.IsNullOrEmpty(name))
            //{
            //    // save image into server file
            //    var image = Request.Files[0];
            //    string fileName = image.FileName;
            //    string path = Path.Combine(Server.MapPath(DISH_CATEGORY_FOLDER_PATH), fileName);
            //    string newFileName = FileHelper.UploadFileToServer(image, path, fileName);
            //    if (string.IsNullOrEmpty(newFileName))
            //    {
            //        return Json("Error", JsonRequestBehavior.AllowGet);
            //    }

            //    // save category to db
            //    DishRepository dishRespository = new DishRepository();
            //    bool result = false;
            //    try
            //    {
            //        result = dishRespository.InsertDishCategory(name, description, isactive, newFileName);
            //    }
            //    catch (Exception)
            //    {
            //        path = Path.Combine(Server.MapPath(DISH_CATEGORY_FOLDER_PATH), newFileName);
            //        FileHelper.DeleteFileFromSystem(path);
            //        return Json("Error", JsonRequestBehavior.AllowGet);
            //    }

            //    if (result)
            //    {
            //        return Json(newFileName, JsonRequestBehavior.AllowGet);
            //    }
            //    else
            //    {
            //        path = Path.Combine(Server.MapPath(DISH_CATEGORY_FOLDER_PATH), newFileName);
            //        FileHelper.DeleteFileFromSystem(path);
            //        return Json("Error", JsonRequestBehavior.AllowGet);
            //    }
            //}

            // save category to db
            bool result = false;
            result = _dishRepository.InsertDishCategory(name, description, image);
            if (result)
            {
                return Json(image, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateDishCategory(int CategoryID)
        {
            DishCategory model = new DishCategory();
            try
            {
                model = _dishRepository.GetDishCategoryByID(CategoryID);
            }
            catch (Exception)
            {
                return View("ERROR");
            }
            if (model == null) return View("ERROR");
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateDishCategory(FormCollection col)
        {
            string idStr = col["id"];
            string name = col["name"];
            //bool isactive = col["isactive"].Contains("true");
            string description = col["description"];
            string image = col["image"];
            if (image.Equals("")) image = DEFAULT_DISH_CATEGORY_IMAGE;

            DishCategory dishCategory = new DishCategory();
            try
            {
                int id = (int)Int64.Parse(idStr);
                dishCategory = _dishRepository.GetDishCategoryByID(id);
            }
            catch (Exception)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

            // save category to db
            bool result = false;
            try
            {
                int id = (int)Int64.Parse(idStr);
                result = _dishRepository.UpdateDishCategory(id, name, description, image);
            }
            catch (Exception)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

            if (result)
            {
                return Json(image, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult CheckDishCategoryName(string name, int categoryid)
        {
            bool result = _dishRepository.CheckDishCategoryName(name, categoryid);
            if (result) return Json("Success", JsonRequestBehavior.AllowGet);
            else return Json("Error", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDishCategoryName(int categoryid)
        {
            bool result = _dishRepository.DeleteDishCategory(categoryid);
            if (result) return Json("Success", JsonRequestBehavior.AllowGet);
            else return Json("Error", JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult UpdateDishCategory(FormCollection col)
        //{
        //    string idStr = col["id"];
        //    string name = col["name"];
        //    bool isactive = col["isactive"].Contains("true");
        //    string description = col["description"];
        //    string path = "";
        //    string newFileName = "";
        //    string oldFileName = "";

        //    DishRepository dishRespository = new DishRepository();
        //    DishCategory dishCategory = new DishCategory();
        //    try
        //    {
        //        int id = (int)Int64.Parse(idStr);
        //        dishCategory = dishRespository.GetDishCategoryByID(id);
        //        newFileName = dishCategory.Image;
        //        oldFileName = newFileName;
        //    }
        //    catch (Exception)
        //    {
        //        return Json("Error", JsonRequestBehavior.AllowGet);
        //    }

        //    if (Request.Files.Count != 0 && !Request.Files[0].FileName.Equals(""))
        //    {
        //        // save image into server file
        //        var image = Request.Files[0];
        //        newFileName = image.FileName;
        //        path = Path.Combine(Server.MapPath(DISH_CATEGORY_FOLDER_PATH), newFileName);
        //        newFileName = FileHelper.UploadFileToServer(image, path, newFileName);
        //        if (string.IsNullOrEmpty(newFileName))
        //        {
        //            return Json("Error", JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    // save category to db
        //    bool result = false;
        //    try
        //    {
        //        int id = (int)Int64.Parse(idStr);
        //        result = dishRespository.UpdateDishCategory(id, name, description, isactive, newFileName);
        //    }
        //    catch (Exception)
        //    {
        //        return Json("Error", JsonRequestBehavior.AllowGet);
        //    }

        //    if (result)
        //    {
        //        // delete old file
        //        if (!oldFileName.Equals(dishCategory.Image))
        //        {
        //            path = Path.Combine(Server.MapPath(DISH_CATEGORY_FOLDER_PATH), oldFileName);
        //            FileHelper.DeleteFileFromSystem(path);
        //        }

        //        return Json(newFileName, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        // delete new image of unsuccessfully updated DishCategory
        //        if (!newFileName.Equals(dishCategory.Image))
        //        {
        //            path = Path.Combine(Server.MapPath(DISH_CATEGORY_FOLDER_PATH), newFileName);
        //            FileHelper.DeleteFileFromSystem(path);
        //        }
        //        return Json("Error", JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}
