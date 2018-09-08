using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WarehouseApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              "Home",                                        // Route name
              "Home",                                        // URL with parameters
              new { controller = "Home", action = "Index" }  // Parameter defaults
          );
            routes.MapRoute(
                "Products",                                        // Route name
                "Products",                                        // URL with parameters
                new { controller = "Product", action = "Index" }  // Parameter defaults
            );

            routes.MapRoute(
                "Batches",                                        // Route name
                "Batches",                                        // URL with parameters
                new { controller = "Batch", action = "Index" }  // Parameter defaults
            );


            routes.MapRoute(
                "Categories",                                        // Route name
                "Categories",                                        // URL with parameters
                new { controller = "Category", action = "Index" }  // Parameter defaults
            );


            routes.MapRoute(
                "Groups",                                        // Route name
                "Groups",                                        // URL with parameters
                new { controller = "Group", action = "Index" }  // Parameter defaults
            );


            routes.MapRoute(
                "AttributeSets",                                        // Route name
                "AttributeSets",                                        // URL with parameters
                new { controller = "ProductAttribute", action = "AttributeSetList" }  // Parameter defaults
            );


            routes.MapRoute(
                "Attributes",                                        // Route name
                "Attributes",                                        // URL with parameters
                new { controller = "ProductAttribute", action = "ListArrtibuteType" }  // Parameter defaults
            );

            routes.MapRoute(
               "SalesOrders",                                        // Route name
               "SalesOrders",                                        // URL with parameters
               new { controller = "SalesOrder", action = "index" }  // Parameter defaults
           ); 
            routes.MapRoute(
               "NewSalesOrder",                                        // Route name
               "NewSalesOrder",                                        // URL with parameters
               new { controller = "SalesOrder", action = "NewSalesOrder" }  // Parameter defaults
           );
            
            routes.MapRoute(
                "AllSales",                                        // Route name
                "AllSales",                                        // URL with parameters
                new { controller = "Invoice", action = "Index" }  // Parameter defaults
            );
            
            routes.MapRoute(
                "NewSales",                                        // Route name
                "NewSales",                                        // URL with parameters
                new { controller = "Invoice", action = "NewSales" }  // Parameter defaults
            );
            routes.MapRoute(
                    "AllPurchases",                                        // Route name
                    "AllPurchases",                                        // URL with parameters
                    new { controller = "Purchase", action = "Index" }  // Parameter defaults
                );

            routes.MapRoute(
                "NewPurchase",                                        // Route name
                "NewPurchase",                                        // URL with parameters
                new { controller = "Purchase", action = "NewPurchase" }  // Parameter defaults
            );


            routes.MapRoute(
                "Stock",                                        // Route name
                "Stock",                                        // URL with parameters
                new { controller = "Stock", action = "Index" }  // Parameter defaults
            );


            routes.MapRoute(
                "Damages",                                        // Route name
                "Damages",                                        // URL with parameters
                new { controller = "Damage", action = "Index" }  // Parameter defaults
            );


            routes.MapRoute(
                "Customers",                                        // Route name
                "Customers",                                        // URL with parameters
                new { controller = "Customer", action = "Index" }  // Parameter defaults
            );
            routes.MapRoute(
                "ExpenseTypeConfigs",                                        // Route name
                "ExpenseTypeConfigs",                                        // URL with parameters
                new { controller = "Expense", action = "ExpenseTypeList" }  // Parameter defaults
            );


            routes.MapRoute(
                "UserList",                                        // Route name
                "UserList",                                        // URL with parameters
                new { controller = "User", action = "Index" }  // Parameter defaults
            );


            routes.MapRoute(
                "Suppliers",                                        // Route name
                "Suppliers",                                        // URL with parameters
                new { controller = "Supplier", action = "Index" }  // Parameter defaults
            );

            routes.MapRoute(
                "Salesmans",                                        // Route name
                "Salesmans",                                        // URL with parameters
                new { controller = "Salesman", action = "Index" }  // Parameter defaults
            );


            routes.MapRoute(
                "Notices",                                        // Route name
                "Notices",                                        // URL with parameters
                new { controller = "Notice", action = "Index" }  // Parameter defaults
            );

            routes.MapRoute(
               "AllSalesReturn",                                        // Route name
               "AllSalesReturn",                                        // URL with parameters
               new { controller = "Invoice", action = "LoadSalesReturn" }  // Parameter defaults
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
