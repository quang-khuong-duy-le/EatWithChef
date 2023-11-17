using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.BusinessLogic.Abstract;
using Domain.Entity;
using Domain.Utility;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;

namespace Domain.BusinessLogic.Concrete
{
    public class OrderServices : IOrderServices
    {
        private readonly IDishRepository _dishRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderServices() {
            _dishRepository = new DishRepository();
            _menuRepository = new MenuRepository();
            _orderRepository = new OrderRepository();
        }

        //Break down order by menu. Each menu is an order have list dish, 
        //delivery date in order base on publish date in menu.
        public List<MenuDTO> DivideMenu(int[] ListMenuID, int[] ListDishID, int[] ListQuantity)
        {
            try
            {
                List<MenuDTO> ListMenuResult = new List<MenuDTO>();
                for (int i = 0; i < ListMenuID.Length; i++)
                {
                    //Menu is in result --> update result at current index --> add dish & quantity.
                    int ElementIndex = CheckMenu(ListMenuID[i], ListMenuResult);
                    if (ElementIndex == -1)
                    {
                        //Not have menu in result --> 
                        //add current menu, dish, quantity at same index to result.
                        MenuDTO MenuItem = new MenuDTO();
                        MenuItem.MenuID = ListMenuID[i];
                        MenuItem.ListDishID.Add(ListDishID[i]);
                        MenuItem.ListQuantity.Add(ListQuantity[i]);
                        ListMenuResult.Add(MenuItem);
                    }
                    else
                    {
                        ListMenuResult.ElementAt(ElementIndex).ListDishID.Add(ListDishID[i]);
                        ListMenuResult.ElementAt(ElementIndex).ListQuantity.Add(ListQuantity[i]);
                    }
                }
                return ListMenuResult;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        //Submit customer check out.
        public bool SubmitCheckout(int[] ListDishID, int[] MenuID, int[] Quantity, OrderDTO OrderInfor, int UserId, int PaymentStatus) {
            try {
                //1. Create bill.
                //2. Create list order in bill.
                //3. Create order detail.

                //1. Create bill
                //1.1 Generate unique code by customer email.
                string BillCode = ConvertStringHelper.GetCodeForEmail(OrderInfor.ReceiverEmail);
                BillCode += DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                //1.2 Create bill.
                Bill BillItem = new Bill();
                BillItem.Code = BillCode;
                BillItem.Status = (int)BillStatusEnum.New;
                if (UserId > 0)
                {
                    BillItem.UserId = UserId;
                }
                int BillId = _orderRepository.CreateBill(BillItem);
                //2. Create list order in bill.
                List<MenuDTO> ListOrderDTO = DivideMenu(MenuID, ListDishID, Quantity);
                if (BillId > 0 && ListOrderDTO != null)
                {
                    for (int i = 0; i < ListOrderDTO.Count; i++)
                    {
                        //Create object order item.
                        Order OrderItem = new Order();
                        //Set Order infor.
                        OrderItem.BillId = BillId;
                        OrderItem.ReceiverName = OrderInfor.ReceiverName;
                        OrderItem.ReceiverPhone = OrderInfor.ReceiverPhone;
                        OrderItem.ReceiverAddress = OrderInfor.ReceiverAddress;
                        OrderItem.ReceiverEmail = OrderInfor.ReceiverEmail;
                        OrderItem.Note = OrderInfor.Note;
                        OrderItem.OrderDate = DateTime.Now;
                        //Get menu infor.
                        Menu menu = _menuRepository.GetMenuById(ListOrderDTO.ElementAt(i).MenuID);
                        //Set delivery date.
                        OrderItem.DeliveryDate = menu.ApplyDate;
                        //Set total payment.
                        int TotalPayment = 0;
                        for (int j = 0; j < ListOrderDTO.ElementAt(i).ListDishID.Count; j++ )
                        {
                            int UnitPrice = _dishRepository.GetPriceFromDishMenu(ListOrderDTO.ElementAt(i).ListDishID.ElementAt(j), menu.Id);
                            int DishQuantity = ListOrderDTO.ElementAt(i).ListQuantity.ElementAt(j);
                            TotalPayment += UnitPrice * DishQuantity;
                        }
                        OrderItem.TotalPayment = TotalPayment;
                        //Set order status = WaitForProcess.
                        OrderItem.OrderStatus = (int)OrderStatusEnum.WaitForProcess;
                        OrderItem.PaymentStatus = PaymentStatus;

                        //Create order item.
                        int OrderId = _orderRepository.CreateOrder(OrderItem);
                        if (OrderId > 0)
                        {
                            //3. Create order detail.
                            for (int j = 0; j < ListOrderDTO.ElementAt(i).ListDishID.Count; j++) {
                                OrderDetail OrderDetailItem = new OrderDetail();
                                OrderDetailItem.OrderID = OrderId;
                                int DishId = ListOrderDTO.ElementAt(i).ListDishID.ElementAt(j);
                                OrderDetailItem.DishID = DishId;
                                OrderDetailItem.Quantity = ListOrderDTO.ElementAt(i).ListQuantity.ElementAt(j);
                                OrderDetailItem.UnitPrice = _dishRepository.GetPriceFromDishMenu(DishId, menu.Id);
                                int result = _orderRepository.CreateOrderDetail(OrderDetailItem);
                                if (result <= 0) {
                                    return false;
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        //Check is processed menu?
        private int CheckMenu(int MenuID, List<MenuDTO> ListMenu)
        {
            try
            {
                for (int i = 0; i < ListMenu.Count; i++)
                {
                    if (MenuID == ListMenu.ElementAt(i).MenuID)
                    {
                        return i;
                    }
                }
                return -1;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}
