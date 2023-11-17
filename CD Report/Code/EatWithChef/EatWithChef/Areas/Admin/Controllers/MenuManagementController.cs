using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;
using Domain.Entity;
using EatWithChef.Areas.Admin.Models;
using Domain.Utility;
using System.Web.Script.Serialization;

namespace EatWithChef.Areas.Admin.Controllers
{
    public class MenuManagementController : Controller
    {
        IMenuRepository menu = new MenuRepository();

        //
        // GET: /MenuManagement/



        //Menu Calendar ------------------------------------------
        public ActionResult Calendar()
        {
            //ViewBag.ListDish = menu.GetAllDish();
            return View();
        }
        // List all menu in Calendar
        public string ListAllMenuInCallendar()
        {
            IEnumerable<Menu> menu1 = menu.GetAll();
            List<EventObject> list = new List<EventObject>();
            foreach (Menu item in menu1)
            {
                EventObject eventOBJ1 = new EventObject();
                eventOBJ1.id = item.Id;
                eventOBJ1.title = item.Name;
                eventOBJ1.start = item.ApplyDate.ToString("yyyy-MM-dd 00:00:00");
                eventOBJ1.end = item.ApplyDate.ToString("yyyy-MM-dd 00:00:00");
                eventOBJ1.allDay = true;
                list.Add(eventOBJ1);
            }
            return JsonHelper.JsonSerializer<List<EventObject>>(list);
        }

        //Partial view menuDetails
        public ActionResult MenuDetails(int Id)
        {
          
            ViewBag.ListCat = menu.GetAllCatg();
            ViewBag.Menu = menu.GetMenuById(Id);
            Menu menu1 = menu.GetMenuById(Id);

            DateTime a = menu1.ClosedTimeSession;

            ViewBag.ClosedTime = a.TimeOfDay;

            var LdishInMenu = menu.GetDishByMenuId(Id);
            ViewBag.ListDishMenu = menu.GetDishCatMenu(LdishInMenu);
            var Ltest = menu.GetDishCatMenu(LdishInMenu);

            var Ldish = menu.GetDishNotInMenu(Id);
            ViewBag.GetAllDishNotInMenu = menu.CatDishNotInMenu(Ldish);

            var t = ViewBag.GetAllDishNotInMenu;
            ViewBag.ListDishId = menu.GetIdDishByMenuId(Id); 
            return PartialView();
        }
        //Partial View Menu Info:
        
        public ActionResult MenuInfo(int Id)
        {

            ViewBag.ListCat = menu.GetAllCatg();
            ViewBag.Menu = menu.GetMenuById(Id);
            Menu menu1 = menu.GetMenuById(Id);

            DateTime a = menu1.ClosedTimeSession;

            ViewBag.ClosedTime = a.TimeOfDay;

            var LdishInMenu = menu.GetDishByMenuId(Id);
            ViewBag.ListDishMenu = menu.GetDishCatMenu(LdishInMenu);

            ViewBag.ListDishInMenu = menu.GetDishByMenuId(Id);
            //ViewBag.ListDishId = menu.GetIdDishByMenuId(Id);
            return PartialView();
        }




        //Partial view CreateMenuAndAddDish
        public ActionResult MenuCreate(DateTime dt)
        {
            ViewBag.ListCat = menu.GetAllCatg();
            ViewBag.ListDish = menu.GetAllDishWithCate();
            ViewBag.MenuDate = dt;
            return PartialView("MenuCreate");
        }


        //Create menu and add dish to menu
        public String CreateListDishToMenu(string info, DateTime dt, DateTime timeClose)
        {
            try
            {
                var rs = menu.Checkdate(dt);
                if (rs)
                {
                    int menuId = menu.InsertMenu("Thực Đơn " + dt.ToString("dd/MM"), "nothing", dt.ToString(), timeClose.ToString());
                    List<DishMenuIP> listDish = JsonHelper.JsonDeserialize<List<DishMenuIP>>(info);
                    foreach (var dish in listDish)
                    {
                        menu.AddDishMenu(menuId, dish.Id, dish.Price, dish.Quota);
                    }
                    return "Success";
                }
                else return "Fail";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Fail";
        }
        //ADD Dish to Menu:
        public String AddDishToMenu(string info, int MenuId)
        {
            try
            {
                   DishMenuIP Dish = JsonHelper.JsonDeserialize<DishMenuIP>(info);
                   menu.AddDishMenu(MenuId, Dish.Id, Dish.Price, Dish.Quota);                   
                    return "Success";  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Fail";
        }

        //Edit time close :
        public string EditTimeCloseSession(int MenuID,DateTime timeClose)
        {
            try
            {
                menu.EditTimeClose(MenuID, timeClose);
                return "Success";

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message); ;
            }
            return "Fail";

        }
       

        //Delete DishMenu
        public string DeleteDishInMenu(int DishID, int MenuID)
        {
            try
            {
                menu.DeteleDishMenu(DishID, MenuID);
               
                return "Success";

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message); ;
            }
            return "Fail";

        }
        
   

