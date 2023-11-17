using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entity;

namespace EatWithChef.Areas.Admin.Models
{
    public class DishMenuIP
    {
        public int Id;
        public int Price;
        public int Quota;

    }

    public class SuggestDish
    {
        public int Id;
        public string Name;
        public int Number;
        public int Price;
    }
}