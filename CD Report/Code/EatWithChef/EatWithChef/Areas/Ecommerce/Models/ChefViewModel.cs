using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatWithChef.Areas.Ecommerce.Models
{
    public class ChefViewModel
    {
        public ChefDTO Chef { get; set; }
        public List<FAQDTO> FAQs { get; set; }
    }
}