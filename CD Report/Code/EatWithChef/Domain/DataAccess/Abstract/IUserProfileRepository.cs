using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.DataAccess.Abstract
{
    public interface IUserProfileRepository:IDisposable
    {
        UserProfile GetUserProfileByID(int UserID);

        bool CreateAnonymousUser(UserProfile user);

        int CheckIsExistEmail(string Email);

        List<UserProfile> GetAllUserProfile();

        List<UserProfile> GetUserProfile(string keyword, int customerType, int roleID, int page, string sortBy, string sortDirection, out int maxPage);

        UserProfile GetUserbyId(int Id);

        Customer GetCustomer(int Id); 

        bool UpdateActiveUser(int userid, bool isactive);

        webpages_Roles GetRolebyId(int roleId);

        List<webpages_Roles> GetRole();

        List<webpages_Roles> GetRoleOfUser(int userid);

        int AddRole(string rolename);

        bool EditRole(int id, string rolename);

        List<Skill> GetSkill();

        List<ChefSkill> GetSkillOfChef(int chefId);

        Chef GetChef(int ChefId);

        bool CreateChefSkill(int chefId, int skillId, int score);

        bool DeleteChefSkill(int chefId, int skillId);

        bool CreateChef(int chefId);


    }
}
