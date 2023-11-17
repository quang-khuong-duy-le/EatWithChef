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
    public class DishRepository : IDishRepository
    {
        private readonly EWCEntities _dbContext;
        private const int DISHPERPAGE = 8;

        public DishRepository()
        {
            _dbContext = new EWCEntities();
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }

        //Get Dish category.
        public List<DishCategory> GetAllCategory()
        {
            var result = _dbContext.DishCategories.Where(dc => dc.IsAvailable).ToList();
            return result;
        }

        //Get list dish by menu id.
        public List<Dish> GetDishByMenuId(int MenuId)
        {
            var ListDishMenu = _dbContext.DishMenus.Where(d => d.MenuID == MenuId).ToList();
            List<Dish> ListDish = new List<Dish>();
            if (ListDishMenu != null)
            {
                foreach (var item in ListDishMenu)
                {
                    var DishObjectItem = _dbContext.Dishes.Where(d => d.Id == item.DishID && d.IsAvailable).FirstOrDefault();
                    ListDish.Add(DishObjectItem);
                }
                return ListDish;
            }
            return null;
        }

        //Get dish by id.
        public Dish GetDishByID(int id)
        {
            Dish dish = new Dish();
            try
            {
                dish = _dbContext.Dishes.Where(d => d.Id == id && d.IsAvailable).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
            return dish;
        }

        //Get price from DishMenu by dish id and menu id.
        public int GetPriceFromDishMenu(int DishID, int MenuID)
        {
            var DishMenu = _dbContext.DishMenus.Where(d => d.DishID == DishID && d.MenuID == MenuID).FirstOrDefault();
            if (DishMenu != null)
            {
                return DishMenu.DishPrice;
            }
            else
            {
                return 0;
            }
        }

        //Get tags by dish id.
        public List<Tag> GetDishTags(int DishID)
        {
            var result = (from listTag in _dbContext.Tags
                          where listTag.Dishes.Any(d => d.Id == DishID)
                          select listTag).ToList();
            return result;
        }

        //Get all dish tags from database.
        public List<Tag> GetAllTag()
        {
            var result = _dbContext.Tags.ToList();
            return result;
        }

        //Check dish quota.
        public bool CheckDishQuota(int DishID, int MenuID, int CheckNumber)
        {
            //1. Get DishMenu by Dish Id and Menu Id.
            //2. Compare Quota > CheckNumber --> return true else --> return false.

            //1. Get DishMenu by Dish Id and Menu Id.
            var DishMenu = _dbContext.DishMenus.Where(d => d.DishID == DishID && d.MenuID == MenuID).FirstOrDefault();
            if (DishMenu != null)
            {
                //2. Compare Quota > CheckNumber --> return true else --> return false.
                if (DishMenu.Quota >= CheckNumber)
                {
                    return true;
                }
            }
            return false;
        }

        //Get dish by chef id.
        public IEnumerable<Dish> GetDishByChefId(int ChefId) {
            var ListDish = _dbContext.Dishes.Where(d => d.IsAvailable && d.ChefID == ChefId);
            return ListDish;
        }

        public bool CheckDishName(string name, int dishid)
        {
            Dish dish = new Dish();
            string url = ConvertStringHelper.ConvertShortName(name);
            try
            {
                if (dishid == 0)
                {
                    dish = _dbContext.Dishes.Where(d => (d.Name.Equals(name) || d.Url.Equals(url)) && d.IsAvailable).FirstOrDefault();
                }
                else
                {
                    dish = _dbContext.Dishes.Where(d => (d.Name.Equals(name) || d.Url.Equals(url)) && d.Id != dishid && d.IsAvailable).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            if (dish == null) return true;
            else return false;
        }

        #region backend
        //---------------- Dish ----------------------
        public List<Dish> GetDish(string keyword, int categoryID, int page, string sortBy, string sortDirection, out int maxPage)
        {
            List<Dish> result = null;
            maxPage = 1;
            try
            {
                var query = _dbContext.Dishes.AsEnumerable().Where(d => (d.UnsignName.Contains(keyword.ToLower()) || d.DishCategory.UnsignName.Contains(keyword) || d.Tags.Any(t => t.UnsignName.Contains(keyword.ToLower()))) && d.IsAvailable);
                if (categoryID != 0)
                {
                    query = query.Where(d => d.CategoryID == categoryID);
                }

                // sort
                string sortStr = "";
                if (sortDirection.Equals("ascending"))
                {
                    sortStr = sortBy + " " + "ASC";
                }
                else if (sortDirection.Equals("descending"))
                {
                    sortStr = sortBy + " " + "DESC";
                }
                query = query.OrderBy(sortStr);

                // paging
                double ratio = (double)query.ToList().Count / (double)DISHPERPAGE;
                maxPage = (int)Math.Ceiling(ratio);

                int itemNeedToSkip = (page - 1) * DISHPERPAGE;
                query = query.Skip(itemNeedToSkip).Take(DISHPERPAGE);

                result = query.ToList();
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }


        public bool InsertDish(string name, int categoryID, int price, string description, string cookingGuide, int rate, int chefID, string image, string tagList, string[] selected_ingredient)
        {
            Dish dish = new Dish();

            DishCategory category = new DishCategory();
            Chef chef = new Chef();
            try
            {
                category = _dbContext.DishCategories.Where(c => c.Id == categoryID).FirstOrDefault();
                chef = _dbContext.Chefs.Where(c => c.UserID == chefID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            if (category == null || chef == null) return false;
            dish.Name = name;
            dish.DishCategory = category;
            dish.Chef = chef;
            dish.Price = price;
            if (description.Equals(""))
            {
                dish.Description = "Chưa có mô tả";
            }
            else
            {
                dish.Description = description;
            }
            if (cookingGuide.Equals(""))
            {
                dish.CookingGuide = "Chưa có hướng dẫn";
            }
            else
            {
                dish.CookingGuide = cookingGuide;
            }
            dish.Rate = 0.0;
            dish.NumberOfLike = 0;
            dish.NumberOfRate = 0;
            dish.IsAvailable = true;
            dish.Image = image;
            dish.Url = ConvertStringHelper.ConvertShortName(name);

            // add Tag to Dish
            if (!tagList.Equals(""))
            {
                foreach (string tagName in tagList.Split(','))
                {
                    // get Tag 
                    Tag tag = GetOrInsertTag(tagName);
                    dish.Tags.Add(tag);
                }
            }
            // add Ingredient to Dish
            IngredientRepository ingredientRepository = new IngredientRepository();
            foreach (string ingredient_id_str in selected_ingredient)
            {
                Ingredient ingredient = new Ingredient();
                try
                {
                    int ingredient_id = Int32.Parse(ingredient_id_str);
                    ingredient = _dbContext.Ingredients.Where(i => i.Id == ingredient_id).FirstOrDefault();
                    if (ingredient == null) return false;
                    dish.Ingredients.Add(ingredient);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            try
            {
                _dbContext.Dishes.Add(dish);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool UpdateDish(int id, string name, int categoryID, int price, string description, string cookingGuide, int rate, string image, string tagList, string[] selected_ingredient)
        {
            Dish dish = new Dish();

            DishCategory category = new DishCategory();
            Chef chef = new Chef();
            try
            {
                dish = _dbContext.Dishes.Where(d => d.Id == id).FirstOrDefault();
                category = _dbContext.DishCategories.Where(c => c.Id == categoryID).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            if (dish == null || category == null || chef == null) return false;

            dish.Name = name;
            dish.DishCategory = category;
            dish.Price = price;
            if (description.Equals(""))
            {
                dish.Description = "Chưa có mô tả";
            }
            else
            {
                dish.Description = description;
            }
            if (cookingGuide.Equals(""))
            {
                dish.CookingGuide = "Chưa có hướng dẫn";
            }
            else
            {
                dish.CookingGuide = cookingGuide;
            }
            dish.Rate = 0;
            dish.IsAvailable = true;
            dish.Image = image;
            dish.Url = ConvertStringHelper.ConvertShortName(name);
            // update Tag
            dish.Tags.Clear();
            if (!tagList.Equals(""))
            {
                foreach (string tagName in tagList.Split(','))
                {
                    // get Tag 
                    Tag tag = GetOrInsertTag(tagName);
                    dish.Tags.Add(tag);
                }
            }

            // add Ingredient to Dish
            IngredientRepository ingredientRepository = new IngredientRepository();
            dish.Ingredients.Clear();
            foreach (string ingredient_id_str in selected_ingredient)
            {
                Ingredient ingredient = new Ingredient();
                try
                {
                    int ingredient_id = Int32.Parse(ingredient_id_str);
                    ingredient = _dbContext.Ingredients.Where(i => i.Id == ingredient_id).FirstOrDefault();
                    if (ingredient == null) return false;
                    dish.Ingredients.Add(ingredient);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool DeleteDish(int id)
        {
            Dish dish = new Dish();
            try
            {
                dish = _dbContext.Dishes.Where(d => d.Id == id).FirstOrDefault();
                dish.IsAvailable = false;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        //---------------- end Dish ------------------


        //---------------- DishCategory --------------
        public List<DishCategory> SearchDishCategoryByName(string name)
        {
            List<DishCategory> result = null;
            try
            {
                result = _dbContext.DishCategories.Where(dc => name.Contains(dc.Name) && dc.IsAvailable).ToList();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return result;
        }


        public List<DishCategory> GetAllDishCategory()
        {
            List<DishCategory> result = null;
            try
            {
                result = _dbContext.DishCategories.Where(dc => dc.IsAvailable).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            return result;
        }

        public DishCategory GetDishCategoryByID(int id)
        {
            DishCategory result = null;
            try
            {
                result = _dbContext.DishCategories.Where(dc => dc.Id == id && dc.IsAvailable).FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return result;
        }

        public bool InsertDishCategory(string name, string description, string image)
        {
            DishCategory dc = null;
            try
            {
                dc = _dbContext.DishCategories.Where(d => d.Name == name && d.IsAvailable).FirstOrDefault();
                if (dc != null) return false;
                dc = new DishCategory();
                dc.Name = name;
                dc.Description = description;
                dc.Image = image;
                dc.IsAvailable = true;
                _dbContext.DishCategories.Add(dc);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        public bool UpdateDishCategory(int id, string name, string description, string image)
        {
            DishCategory dc = null;
            try
            {
                dc = _dbContext.DishCategories.Where(d => d.Id == id && d.IsAvailable).FirstOrDefault();
                if (dc == null) return false;
                dc.Name = name;
                dc.Description = description;
                dc.Image = image;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool DeleteDishCategory(int id)
        {
            DishCategory dc = null;
            try
            {
                dc = _dbContext.DishCategories.Where(d => d.Id == id && d.IsAvailable).FirstOrDefault();
                if (dc == null) return false;
                dc.IsAvailable = false;
                foreach (Dish dish in dc.Dishes)
                {
                    dish.IsAvailable = false;
                }
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool CheckDishCategoryName(string name, int categoryid)
        {
            DishCategory category = new DishCategory();
            try
            {
                if (categoryid == 0)
                {
                    category = _dbContext.DishCategories.Where(d => d.Name.Equals(name) && d.IsAvailable).FirstOrDefault();
                }
                else
                {
                    category = _dbContext.DishCategories.Where(d => d.Name.Equals(name) && d.Id != categoryid && d.IsAvailable).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            if (category == null) return true;
            else return false;
        }
        //---------------- end DishCateory -----------


        //---------------- Tag -----------------------
        public List<Tag> GetTagsByName(string name)
        {
            List<Tag> result = new List<Tag>();
            try
            {
                result = _dbContext.Tags.Where(t => t.Name.ToLower().Contains(name.ToLower())).ToList();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return result;
        }

        public Tag GetOrInsertTag(string name)
        {
            Tag tag = null;
            try
            {
                tag = _dbContext.Tags.Where(t => t.Name == name).FirstOrDefault();
                if (tag != null) return tag;
                tag = new Tag();
                tag.Name = name;
                _dbContext.Tags.Add(tag);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return tag;
        }

        public bool RemoveTagFromDish(int dishID, int tagID)
        {
            Tag tag = null;
            try
            {
                tag = _dbContext.Tags.Where(t => t.Id == tagID).FirstOrDefault();
                if (tag == null) return false;

                Dish dish = _dbContext.Dishes.Where(d => d.Id == dishID && d.IsAvailable).FirstOrDefault();
                if (dish == null) return false;
                dish.Tags.Remove(tag);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return true;
        }
        //---------------- end Tag -------------------
        #endregion

        #region frontend
        //Load dish from menu by menuID.
        public List<Dish> LoadDishFromMenu(int menuID)
        {
            var ListDishMenu = _dbContext.DishMenus.Where(d => d.MenuID == menuID).ToList();
            List<Dish> ListDish = new List<Dish>();
            if (ListDishMenu != null)
            {
                foreach (var item in ListDishMenu)
                {
                    var DishObjectItem = _dbContext.Dishes.Where(d => d.Id == item.DishID && d.IsAvailable).FirstOrDefault();
                    ListDish.Add(DishObjectItem);
                }
                return ListDish;
            }
            else
            {
                return null;
            }
        }

        //Get List price by id.
        public DishPriceDTO GetDishPrice(int DishID, int MenuID)
        {
            DishPriceDTO result = new DishPriceDTO();
            var DishObject = _dbContext.Dishes.Where(d => d.Id == DishID && d.IsAvailable).FirstOrDefault();
            result.DishID = DishID;
            result.DishName = DishObject.Name;
            //get price in DishMenu by dish id and menu id
            var DishMenu = _dbContext.DishMenus.Where(d => d.DishID == DishID && d.MenuID == MenuID).FirstOrDefault();
            if (DishMenu != null)
            {
                //If have dish in menu --> get menu price.
                result.Price = DishMenu.DishPrice;
            }
            else
            {
                //If not have dish in menu --> get dish price.
                if (DishObject != null)
                {
                    result.Price = DishObject.Price;
                }
                else
                {
                    result.Price = 0;
                }
            }
            return result;
        }

        //Get tag list by dish id.
        public List<Tag> GetTagByDishID(int DishID)
        {
            var ListTag = (from tag in _dbContext.Tags
                           where tag.Dishes.Any(d => d.Id == DishID && d.IsAvailable)
                           select tag).ToList();
            return ListTag;
        }

        #endregion
    }
}
