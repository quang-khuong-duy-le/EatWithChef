using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EatWithChef.Areas.Admin.Controllers
{
    public partial class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        //ElFinder
        //
        // GET: /Admin/ManageFile

        public ActionResult ManageFile()
        {
            return View();
        }

        public ActionResult BrowseFile()
        {
            return PartialView("_Elfinder");
        }
    }
}
