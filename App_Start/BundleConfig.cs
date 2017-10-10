using System.Web;
using System.Web.Optimization;

namespace RioManager
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.2.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/infinite-scroll-docs.css",
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/Rio.css"));

            bundles.Add(new ScriptBundle("~/bundles/fancybox").Include(
                        "~/Scripts/jquery.fancybox.js",
                        "~/Scripts/jquery.fancybox.pack.js"));

            bundles.Add(new ScriptBundle("~/bundles/infinitescroll").Include(
                        "~/Scripts/infinite-scroll.pkgd.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/createAlbum").Include(
                        "~/Scripts/RioCreateAlbum.js"));

            bundles.Add(new ScriptBundle("~/bundles/RioTools").Include(
                        "~/Scripts/RioTools.js"));
        }
    }
}
