using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ChefDTO
    {
        public int UserId { get; set; }
        public string ChefName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<ChefSkillView> ChefSkill { get; set; }

        public ChefDTO() {
            ChefSkill = new List<ChefSkillView>();
        }
    }

    public class ChefSkillView {
        public string SkillName { get; set; }
        public int Score { get; set; }
    }
}
