using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using EatWithChef.Filters;
using EatWithChef.Areas.Admin.Models;
using Domain.Entity;
using System.Globalization;
using System.Web.Configuration;
using System.Web.Profile;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;
using Domain.Utility;

namespace EatWithChef.Areas.Admin.Controllers
{
    [InitializeSimpleMembership]
    public class UserManagermentController : Controller
    {
        private IUserProfileRepository _userprofileRepository;

        public UserManagermentController()
        {
            _userprofileRepository = new UserProfileRepository();
        }
        //
        // GET: /UserManagerment/

        public ActionResult Index(int RoleId)
        {
            UserProfileModel userprofilemodel = new UserProfileModel();
            userprofilemodel.RolebyId = _userprofileRepository.GetRolebyId(RoleId);
            userprofilemodel.ListSkill = _userprofileRepository.GetSkill();
            if (userprofilemodel.AllRoles == null || userprofilemodel.RolebyId == null) return View("Error");
            return View(userprofilemodel);
        }

        [HttpPost]
        public JsonResult GetUser(string keyword, int customerType, int RoleId, string sortBy, string sortDirection, int page)
        {
            int maxPage = 0;
            List<Domain.Entity.UserProfile> listuser = _userprofileRepository.GetUserProfile(keyword,customerType, RoleId, page, sortBy, sortDirection, out maxPage);

            if (listuser == null || listuser.Count == 0)
            {
                return Json(new { MaxPage = 1 }, JsonRequestBehavior.AllowGet);
            }

            List<object> userJson = new List<object>();
            if (RoleId == (int)UserRoleEnum.Customer)
            {
                foreach (Domain.Entity.UserProfile user in listuser)
                {
                    var customer = _userprofileRepository.GetCustomer(user.UserId);
                    var model = new { ID = user.UserId, UserName = user.UserName, FullName = user.FullName, Phone = user.Telephone, Address = user.Address, IsActive = user.IsActive, Email = user.Email , CustomerType = customer.CustomerType};
                    userJson.Add(model);
                }
            }
            else {
                foreach (Domain.Entity.UserProfile user in listuser)
                {
                    var model = new { ID = user.UserId, UserName = user.UserName, FullName = user.FullName, Phone = user.Telephone, Address = user.Address, IsActive = user.IsActive, Email = user.Email };
                    userJson.Add(model);
                }
            }
            var result = new { Listuser = userJson, MaxPage = maxPage };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditUser(int id)
        {
            UserProfileModel usermodel = new UserProfileModel();
            usermodel.GetuserbyId = _userprofileRepository.GetUserbyId(id);                     
            return PartialView(usermodel);
        }
        public ActionResult EditCutomer(int id)
        {
            UserProfileModel usermodel = new UserProfileModel();
            usermodel.GetuserbyId = _userprofileRepository.GetUserbyId(id);
            usermodel.Customer = _userprofileRepository.GetCustomer(id);
            return PartialView(usermodel);
        }

        public ActionResult AddnewUser(int id)
        {
            ViewBag.RolebyId = _userprofileRepository.GetRolebyId(id);
            return PartialView();
        }

        public ActionResult AddnewChef()
        {            
            return PartialView();
        }
        public ActionResult EditChef(int id)
        {
            UserProfileModel usermodel = new UserProfileModel();
            usermodel.ListSkill = _userprofileRepository.GetSkill();
            usermodel.GetuserbyId = _userprofileRepository.GetUserbyId(id);
            usermodel.ChefSkill = _userprofileRepository.GetSkillOfChef(id);
            usermodel.Chef = _userprofileRepository.GetChef(id);
            
            return PartialView(usermodel);
        }

        [HttpPost]
        public int CreateChef(string UserName, string Password, string FullName, string Email, string Phone, string Address, bool IsFemale, string Birthday, string RoleName, string listskill)
        {
            List<UserProfileModel.SkillofChef> skills = JsonHelper.JsonDeserialize<List<UserProfileModel.SkillofChef>>(listskill);
            try
            {
                WebSecurity.CreateUserAndAccount(UserName, Password, new
                {
                    FullName = FullName,
                    Email = Email,
                    Telephone = Phone,
                    Address = Address,
                    IsFemale = IsFemale,
                    DateOfBirth = Birthday,
                    IsActive = true
                });
                Roles.AddUserToRole(UserName, RoleName);
                int ChefId = WebSecurity.GetUserId(UserName);
                bool result2 = _userprofileRepository.CreateChef(ChefId);
                if (!result2) return 0;
                foreach (var skill in skills)
                {
                    bool result = _userprofileRepository.CreateChefSkill(ChefId, skill.skillId, skill.scoreskill);
                    if (!result) return 0;
                }
                return 1;
            }
            catch (MembershipCreateUserException)
            {
                return 0;
            }
        }

        [HttpPost]
        public int CreateUser(string UserName, string Password, string FullName, string Email, string Phone, string Address, bool IsFemale, string Birthday, string RoleName)
        {
            //string[] roles = roles_str.Split(',');
            try
            {
                WebSecurity.CreateUserAndAccount(UserName, Password, new
                {
                    FullName = FullName,
                    Email = Email,
                    Telephone = Phone,
                    Address = Address,
                    IsFemale = IsFemale,
                    DateOfBirth = Birthday,
                    IsActive = true
                });
                Roles.AddUserToRole(UserName, RoleName);
                return 1;
            }
            catch (MembershipCreateUserException)
            {
                return 0;
            }
        }
        [HttpPost]
        public int SaveEdit(int userid, string username, string roles_str)
        {
            var roleOfUser = _userprofileRepository.GetRoleOfUser(userid);
            foreach (Domain.Entity.webpages_Roles role in roleOfUser)
            {
                Roles.RemoveUserFromRole(username, role.RoleName);
            }
            string[] roles = roles_str.Split(',');
            try
            {
                if (roles != null && !roles[0].Equals(""))
                {
                    Roles.AddUserToRoles(username, roles);
                }
                return 1;
            }
            catch (MembershipCreateUserException)
            {
                return 0;
            }
        }

        [HttpPost]
        public int SaveEditChef(int id, string listskill)
        {
            List<UserProfileModel.SkillofChef> skills = JsonHelper.JsonDeserialize<List<UserProfileModel.SkillofChef>>(listskill);
            var skillofchef = _userprofileRepository.GetSkillOfChef(id);
            foreach (var item in skillofchef) {
                bool result = _userprofileRepository.DeleteChefSkill(id, item.SkillId);
                if (!result) return 0;
            }
            foreach (var skill in skills)
            {
                bool result = _userprofileRepository.CreateChefSkill(id, skill.skillId, skill.scoreskill);
                if (!result) return 0;
            }
            return 1;
        }

        [HttpPost]
        public int DeActiveUser(int userid)
        {
            bool result = _userprofileRepository.UpdateActiveUser(userid, false);
            if (result)
                return 1;
            return 0;
        }
        [HttpPost]
        public int ActiveUser(int userid)
        {
            bool result = _userprofileRepository.UpdateActiveUser(userid, true);
            if (result)
                return 1;
            return 0;
        }
        [HttpPost]
        public JsonResult CheckUserName(string username)
        {
            bool result = WebSecurity.UserExists(username);
            if (!result) return Json("Success", JsonRequestBehavior.AllowGet);
            else return Json("Error", JsonRequestBehavior.AllowGet);
        }
        public ActionResult ManageRole()
        {
            UserProfileModel user = new UserProfileModel();
            user.AllRoles = _userprofileRepository.GetRole();
            return PartialView(user);
        }
        [HttpPost]
        public int CreateRole(string rolename)
        {
            int result = _userprofileRepository.AddRole(rolename);
            if (result != 0)
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        [HttpPost]
        public int EditRole(int id, string rolename)
        {
            bool result = _userprofileRepository.EditRole(id, rolename);
            if (!result) return 0;
            else return 1;
        }
    }
}
