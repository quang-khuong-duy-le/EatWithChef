//Get today order paging list.
function GetOrderPagingList(PageIndex) {
    $.ajax({
        url: "/Admin/OrderManagement/GetListPagingOrder",
        type: "GET",
        data: { PageSize: 8, PageIndex: PageIndex },
        success: function (result) {
            $("#list").html(result);
        }
    });
}

function GetOrderById(OrderId) {
    $.ajax({
        url: "/Admin/OrderManagement/GetOrderById",
        type: "GET",
        data: { OrderId: OrderId},
        success: function (result) {
            $("#detail").html(result);
        }
    });
}