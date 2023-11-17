using System.Web;
using System.Web.Optimization;

namespace EatWithChef
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
            //Front end layout css.
            bundles.Add(new StyleBundle("~/Content/frontend/css/Style").Include(
                        "~/Content/frontend/css/custom.css",
                        "~/Content/frontend/css/colors.css",
                        "~/Content/frontend/css/bootstrap.css",
                        "~/Content/frontend/css/bootstrap-responsive.css",
                        "~/Content/frontend/css/fullcalendar.css",
                        "~/Content/frontend/css/font-awesome.css",
                        "~/Content/frontend/css/flexslider.css",
                        "~/Content/frontend/css/jquery.mCustomScrollbar.css",
                        "~/Content/frontend/css/style.css"));

            //Front end script.
            bundles.Add(new ScriptBundle("~/Content/frontend/js/Scripts").Include(
                "~/Content/frontend/js/jquery.flexslider.js",
                "~/Content/frontend/js/fullcalendar.js",
                "~/Content/frontend/js/jquery.mCustomScrollbar.concat.min.js",
                "~/Content/frontend/js/excanvas.js",
                "~/Content/frontend/js/jquery.easy-pie-chart.js",
                "~/Content/frontend/js/jquery.bxslider.js",
                "~/Content/frontend/js/jquery.fitvids.js",
                "~/Content/frontend/js/jquery.jcarousel.min.js"));

            //Admin css
            bundles.Add(new StyleBundle("~/Content/backend/assets/stylesheets/css").Include(
                "~/Content/backend/assets/stylesheets/light-theme.css",
                "~/Content/backend/assets/stylesheets/theme-colors.css",
                "~/Content/backend/assets/stylesheets/demo.css"));
        }

    }
}