        //Edit Price in DishMenu:
        public string EditPriceDishMenu(int Price, int DishID, int MenuID)
        {
            try
            {
                menu.EditPrice(Price, DishID, MenuID);
                return "Success";

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message); ;
            }
            return "Fail";

        }
        //Edit Quota in DishMenu:


        public string EditQuotaDishMenu(int quota, int DishID, int MenuID)
        {
            try
            {
                menu.EditQuota(quota, DishID, MenuID);
                return "Success";

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message); ;
            }
            return "Fail";

        }
        //Copy Menu
        public String CopyMenuInCallendar(int Id, DateTime dt)
        {
            try
            {
                Menu menuOriginal = menu.GetMenuById(Id);
                DateTime dtOr = menuOriginal.ClosedTimeSession;
                DateTime dtSession = new DateTime(dt.Year, dt.Month, dt.Day, dtOr.Hour, dtOr.Minute, dtOr.Second);

                menu.InsertMenu("Menu " + dt.ToString("dd/MM"), "nothing", dt.ToString(), dtSession.ToString());
                int menuId = menu.getIdbyDate(dt);
                menu.CopyDishFromAthotherMenu(menuId, Id);
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Fail";
        }
        //Move Menu
        public String MoveMenuInCallendar(int Id, DateTime dt)
        {
            try
            {
                menu.EditApplyDate(Id, dt);
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "Fail";
        }
        //Delete Menu
        public String DeleteMenu(int Id)
        {
            try
            {
                menu.DeleteMenu(Id);
                menu.DeleteAllDishInMenu(Id);
                return "Success";
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return "Fail";
        }
        //Pin Menu
        public String PinMenu(int Id, string title)
        {
            if (Session["MenuId"] == null)
            {
                Session.Add("MenuId", Id);
            }
            else
            {
                Session["MenuId"] = Id;
            }
            if (Session["MenuTitle"] == null)
            {
                Session.Add("MenuTitle", title);
            }
            else
            {
                Session["MenuTitle"] = title;
            }
            return "success";
        }
        //Check date is had menu or not?
        public Boolean CheckDate(DateTime dt)
        {
           
            var r = menu.Checkdate(dt);
            if (r == true)
            {
                return true;
            }
            else { return false; }

        }
        //suggest menu by time
        public ActionResult SuggestMenuByTime(DateTime star, DateTime end)
        {
            var ListOr = menu.GetListOrderByTime(star, end);
            bool isIncrease = false;


            List<SuggestDish> suggestDish = new List<SuggestDish>();
            foreach (var item in ListOr)
            {
                IEnumerable<OrderDetail> OdByOId = menu.GetOrderDetailsById(item.Id);
                foreach (OrderDetail item1 in OdByOId)
                {
                    SuggestDish Sitem = new SuggestDish();
                    if (suggestDish.Count == 0)
                    {
                        Sitem.Id = item1.DishID;
                        Sitem.Number = item1.Quantity;
                        suggestDish.Add(Sitem);
                    }
                    else
                    {
                        foreach (var menuSuggestItem in suggestDish)
                        {
                            if (menuSuggestItem.Id==item1.DishID )
                            {
                                menuSuggestItem.Number=menuSuggestItem.Number+item1.Quantity;
                                isIncrease = true;
                            }
                        }
                        if (!isIncrease)
                        {
                            Sitem.Id = item1.DishID;
                            Sitem.Number = item1.Quantity;
                             suggestDish.Add(Sitem);
                        }
                        //reset flag.
                        if (isIncrease) {
                            isIncrease = false;
                        }
                    }
                }
                foreach (var itemD in suggestDish)
                {
                   Dish dish = menu.GetDishByDishId(itemD.Id);
                   itemD.Name = dish.Name;
                   itemD.Price = dish.Price;
                   Console.WriteLine(itemD.Number);
                }
            }
            var jsonSerializer = new JavaScriptSerializer();
            var json = jsonSerializer.Serialize(suggestDish);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

      
        public ActionResult SuggestMenu()
        {
            //IEnumerable<DishMenu> ListDishMenu = menu.GetListDish(Id);
            //ViewBag.Suggest = menu.GetListSuggest();
            ViewBag.NumberYear = menu.GetNumberYear();
            return PartialView();
        }
      
      

        //sugest by qua
        public ActionResult GetSuggestByQuater(int quaNumber, int topNumber, int selectYear)
        {
            IEnumerable<DishOrderSummary> ListSummary = menu.GetSummaryByQuaNumber(quaNumber, topNumber, selectYear);
            IEnumerable<Dish> listDishByQuaNumber = menu.GetDishByQuaNumber(ListSummary);
            List<SuggestDish> listSuggestDish = new List<SuggestDish>();


            foreach (var item in listDishByQuaNumber)
            {
                SuggestDish suggestDish = new SuggestDish();
                suggestDish.Id = item.Id;
                suggestDish.Name = item.Name;
                suggestDish.Price = item.Price;

                suggestDish.Number = menu.getQuaSummaryNumber(item.Id, selectYear, quaNumber);
                listSuggestDish.Add(suggestDish);
            }
            var jsonSerializer = new JavaScriptSerializer();
            var json = jsonSerializer.Serialize(listSuggestDish);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


    }
}
