using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EWC.Core.DataAccess;

namespace EWC.Core.Entity
{
    public class CatDishMenu
    {
        public DishOfMenu DishMenu { get; set; }
        public DishCategory DishCategory { get; set; }
    }
}
