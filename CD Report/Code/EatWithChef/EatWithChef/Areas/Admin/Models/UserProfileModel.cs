using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entity;

namespace EatWithChef.Areas.Admin.Models
{
    public class UserProfileModel
    {
        public List<webpages_Roles> AllRoles;
        public Domain.Entity.UserProfile GetuserbyId;
        public List<Domain.Entity.webpages_Roles> GetRoleOfUser;
        public webpages_Roles RolebyId;
        public Customer Customer;
        public List<Skill> ListSkill;
        public List<ChefSkill> ChefSkill;
        public Chef Chef;
        public class SkillofChef{
            public int skillId;
            public int scoreskill;
        }
        public UserProfileModel()
        {
            GetuserbyId = new Domain.Entity.UserProfile();
            AllRoles = new List<webpages_Roles>();
            GetRoleOfUser = new List<webpages_Roles>();
            RolebyId = new webpages_Roles();
            ListSkill = new List<Skill>();
            ChefSkill = new List<ChefSkill>();
            Chef = new Chef();
            Customer = new Customer();
        }
    }  
}