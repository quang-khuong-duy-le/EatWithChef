using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;
using System.Web.Script.Serialization;
using EatWithChef.Areas.Ecommerce.Models;

namespace EatWithChef.Areas.Ecommerce.Controllers
{
    public class ChefController : Controller
    {
        private readonly IChefRepository _chefRepository;
        private readonly IDishRepository _dishRepository;

        public ChefController() {
            _chefRepository = new ChefRepository();
            _dishRepository = new DishRepository();
        }
        //
        // GET: /Ecommerce/Chef/

        public ActionResult Index()
        {
            //Get all active chef.
            List<ChefDTO> ListChef = _chefRepository.GetAllChef();
            return View("Index",ListChef);
        }
        
        //Get dish data for chef page.
        public ActionResult GetDishByChefId(int ChefId) {
            List<Dish> ListDish = _dishRepository.GetDishByChefId(ChefId).ToList();

            var jsonSerialiser = new JavaScriptSerializer();
            var chef = _chefRepository.GetChefProfileById(ChefId);
            var chef1 = jsonSerialiser.Serialize(new { Name = chef.ChefName });
            var json = jsonSerialiser.Serialize(ListDish.Select(d => new { d.Id, d.UnsignName, d.Name, d.Image, d.Description, d.Url }));

            var obj = new { List = json, Chef = chef1 };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChefDetailPage(int chefId)
        {
            ChefViewModel model = new ChefViewModel(); 

            model.Chef = _chefRepository.GetChefProfileById(chefId);
            model.FAQs = _chefRepository.GetAllFAQOfChef(chefId);

            return View("ChefDetailPage", model);
        }

        [HttpPost]
        public ActionResult CreateFAQ(int chefId, int customerId, string question) 
        {
            FAQ faq = new FAQ();
            faq.ChefId = chefId;
            faq.UserId = customerId;
            faq.Question = question;
            faq.Status = 1;
            faq.NumOfLike = 0;

            _chefRepository.CreateFAQ(faq);

            return Content("1");
        }
    }
}
