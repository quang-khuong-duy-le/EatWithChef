using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.DataAccess.Abstract
{
    public interface IOrderRepository:IDisposable
    {
        /// <summary>
        /// Get bill by bill code user input to search orders.
        /// </summary>
        /// <param name="BillCode">Bill code user input</param>
        /// <returns>Bill which has code that user inputed</returns>
        Bill GetBillByCode(string BillCode);

        /// <summary>
        /// Get list order of specific bill.
        /// </summary>
        /// <param name="BillId">Bill id input to search orders.</param>
        /// <returns>List order of Bill which has Id inputed.</returns>
        List<Order> GetOrdersByBillId(int BillId);

        /// <summary>
        /// Get order detail by order id.
        /// </summary>
        /// <param name="OrderId">OrderId</param>
        /// <returns>List order detail by order id.</returns>
        List<OrderDetail> GetOrderDetail(int OrderId);

        /// <summary>
        /// Get dish item by order detail.
        /// </summary>
        /// <param name="OrderDetailId">OrderDetailId</param>
        /// <returns>List dish item which assign to order detail.</returns>
        List<OrderDetailDishItem> GetOrderDetailDishItem(int OrderDetailId);

        /// <summary>
        /// Create bill check out page. 
        /// </summary>
        /// <param name="BillItem">Bill item to create.</param>
        /// <returns>BillId if create success, 0 if error occur</returns>
        int CreateBill(Bill BillItem);

        /// <summary>
        /// Create order of bill.
        /// </summary>
        /// <param name="OrderItem">Order item in bill</param>
        /// <returns>OrderId if create success, 0 if error occur</returns>
        int CreateOrder(Order OrderItem);

        /// <summary>
        /// Create order detail of an order.
        /// </summary>
        /// <param name="OrderDetailItem">Order detail item of an order.</param>
        /// <returns>OrderDetailId if create success, 0 if error occur.</returns>
        int CreateOrderDetail(OrderDetail OrderDetailItem);

        /// <summary>
        /// Get number of today order.
        /// </summary>
        /// <returns>Number of today order</returns>
        int GetNumberOfTodayOrder();

        /// <summary>
        /// Get list paging today order.
        /// </summary>
        /// <returns>List today order with paging.</returns>
        List<Order> GetTodayOrder(int PageSize, int PageIndex);

        /// <summary>
        /// Get order by id for view detail.
        /// </summary>
        /// <param name="OrderId">Id of order.</param>
        /// <returns>Order which has id input.</returns>
        Order GetOrderById(int OrderId);

        List<OrderDetail> GetOrderDetailByOrderId(int OrderId);

        List<Order> GetOrderByStatus(int OrderStatus);

        bool Update(Order newOrder);

        bool UpdateStatus(int orderID, int status);
    }
}
