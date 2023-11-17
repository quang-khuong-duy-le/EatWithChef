using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.DataAccess.Abstract;

namespace Domain.DataAccess.Concrete
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly EWCEntities _dbContext;
        private const int DISHPERPAGE = 7;
        string defaultImage = "/Images/Ingredient/noimage.jpg";

        public UserProfileRepository()
        {
            _dbContext = new EWCEntities();
        }

        public void Dispose() {
            if (_dbContext != null) {
                _dbContext.Dispose();
            }
        }

        #region frontend
        //Get user profile by id.
        ///<summary>
        ///<para>String UserID: UserId to get user information.</para>
        ///<returns>
        ///value UserProfile: Profile of user found in data.
        ///</returns>
        ///</summary>
        public UserProfile GetUserProfileByID(int UserID)
        {
            UserProfile userProfile = new UserProfile();
            userProfile = _dbContext.UserProfiles.Where(u => u.UserId == UserID && u.IsActive).FirstOrDefault();
            return userProfile;
        }

        //Create user profile anonymous user.
        public bool CreateAnonymousUser(UserProfile user)
        {
            try
            {
                UserProfile result = _dbContext.UserProfiles.Add(user);
                _dbContext.SaveChanges();
                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Check is exist email.
        ///<summary>
        ///<para>String Email: email string to check.</para>
        ///<returns>
        ///value UserId: Email is exist.
        ///value 0: Email does not exist.
        ///</returns>
        ///</summary>
        public int CheckIsExistEmail(string Email)
        {
            var userProfile = (from user in _dbContext.UserProfiles
                               where user.Email.ToLower().Equals(Email.ToLower())
                               select user).FirstOrDefault();
            if (userProfile != null)
            {
                return userProfile.UserId;
            }
            return 0;
        }
        #endregion

        #region backend
        //Get all user profile.
        public List<UserProfile> GetAllUserProfile()
        {
            return _dbContext.UserProfiles.ToList();
        }
        //Get UserProfile
        public List<UserProfile> GetUserProfile(string keyword, int customerType,int roleID, int page, string sortBy, string sortDirection, out int maxPage)
        {
            List<UserProfile> result = null;
            var query = _dbContext.UserProfiles.AsQueryable();
            try
            {
                if (roleID != 0)
                {
                    query = (from i in _dbContext.UserProfiles
                                 where i.webpages_Roles.Any(s => s.RoleId == roleID)
                                 select i).AsQueryable();
                }
                if (customerType != 0)
                {
                    var customer = from i in _dbContext.Customers
                                   where i.CustomerType == customerType
                                   select i.CustomerId;
                    List<int> ids = new List<int>();
                    foreach (var i in customer)
                    {
                        ids.Add(i);
                    }
                    query = _dbContext.UserProfiles.Where(s => ids.Contains(s.UserId));
                }
                    query = query.Where(u => u.FullName.Contains(keyword));
                

                // sort
                string sortStr = "";
                if (sortDirection.Equals("ascending"))
                {
                    sortStr = sortBy + " " + "ASC";
                    query = query.OrderBy(n => n.UserName);
                }
                else if (sortDirection.Equals("descending"))
                {
                    sortStr = sortBy + " " + "DESC";
                    query = query.OrderByDescending(n => n.UserName);

                }

                // paging
                double ratio = (double)query.ToList().Count / (double)DISHPERPAGE;
                maxPage = (int)Math.Ceiling(ratio);

                int itemNeedToSkip = (page - 1) * DISHPERPAGE;
                query = query.Skip(itemNeedToSkip).Take(DISHPERPAGE);

                result = query.ToList();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return result;
        }

        public UserProfile GetUserbyId(int Id) {
            try
            {
                return _dbContext.UserProfiles.Where(s => s.UserId == Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public Customer GetCustomer(int Id)
        {
            try
            {
                return _dbContext.Customers.Where(s => s.CustomerId == Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public bool UpdateActiveUser(int userid, bool isactive)
        {
            UserProfile user = new UserProfile();
            user = _dbContext.UserProfiles.Where(s => s.UserId == userid).FirstOrDefault();
            try
            {
                if (user != null)
                {
                    user.IsActive = isactive;
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return false;
        }

        //public bool checkUserName(string username)
        //{
        //    UserProfile userprofile = new UserProfile();
        //    userprofile = _dbContext.UserProfiles.Where(d => d.UserName.Equals(username)).FirstOrDefault();
        //    if (userprofile == null)
        //        return true;
        //    else return false;
        //}

        #region Role
        public webpages_Roles GetRolebyId(int roleId)
        {
            try
            {
                return _dbContext.webpages_Roles.Where(s => s.RoleId == roleId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public List<webpages_Roles> GetRole()
        {
            try
            {
                return _dbContext.webpages_Roles.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public List<webpages_Roles> GetRoleOfUser(int userid)
        {
            var query = (from i in _dbContext.webpages_Roles
                         where i.UserProfiles.Any(s => s.UserId == userid)
                         select i);
            return query.ToList();
        }
        public int AddRole(string rolename)
        {
            webpages_Roles role = new webpages_Roles();
            role.RoleName = rolename;
            try
            {
                _dbContext.webpages_Roles.Add(role);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            if (role.RoleId > 0)
            {
                return role.RoleId;
            }
            else
            {
                return 0;
            }
        }
        public bool EditRole(int id, string rolename)
        {
            webpages_Roles role = new webpages_Roles();
            try
            {
                role = _dbContext.webpages_Roles.Where(s => s.RoleId == id).FirstOrDefault();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            if (role == null) return false;
            role.RoleName = rolename;
            try
            {
                _dbContext.webpages_Roles.Add(role);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return true;
        }
        #endregion

        #region Skill
        public List<Skill> GetSkill()
        {
            try
            {
                return _dbContext.Skills.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public List<ChefSkill> GetSkillOfChef(int chefId)
        {
            try
            {
                return _dbContext.ChefSkills.Where(s => s.ChefId == chefId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Chef GetChef(int ChefId)
        {
            try
            {
                return _dbContext.Chefs.Where(s => s.UserID == ChefId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool CreateChefSkill(int chefId, int skillId, int score)
        {
            ChefSkill skill = new ChefSkill();
            skill.ChefId = chefId;
            skill.SkillId = skillId;
            skill.Score = score;
            try
            {
                _dbContext.ChefSkills.Add(skill);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool DeleteChefSkill(int chefId, int skillId)
        {
            ChefSkill chefskill = new ChefSkill();
            try
            {
                chefskill = _dbContext.ChefSkills.Where(s => s.ChefId == chefId && s.SkillId == skillId).FirstOrDefault();
                if (chefskill == null) return false;
                _dbContext.ChefSkills.Remove(chefskill);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public bool CreateChef(int chefId)
        {
            Chef chef = new Chef();
            chef.UserID = chefId;
            chef.ImageURL = defaultImage;
            chef.Description = "Chưa có mô tả";
            try
            {
                _dbContext.Chefs.Add(chef);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion
        #endregion
    }
}
