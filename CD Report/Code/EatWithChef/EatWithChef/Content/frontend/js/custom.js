jQuery(document).ready(function ($) {

    "use strict";
    if ($('.bxslider').length) {
        $('.bxslider').bxSlider({
            pager: false,
        });
    }

    if ($('#timepicker1').length) {
        $('#timepicker1').timepicker();
    }

    if ($('.bxslider-2').length) {
        $('.bxslider-2').bxSlider({
            mode: 'fade',
            captions: true
        });
    }

    $(".navbar-inner ul >li").hover(
		function () {
		    $(this).addClass('open');
		},
		function () {
		    $(this).removeClass('open');
		}
	);

    if ($('.gallery_fun').length) {
        $(".gallery_fun:first a[rel^='prettyPhoto']").prettyPhoto({
            animation_speed: 'normal',
            slideshow: 10000,
            autoplay_slideshow: true
        });
        $(".gallery_fun:gt(0) a[rel^='prettyPhoto']").prettyPhoto({
            animation_speed: 'slow',
            slideshow: 10000,
            hideflash: true
        });
    }


    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    $('#calendar').fullCalendar({
        editable: true,
        //weekends: false, // will hide Saturdays and Sundays
        events: [
			{
			    title: 'All Day Event',
			    start: new Date(y, m, 1)
			},
			{
			    title: 'Long Event',
			    start: new Date(y, m, d - 5),
			    end: new Date(y, m, d - 2)
			},
			{
			    id: 999,
			    title: 'Repeating Event',
			    start: new Date(y, m, d - 3, 16, 0),
			    allDay: false
			},
			{
			    id: 999,
			    title: 'Repeating Event',
			    start: new Date(y, m, d + 4, 16, 0),
			    allDay: false
			},
			{
			    title: 'Meeting',
			    start: new Date(y, m, d, 10, 30),
			    allDay: false
			},
			{
			    title: 'Lunch',
			    start: new Date(y, m, d, 12, 0),
			    end: new Date(y, m, d, 14, 0),
			    allDay: false
			},
			{
			    title: 'Birthday Party',
			    start: new Date(y, m, d + 1, 19, 0),
			    end: new Date(y, m, d + 1, 22, 30),
			    allDay: false
			},
			{
			    title: 'Click for Google',
			    start: new Date(y, m, 28),
			    end: new Date(y, m, 29),
			    url: 'http://google.com/'
			}
        ]
    });

    if ($('#map_canvas').length) {
        var map;
        var myLatLng = new google.maps.LatLng(40.676498, -73.623132)
        //Initialize MAP
        var myOptions = {
            zoom: 16,
            center: myLatLng,
            disableDefaultUI: true,
            zoomControl: true,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        map = new google.maps.Map(document.getElementById('map_canvas'), myOptions);
        //End Initialize MAP
        //Set Marker
        var marker = new google.maps.Marker({
            position: map.getCenter(),
            map: map
        });
        marker.getPosition();
        //End marker

        //Set info window
        var infowindow = new google.maps.InfoWindow({
            content: '',
            position: myLatLng
        });
        infowindow.open(map);
    }



    //Percent Script
    if ($('.percentage').length) {
        $('.percentage').easyPieChart({
            animate: 1000,

            onStep: function (value) {
                this.$el.find('span').text(~~value);
            }
        });
    }

    //Scroll Script
    if ($('.scroll').length) {
        $(".scroll").mCustomScrollbar({
            set_width: false, /*optional element width: boolean, pixels, percentage*/
            set_height: false, /*optional element height: boolean, pixels, percentage*/
            horizontalScroll: false, /*scroll horizontally: boolean*/
            scrollInertia: 950, /*scrolling inertia: integer (milliseconds)*/
            mouseWheel: true, /*mousewheel support: boolean*/
            mouseWheelPixels: "auto", /*mousewheel pixels amount: integer, "auto"*/
            autoDraggerLength: true, /*auto-adjust scrollbar dragger length: boolean*/
            autoHideScrollbar: false, /*auto-hide scrollbar when idle*/
            scrollButtons: { /*scroll buttons*/
                enable: false, /*scroll buttons support: boolean*/
                scrollType: "continuous", /*scroll buttons scrolling type: "continuous", "pixels"*/
                scrollSpeed: "auto", /*scroll buttons continuous scrolling speed: integer, "auto"*/
                scrollAmount: 40 /*scroll buttons pixels scroll amount: integer (pixels)*/
            },
            advanced: {
                updateOnBrowserResize: true, /*update scrollbars on browser resize (for layouts based on percentages): boolean*/
                updateOnContentResize: false, /*auto-update scrollbars on content resize (for dynamic content): boolean*/
                autoExpandHorizontalScroll: false, /*auto-expand width for horizontal scrolling: boolean*/
                autoScrollOnFocus: true, /*auto-scroll on focused elements: boolean*/
                normalizeMouseWheelDelta: false /*normalize mouse-wheel delta (-1/1)*/
            },
            contentTouchScroll: true, /*scrolling by touch-swipe content: boolean*/
            callbacks: {
                onScrollStart: function () { }, /*user custom callback function on scroll start event*/
                onScroll: function () { }, /*user custom callback function on scroll event*/
                onTotalScroll: function () { }, /*user custom callback function on scroll end reached event*/
                onTotalScrollBack: function () { }, /*user custom callback function on scroll begin reached event*/
                onTotalScrollOffset: 0, /*scroll end reached offset: integer (pixels)*/
                onTotalScrollBackOffset: 0, /*scroll begin reached offset: integer (pixels)*/
                whileScrolling: function () { } /*user custom callback function on scrolling event*/
            },
            theme: "light" /*"light", "dark", "light-2", "dark-2", "light-thick", "dark-thick", "light-thin", "dark-thin"*/
        });
    }

    //Datepicker Script
    var date_picker_CP = $('#dp3');
    if (date_picker_CP.length) {
        $('#dp3').datepicker({
            format: 'mm-dd-yyyy'
        });
    }


    window.onload = function () {
        if ($('#mycarousel').length) {
            $('#mycarousel').jcarousel();
        }
    };
    //Carousel Script


    //Bootstrap Tab Script
    if ($('#myTab').length) {
        $('#myTab a').click(function (e) {
            e.preventDefault();
            $(this).tab('show');
        });
    }

    // hide #back-top first
    if ($('#back-top').length) {
        $("#back-top").hide();

        // fade in #back-top
        $(function () {
            $(window).scroll(function () {
                if ($(this).scrollTop() > 100) {
                    $('#back-top').fadeIn();
                } else {
                    $('#back-top').fadeOut();
                }
            });

            // scroll body to 0px on click
            $('#back-top a').click(function () {
                $('body,html').animate({
                    scrollTop: 0
                }, 800);
                return false;
            });
        });
    }
    if ($('.accordion-body').length) {
        $('.accordion-body').on('show',
		  function (e) {
		      $(e.currentTarget).parent().find('.accordion-heading').addClass('active')
		  }
		)

        $('.accordion-body').on('hide',
		  function (e) {
		      $(e.currentTarget).parent().find('.accordion-heading').removeClass('active')
		  }
		)
    }
});



// Javascript for cart
window.onload = initCart();

//Check Dish quota
function checkQuota(DishID, MenuID, Quantity) {
    var result = false;
    $.ajax({
        url: "/ecommerce/OrderServices/CheckDishQuota",
        type: "POST",
        async: false,
        data: { DishID: DishID, MenuID: MenuID, NumOfDishInCart: Quantity },
        success: function (data) {
            if (data == 1) {
                result = true;
            }
        }
    });
    return result;
}

function showcart(pos, isElement) {
    var status = $("#status-show").val();

    if (isElement == -1) {
        $("#shopping-cart").css("top", '-300px');
        $("#shopping-cart").css("z-index", '-1');
        $(".cart-icon > div").css("background-image", "url(/Content/images/cart.png)");
    } else if (status == 1) {
        $("#shopping-cart").css("top", '65px');
        $(".cart-icon > div").css("background-image", "url(/Content/images/cart-hover.png)");
    } else if (isElement == 1) {
        $("#shopping-cart").css("top", pos);
        $("#shopping-cart").css("z-index", '1');
    }
}

function showFilter(pos, isElement) {
    var status = $("#status-show").val();

    if (isElement == -1) {
        $("#filter-region").css("top", '-300px');
    } else if (status == 1) {
        $("#filter-region").css("top", '65px');
    } else if (isElement == 1) {
        $("#filter-region").css("top", pos);
    }
}

function showNoneFace(zIndex, opacity) {
    $("#none-item").css("z-index", zIndex);
    $("#none-item").css("opacity", opacity);
}

function initCart() {
    $("#slider-cart").html("");
    $("#control-cart").remove();

    var cart = [];
    if (localStorage["EWC-CART"] != null) {
        cart = JSON.parse(localStorage.getItem("EWC-CART"));
    }
    var listItem = [];
    var menu = [];
    for (var i = 0; i < cart.length; i++) {
        listItem.push(cart[i].itemID);
        menu.push(cart[i].menuID);
    }

    $.ajax({
        url: "/ecommerce/OrderServices/GetCartData",
        type: "GET",
        traditional: true,
        data: { cartIem: listItem, cartMenu: menu },
        success: function (data) {
            var list = JSON.parse(data);
            if (list != null) {
                var cartQuanlity = 0;
                var total = 0;
                for (var i = 0; i < list.length; i++) {
                    $("#slider-cart").css("width", (210 * (i + 1)) + "px");
                    path = window.location.protocol + "//" + window.location.host + list[i].DishImage;
                    var htmlItem = "<div class='item' id='item-" + list[i].DishID + "'><center><img src='" + path + "'/></center><div class='title' title='" + list[i].DishName + "'> " + list[i].DishName + " </div>";
                    htmlItem += createCBox(list[i].DishID, cart[i].quantity) + "<div class='price-item' id='price-cart-item-" + list[i].DishID + "'>" + list[i].DishPrice + "</div></div>";
                    $("#slider-cart").append(htmlItem);

                    $("#dish-in-cart-" + list[i].DishID).html(cart[i].quantity);
                    $("#dish-in-cart-" + list[i].DishID).css("z-index", 0);
                    $("#dish-in-cart-" + list[i].DishID).css("opacity", 1);
                    $("#dish-in-cart-details-" + list[i].DishID).html(cart[i].quantity);
                    $("#dish-in-cart-details-" + list[i].DishID).css("opacity", 1);
                    $("#add-btn-" + list[i].DishID).addClass("active-btn");

                    cartQuanlity += parseInt(cart[i].quantity);
                    total += list[i].DishPrice * cart[i].quantity;

                    showNoneFace('-2', 0);
                    $("#price-table").css("visibility", "visible");
                    $("#add-button-" + list[i].DishID).addClass("active-btn");
                }
                $("#number-of-cart-item").html(cartQuanlity);
                $("#total-price").html(total);

                if ($("#total-count-checkout") != "") {
                    $("#total-count-checkout").html(cartQuanlity);
                    $("#total-price-checkout").html(total);
                    $("#all-price-checkout").html(total);
                }
            }

            if (list.length > 3) {
                $("#contain-item").append("<div id='control-cart'><div class='add-button active-btn' style='position:absolute;bottom:20px; left:40px; border: 1px solid #f27242;background-color: #f27242 !important;width: auto;font-weight: bold;' onclick='moveCart(1)'>></div><div class='add-button active-btn' style='position:absolute;bottom:20px; left:0;border: 1px solid #f27242;background-color: #f27242 !important;width: auto;font-weight: bold;' onclick='moveCart(0)'><</div><</div>");
            }
        }
    });
}

function addToCart(dishID) {
    var MenuId = $("#menu-id").val();
    var isChange = false;

    var flagIsExsit = false;
    var cart = [];
    if (localStorage["EWC-CART"] != null) {
        cart = JSON.parse(localStorage.getItem("EWC-CART"));
    }

    for (var i = 0; i < cart.length; i++) {
        var item = cart[i];
        if (item.itemID == dishID) {
            //Check dish quota.
            flagIsExsit = true;
            if (item.quantity < 10) {
                isChange = checkQuota(dishID, MenuId, (item.quantity + 1));
                if (isChange) {
                    item.quantity++;
                    $("#quanlity-item-" + dishID).val(item.quantity);
                    $("#dish-in-cart-" + dishID).html(item.quantity);
                    $("#dish-in-cart-details-" + dishID).html(item.quantity);
                } else {
                    alert("Đã hết số lượng của món!");
                }
            } else {
                alert("Số lượng mọi món phải nhỏ hơn 10!");
            }
            break;
        }
    }

    if (!flagIsExsit) {
        isChange = checkQuota(dishID, MenuId, 1);
        if (isChange) {
            $("#slider-cart").css("width", (210 * (cart.length + 1)) + "px");
            var item = { itemID: dishID, quantity: 1, menuID: $("#menu-id").val() };
            cart.push(item);
            var htmlItem = "<div class='item' id='item-" + dishID + "'><center><img src='" + document.getElementById("image-" + dishID).src + "'/></center><div class='title' title='" + $("#disk-" + dishID).html() + "'> " + $("#disk-" + dishID).html() + " </div>";
            htmlItem += createCBox(dishID, item.quantity) + "<div class='price-item' id='price-cart-item-" + dishID + "'>" + $("#price-dish-" + dishID).html() + "</div></div>";

            $("#slider-cart").append(htmlItem);

            $("#dish-in-cart-" + dishID).html("1");
            $("#dish-in-cart-" + dishID).css("z-index", 0);
            $("#dish-in-cart-" + dishID).css("opacity", 1);
            $("#dish-in-cart-details-" + dishID).html("1");
            $("#dish-in-cart-details-" + dishID).css("opacity", 1);

            $("#add-button-" + dishID).addClass("active-btn");
        } else {
            alert("Đã hết số lượng của món!");
        }
    }

    if (isChange) {
        showNoneFace('-2', 0);
        $("#price-table").css("visibility", "visible");

        if (cart.length > 3 && $("#control-cart").val() == null) {
            $("#contain-item").append("<div id='control-cart'><div class='add-button active-btn' style='position:absolute;bottom:20px; left:40px;border: 1px solid #f27242;background-color: #f27242 !important;width: auto;font-weight: bold;' onclick='moveCart(1)'>></div><div class='add-button active-btn' style='position:absolute;bottom:20px; left:0;border: 1px solid #f27242;background-color: #f27242 !important;width: auto;font-weight: bold;' onclick='moveCart(0)'><</div><</div>");
        }

        var total = parseInt($("#total-price").html().trim()) + parseInt($("#price-dish-" + dishID).html().trim());
        $("#total-price").html(total);

        var value = parseInt($("#number-of-cart-item").html().trim());
        $("#number-of-cart-item").html(++value);

        $("#add-btn-" + dishID).addClass("active-btn");
    }

    localStorage.setItem("EWC-CART", JSON.stringify(cart));

}

function updateCart(dishID) {
    var value = $("#quanlity-item-" + dishID).val();
    var cart = [];
    if (localStorage["EWC-CART"] != null) {
        cart = JSON.parse(localStorage.getItem("EWC-CART"));
    }

    for (var i = 0; i < cart.length; i++) {
        var item = cart[i];
        if (item.itemID == dishID) {
            if (value == 0) {

                $("#add-button-" + dishID).removeClass("active-btn");

                var totalItem = parseInt($("#number-of-cart-item").html().trim());
                totalItem -= item.quantity;
                $("#number-of-cart-item").html(totalItem);

                var totalPrice = parseInt($("#total-price").html().trim());
                totalPrice -= item.quantity * parseInt($("#price-cart-item-" + dishID).html().trim());
                $("#total-price").html(totalPrice);

                $("#item-" + dishID).remove();
                cart.splice(i, 1);

                $("#dish-in-cart-" + dishID).html(0);
                $("#dish-in-cart-" + dishID).css("z-index", -1);
                $("#dish-in-cart-" + dishID).css("opacity", 0);
                $("#dish-in-cart-details-" + dishID).css("opacity", 0);
                $("#dish-in-cart-details-" + dishID).html(0);

                if (cart.length < 4) {
                    $("#control-cart").remove();
                }

                $("#slider-cart").css("left", 0);
                currentSlider = 1;

                if (cart.length == 0) {
                    showNoneFace('2', 1);
                    $("#price-table").css("visibility", "hidden");
                }

            } else {
                if (checkQuota(dishID, item.menuID, value)) {
                    var totalItem = parseInt($("#number-of-cart-item").html().trim());
                    totalItem += (value - item.quantity);
                    $("#number-of-cart-item").html(totalItem);

                    var totalPrice = parseInt($("#total-price").html().trim());
                    totalPrice += (value - item.quantity) * parseInt($("#price-cart-item-" + dishID).html().trim());
                    $("#total-price").html(totalPrice);

                    item.quantity = value;
                    $("#dish-in-cart-" + dishID).html(item.quantity);
                    $("#dish-in-cart-details-" + dishID).html(item.quantity);
                } else {
                    $("#quanlity-item-" + dishID).val(item.quantity);
                    alert("Đã hết số lượng của món!");
                }
            }
            break;
        }
    }

    localStorage.setItem("EWC-CART", JSON.stringify(cart));
}

var currentSlider = 1;
function moveCart(choice) {
    var cart = JSON.parse(localStorage.getItem("EWC-CART"));
    if (choice == 1) {
        if (cart.length > (currentSlider * 3)) {
            $("#slider-cart").css("left", "-" + (630 * currentSlider) + "px");
            currentSlider++;
        }
    } else {
        if (currentSlider > 1) {
            currentSlider--;
            $("#slider-cart").css("left", "-" + (630 * (currentSlider - 1)) + "px");
        }
    }
}

function createCBox(id, choice) {
    var result = "<select id='quanlity-item-" + id + "' onchange='updateCart(" + id + ")'>";
    result += "<option value='0'>Remove</option>";
    for (var i = 1; i <= 10; i++) {
        if (i == choice) {
            result += "<option selected='selected' value='" + i + "'>" + i + "</option>";
        } else {
            result += "<option value='" + i + "'>" + i + "</option>";
        }
    }
    result += "</select>";
    return result;
}

//Javascript for order process frontend.
//Get product for checkout page.
function getCheckoutData() {
    var dishID = [];
    var menu = [];
    var dishQuantity = [];
    var cart = JSON.parse(localStorage.getItem("EWC-CART"));
    if (cart != null) {
        for (var i = 0; i < cart.length; i++) {
            dishID.push(cart[i].itemID);
            dishQuantity.push(cart[i].quantity);
            menu.push(cart[i].menuID);
        }

        //ajax to get dish price.
        $.ajax({
            url: "/ecommerce/OrderServices/GetOrderCheckoutPage",
            type: "GET",
            traditional: true,
            data: { ListDishID: dishID, MenuID: menu, Quantity: dishQuantity },
            success: function (result) {
                $(".order-cart-confirm").html(result);
            }
        });
    } else {
        alert("Chưa có hàng trong giỏ. Vui lòng thêm vào giỏ hàng!");
        window.location.href = "/Ecommerce/Menu";
    }
}

//Submit Checkout.
function SubmitCheckOut() {
    //Get cart in local storage.
    var cart = [];
    if (localStorage["EWC-CART"] != null) {
        cart = JSON.parse(localStorage.getItem("EWC-CART"));
    }
    if (cart != null) {
        var ListMenuID = [];
        var ListDishID = [];
        var ListQuantity = [];
        for (var i = 0; i < cart.length ; i++) {
            ListMenuID.push(cart[i].menuID);
            ListDishID.push(cart[i].itemID);
            ListQuantity.push(cart[i].quantity);
        }
        //Get Receiver Information.
        var OrderInfo = {
            ReceiverName: $("#ReceiverName").val(),
            ReceiverPhone: $("#ReceiverPhone").val(),
            ReceiverAddress: $("#ReceiverAddress").val(),
            ReceiverEmail: $("#ReceiverEmail").val(),
            Note: $("#OrderNote").val(),
            ListDishID: ListDishID,
            MenuID: ListMenuID,
            Quantity: ListQuantity
        }
        //Submit check out ajax.
        $.ajax({
            url: "/Ecommerce/OrderServices/SubmitCheckout",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            datatype: "json",
            traditional: true,
            data: JSON.stringify(OrderInfo),
            success: function (result) {
                if (result != 0) {
                    //Clear cart data.
                    localStorage.removeItem("EWC-CART");
                    $(".checkout-form-container").html("<h4>Đặt hàng thành công. Cảm ơn bạn đã sử dụng dịch vụ! Click vào <a href='/ecommerce/xem-hoa-don'>đây</a> để tra cứu thông tin đơn hàng.</h4>");
                }
            }
        });
    } else {
        alert("Chưa có hàng trong giỏ. Vui lòng thêm vào giỏ hàng!");
        window.location.href = "/Ecommerce/Menu";
    }
}

var current = 0;
function nextItemSlider() {
    var max = parseInt($("#max-dish").val());
    if (current == 0) {
        $(".control-silder").append("<div id='prev-item' class='control-item' style='right:740px;text-align:left;' onclick='prevItemSlider()'></div>");
    }
    
    $(".slider-list").css("left", "-" + (current + 1) * 940 + "px");
    $("#item-slider-" + (current + 1)).css("opacity", 1);
    $("#item-slider-" + current).css("opacity", 0);

    $("#prev-item").html("<span>< Món trước</span><p>" + $("#name-dish-detail").html() + "</p>");
    $("#name-dish-detail").html($("#name-dish-" + (current + 1)).val());
    current++;

    if (current == max - 1) {
        $("#next-item").remove();
    } else {
        $("#next-item p").html($("#name-dish-" + (current + 1)).val());
    }
}

function prevItemSlider() {
    var max = parseInt($("#max-dish").val());
    if (current == max - 1) {
        $(".control-silder").append("<div id='next-item' class='control-item' onclick='nextItemSlider()'></div>");
    }

    $(".slider-list").css("left", "-" + (current - 1) * 940 + "px");
    $("#item-slider-" + (current - 1)).css("opacity", 1);
    $("#item-slider-" + current).css("opacity", 0);

    $("#next-item").html("<span>Món tiếp ></span><p>" + $("#name-dish-detail").html() + "</p>");
    $("#name-dish-detail").html($("#name-dish-" + (current - 1)).val());
    current--;

    if (current == 0) {
        $("#prev-item").remove();
    } else {
        $("#prev-item p").html($("#name-dish-" + (current - 1)).val());
    }
}

//Show dish detail page.
function ShowDishDetail(CurrentIndex) {
    current = CurrentIndex;
    $("#item-slider-" + current).css("opacity", 1);
    var max = parseInt($("#max-dish").val());

    if (current == 0) {
        $("#prev-item").remove();
    } else if (current > 0 && $("#prev-item").length == 0) {
        $(".control-silder").append("<div id='prev-item' class='control-item' style='right:740px;text-align:left;' onclick='prevItemSlider()'></div>");
    }

    if (current == max - 1) {
        $("#next-item").remove();
    } else if (current < max - 1 && $("#next-item").length == 0) {
        $(".control-silder").append("<div id='next-item' class='control-item' onclick='nextItemSlider()'></div>");
    }

    $(".slider-list").css("left", "-" + current * 940 + "px");

    $("#next-item").html("<span>Món tiếp ></span><p>" + $("#name-dish-" + (current + 1)).val() + "</p>");
    $("#name-dish-detail").html($("#name-dish-" + (current)).val());
    $("#prev-item").html("<span>< Món trước</span><p>" + $("#name-dish-" + (current - 1)).val() + "</p>");

    $(".list-of-dish").css("z-index", 10);
    $(".list-of-dish").css("opacity", 1);
    $(".list-of-dish").css("background-color", "rgba(247, 247, 247, 0.97)");
    $("body").css("overflow", "hidden");
}

function CloseDetailDish() {
    $("#item-slider-" + current).css("opacity", 0);
    $("body").css("overflow", "visible");
    $(".list-of-dish").css("z-index", -1);
    $(".list-of-dish").css("opacity", 0);
}

//Get Dish by menu id.
function GetDataMenu(MenuId, thisElement) {

    $(".loading-process").css("opacity", "1");
    $.ajax({
        url: "/ecommerce/Menu/GetDishByMenuIDPartial",
        type: "GET",
        data: { MenuId: MenuId },
        async:false,
        success: function (result) {
            window.history.pushState("", "Menu State", "/ecommerce/thuc-don/" + MenuId);
            $("#Container").html(result);
            $('#Container').mixItUp('destroy');
            $("#Container").mixItUp();

            var listElement = $("#list-choice-menu").find("li");
            for (var i = 0; i < listElement.length; i++) {
                if (listElement[i] == thisElement) {
                    $(thisElement).css("background-color", "rgb(243, 243, 243)");
                    $(listElement[i]).attr("data-active", "1");
                } else {
                    $(listElement[i]).css("background-color", "rgb(255, 255, 255)");
                    $(listElement[i]).attr("data-active", "0");
                }
            }

            initCart();
            $("#show-menu").html($("#choice-menu-" + MenuId).html());

            setTimeout(function () { $(".loading-process").css("opacity", "0"); }, 200);
        }
    });
    
}

function moveMenu(action) {
    var listElement = $("#list-choice-menu").find("li");
    for (var i = 0; i < listElement.length; i++)
    {
        if (action == 1)
        {
            if ($(listElement[i]).attr("data-active") == 1 && i < (listElement.length - 1))
            {
                $(listElement[i + 1]).click();
                break;
            }
        } else {
            if ($(listElement[i]).attr("data-active") == 1 && i > 0)
            {
                $(listElement[i - 1]).click();
                break;
            }
        }
    }
}

function activeTabDetail(thisElment, dishID) {
    var aElements = thisElment.parentNode.getElementsByTagName("li");

    for (var i = 0; i < aElements.length; i++) {
        if (aElements[i] == thisElment) {
            thisElment.setAttribute("class", "active-li");
            $("#tab-" + dishID + "-" + i).addClass("active");
        } else {
            aElements[i].setAttribute("class", "");
            $("#tab-" + dishID + "-" + i).removeClass("active");
        }
    }
}

//Search order by bill id.
function SearchOrderByBill() {
    var BillCode = $("#BillCode").val();
    $.ajax({
        url: "/ecommerce/OrderServices/GetOrderByBillCode",
        type: "GET",
        data: { BillCode: BillCode},
        success: function (result) {
            $("#list-bill-order").html(result);
        }
    });
}