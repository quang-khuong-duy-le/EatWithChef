using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.DataAccess.Abstract
{
    public interface IProduceSessionRepository:IDisposable
    {
        List<DishItem> GetDishItemByDate(DateTime date);

        bool DeleteDishItem(int dishItemId);

        Menu GetMenuByDate(DateTime date);

        List<Dish> LoadDishFromMenu(int menuID);

        DishItem GetDishItemById(int dishItemId);

        int CalculateNumberProducedDishItemOfMenu(Menu menu, int dishID);

        int CalculateNumberOrderedDishItem(Menu menu, int dishID);

        List<IngredientItem> GetAvailableIngredientItemForDish(int dishId);

        DishItem ProduceDishItem(int dishID, int quantity, string ingredients_str);

        bool ProduceSessionGenerate(List<ProduceSessionGenerateDTO> ListProduceSessionGenerate, string serverPath);

        bool DeleteDishItemPermanent(DishItem dishItem);

        bool CreateQRCodeForDishItem(DishItem dishItem, string path);
    }
}
