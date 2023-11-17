using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EatWithChef.Areas.Ecommerce.Models;
using Domain.Entity;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;

namespace EatWithChef.Areas.Ecommerce.Controllers
{
    public class MenuController : Controller
    {
        private IMenuRepository _menuRepository;
        private IDishRepository _dishRepository;
        private IChefRepository _chefRepository;

        public MenuController() {
            _menuRepository = new MenuRepository();
            _dishRepository = new DishRepository();
            _chefRepository = new ChefRepository();
        }


        //Private method get dish view by menu Id.
        private List<DishCategoryViewModel> GetDishViewByMenuId(int menuID)
        {
            List<Dish> ListDish = new List<Dish>();
            List<DishCategoryViewModel> ListDishByCategory = new List<DishCategoryViewModel>();
            //Load menu.
            var menu = _menuRepository.GetMenuById(menuID);

            if (menu != null)
            {
                ListDish = _dishRepository.GetDishByMenuId(menu.Id);
                if (ListDish != null)
                {
                    //Get all dish category.
                    List<DishCategory> ListCategory = _dishRepository.GetAllCategory();
                    if (ListCategory != null)
                    {
                        foreach (var item in ListCategory)
                        {
                            DishCategoryViewModel DishCategoryItem = new DishCategoryViewModel();
                            DishCategoryItem.CategoryID = item.Id;
                            DishCategoryItem.CategoryName = item.Name;
                            ListDishByCategory.Add(DishCategoryItem);
                        }
                        foreach (var item in ListDish)
                        {
                            //Add DishViewModel to category.
                            for (int i = 0; i < ListDishByCategory.Count; i++)
                            {
                                if (item.CategoryID == ListDishByCategory.ElementAt(i).CategoryID)
                                {
                                    DishViewModel DishViewModelItem = new DishViewModel();
                                    DishViewModelItem.DishID = item.Id;
                                    DishViewModelItem.DishImage = item.Image;
                                    DishViewModelItem.DishName = item.Name;
                                    DishViewModelItem.DishRate = item.Rate;
                                    DishViewModelItem.Quota = item.DishMenus.Where(d => d.MenuID == menuID).FirstOrDefault().Quota;
                                    DishViewModelItem.DishDescription = item.Description;
                                    DishViewModelItem.DishPrice = _dishRepository.GetPriceFromDishMenu(item.Id, menu.Id);
                                    DishViewModelItem.ChefName = _chefRepository.GetChefProfileById(item.ChefID).ChefName;
                                    DishViewModelItem.ImageURL = item.Chef.ImageURL;
                                    var ListTag = _dishRepository.GetDishTags(item.Id);
                                    string ListTagID = "";
                                    if (ListTag != null)
                                    {
                                        foreach (var tag in ListTag)
                                        {
                                            ListTagID += tag.Id + " ";
                                        }
                                    }
                                    DishViewModelItem.ListTagID = ListTagID;
                                    //Add  dish view model to category.
                                    ListDishByCategory.ElementAt(i).ListDish.Add(DishViewModelItem);
                                }
                            }
                        }
                    }
                }
                ViewBag.MenuDate = menu.ApplyDate;
                ViewBag.MenuID = menuID;
            }
            return ListDishByCategory;
        }

        public ActionResult Index(int menuID = 0)
        {
            List<DishCategoryViewModel> ListDishByCategory = new List<DishCategoryViewModel>();

            if (menuID > 0)
            {
                ListDishByCategory = GetDishViewByMenuId(menuID);
            }
            else
            {
                //Load today menu.
                var menu = _menuRepository.GetMenuByDate(DateTime.Now);

                if (menu != null)
                {
                    ViewBag.Menu = menu.Id;
                    ListDishByCategory = GetDishViewByMenuId(menu.Id);
                }
            }
            return View(ListDishByCategory);
        }

        //Get future menu from today, maximum 7 menus.
        public ActionResult GetListFutureMenu(DateTime MenuDay)
        {
            var result = _menuRepository.GetFutureMenuFromToday(7);
            ViewBag.MenuDate = MenuDay;
            return PartialView("ListFutureMenu", result);
        }

        //Get dish category.
        public ActionResult GetAllDishCategory()
        {
            var ListDishCategory = _dishRepository.GetAllCategory();
            return PartialView("DishCategoryPartial", ListDishCategory);
        }

        //Get all tags to filter.
        public ActionResult GetAllTags()
        {
            var AllTags = _dishRepository.GetAllTag();
            return PartialView("TagsFilterPartial", AllTags);
        }

        //Get dish by menu ID partial.
        public ActionResult GetDishByMenuIDPartial(int MenuId)
        {
            List<DishCategoryViewModel> ListDishByCategory = new List<DishCategoryViewModel>();
            ListDishByCategory = GetDishViewByMenuId(MenuId);
            if (MenuId > 0)
            {
                ViewBag.Menu = MenuId;
            }
            return PartialView("ListDishCategoryPartial", ListDishByCategory);
        }

        //Get dish by menu ID view.
        public ActionResult GetDishByMenuID(int MenuId) {
            List<DishCategoryViewModel> ListDishByCategory = new List<DishCategoryViewModel>();
            ListDishByCategory = GetDishViewByMenuId(MenuId);
            ViewBag.Menu = MenuId;
            return View("Index", ListDishByCategory);
        }
    }
}
