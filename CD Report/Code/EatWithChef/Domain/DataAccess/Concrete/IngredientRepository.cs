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
    public class IngredientRepository : IIngredientRepository
    {
        private readonly EWCEntities _dbContext;
        private const int INGREPERPAGE = 8;

        public IngredientRepository()
        {
            _dbContext = new EWCEntities();
        }

        public void Dispose() {
            if (_dbContext != null) {
                _dbContext.Dispose();
            }
        }

        public List<IngredientCategory> GetAllCategory()
        {
            try
            {
                return _dbContext.IngredientCategories.Where(s => s.IsAvailable == true).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        //Insert ingredient category..
        public int Insert(IngredientCategory icategory)
        {
            IngredientCategory ica = _dbContext.IngredientCategories.Where(d => d.Name == icategory.Name).FirstOrDefault();
            if (ica != null)
            {
                return 0;
            }

            icategory.IsAvailable = true;
            _dbContext.IngredientCategories.Add(icategory);
            _dbContext.SaveChanges();
            if (icategory.Id > 0)
            {
                return icategory.Id;
            }
            else return 0;
        }
        //Delete ingredient category.
        public int Delete(int incaId)
        {
            //Get ingredient category. to deactive.
            IngredientCategory inca = _dbContext.IngredientCategories.Where(s => s.Id == incaId).FirstOrDefault();
            if (inca != null)
            {
                inca.IsAvailable = false;
                _dbContext.SaveChanges();
                return 1;
            }
            return 0;
        }
        //Update ingredient category.
        public bool Update(IngredientCategory inca)
        {
            //Get old ingredient category to update.
            IngredientCategory Oldinca = _dbContext.IngredientCategories.Where(s => s.Id == inca.Id).FirstOrDefault();
            if (Oldinca != null)
            {
                Oldinca.Name = inca.Name;
                Oldinca.IsAvailable = true;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }


        #region Ingredient
        public List<Ingredient> GetIngredient(string keyword, int categoryID, int page, string sortBy, string sortDirection, string[] except_ingredients, out int maxPage)
        {
            List<Ingredient> result = null;
            maxPage = 1;
            try
            {
                var query = _dbContext.Ingredients.Where(d => d.Name.Contains(keyword) && d.IsAvailable == true);
                if (categoryID != 0)
                {
                    query = query.Where(d => d.Category == categoryID);
                }

                // except Ingredients
                if (except_ingredients.Length > 0 && !except_ingredients[0].Equals(""))
                {
                    try
                    {
                        List<int> except_ingredients_id = new List<int>();
                        foreach (string ingredient_str in except_ingredients)
                        {
                            except_ingredients_id.Add(Int32.Parse(ingredient_str));
                        }
                        var except_ingredients_query = _dbContext.Ingredients.Where(i => except_ingredients_id.Contains(i.Id));
                        query = query.Except(except_ingredients_query);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return null;
                    }
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
                if (page != 0)
                {
                    double ratio = (double)query.ToList().Count / (double)INGREPERPAGE;
                    maxPage = (int)Math.Ceiling(ratio);

                    int itemNeedToSkip = (page - 1) * INGREPERPAGE;
                    query = query.Skip(itemNeedToSkip).Take(INGREPERPAGE);
                }
                
                result = query.ToList();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return result;
        }
        //Get ingredient by ID
        public Ingredient GetIngredientByID(int id)
        {
            Ingredient ingredient = new Ingredient();
            try
            {
                ingredient = _dbContext.Ingredients.Where(d => d.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return ingredient;
        }
        //Get list ingredient
        public List<Ingredient> GetAllIngredient()
        {
            return _dbContext.Ingredients.ToList();
        }

        //Insert ingerdient
        public int InsertIngredient(string name, int categoryID, string imageurl, bool isTracibility, int DefaultSupplier)
        {
            Ingredient ingredient = new Ingredient();
            IngredientCategory category = new IngredientCategory();
            IngredientItem ingredientitem = new IngredientItem();
            Supplier supplier = new Supplier();
            try
            {
                category = _dbContext.IngredientCategories.Where(c => c.Id == categoryID).FirstOrDefault();
                supplier = _dbContext.Suppliers.Where(s => s.Id == DefaultSupplier).FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            if (category == null || supplier == null) return 0;
            //Add Ingredient
            ingredient.Name = name;
            ingredient.Category = categoryID;
            ingredient.ImageUrl = imageurl;
            ingredient.IsAvailable = true;
            ingredient.IsTracibility = isTracibility;
            try
            {
                _dbContext.Ingredients.Add(ingredient);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            //Add Ingredient Item
            bool isAvailable;
            if (isTracibility)
            {
                isAvailable = true;
            }
            else {
                isAvailable = false;
            }
            InsertIngredientItem(DefaultSupplier, ingredient.Id, isAvailable, true);
            return 1;
        }

        //Update Ingerdient
        public bool UpdateIngerdient(int id, string name, int categoryID, string imageurl, bool isTracibility)
        {
            Ingredient ingredient = new Ingredient();

            IngredientCategory category = new IngredientCategory();
            try
            {
                ingredient = _dbContext.Ingredients.Where(d => d.Id == id).FirstOrDefault();
                category = _dbContext.IngredientCategories.Where(c => c.Id == categoryID).FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            if (ingredient == null || category == null) return false;

            ingredient.Name = name;
            ingredient.Category = categoryID;
            ingredient.ImageUrl = imageurl;
            ingredient.IsTracibility = isTracibility;
            //ingredient.IsAvailable = Available;
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return true;
        }
        //Delete Ingredient
        public bool DeleteIngredient(int inId)
        {
            //Get ingredient to deactive.
            try
            {
                Ingredient inca = _dbContext.Ingredients.Where(s => s.Id == inId).FirstOrDefault();
                if (inca != null)
                {
                    inca.IsAvailable = false;
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                throw new Exception();            
            }
            return false;
        }
        //Check name
        public bool checkName(int id, string name)
        {
            Ingredient ingredient = new Ingredient();
            try
            {
                if (id == 0)
                {
                    ingredient = _dbContext.Ingredients.Where(d => d.Name.Equals(name)).FirstOrDefault();
                }
                else
                {
                    ingredient = _dbContext.Ingredients.Where(d => d.Name.Equals(name) && d.Id != id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            if (ingredient == null) return false;
            else return true;
        }
        #endregion


        #region IngredientItem
        //Get IngredientItem
        public IngredientItem getIngredientItem(int ingreId)
        {
            IngredientItem ingredienitem = new IngredientItem();
            try
            {
                ingredienitem = _dbContext.IngredientItems.Where(s => s.IngredientID == ingreId && s.IsDefaultSupplier == true).FirstOrDefault();
                if (ingredienitem != null) return ingredienitem;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }     
            return null;
        }
       
        //check isexist
        public bool IsExist(int supId, int ingredientId)
        {
            IngredientItem ingredienitem = new IngredientItem();
            try
            {
                ingredienitem = _dbContext.IngredientItems.Where(s => s.SupplierID == supId && s.IngredientID == ingredientId).FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            if (ingredienitem != null)
                return true;
            return false;
        }
        //Add Ingredient Item
        public bool InsertIngredientItem(int supId, int ingredientId, bool IsAvailable, bool IsDefault)
        {
            Ingredient ingredient = new Ingredient();
            Supplier supplier = new Supplier();
            IngredientItem newingredientitem = new IngredientItem();
            try
            {
                ingredient = _dbContext.Ingredients.Where(c => c.Id == ingredientId).FirstOrDefault();
                supplier = _dbContext.Suppliers.Where(s => s.Id == supId).FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            if (ingredient == null || supplier == null) return false;
            newingredientitem.SupplierID = supId;
            newingredientitem.IngredientID = ingredientId;
            newingredientitem.IsAvailable = IsAvailable;
            newingredientitem.IsDefaultSupplier = IsDefault;
            try
            {
                _dbContext.IngredientItems.Add(newingredientitem);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return true;
        }
        //update Isdefault ingredientitem
        public bool UpdateIngredientItem(int supId, int ingreId, bool IsAvailable, bool DefaultSupplier)
        {
            try
            {
                IngredientItem ingredienitem = _dbContext.IngredientItems.Where(s => s.SupplierID == supId && s.IngredientID == ingreId).FirstOrDefault();

                if (ingredienitem == null) return false;

                ingredienitem.IsDefaultSupplier = DefaultSupplier;
                ingredienitem.IsAvailable = IsAvailable;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                
            }
            return true;
        }
        //update Isavailable IngredientItem
        public bool UpdateIsAvailableIngredientItem(int supId, int ingreId, bool IsAvailable)
        {
            try
            {
                IngredientItem ingredienitem = _dbContext.IngredientItems.Where(s => s.SupplierID == supId && s.IngredientID == ingreId).FirstOrDefault();

                if (ingredienitem == null) return false;
                ingredienitem.IsAvailable = IsAvailable;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return true;
        }
        //Delete Ingredient Item
        public bool DeleteAllIngredientInSupplier(int SupId)
        {
            List<IngredientItem> result = _dbContext.IngredientItems.Where(s => s.SupplierID == SupId).ToList();
            if (result != null)
            {
                foreach (var i in result)
                {
                    i.IsAvailable = false;
                    _dbContext.SaveChanges();
                }
                return true;
            }
            return false;
        }
        //Delete All IngredientItem where DefaulSupplier
        public bool DeleteAllIngredientItemDefaulSupplier(int supid) {
            List<IngredientItem> result = _dbContext.IngredientItems.Where(s => s.SupplierID == supid).ToList();
            if (result != null)
            {
                foreach (var i in result)
                {
                    i.IsAvailable = false;
                    i.IsDefaultSupplier = false;
                    _dbContext.SaveChanges();
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
