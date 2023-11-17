using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.DataAccess.Abstract;
using Domain.Utility;

namespace Domain.DataAccess.Concrete
{
    public class MenuRepository:IMenuRepository
    {
        private readonly EWCEntities _dbContext;

        public MenuRepository() {
            _dbContext = new EWCEntities();
        }

        public void Dispose() {
            if (_dbContext != null) {
                _dbContext.Dispose();
            }
        }
        #region backend
        //GetAll Menu
        public IEnumerable<Menu> GetAll()
        {
            return _dbContext.Menus.Where(m => m.IsAvailable == true).ToList();
        }
        //Get Menu by Id
        public Menu GetMenuById(int Id)
        {
            return _dbContext.Menus.Where(m => m.Id == Id && m.IsAvailable).FirstOrDefault();
        }
        //Insert Menu

        public int InsertMenu(string strname, string strDescription, string strApplydate, string strCloseTime)
        {
            Menu menu = new Menu();
            menu.Name = strname;
            menu.Description = strDescription;
            menu.ApplyDate = DateTime.Parse(strApplydate);
            menu.IsAvailable = true;
            menu.ClosedTimeSession = DateTime.Parse(strCloseTime);



            _dbContext.Menus.Add(menu);
            _dbContext.SaveChanges();
            if (menu.Id > 0)
            {
                return menu.Id;

            }
            return 0;
        }

