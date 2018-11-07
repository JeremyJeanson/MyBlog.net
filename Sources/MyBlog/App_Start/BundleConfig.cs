using MyBlog.Engine;
using System.Web.Optimization;

namespace MyBlog
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region CSS Layout

            // Common parts
            bundles.Add(new StyleBundle("~/Content/Layout").Include(
                "~/Content/bundle-layout.css"));

            // Style produit via Less (Default)
            bundles.Add(new StyleBundle(Urls.ContentDefault).Include(
                "~/Content/bundle-default.css"));

            // Style produit via Less (Default)
            bundles.Add(new StyleBundle(Urls.ContentDys).Include(
                "~/Content/bundle-dys.css"));

            // CSS for Post
            bundles.Add(new StyleBundle("~/Content/Post").Include(
                //"~/Content/Highlight/default.css",
                "~/Content/Highlight/vs.css"));

            #endregion

            #region Scripts

            // Scripts for layout
            bundles.Add(new ScriptBundle("~/Scripts/Default").Include(
                "~/Scripts/jquery-{version}.js",
                // "~/Scripts/popper.js",
                "~/Scripts/bootstrap.bundle.js",
                "~/Scripts/jquery.blockUI.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Layout").Include(
                "~/Scripts/Views/Layout.js",
                "~/Scripts/Views/GetMoreItems.js",
                "~/Scripts/Views/Accessibility.js"));

            // Scripts for Post
            bundles.Add(new ScriptBundle("~/Scripts/Post").Include(
                "~/Scripts/Highlight/highlight.pack.js",
                "~/Scripts/Views/Post.js"));

            // Post-Details
            bundles.Add(new ScriptBundle("~/Scripts/Post-Details").Include(
                "~/Scripts/Views/Post-Details.js"));

            // Account Edit view
            bundles.Add(new ScriptBundle("~/Scripts/Account-Edit").Include(
                "~/Scripts/Views/Account-Edit.js"));

            #endregion

            // BundleTable.EnableOptimizations = true;
        }
    }
}