using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WarehouseApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

    public static class ProjectGlobalProperties
    {

        public const string ProjectName = "EBSM";
        //public const string CopyRightName = "Welburg International";
        public const string CopyRightName = "Encoders Infotech Ltd.";
        public const string DeveloperName = "Encoders Infotech Ltd.";
        public const string DeveloperWebsite = "http://www.encodersbd.com/";
        public static string LogoPath = "~/Content/Images/ebsm-icon.png";
        //public static string FaviconPath = "~/Content/Images/project-favicon.png";
        public static int DeploymentYear = 2017;
        public static string AppVersion = "1.1";
        public static string AppLastUpdate = "12-11-2017";
    }


}