        //Delete Menu
        public bool DeleteMenu(int Id)
        {
            Menu menu = GetMenuById(Id);

            if (menu.Id != 0)
            {
                menu.IsAvailable = false;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

   

        //add dish to menu
        public Boolean AddDishMenu(int MenuId, int DishId, int Price, int Quota)
        {
            DishMenu dishMenu = new DishMenu();
            dishMenu.MenuID = MenuId;
            dishMenu.DishID = DishId;
            dishMenu.DishPrice = Price;
            dishMenu.Quota = Quota;
            _dbContext.DishMenus.Add(dishMenu);
            _dbContext.SaveChanges();
            if (dishMenu.MenuID > 0)
            {
                return true;

            }
            return false;
        }
  
        //Get dish in menu:
        public IEnumerable<DishOfMenuDTO> GetDishByMenuId(int Id)
        {
            int year = GetYearNow();
            var Result = from T1 in _dbContext.Dishes
                         join T2 in _dbContext.DishMenus on T1.Id
                             equals T2.DishID
                         where T2.MenuID == Id
                         select new DishOfMenuDTO { dishmenus = T2, dishs = T1 };
            return Result.ToList();

        }
        public List<int> GetIdDishByMenuId(int Id)
        {
            List<int> ListId = new List<int>();
            var Result = _dbContext.DishMenus.Where(d => d.MenuID == Id).ToList();
            foreach (var item in Result)
            {
                ListId.Add(item.DishID);
            }
            return ListId;

        }
        public IEnumerable<CatDishMenuDTO> GetDishCatMenu(IEnumerable<DishOfMenuDTO> LDishOfMenu)
        {
            var Result = from T1 in LDishOfMenu
                         join T2 in _dbContext.DishCategories on T1.dishs.CategoryID
                             equals T2.Id
                         select new CatDishMenuDTO { DishMenu = T1, DishCategory = T2 };
            return Result.ToList();

        }

        // Show all dish with Category:
        public IEnumerable<CatOfDishDTO> GetAllDishWithCate()
        {
            int year = GetYearNow();
            var Result = from T1 in _dbContext.Dishes
                         join T2 in _dbContext.DishCategories on T1.CategoryID
                             equals T2.Id
                         select new CatOfDishDTO { DishCategory = T2, Dish = T1 };
            return Result.ToList();

        }
        //Get all category:
        public IEnumerable<DishCategory> GetAllCatg()
        {
            return _dbContext.DishCategories.Where(c => c.IsAvailable == true).ToList();
        }

        //Show Dish not in menu
        public IEnumerable<Dish> GetDishNotInMenu(int Id)
        {
            int year = GetYearNow();
            var result = (from T1 in _dbContext.Dishes
                          where !(from T2 in _dbContext.DishMenus
                                  where T2.MenuID == Id
                                  select T2.DishID).Contains(T1.Id)
                          select T1).ToList();
            return result;
        }
        // Get list CatDish by list dish
        public IEnumerable<CatOfDishDTO> CatDishNotInMenu(IEnumerable<Dish> LDish)
        {
            var Result = from T1 in LDish
                         join T2 in _dbContext.DishCategories on T1.CategoryID
                             equals T2.Id
                         select new CatOfDishDTO { DishCategory = T2, Dish = T1 };
            return Result.ToList();
        }

        //Delete All dish from menu
        public bool DeleteAllDishInMenu(int MenuId)
        {
            List<DishMenu> result = _dbContext.DishMenus.Where(dm => dm.MenuID == MenuId).ToList();
            if (result != null)
            {
                foreach (var i in result)
                {
                    _dbContext.DishMenus.Remove(i);
                    _dbContext.SaveChanges();
                }
                return true;
            }
            return false;
        }
        //Delete Dish in Menu
        public bool DeteleDishMenu(int DishId, int MenuId)
        {
            DishMenu result = _dbContext.DishMenus.Where(dm => dm.MenuID == MenuId && dm.DishID == DishId).FirstOrDefault();
            if (result != null)
            {
                _dbContext.DishMenus.Remove(result);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public int getIdbyDate(DateTime dt)
        {
            var menu = _dbContext.Menus.Where(m => m.ApplyDate == dt && m.IsAvailable == true).FirstOrDefault();
            return menu.Id;
        }
        //Get all DishMenu Iteam by MenuId
        public IEnumerable<DishMenu> GetlistDishMenu(int MenuId)
        {
            List<DishMenu> result = _dbContext.DishMenus.Where(dm => dm.MenuID == MenuId).ToList();
            return result;
        }
        // Copy Dish - Callendar
        public bool CopyDishFromAthotherMenu(int MenuIdOr, int MenuIdCp)
        {
            DeleteAllDishInMenu(MenuIdOr);
            var result = GetlistDishMenu(MenuIdCp);
            if (result != null)
            {
                foreach (var item in result)
                {
                    AddDishMenu(MenuIdOr, item.DishID, item.DishPrice, item.Quota);
                }
                return true;
            }

            return false;
        }
        //Edit dateTime in Menu
        public Boolean EditApplyDate(int MenuId, DateTime date)
        {
            var menu = _dbContext.Menus.Where(m => m.Id == MenuId && m.IsAvailable == true).FirstOrDefault();
            if (menu != null)
            {
                menu.Name = " Menu " + date.ToString("dd/MM");
                menu.ApplyDate = date;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        //Edit TimeClose in Menu
        public Boolean EditTimeClose(int MenuId, DateTime timeClose)
        {
            var menu = _dbContext.Menus.Where(m => m.Id == MenuId && m.IsAvailable == true).FirstOrDefault();
            if (menu != null)
            {
                menu.ClosedTimeSession = timeClose;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        //Edit Price in DishMenu:
        public Boolean EditPrice(int Price, int DishID, int MenuID)
        {
            DishMenu dishMenu = _dbContext.DishMenus.Where(d => d.MenuID == MenuID && d.DishID == DishID).FirstOrDefault();
            if (dishMenu != null)
            {
                dishMenu.DishPrice = Price;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        //Edit Quota in DishMenu:
        public Boolean EditQuota(int quota, int DishID, int MenuID)
        {
            DishMenu dishMenu = _dbContext.DishMenus.Where(d => d.MenuID == MenuID && d.DishID == DishID).FirstOrDefault();
            if (dishMenu != null)
            {
                dishMenu.Quota = quota;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }


        //Get DishById
        public Dish GetDishByDishId(int DishID)
        {
            return _dbContext.Dishes.Where(d => d.Id == DishID && d.IsAvailable == true).FirstOrDefault();

        }

        //Get number Year in suggestMenu view
        public IEnumerable<int> GetNumberYear()
        {
            List<int> ListYear = new List<int>();
            int y = GetYearNow();
            int n = y - 2013;
            int a = 2013;
            ListYear.Add(a);
            for (int i = 0; i < n; i++)
            {
                ListYear.Add(a + 1);
                a++;
            }
            return ListYear;
        }

        //Check Ord by date
        public Boolean checkOrderByDate(DateTime start, DateTime end)
        {
            IEnumerable<Order> CheckOrder = _dbContext.Orders.Where(od => od.OrderDate >= start && od.OrderDate <= end && od.OrderStatus == (int)OrderStatusEnum.Delivered).ToList();
            if (CheckOrder == null)
            {
                return false;
            }
            else { return true; }
        }

        //Get Dish by quaNumber
        public IEnumerable<DishOrderSummary> GetSummaryByQuaNumber(int quaNumber, int topNumber, int selectYear)
        {

            //IEnumerable<Dish> ListDishByQua = null;
            IEnumerable<DishOrderSummary> ListSummary = null;
            if (quaNumber == 1)
            {
                ListSummary = _dbContext.DishOrderSummaries.Where(d => d.NumOfOrderQua1 != 0 && d.Year == selectYear).OrderByDescending(d => d.NumOfOrderQua1).Take(topNumber).ToList();

            }
            else if (quaNumber == 2)
            {
                ListSummary = _dbContext.DishOrderSummaries.Where(d => d.NumOfOrderQua2 != 0 && d.Year == selectYear).OrderByDescending(d => d.NumOfOrderQua2).Take(topNumber).ToList();
            }
            else if (quaNumber == 3)
            {
                ListSummary = _dbContext.DishOrderSummaries.Where(d => d.NumOfOrderQua3 != 0 && d.Year == selectYear).OrderByDescending(d => d.NumOfOrderQua3).Take(topNumber).ToList();
            }
            else if (quaNumber == 4)
            {
                ListSummary = _dbContext.DishOrderSummaries.Where(d => d.NumOfOrderQua4 != 0 && d.Year == selectYear).OrderByDescending(d => d.NumOfOrderQua4).Take(topNumber).ToList();
            }
            return ListSummary;
        }
        public List<Dish> GetDishByQuaNumber(IEnumerable<DishOrderSummary> listSummary)
        {
            List<Dish> ListDishByQua = new List<Dish>();
            foreach (var item in listSummary)
            {
                Dish a = _dbContext.Dishes.Where(d => d.Id == item.DishId && d.IsAvailable == true).FirstOrDefault();
                ListDishByQua.Add(a);
            }
            return ListDishByQua;
        }
        public int getQuaSummaryNumber(int DishId, int selectYear, int quaNumber)
        {
            var a = _dbContext.DishOrderSummaries.Where(d => d.DishId == DishId && d.Year == selectYear).FirstOrDefault();
            int b = 0;
            if (quaNumber == 1)
            {

                b = a.NumOfOrderQua1;
            }
            else if (quaNumber == 2)
            {
                b = a.NumOfOrderQua2;
            }
            else if (quaNumber == 3)
            {
                b = a.NumOfOrderQua3;
            }
            else if (quaNumber == 4)
            {
                b = a.NumOfOrderQua4;
            }
            return b;

        }



        //Check date menu is avaiable?
        public Boolean Checkdate(DateTime dt)
        {
            Menu a = new Menu();

            a = _dbContext.Menus.Where(m => m.ApplyDate == dt && m.IsAvailable == true).FirstOrDefault();

            if (a != null)
            {
                return false;
            }
            return true;
        }
        //Get list Order by time:
        public IEnumerable<Order> GetListOrderByTime(DateTime star, DateTime end)
        {

            return _dbContext.Orders.Where(o => o.DeliveryDate >= star && o.DeliveryDate <= end && o.OrderStatus == 3).ToList();

        }
        public IEnumerable<OrderDetail> GetOrderDetailsById(int Id)
        {
            return _dbContext.OrderDetails.Where(od => od.OrderID == Id).ToList();
        }

        public int GetYearNow()
        {
            DateTime dt = DateTime.Today;
            return dt.Year;
        }


        #endregion

        #region frontend
        //Get menu by Apply date.
        public Menu GetMenuByDate(DateTime date) {
            var menu = _dbContext.Menus.Where(m => m.IsAvailable && m.ApplyDate.Equals(date.Date)).FirstOrDefault();
            return menu;
        }

        //Get future menu from today.
        public List<Menu> GetFutureMenuFromToday(int MaxNumOfMenu) {
            DateTime currentDate = DateTime.Now;
            List<Menu> result = new List<Menu>();
            List<Menu> ListFutureMenu = new List<Menu>();
            //Get today menu.
            var todayMenu = _dbContext.Menus.Where(m => m.IsAvailable && m.ApplyDate.Equals(currentDate.Date)).FirstOrDefault();
            if (todayMenu != null)
            {
                //Get maximum number of menu.
                ListFutureMenu = _dbContext.Menus.Where(m => m.IsAvailable && m.ApplyDate > todayMenu.ApplyDate).OrderBy(m => m.ApplyDate).Take(MaxNumOfMenu).ToList();
            }
            result.Add(todayMenu);
            result.AddRange(ListFutureMenu);
            return result;
        }
        #endregion
    }
}
