using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;
using EatWithChef.Areas.Ecommerce.Models;
using Domain.BusinessLogic.Abstract;
using Domain.BusinessLogic.Concrete;
using Domain.Utility;
using System.Web.Script.Serialization;
using System.Web.Security;
using EatWithChef.Filters;

namespace EatWithChef.Areas.Ecommerce.Controllers
{
    [InitializeSimpleMembership]
    public class OrderServicesController : Controller
    {
        private readonly IDishRepository _dishRepository;
        private readonly IOrderServices _orderServices;
        private readonly IMenuRepository _menuRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderServicesController() {
            _dishRepository = new DishRepository();
            _orderServices = new OrderServices();
            _menuRepository = new MenuRepository();
            _orderRepository = new OrderRepository();
        }

        //Get cart data.
        public ActionResult GetCartData(int[] cartIem, int[] cartMenu)
        {
            List<DishViewModel> ListDishViewModel = new List<DishViewModel>();
            if (cartIem != null)
            {
                for (int i = 0; i < cartIem.Length; i++)
                {
                    Dish dish = new Dish();
                    dish = _dishRepository.GetDishByID(cartIem[i]);

                    DishViewModel DishViewModelItem = new DishViewModel();
                    DishViewModelItem.DishID = dish.Id;
                    DishViewModelItem.DishImage = dish.Image;
                    DishViewModelItem.DishName = dish.Name;
                    DishViewModelItem.DishPrice = _dishRepository.GetPriceFromDishMenu(cartIem[i], cartMenu[i]);

                    ListDishViewModel.Add(DishViewModelItem);
                }
            }
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(ListDishViewModel);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //Check dish quota when add to cart and check out.
        [HttpPost]
        public ActionResult CheckDishQuota(int DishID, int MenuID, int NumOfDishInCart)
        {
            bool result = _dishRepository.CheckDishQuota(DishID, MenuID, NumOfDishInCart);
            if (result)
            {
                return Content("1");
            }
            return Content("0");
        }

        //Get check out page.
        public ActionResult GetCheckOutPage()
        {
            ViewBag.ReturnUrl = "/thanh-toan";
            return View();
        }

        //Get dish price for check out view
        public ActionResult GetOrderCheckoutPage(int[] ListDishID, int[] MenuID, int[] Quantity)
        {
            List<MenuDTO> orderDTO = _orderServices.DivideMenu(MenuID, ListDishID, Quantity);
            List<DishOrderViewModel> ListOrder = new List<DishOrderViewModel>();
            if (orderDTO != null)
            {
                for (int i = 0; i < orderDTO.Count; i++)
                {
                    //Get DishOrderViewModel at index i --> add to list include MenuDate, List dish of menu, list quantity from MenuDTO.
                    //1. Get menu info --> get menu date customer added.
                    //2. Get list dish id from each orderDTO.
                    //3. Query Dish by id to get Dish info.
                    //4. Get chef name by user id in dish info. (optional)
                    //5. Add dish and quantity to DishOrderViewModel.
                    //6. Add DishOrderViewModel to list.

                    DishOrderViewModel OrderItem = new DishOrderViewModel();
                    //1. Get menu info
                    Menu menu = _menuRepository.GetMenuById(orderDTO.ElementAt(i).MenuID);
                    //Get delivery date.
                    OrderItem.MenuDate = menu.ApplyDate;
                    //2. Get list dish id from each orderDTO.
                    for (int j = 0; j < orderDTO.ElementAt(i).ListDishID.Count; j++)
                    {
                        DishViewModel dishItem = new DishViewModel();
                        //3. Query Dish by id to get Dish info.
                        Dish dish = _dishRepository.GetDishByID(orderDTO.ElementAt(i).ListDishID.ElementAt(j));
                        dishItem.DishID = dish.Id;
                        dishItem.DishImage = dish.Image;
                        dishItem.DishName = dish.Name;
                        dishItem.DishPrice = _dishRepository.GetPriceFromDishMenu(dish.Id, menu.Id);
                        //5. Add dish and quantity to DishOrderViewModel.
                        OrderItem.ListDish.Add(dishItem);
                        OrderItem.ListQuantity.Add(orderDTO.ElementAt(i).ListQuantity.ElementAt(j));
                    }
                    //6. Add DishOrderViewModel to list.
                    ListOrder.Add(OrderItem);
                }
            }
            return PartialView("DishOrderViewPartial", ListOrder);
        }

        //Get View order by bill page.
        public ActionResult GetOrderByBillPage() {
            return View();
        }

        //Get orders by bill id.
        public ActionResult GetOrderByBillCode(string BillCode) {
            BillViewModel BillModel = new BillViewModel();
            //Get bill by code.
            Bill bill = _orderRepository.GetBillByCode(BillCode);
            if (bill != null) { 
                //Get order by bill id.
                List<Order> ListOrder = _orderRepository.GetOrdersByBillId(bill.Id);
                List<OrderViewModel> ListOrderModel = new List<OrderViewModel>();
                if (ListOrder != null) {
                    foreach (var item in ListOrder) {
                        OrderViewModel OrderModel = new OrderViewModel();
                        OrderModel.OrderId = item.Id;
                        OrderModel.OrderDate = item.OrderDate;
                        OrderModel.DeliveryDate = item.DeliveryDate;
                        OrderModel.TotalPayment = item.TotalPayment;
                        OrderModel.OrderStatus = item.OrderStatus;
                        OrderModel.PaymentStatus = item.PaymentStatus;

                        //Get order detail by order id.
                        List<OrderDetailViewModel> ListOrderDetailModel = new List<OrderDetailViewModel>();
                        List<OrderDetail> ListOrderDetail = _orderRepository.GetOrderDetail(item.Id);
                        if (ListOrderDetail != null) {
                            foreach (var OrderDetailItem in ListOrderDetail)
                            {
                                OrderDetailViewModel OrderDetailModel = new OrderDetailViewModel();
                                //Get dish information by id.
                                Dish DishItem = _dishRepository.GetDishByID(OrderDetailItem.DishID);
                                if (DishItem != null) {
                                    OrderDetailModel.OrderDetailId = OrderDetailItem.Id;
                                    OrderDetailModel.Quantity = OrderDetailItem.Quantity;
                                    OrderDetailModel.UnitPrice = OrderDetailItem.UnitPrice;
                                    OrderDetailModel.DishName = DishItem.Name;
                                    OrderDetailModel.DishUrl = DishItem.Url;
                                    OrderDetailModel.DishImageUrl = DishItem.Image;
                                    //Get List Dish Item by Order Detail Id.
                                    OrderDetailModel.OrderDetailDishItemModel.AddRange(_orderRepository.GetOrderDetailDishItem(OrderDetailItem.Id));
                                }
                                ListOrderDetailModel.Add(OrderDetailModel);
                            }
                            OrderModel.OrderDetailView.AddRange(ListOrderDetailModel);
                        }
                        ListOrderModel.Add(OrderModel);
                    }
                }
                BillModel.BillCode = BillCode;
                BillModel.OrderView.AddRange(ListOrderModel);
            }
            return PartialView("ListOrderInBillPartial", BillModel);
        }

        //Submit check out.
        [HttpPost]
        public ActionResult SubmitCheckout(OrderDTO OrderInfor) {
            int[] ListDishID = OrderInfor.ListDishID;
            int[] MenuID = OrderInfor.MenuID;
            int[] Quantity = OrderInfor.Quantity;
            int UserId = 0;
            MembershipUser CurrentUser = Membership.GetUser();
            if (CurrentUser != null)
            {
                string CurrentUserId = CurrentUser.ProviderUserKey.ToString();
                if (CurrentUserId != "")
                {
                    UserId = int.Parse(CurrentUserId);
                }
            }
            bool result = _orderServices.SubmitCheckout(ListDishID, MenuID, Quantity, OrderInfor, UserId, (int)PaymentStatusEnum.UnPay);
            if (result)
            {
                return Content("1");
            }
            return Content("0");
        }
    }
}
