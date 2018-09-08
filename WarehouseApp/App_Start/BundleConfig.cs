using System.Web;
using System.Web.Optimization;

namespace WarehouseApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //============================== Jquery =========================================================
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //"~/Scripts/jquery-{version}.js"));
                        "~/Scripts/jquery.min.js"));

            //==============================Jquery validation =========================================================

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //==============================JqueryUi =========================================================
            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
        "~/Content/Plugins/jquery-ui/minified/jquery-ui.min.css",
        "~/Content/Plugins/jquery-ui/minified/jquery.ui.core.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui-1.10.4.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //==============================bootstrap=========================================================
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.min.css",
                //"~/Content/bootstrap-datepicker.css",
               "~/Content/font-awesome.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

       

            //=======================jquery confirm================================
            bundles.Add(new StyleBundle("~/Content/JqueryConfirm").Include(
        "~/Content/Plugins/JqueryConfirm/jquery-confirm.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/JqueryConfirm").Include(
         "~/Content/Plugins/JqueryConfirm/jquery-confirm.min.js"));

           

            //=======================nprogress================================
            bundles.Add(new StyleBundle("~/Content/nprogress").Include(
        "~/Content/Plugins/nprogress/nprogress.css"));

            bundles.Add(new ScriptBundle("~/bundles/nprogress").Include(
         "~/Content/Plugins/nprogress/nprogress.js"));

            //=======================fastclick================================
            bundles.Add(new ScriptBundle("~/bundles/fastclick").Include(
         "~/Content/Plugins/fastclick/lib/fastclick.js"));

            //=======================bootstrap-progressbar================================
            bundles.Add(new StyleBundle("~/Content/bootstrapProgressbar").Include(
     "~/Content/Plugins/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrapProgressbar").Include(
         "~/Content/Plugins/bootstrap-progressbar/bootstrap-progressbar.min.js"));

            //=======================bootstrap-daterangepicker================================
            bundles.Add(new StyleBundle("~/Content/bootstrapDaterangepicker").Include(
     "~/Content/Plugins/bootstrap-daterangepicker/daterangepicker.css"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrapDaterangepicker").Include(
         "~/Content/Plugins/DateJS/production/date.min.js",
         "~/Content/Plugins/moment/min/moment.min.js",
         "~/Content/Plugins/bootstrap-daterangepicker/daterangepicker.js"));

            //=======================bootstrap-datetimepicker================================
            bundles.Add(new StyleBundle("~/Content/bootstrap-datetimepicker").Include(
     "~/Content/Plugins/bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datetimepicker").Include(
         "~/Content/Plugins/bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js"));

            //=======================iCheck================================
            bundles.Add(new StyleBundle("~/Content/iCheck").Include(
     "~/Content/Plugins/iCheck/skins/flat/green.css"));
            bundles.Add(new ScriptBundle("~/bundles/iCheck").Include(
         "~/Content/Plugins/iCheck/icheck.min.js"));

            //=======================Select2================================
            bundles.Add(new StyleBundle("~/Content/Select2").Include(
     "~/Content/Plugins/select2/dist/css/select2.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/Select2").Include(
         "~/Content/Plugins/select2/dist/js/select2.full.min.js"));

            //=======================bootstrap chosen================================
            bundles.Add(new StyleBundle("~/Content/chosen").Include(
        "~/Content/Plugins/chosen/chosen.css"));


            bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
         "~/Content/Plugins/chosen/chosen.jquery.js"));

            //=======================Switchery================================
            bundles.Add(new StyleBundle("~/Content/Switchery").Include(
     "~/Content/Plugins/switchery/dist/switchery.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/Switchery").Include(
         "~/Content/Plugins/switchery/dist/switchery.min.js"));

            //=======================jQuery Tags Input================================
      
            bundles.Add(new ScriptBundle("~/bundles/jquery-tagsinput").Include(
         "~/Content/Plugins/jquery.tagsinput/src/jquery.tagsinput.js")); 

            //=======================chartJs================================
            bundles.Add(new ScriptBundle("~/bundles/chartJs").Include(
         "~/Content/Plugins/Chart.js/dist/Chart.min.js")); 

            //=======================gaugeJs================================
            bundles.Add(new ScriptBundle("~/bundles/gaugeJs").Include(
         "~/Content/Plugins/gauge.js/dist/gauge.min.js"));

            //=======================Skycons================================
            bundles.Add(new ScriptBundle("~/bundles/Skycons").Include(
         "~/Content/Plugins/skycons/skycons.js"));
            //=======================validator================================
            bundles.Add(new ScriptBundle("~/bundles/validator").Include(
         "~/Content/Plugins/validator/validator.js"));

            //=========================Moris Chart====================================

            bundles.Add(new StyleBundle("~/Content/morris").Include(
                    "~/Content/Plugins/morrisjs/morris.css"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/morris").Include(
            "~/Content/Plugins/morrisjs/raphael.min.js",
            "~/Content/Plugins/morrisjs/morris.min.js",
            "~/Content/Plugins/morrisjs/morris-data.js"
            ));
            //=======================Barcode Js================================

            bundles.Add(new ScriptBundle("~/bundles/barcode-scanner-detection").Include(
        "~/Scripts/ProjectJs/jquery.scannerdetection.js"));
            bundles.Add(new ScriptBundle("~/bundles/JsBarcode").Include(
        "~/Scripts/ProjectJs/JsBarcode.all.min.js"));

            //=======================Custom================================
            bundles.Add(new StyleBundle("~/Content/css").Include(
						"~/Content/PagedList.css",
                      "~/Content/custom.min.css",
                      "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/Custom").Include(
        "~/Scripts/custom.js"));

            //=======================Project Js================================
		  bundles.Add(new ScriptBundle("~/bundles/ProjectJs").Include(
                "~/Scripts/ProjectJs/ProjectJs.js"));  
            bundles.Add(new ScriptBundle("~/bundles/ShopScript").Include(
                "~/Scripts/ProjectJs/Shop/ShopScript.js"));
            bundles.Add(new ScriptBundle("~/bundles/PurchaseScript").Include(
                "~/Scripts/ProjectJs/Shop/PurchaseScript.js"));
            bundles.Add(new ScriptBundle("~/bundles/SalesScript").Include(
                "~/Scripts/ProjectJs/Shop/SalesScript.js"));

        }
    }
}
