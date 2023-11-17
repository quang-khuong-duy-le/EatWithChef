using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DataAccess.Abstract;
using Domain.Entity;
using Domain.Utility;

namespace Domain.DataAccess.Concrete
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EWCEntities _dbContext;

        public OrderRepository()
        {
            _dbContext = new EWCEntities();
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }

        //Private usage method.
        #region PrivateUsageMethod

        #endregion

        //Get bill by bill code.
        public Bill GetBillByCode(string BillCode)
        {
            var bill = _dbContext.Bills.Where(b => b.Code.Equals(BillCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            return bill;
        }

        //Get orders by bill Id.
        public List<Order> GetOrdersByBillId(int BillId)
        {
            var ListOrder = _dbContext.Orders.Where(order => order.BillId == BillId && order.OrderStatus != (int)OrderStatusEnum.Cancel).ToList();
            return ListOrder;
        }

        //Get Order detail by order id.
        public List<OrderDetail> GetOrderDetail(int OrderId)
        {
            var ListOrderDetail = _dbContext.OrderDetails.Where(orderdetail => orderdetail.OrderID == OrderId).ToList();
            return ListOrderDetail;
        }

        //Get Dish item by Order detail.
        public List<OrderDetailDishItem> GetOrderDetailDishItem(int OrderDetailId)
        {
            List<DishItem> ListDishItem = new List<DishItem>();
            var ListDishItemOrderDetail = _dbContext.OrderDetailDishItems.Where(d => d.OrderDetailId == OrderDetailId).ToList();
            return ListDishItemOrderDetail;
        }

        //Create bill.
        public int CreateBill(Bill BillItem)
        {
            try
            {
                _dbContext.Bills.Add(BillItem);
                _dbContext.SaveChanges();
                return BillItem.Id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //Create order.
        public int CreateOrder(Order OrderItem)
        {
            try
            {
                _dbContext.Orders.Add(OrderItem);
                _dbContext.SaveChanges();
                return OrderItem.Id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //Create order detail.
        public int CreateOrderDetail(OrderDetail OrderDetailItem)
        {
            try
            {
                _dbContext.OrderDetails.Add(OrderDetailItem);
                _dbContext.SaveChanges();
                return OrderDetailItem.Id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //Get number of today order.
        public int GetNumberOfTodayOrder()
        {
            var ListOrder = _dbContext.Orders.Where(order => order.OrderDate.Year == DateTime.Now.Year && order.OrderDate.Month == DateTime.Now.Month && order.OrderDate.Day == DateTime.Now.Day).ToList();
            if (ListOrder != null)
            {
                return ListOrder.Count;
            }
            return 0;
        }

        //Get today paging order exclude cancel status.
        public List<Order> GetTodayOrder(int PageSize, int PageIndex)
        {
            var result = _dbContext.Orders.Where(order => ((order.OrderDate.Year == DateTime.Now.Year 
                        && order.OrderDate.Month == DateTime.Now.Month 
                        && order.OrderDate.Day == DateTime.Now.Day)
                        ||(order.DeliveryDate.Year == DateTime.Now.Year
                        && order.DeliveryDate.Month == DateTime.Now.Month
                        && order.DeliveryDate.Day == DateTime.Now.Day))).OrderBy(ord => ord.OrderDate);
            double ratio = (double)result.ToList().Count / (double)PageSize;
            int MaxPage = (int)Math.Ceiling(ratio);
            int SkipItem = (PageIndex - 1) * PageSize;
            return result.Skip(SkipItem).Take(PageSize).ToList();
        }

        //Get order by id for view detail.
        public Order GetOrderById(int OrderId) {
            var result = _dbContext.Orders.Find(OrderId);
            return result;
        }

        public List<OrderDetail> GetOrderDetailByOrderId(int OrderId) {
            var result = _dbContext.OrderDetails.Where(od => od.OrderID == OrderId).ToList();
            return result;
        }

        //Update order information.
        public bool Update(Order newOrder)
        {
            try
            {
                //Get old order in database.
                Order oldOrder = _dbContext.Orders.Find(newOrder.Id);
                if (oldOrder != null)
                {
                    oldOrder.ReceiverName = newOrder.ReceiverName;
                    oldOrder.ReceiverAddress = newOrder.ReceiverAddress;
                    oldOrder.ReceiverEmail = newOrder.ReceiverEmail;
                    oldOrder.ReceiverPhone = newOrder.ReceiverPhone;
                    oldOrder.TotalPayment = newOrder.TotalPayment;
                    oldOrder.DeliveryDate = newOrder.DeliveryDate;
                    oldOrder.Note = newOrder.Note;
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        //Update order status.
        public bool UpdateStatus(int orderID, int status)
        {
            try
            {
                Order orderItem = _dbContext.Orders.Find(orderID);
                if (orderItem != null)
                {
                    orderItem.OrderStatus = status;
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public List<Order> GetOrderByStatus(int OrderStatus)
        {
            var listOrder = _dbContext.Orders.Where(order => order.OrderStatus == OrderStatus).ToList();
            return listOrder;
        }
    }
}
