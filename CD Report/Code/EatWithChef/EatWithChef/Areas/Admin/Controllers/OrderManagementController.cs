using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using Domain.DataAccess;
using Domain.Utility;
using Domain.DataAccess.Abstract;
using Domain.DataAccess.Concrete;

namespace EatWithChef.Areas.Admin.Controllers
{
    public class OrderManagementController : Controller
    {
        private IOrderRepository _orderRepository;

        public OrderManagementController() {
            _orderRepository = new OrderRepository();
        }

        //Index view page.
        public ActionResult Index() {
            return View();
        }

        //Get all orders max item in page = 8.
        public ActionResult GetListPagingOrder(int PageSize, int PageIndex) {
            List<Order> ListOrder = _orderRepository.GetTodayOrder(PageSize,PageIndex);
            double ratio = (double)_orderRepository.GetNumberOfTodayOrder() / (double)PageSize;
            ViewBag.MaxPage = (int)Math.Ceiling(ratio);
            ViewBag.PageIndex = PageIndex;
            return PartialView("ListOrderPartial",ListOrder);
        }

        //Get first today order.
        public ActionResult GetFirstTodayOrder() {
            Order result = null;
            var order = _orderRepository.GetTodayOrder(8,1);
            if (order != null) {
                result = order.ElementAt(0);
            }
            return PartialView("OrderDetailPartial",result);
        }

        //Get order by id for detail view.
        public ActionResult GetOrderById(int OrderId) {
            var order = _orderRepository.GetOrderById(OrderId);
            return PartialView("OrderDetailPartial", order); 
        }

        //Get order detail by order id.
        public ActionResult GetOrderDetailByOrderId(int OrderId) {
            var orderdetail = _orderRepository.GetOrderDetailByOrderId(OrderId);
            return PartialView("ListOrderDetailPartial",orderdetail);
        }

        private List<Order> GetOrderByStatus(int OrderStatus) {
            var result = _orderRepository.GetOrderByStatus(OrderStatus);
            return result;
        }
    }
}
