using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Restaurantbookingpage
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            StyleBundle cssBundle = new StyleBundle("~/Content/CSS");
            cssBundle.Include("~/Content/bootstrap.min.css", "~/Content/Site.css", "~/Content/DataTables/css/jquery.dataTables.min.css",
                "~/Content/bootstrap-theme.min.css", "~/Content/toastr.min.css");
            ScriptBundle jsBundle=new ScriptBundle("~/Scripts/Script");
            jsBundle.Include("~/Scripts/jquery-3.5.1.min.js", "~/Scripts/bootstrap.min.js",
                "~/Scripts/DataTables/jquery.dataTables.min.js", "~/Scripts/jquery.validate.min.js", "~/Scripts/toastr.min.js","~/Scripts/buttons.min.js", "~/Scripts/jszip.min.js", "~/Scripts/html5.min.js");
            
            bundles.Add(cssBundle);
            bundles.Add(jsBundle);

            //BundleTable.EnableOptimizations = true;
        }
    }
}