using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.DataAccess.Abstract
{
    public interface IIngredientRepository:IDisposable
    {
        List<IngredientCategory> GetAllCategory();

        int Insert(IngredientCategory icategory);

        int Delete(int incaId);

        bool Update(IngredientCategory inca);

        List<Ingredient> GetIngredient(string keyword, int categoryID, int page, string sortBy, string sortDirection, string[] except_ingredients, out int maxPage);

        Ingredient GetIngredientByID(int id);

        List<Ingredient> GetAllIngredient();

        int InsertIngredient(string name, int categoryID, string imageurl, bool isTracibility, int DefaultSupplier);

        bool UpdateIngerdient(int id, string name, int categoryID, string imageurl, bool isTracibility);

        bool DeleteIngredient(int inId);

        bool checkName(int id, string name);

        IngredientItem getIngredientItem(int ingreId);

        bool IsExist(int supId, int ingredientId);

        bool InsertIngredientItem(int supId, int ingredientId, bool IsAvailable, bool IsDefault);

        bool UpdateIngredientItem(int supId, int ingreId, bool IsAvailable, bool DefaultSupplier);

        bool UpdateIsAvailableIngredientItem(int supId, int ingreId, bool IsAvailable);

        bool DeleteAllIngredientInSupplier(int SupId);

        bool DeleteAllIngredientItemDefaulSupplier(int supid);
    }
}
