using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.BusinessLogic.Abstract
{
    public interface IOrderServices
    {
        /// <summary>
        /// Break down order by menu. Each menu is an order have list dish, delivery date in order base on publish date in menu.
        /// </summary>
        /// <param name="ListMenuID">List Menu Id in cart</param>
        /// <param name="ListDishID">List Dish Id in cart</param>
        /// <param name="ListQuantity">List Quantity in cart</param>
        /// <returns>List Menu DTO include list menu divide by delivery date, each menu have list dish and quantity.</returns>
        List<MenuDTO> DivideMenu(int[] ListMenuID, int[] ListDishID, int[] ListQuantity);

        /// <summary>
        /// Submit customer check out.
        /// 1. Create Bill
        /// 2. Create Order
        /// 3. Create Order Detail
        /// </summary>
        /// <param name="ListDishID">List Dish Id from local storage.</param>
        /// <param name="MenuID">List Menu Id from local storage.</param>
        /// <param name="Quantity">List Quantity from local storage.</param>
        /// <param name="OrderInfor">Receiver information.</param>
        /// <param name="UserId">Order user id.</param>
        /// <returns></returns>
        bool SubmitCheckout(int[] ListDishID, int[] MenuID, int[] Quantity, OrderDTO OrderInfor, int UserId, int PaymentStatus);
    }
}
