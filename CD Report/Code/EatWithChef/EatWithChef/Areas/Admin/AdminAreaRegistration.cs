using System.Web.Mvc;

namespace EatWithChef.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //Set route for FilesController
            context.MapRoute(null, "Admin/Admin/connector", new { controller = "Admin", action = "LoadFile" });
            context.MapRoute(null, "Admin/Admin/Thumbnails/{tmb}", new { controller = "Admin", action = "Thumbs", tmb = UrlParameter.Optional });
            context.MapRoute(
                name: "Order_Management",
                url: "admin/don-hang",
                defaults: new { controller = "OrderManagement", action = "Index"},
                namespaces: new[] { "EatWithChef.Areas.Admin.Controllers" }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
