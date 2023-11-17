using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.DataAccess.Abstract
{
    public interface ISupplierRepository:IDisposable
    {
        List<Supplier> GetSupplier(string keyword, int categoryID, int page, string sortBy, string sortDirection, out int maxPage);

        List<Supplier> GetAllSupplierAvaible();

        Supplier GetSupplierById(int Id);

        IEnumerable<Supplier> GetSupplierByCategory(List<int> Id);

        List<Ingredient> GetIngredientbySupplierId(int Id);

        List<Ingredient> getIngredientbyDefaultSupplier(int supid);

        bool Insert(string name, string address, string phone, double lat, double lon, int SupCategory);

        bool Update(int Id, string Name, string Address, string Phone, double Latitude, double Longitude, int SupplierCategory);

        bool Delete(int SupplierId);

        bool ActiveSupplier(int id);

        List<SupplierCategory> GetAllCategory();

        bool checkName(int id, string name);
    }
}
