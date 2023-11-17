using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HotelManagement.Models.User;
using MvcMembership;
using MvcMembership.Settings;
using SampleWebsite.Mvc3.Areas.MvcMembership.Models.UserAdministration;

namespace HotelManagement.Controllers
{
    [AuthorizeUnlessOnlyUser(Roles = "Admin")] 
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            MembershipUserCollection collection  = Membership.GetAllUsers();
            return View(collection);
        }

        public ActionResult CreateUser()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateUser(UserModel user)
        {
            try
            {
                if (user.Password.Equals(user.ConfirmPassword))
                {
                  var membership =  Membership.CreateUser(user.Username, user.Password, user.Email);
                    return PartialView("_UserItem",membership);
                }
                return Content("0");
            }
            catch (Exception)
            {

                return Content("0");
            }
        }

        public ActionResult SearchByUsername(string username) {
            try{
                var membership = Membership.GetUser(username);
                if (membership != null) {
                    return Content("1");
                }
                return Content("0");
            }catch{
                return Content("1");
            }
        }

        public ActionResult SearchByUserId(Guid userId) {
            var membership = Membership.GetUser(userId);
            return PartialView("_UserItem", membership);
        }
    }
}
