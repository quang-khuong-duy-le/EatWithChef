using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DataAccess.Abstract;
using Domain.Entity;
using Domain.Utility;

namespace Domain.DataAccess.Concrete
{
    public class SupplierRepository:ISupplierRepository
    {
        private readonly EWCEntities _dbContext;
        private const int SUPPERPAGE = 7;

        public SupplierRepository()
        {
            _dbContext = new EWCEntities();
        }

        public void Dispose() {
            if (_dbContext != null) {
                _dbContext.Dispose();
            }
        }

        //Get Supplier after sort
        public List<Supplier> GetSupplier(string keyword, int categoryID, int page, string sortBy, string sortDirection, out int maxPage)
        {
            List<Supplier> result = null;
            try
            {
                var query = _dbContext.Suppliers.Where(d => d.Name.Contains(keyword));
                if (categoryID != 0)
                {
                    query = query.Where(d => d.SupplierCategory == categoryID);
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
                double ratio = (double)query.ToList().Count / (double)SUPPERPAGE;
                maxPage = (int)Math.Ceiling(ratio);

                int itemNeedToSkip = (page - 1) * SUPPERPAGE;
                query = query.Skip(itemNeedToSkip).Take(SUPPERPAGE);

                result = query.ToList();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return result;
        }

        //Get all Supplier.
        public List<Supplier> GetAllSupplierAvaible()
        {
            try
            {
                return _dbContext.Suppliers.Where(s => s.IsAvailable == true).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        //Get Supplier by id.
        public Supplier GetSupplierById(int Id)
        {
            Supplier supplier = new Supplier();
            supplier = _dbContext.Suppliers.Where(s => s.Id == Id).FirstOrDefault();
            return supplier;
        }
        //Get Supplier by Category
        public IEnumerable<Supplier> GetSupplierByCategory(List<int> Id)
        {
            var supplier = _dbContext.Suppliers.Where(s => Id.Contains(s.SupplierCategory));
            return supplier;
        }
        //get IngredientSupplier
        public List<Ingredient> GetIngredientbySupplierId(int Id)
        {
            //var result = (from i in _dbContext.Ingredients
            //              where i.Suppliers.Any(s => s.Id == Id)
            //              select i).ToList();
            var Result = from T1 in _dbContext.Ingredients
                         join T2 in _dbContext.IngredientItems on T1.Id
                             equals T2.IngredientID
                         where T2.SupplierID == Id && T2.IsAvailable == true && T1.IsAvailable == true
                         select T1;
            return Result.ToList();
        }
        //Get Ingredient not in Supplier
        //public List<Ingredient> GetIngredientNotInSup(int Id)
        //{
        //    //var result = (from i in _dbContext.Ingredients
        //    //              where !(i.Suppliers.Any(s => s.Id == Id))
        //    //              select i).ToList();
        //    //return result;
        //    var result = from T1 in _dbContext.Ingredients where T1.IsAvailable == true
        //                 where !(from T2 in _dbContext.IngredientItems
        //                         where T2.SupplierID == Id && T2.IsAvailable == true
        //                         select T2.IngredientID).Contains(T1.Id) 
        //                 select T1;
        //    return result.ToList();
        //}
        //Paging Ingredient by SupId
        //public List<Ingredient> GetIngredientbySupPaging(int Id,string keyword, int categoryID, string sortBy, string sortDirection)
        //{
        //    List<Ingredient> result = null;
        //    try
        //    {
        //        var query = from T1 in _dbContext.Ingredients
        //                    where T1.IsAvailable == true
        //                    where !(from T2 in _dbContext.IngredientItems
        //                            where T2.SupplierID == Id && T2.IsAvailable == true
        //                            select T2.IngredientID).Contains(T1.Id)
        //                    select T1;                
        //        query = query.Where(d => d.Name.Contains(keyword));
        //        if (categoryID != 0)
        //        {
        //            query = query.Where(d => d.IngredientCategory.Id == categoryID);
        //        }

        //        // sort
        //        string sortStr = "";
        //        if (sortDirection.Equals("ascending"))
        //        {
        //            sortStr = sortBy + " " + "ASC";
        //        }
        //        else if (sortDirection.Equals("descending"))
        //        {
        //            sortStr = sortBy + " " + "DESC";
        //        }
        //        query = query.OrderBy(sortStr);



        //        result = query.ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception();
        //    }
        //    return result;
        //}

        //Get Ingredient by Default Supplier
        public List<Ingredient> getIngredientbyDefaultSupplier(int supid)
        {
            var Result = from T1 in _dbContext.Ingredients
                         join T2 in _dbContext.IngredientItems on T1.Id
                             equals T2.IngredientID
                         where T2.SupplierID == supid && T2.IsAvailable == true && T2.IsDefaultSupplier == true
                         select T1;
            return Result.ToList();
        }
        //Insert Supplier.
        public bool Insert(string name, string address, string phone, double lat, double lon, int SupCategory)
        {
            Supplier supplier = new Supplier();
            supplier.Name = name;
            supplier.Address = address;
            supplier.Phone = phone;
            supplier.Latitude = lat;
            supplier.Longitude = lon;
            supplier.SupplierCategory = SupCategory;
            supplier.IsAvailable = true;
            try
            {
                _dbContext.Suppliers.Add(supplier);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            if (supplier.Id > 0)
            {
                return true;
            }
            return false;
        }

        //Update Supplier.
        public bool Update(int Id, string Name, string Address, string Phone, double Latitude, double Longitude, int SupplierCategory)
        {
            //Get old supplier to update.
            Supplier OldSupplier = _dbContext.Suppliers.Where(s => s.Id == Id).FirstOrDefault();
            if (OldSupplier != null)
            {
                OldSupplier.Name = Name;
                OldSupplier.Phone = Phone;
                OldSupplier.Latitude = Latitude;
                OldSupplier.Longitude = Longitude;
                OldSupplier.Address = Address;
                OldSupplier.SupplierCategory = SupplierCategory;
                //OldSupplier.IsAvailable = IsAvailable;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        //Delete Supplier.
        public bool Delete(int SupplierId)
        {
            //Get supplier to deactive.
            Supplier supplier = _dbContext.Suppliers.Where(s => s.Id == SupplierId).FirstOrDefault();
            if (supplier != null)
            {
                supplier.IsAvailable = false;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        //active Supplier
        public bool ActiveSupplier(int id)
        {
            //Get supplier to active.
            Supplier supplier = _dbContext.Suppliers.Where(s => s.Id == id).FirstOrDefault();
            if (supplier != null)
            {
                supplier.IsAvailable = true;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        //Get category
        public List<SupplierCategory> GetAllCategory()
        {
            try
            {
                return _dbContext.SupplierCategories.Where(s => s.IsAvailable == true).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        //Check Name
        public bool checkName(int id, string name)
        {
            Supplier supplier = new Supplier();
            try
            {
                if (id == 0)
                {
                    supplier = _dbContext.Suppliers.Where(d => d.Name.Equals(name)).FirstOrDefault();
                }
                else
                {
                    supplier = _dbContext.Suppliers.Where(d => d.Name.Equals(name) && d.Id != id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            if (supplier == null) return false;
            else return true;
        }

        ////Delete All Ingredient in Supplier
        //public bool DeleteAllIngredientInSupplier(int SupId)
        //{
        //    //Supplier supplier = (from s in _dbContext.Suppliers
        //    //                     where s.Id == SupId
        //    //                     select s).FirstOrDefault<Supplier>();
        //    //if (supplier != null)
        //    //{
        //    //    List<Ingredient> ingre = supplier.Ingredients.ToList<Ingredient>();
        //    //    foreach (var ingredient in ingre)
        //    //    {
        //    //        supplier.Ingredients.Remove(ingredient);
        //    //        _dbContext.SaveChanges();
        //    //    }
        //    //    return true;
        //    //}
        //    //return false;
        //}
    }
}
