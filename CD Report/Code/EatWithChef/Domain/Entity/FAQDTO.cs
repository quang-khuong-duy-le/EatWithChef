using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class FAQDTO
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerImageURL { get; set; }

        public string ChefName { get; set; }
        public string ChefImageURL { get; set; }

        public int FAQId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int NumOfLike { get; set; }

        public List<CommentView> ListComment { get; set; }

        public FAQDTO() {
            ListComment = new List<CommentView>();
        }
    }

    public class CommentView {
        public int CommentId { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ImageURL { get; set; }

        public List<CommentView> ChildComment { get; set; }

        public CommentView() {
            ChildComment = new List<CommentView>();
        }
    }
}
