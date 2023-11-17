using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;
using EatWithChef.Areas.Ecommerce.Models;
using Domain.Entity;

namespace EatWithChef.Areas.Ecommerce.Controllers
{
    public class TrackDishController : Controller
    {
        private IProduceSessionRepository _produceSessionRepository;

        public TrackDishController()
        {
            _produceSessionRepository = new ProduceSessionRepository();
        }
        //
        // GET: /Ecommerce/TrackDish/

        public ActionResult QRCodeScan()
        {
            return View();
        }

        public ActionResult TrackDishItem(int dishItemId)
        {
            TrackDishItemModel model;
            DishItem dishItem = _produceSessionRepository.GetDishItemById(dishItemId);
            if (dishItem == null)
            {
                model = null;
                return View(model);
            }

            model = new TrackDishItemModel(dishItem);
            foreach (IngredientItem ingredientItem in dishItem.IngredientItems)
            {
                bool found = false;
                foreach (IngredientForTrackingModel ingredientWithSupplier in model.ListIngredientForTracking)
                {
                    // if found in ListIngredientWithSupplier of Model, then add new Supplier
                    if (ingredientItem.IngredientID == ingredientWithSupplier.ID)
                    {
                        SupplierForTrackingModel supplierModel = new SupplierForTrackingModel(ingredientItem);
                        ingredientWithSupplier.ListSupplier.Add(supplierModel);
                        found = true;
                        break;
                    }
                }
                // if not found in ListIngredientWithSupplier of Model, insert new record of IngredientWithSupplierModel
                if (!found)
                {
                    IngredientForTrackingModel ingredientWithSupplier = new IngredientForTrackingModel(ingredientItem);
                    model.ListIngredientForTracking.Add(ingredientWithSupplier);
                }
            }

            return View(model);
        }

    }
}
