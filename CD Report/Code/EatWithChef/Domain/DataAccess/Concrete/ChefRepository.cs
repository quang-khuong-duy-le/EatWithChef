using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.DataAccess.Abstract;

namespace Domain.DataAccess.Concrete
{
    public class ChefRepository : IChefRepository
    {
        private readonly EWCEntities _dbContext;

        public ChefRepository() {
            _dbContext = new EWCEntities();
        }

        public void Dispose() {
            if (_dbContext != null) {
                _dbContext.Dispose();
            }
        }

        #region Private orther table method.
        //Get list skill of chef.
        private List<ChefSkill> GetChefSkillByChefId(int ChefId) {
            var ChefSkill = _dbContext.ChefSkills.Where(c => c.ChefId == ChefId).ToList();
            return ChefSkill;
        }

        //Get skill info by id.
        private Skill GetSkillById(int SkillId) {
            var skill = _dbContext.Skills.Find(SkillId);
            return skill;
        }
        #endregion

        #region Public method.
        //Get user profile by id.
        public ChefDTO GetChefProfileById(int ChefId)
        {
            UserProfile ChefProfile = _dbContext.UserProfiles.Where(u => u.UserId == ChefId && u.IsActive).FirstOrDefault();
            Chef ChefSuperInfo = _dbContext.Chefs.Where(c => c.UserID == ChefId).FirstOrDefault();

            ChefDTO ChefView = new ChefDTO();//Chef item in view.

            if (ChefProfile == null) {
                return null;
            }
            else
            {
                List<ChefSkillView> ListChefSkillView = new List<ChefSkillView>(); //List Chef skill view include Name and Score.
                //Get chef skills.
                List<ChefSkill> ChefSkillItem = GetChefSkillByChefId(ChefSuperInfo.UserID);
                if (ChefSkillItem != null)
                {
                    foreach (var chefSkill in ChefSkillItem)
                    {
                        var skill = GetSkillById(chefSkill.SkillId);
                        ListChefSkillView.Add(new ChefSkillView() { SkillName = skill.SkillName, Score = chefSkill.Score });
                    }
                }

                //Add Chef View item.
                ChefView.UserId = ChefProfile.UserId;
                ChefView.ChefName = ChefProfile.FullName;
                ChefView.Description = ChefSuperInfo.Description;
                ChefView.Image = ChefSuperInfo.ImageURL;
                ChefView.ChefSkill = ListChefSkillView;
            }

            return ChefView;
        }

        //Get all chef.
        public List<ChefDTO> GetAllChef() { 
            //1. Get all chef from chef table.
            //2. Remove inactive account.

            //1. Get all chef.
            List<ChefDTO> ListChefView = new List<ChefDTO>();
            List<Chef> ListChef = _dbContext.Chefs.ToList();
            if (ListChef != null) {
                foreach (var item in ListChef) {
                    var ChefProfile = GetChefProfileById(item.UserID);

                    if (ChefProfile == null)
                    {
                        ListChef.Remove(item);
                    }
                    else 
                    {
                        ListChefView.Add(ChefProfile);   
                    }
                    //if (ChefProfile != null) {
                    //    //2. Remove inactive account.
                    //    if (ChefProfile == null)
                    //    {
                    //        ListChef.Remove(item);
                    //    }
                    //    else {
                    //        ChefDTO ChefViewItem = new ChefDTO();//Chef item in view.
                    //        List<ChefSkillView> ListChefSkillView = new List<ChefSkillView>(); //List Chef skill view include Name and Score.
                    //        //Get chef skills.
                    //        List<ChefSkill> ChefSkillItem = GetChefSkillByChefId(item.UserID);
                    //        if (ChefSkillItem != null) {
                    //            foreach (var chefSkill in ChefSkillItem) { 
                    //                var skill = GetSkillById(chefSkill.SkillId);
                    //                ListChefSkillView.Add(new ChefSkillView() { SkillName = skill.SkillName, Score = chefSkill.Score });
                    //            }
                    //        }

                    //        //Add Chef View item.
                    //        ChefViewItem.ChefName = ChefProfile.FullName;
                    //        ChefViewItem.Description = item.Description;
                    //        ChefViewItem.Image = item.ImageURL;
                    //        ChefViewItem.ChefSkill = ListChefSkillView;

                    //        //Add chef view to list.
                    //        ListChefView.Add(ChefViewItem);
                    //    }
                    //}
                }
                //Get chef view info.

            }
            return ListChefView;
        }

        public FAQDTO GetFAQById(int FAQId) {
            var FAQ = _dbContext.FAQs.Where(f => f.FAQId == FAQId).FirstOrDefault();

            if (FAQ != null)
            {
                var listComment = _dbContext.FAQComments.Where(f => f.FAQId == FAQId).ToList();
                var customerProfile = _dbContext.UserProfiles.Find(FAQ.UserId);
                var chefProfile = _dbContext.UserProfiles.Find(FAQ.ChefId);


                FAQDTO FAQDTO = new FAQDTO();
                FAQDTO.CustomerId = FAQ.UserId;
                FAQDTO.CustomerName = customerProfile.FullName;
                FAQDTO.CustomerImageURL = FAQ.Customer.ImageURL;

                FAQDTO.ChefName = chefProfile.FullName;
                FAQDTO.ChefImageURL = FAQ.Chef.ImageURL;

                FAQDTO.FAQId = FAQId;
                FAQDTO.Question = FAQ.Question;
                FAQDTO.Answer = FAQ.Answer;
                FAQDTO.NumOfLike = FAQ.NumOfLike;

                foreach (FAQComment item in listComment) 
                {
                    if(item.ParentComment == null || item.ParentComment == 0)
                    {
                        var customerComment = _dbContext.Customers.Find(item.UserId);
                        CommentView comment = new CommentView();

                        comment.CommentId = item.FAQCommentId;
                        comment.CustomerId = item.UserId;
                        comment.CustomerName = item.UserProfile.FullName;
                        comment.ImageURL = customerComment.ImageURL;

                        foreach (FAQComment subItem in item.FAQComment1) {
                            var subCustomerComment = _dbContext.Customers.Find(subItem.UserId);
                            CommentView subcomment = new CommentView();

                            subcomment.CommentId = subItem.FAQCommentId;
                            subcomment.CustomerId = subItem.UserId;
                            subcomment.CustomerName = subItem.UserProfile.FullName;
                            subcomment.ImageURL = subCustomerComment.ImageURL;

                            comment.ChildComment.Add(subcomment);
                        }

                        FAQDTO.ListComment.Add(comment);
                    }
                }

                return FAQDTO;
            }

            return null;
        }

        public List<FAQDTO> GetAllFAQOfChef(int chefId) 
        {
            List<FAQ> FAQOfChef = _dbContext.FAQs.Where(f => f.ChefId == chefId).ToList();
            
            if(FAQOfChef != null && FAQOfChef.Count > 0)
            {
                List<FAQDTO> result = new List<FAQDTO>();

                foreach(FAQ item in FAQOfChef)
                {
                    FAQDTO FaqDTO = GetFAQById(item.FAQId);

                    result.Add(FaqDTO);
                }

                return result;
            }

            return null;
        }

        public bool CreateFAQ(FAQ faq) 
        {
            _dbContext.FAQs.Add(faq);
            _dbContext.SaveChanges();
            return true;
        }

        #endregion
    }
}
