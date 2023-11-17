using System.Web.Mvc;

namespace EatWithChef.Areas.Ecommerce
{
    public class EcommerceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ecommerce";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "MenuPage",
                url: "ecommerce/thuc-don/{menuID}",
                defaults: new { controller = "Menu", action = "Index", menuID = UrlParameter.Optional },
                namespaces: new[] { "EatWithChef.Areas.Ecommerce.Controllers" }
            );

            //Search order by bill code.
            context.MapRoute(
                "Ecommerce_OrderBillPage",
                "ecommerce/xem-hoa-don",
                new { controller = "OrderServices", action = "GetOrderByBillPage", id = UrlParameter.Optional },
                namespaces: new[] { "EatWithChef.Areas.Ecommerce.Controllers" }
            );

            //get check out page.
            context.MapRoute(
                "Ecommerce_checkout",
                "ecommerce/thanh-toan",
                new { controller = "OrderServices", action = "GetCheckOutPage", id = UrlParameter.Optional },
                namespaces: new[] { "EatWithChef.Areas.Ecommerce.Controllers" }
            );

            context.MapRoute(
                "Ecommerce_Chef",
                "ecommerce/dau-bep",
                new { controller = "Chef", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "EatWithChef.Areas.Ecommerce.Controllers" }
            );

            context.MapRoute(
                "Ecommerce_ChefDetail",
                "ecommerce/dau-bep/{chefId}",
                new { controller = "Chef", action = "ChefDetailPage", id = UrlParameter.Optional },
                namespaces: new[] { "EatWithChef.Areas.Ecommerce.Controllers" }
            );

            context.MapRoute(
                "Ecommerce_QRCodeScan",
                "ecommerce/quet-qrcode",
                new { controller = "TrackDish", action = "QRCodeScan", id = UrlParameter.Optional },
                namespaces: new[] { "EatWithChef.Areas.Ecommerce.Controllers" }
            );

            context.MapRoute(
                "Ecommerce_TrackDishItem",
                "ecommerce/nguon-goc-thuc-pham/{dishItemId}",
                new { controller = "TrackDish", action = "TrackDishItem", id = UrlParameter.Optional },
                namespaces: new[] { "EatWithChef.Areas.Ecommerce.Controllers" }
            );

            context.MapRoute(
                "Ecommerce_default",
                "ecommerce/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "EatWithChef.Areas.Ecommerce.Controllers" }
            );
        }
    }
}
