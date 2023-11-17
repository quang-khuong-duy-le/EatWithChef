using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.DataAccess.Abstract
{
    public interface IMenuRepository:IDisposable
    {
        #region backend 
        /// <summary>
        /// Get menu by menu id.
        /// </summary>
        /// <param name="Id">Id of menu</param>
        /// <returns>Menu which have id input from parameter</returns>
        Menu GetMenuById(int Id);

        IEnumerable<Menu> GetAll();

        int InsertMenu(string strname, string strDescription, string strApplydate, string strCloseTime);

        bool DeleteMenu(int Id);

        //bool Update(Menu menu);

        //IEnumerable<DishMenu> GetListDish(int Id);

        Boolean AddDishMenu(int MenuId, int DishId, int Price, int Quota);

        //Boolean SaveDishToMenu(int MenuId, int DishId, int Price);

        IEnumerable<DishOfMenuDTO> GetDishByMenuId(int Id);

        List<int> GetIdDishByMenuId(int Id);

        IEnumerable<CatDishMenuDTO> GetDishCatMenu(IEnumerable<DishOfMenuDTO> LDishOfMenu);

        IEnumerable<CatOfDishDTO> GetAllDishWithCate();

        IEnumerable<DishCategory> GetAllCatg();

        IEnumerable<Dish> GetDishNotInMenu(int Id);

        IEnumerable<CatOfDishDTO> CatDishNotInMenu(IEnumerable<Dish> LDish);

        //bool DeleteDishInMenu(int MenuId, int DishId);

        //List<Dish> GetListDishByStringSearch(string a);

        bool DeleteAllDishInMenu(int MenuId);

        bool DeteleDishMenu(int DishId, int MenuId);

        IEnumerable<DishMenu> GetlistDishMenu(int MenuId);

        //IEnumerable<Dish> GetAllDish();

        int getIdbyDate(DateTime dt);

        bool CopyDishFromAthotherMenu(int MenuIdOr, int MenuIdCp);

        Boolean EditApplyDate(int MenuId, DateTime date);

        Boolean EditTimeClose(int MenuId, DateTime timeClose);

        Boolean EditPrice(int Price, int DishID, int MenuID);

        Boolean EditQuota(int quota, int DishID, int MenuID);

        Dish GetDishByDishId(int DishID);

        IEnumerable<int> GetNumberYear();

        Boolean checkOrderByDate(DateTime start, DateTime end);

        IEnumerable<DishOrderSummary> GetSummaryByQuaNumber(int quaNumber, int topNumber, int selectYear);

        List<Dish> GetDishByQuaNumber(IEnumerable<DishOrderSummary> listSummary);

        int getQuaSummaryNumber(int DishId, int selectYear, int quaNumber);

        Boolean Checkdate(DateTime dt);

        IEnumerable<Order> GetListOrderByTime(DateTime star, DateTime end);

        IEnumerable<OrderDetail> GetOrderDetailsById(int Id);

        #endregion

        #region frontend
        /// <summary>
        /// Get menu by date input.
        /// </summary>
        /// <param name="date">Input date.</param>
        /// <returns>Menu which have publish date equal input date from parameter.</returns>
        Menu GetMenuByDate(DateTime date);

        /// <summary>
        /// Get future menu from to day.
        /// </summary>
        /// <param name="MaxNumOfMenu">Number of maximum menu to load (current context MaxNumOfMenu = 7)</param>
        /// <returns>List MaxNumOfMenu future menu from today</returns>
        List<Menu> GetFutureMenuFromToday(int MaxNumOfMenu);
        #endregion
    }
}
