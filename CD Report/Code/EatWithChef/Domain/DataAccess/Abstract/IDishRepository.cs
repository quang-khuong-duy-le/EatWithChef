using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.DataAccess.Abstract
{
    public interface IDishRepository:IDisposable
    {
        /// <summary>
        /// Get all available dish category
        /// </summary>
        /// <returns>List available dish category</returns>
        List<DishCategory> GetAllCategory();

        /// <summary>
        /// Get all dish from menu which have menu id input
        /// </summary>
        /// <param name="MenuId">MenuId</param>
        /// <returns>List dish in menu which have id input.</returns>
        List<Dish> GetDishByMenuId(int MenuId);

        /// <summary>
        /// Get dish object by id.
        /// </summary>
        /// <param name="Id">Dish id</param>
        /// <returns>Dish entity which have id input.</returns>
        Dish GetDishByID(int Id);

        /// <summary>
        /// Get dish price in menu.
        /// </summary>
        /// <param name="DishID">Dish id</param>
        /// <param name="MenuID">Menu id</param>
        /// <returns>Price of dish in menu</returns>
        int GetPriceFromDishMenu(int DishID, int MenuID);

        /// <summary>
        /// Get all tag of dish.
        /// </summary>
        /// <param name="DishID">Dish id parameter.</param>
        /// <returns>List tag of dish which has id input</returns>
        List<Tag> GetDishTags(int DishID);

        /// <summary>
        /// Get all dish tags from database.
        /// </summary>
        /// <returns>All list tags.</returns>
        List<Tag> GetAllTag();

        /// <summary>
        /// Check dish quota when add to cart and check out.
        /// </summary>
        /// <param name="DishID">Id of dish</param>
        /// <param name="MenuID">Id of menu</param>
        /// <param name="CheckNumber">Number to check.</param>
        /// <returns></returns>
        bool CheckDishQuota(int DishID, int MenuID, int CheckNumber);

        /// <summary>
        /// Get all available dish design by chef input.
        /// </summary>
        /// <param name="ChefId">Id of Chef</param>
        /// <returns>List dish which are design by chef input.</returns>
        IEnumerable<Dish> GetDishByChefId(int ChefId);


        bool CheckDishName(string name, int dishid);


        List<Dish> GetDish(string keyword, int categoryID, int page, string sortBy, string sortDirection, out int maxPage);


        bool InsertDish(string name, int categoryID, int price, string description, string cookingGuide, int rate, int chefID, string image, string tagList, string[] selected_ingredient);


        bool UpdateDish(int id, string name, int categoryID, int price, string description, string cookingGuide, int rate, string image, string tagList, string[] selected_ingredient);


        bool DeleteDish(int id);


        List<DishCategory> SearchDishCategoryByName(string name);


        DishCategory GetDishCategoryByID(int id);


        bool InsertDishCategory(string name, string description, string image);


        bool UpdateDishCategory(int id, string name, string description, string image);


        bool DeleteDishCategory(int id);


        bool CheckDishCategoryName(string name, int categoryid);

        List<DishCategory> GetAllDishCategory();

        List<Tag> GetTagsByName(string name);

        Tag GetOrInsertTag(string name);

        bool RemoveTagFromDish(int dishID, int tagID);
    }
}
