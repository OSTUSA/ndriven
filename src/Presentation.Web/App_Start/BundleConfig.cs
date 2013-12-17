using System.Web;
using System.Web.Optimization;

namespace Presentation.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                 "~/Scripts/jquery/*.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/*.js",
                "~/Scripts/angular/*.js",
                "~/Scripts/i18n/*.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap/*.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/date").Include(
                "~/Scripts/date/*.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                "~/Scripts/select2/*.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/ui-bootstrap").Include(
                "~/Scripts/ui-bootstrap/*.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/app.js",
                "~/Scripts/app/controllers/*.js",
                "~/Scripts/app/services/*.js",
                "~/Scripts/app/directives/*.js",
                "~/Scripts/app/filters/*.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/toaster").Include(
                "~/Scripts/toaster/*.js"
            ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap/css/*.css",
                "~/Content/modules/*.css",
                "~/Scripts/select2/select2.css",
                "~/Content/angular-ui/angular-ui.css",
                "~/Content/toaster/toastr.css",
                "~/Content/nDriven/css/*.css",
                "~/Content/nDriven/css/base.css"
           ));
        }
    }
}