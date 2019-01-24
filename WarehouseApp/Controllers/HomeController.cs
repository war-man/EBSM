using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;
using EBSM.Entities;
using EBSM.Services;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private NoticeService _noticeService = new NoticeService();
        private ProductService _productService = new ProductService();
        private SalesService _salesService = new SalesService();
        private StockService _stockService = new StockService();
        private CustomerService _customerService = new CustomerService();
        private PurchaseService _purchaseService = new PurchaseService();
        private SalesOrderService _salesOrderService = new SalesOrderService();
        private ProductAttributeService _productAttributeService = new ProductAttributeService();


        
        public ActionResult Index()
        {
            //List<Notice> allNotices = db.Notices.Where(x => x.Status != 0).OrderByDescending(t => t.CreatedDate).ToList();
            ViewBag.NewNoticesCount = _noticeService.GetCount()> 0 ?_noticeService.LastDateNoticeCount() : 0;
            ViewBag.NewNoticesTimeAgo = _noticeService.GetCount() > 0 ? SettingsController.TimeAgo(_noticeService.LastDateOfNoticePublished()) : "No time";


            //List<Invoice> todaySales = db.Invoices.Include(t => t.Customer).ToList().Where(t => t.InvoiceDate.Date == DateTime.Now.Date).OrderByDescending(t => t.InvoiceDate).ToList();
            ViewBag.todaySoldItems = _salesService.GetTodaysSalesAmount();
            var top = _salesService.GetTopSalesProduct().GroupBy(x => x.ProductId).Select(x => new TopSellsMonthly { Product = x.FirstOrDefault().Product, TotalSoldQuantity = x.Sum(y => y.Quantity), TotalSoldAmount = x.Sum(y => y.TotalPrice) }).ToList().OrderByDescending(x => x.TotalSoldQuantity).Take(10);
            ViewBag.topSellingProducts = top;

            //Purchase related--------------------------------------
            List<Purchase> allPurchase = _purchaseService.GetAllPurchases().ToList();
            ViewBag.latestPurchasedItems = allPurchase.Count > 0 ? allPurchase.GroupBy(t => t.PurchaseDate.Date).First().ToList().Sum(x => x.TotalQuantity) : 0;
            ViewBag.latestPurchaseDate = allPurchase.Count > 0 ? allPurchase.GroupBy(t => t.PurchaseDate.Date).First().ToList().First().PurchaseDate : DateTime.Now;

            //Products Related------------------------------------------
            //List<Product>allProducts= db.Products.Where(t => t.Status != 0).OrderByDescending(t => t.ProductFullName).ToList();
            ViewBag.totalProducts =_productService.GetCount();
            ViewBag.totalCustomers =_customerService.GetCount();

            //ViewBag.topSellingProducts = allProducts.ToList().OrderByDescending(x => x.InvoiceProducts.Sum(y => y.TotalQuantity)).Take(10);

            //Stock related---------------------------------
            //List<Stock> stocks = db.Stocks.ToList();
            ViewBag.Inventory = _stockService.GetTotalRemainingStock();
            ViewBag.quickStocksProducts = _stockService.LimitedStockProducts().Take(10);
            ViewBag.AttributeSetId = _productAttributeService.GetAllAttributeSets().ToList();
            ViewBag.PendingOrders = _salesOrderService.GetPendingOrdersCount();
            if (User.IsInRole("Global_Manager"))
            {
                return View("../Home/DashboardManager");
            }
            if (User.IsInRole("Aquarium_Admin") || User.IsInRole("Gamming_Zone_Admin") || User.IsInRole("3D_Zone_Admin") || User.IsInRole("Mirror_Tunnel_Admin"))
            {
                return View("../Home/DashboardAdmin");
            }
            if (User.IsInRole("Aquarium_Staff") || User.IsInRole("Gamming_Zone_Staff") || User.IsInRole("3D_Zone_Staff") || User.IsInRole("Mirror_Tunnel_Staff"))
            {
                return View("../Home/DashboardStaff");
            }
           
            if (User.IsInRole("Shop_Admin"))
            {
                //Sales Related---------------------------------

                
                return View("../Home/DashboardShopAdmin");
            }
            if (User.IsInRole("Shop_Staff"))
            {
                return View("../Home/DashboardShopStaff");
            }
            if (User.IsInRole("Restaurant_Admin"))
            {
                return View("../Home/DashboardAdmin");
            }
            if (User.IsInRole("Restaurant_Staff"))
            {
                return View("../Home/DashboardRestaurantStaff");
            }
            //return View("../Home/DashboardSuperadmin");
            return View("../Home/Index");
        }

         [Roles("Global_SupAdmin", "Global_Manager")]
        public ActionResult ShopDashBoard()
        {
          
            ViewBag.NewNoticesCount = _noticeService.GetCount() > 0 ? _noticeService.LastDateNoticeCount() : 0;
            ViewBag.NewNoticesTimeAgo = _noticeService.GetCount() > 0 ? SettingsController.TimeAgo(_noticeService.LastDateOfNoticePublished()) : "No time";

            //Sales Related---------------------------------

            ViewBag.todaySoldItems = _salesService.GetTodaysSalesAmount();
            var top = _salesService.GetTopSalesProduct().GroupBy(x => x.ProductId).Select(x => new TopSellsMonthly { Product = x.FirstOrDefault().Product, TotalSoldQuantity = x.Sum(y => y.Quantity), TotalSoldAmount = x.Sum(y => y.TotalPrice) }).ToList().OrderByDescending(x => x.TotalSoldQuantity).Take(10);
            ViewBag.topSellingProducts = top;
            //Purchase related--------------------------------------
            List<Purchase> allPurchase = _purchaseService.GetAllPurchases().ToList();
            ViewBag.latestPurchasedItems = allPurchase.Count > 0 ? allPurchase.GroupBy(t => t.PurchaseDate.Date).First().ToList().Sum(x => x.TotalQuantity) : 0;
            ViewBag.latestPurchaseDate = allPurchase.Count > 0 ? allPurchase.GroupBy(t => t.PurchaseDate.Date).First().ToList().First().PurchaseDate : DateTime.Now;

            //Products Related------------------------------------------
            //List<Product>allProducts= db.Products.Where(t => t.Status != 0).OrderByDescending(t => t.ProductFullName).ToList();
            ViewBag.totalProducts = _productService.GetCount();
            ViewBag.totalCustomers = _customerService.GetCount();
            //ViewBag.topSellingProducts = allProducts.ToList().OrderByDescending(x => x.InvoiceProducts.Sum(y => y.TotalQuantity)).Take(10);

            //Stock related---------------------------------
            ViewBag.Inventory = _stockService.GetTotalRemainingStock();
            ViewBag.quickStocksProducts = _stockService.LimitedStockProducts().Take(10);
            ViewBag.AttributeSetId = _productAttributeService.GetAllAttributeSets().ToList();
            ViewBag.PendingOrders = _salesOrderService.GetPendingOrdersCount();
            return View("../Home/DashboardShopAdmin");

        } 
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Page for all notices";

            return View();
        }
 public ActionResult Form()
        {


            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult FindSalesDaily(int? deptId)
        {
            var currentDate = DateTime.Now;
            var arrayList = new List<dynamic>();
            char xkey = 'd';
            List<string> ykeys = new List<string>();
            List<string> labels = new List<string>();

            var sales =_salesService.GetAllSalesByCurrentMonth().GroupBy(g => g.InvoiceDate.Day).Select(g => new { year = g.FirstOrDefault().InvoiceDate.Year, month = g.FirstOrDefault().InvoiceDate.Month, day = g.FirstOrDefault().InvoiceDate.Day, saleAmount = g.Sum(x => ((x.TotalPrice - BankAccountController.DiscountCalculator(x.DiscountAmount, x.DiscountType, x.TotalPrice).DiscountValue) + x.TotalVat)) });
            for (int i = 1; i <= DateTime.Now.Day; i++)
            {
                double currentSale = 0.0;
                var currentSaleByDate = sales.FirstOrDefault(gm => gm.year == currentDate.Year && gm.month == currentDate.Month && gm.day.ToString() == i.ToString());
                if (currentSaleByDate != null)
                {
                    currentSale = Math.Round(Convert.ToDouble(currentSaleByDate.saleAmount), 2);
                }
                dynamic dailyData = new { d = currentDate.Year + "-" + currentDate.Month + "-" + i, sales = currentSale };

                arrayList.Add(dailyData);
            }

            ykeys.Add("sales");
            labels.Add("Total Sales");
            var data = new { xkey = xkey, ykeys = ykeys.ToArray(), labels = labels.ToArray(), arrayList = arrayList };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindMonthlyAnalytics(int? deptId)
        {
            var currentDate = DateTime.Now;
            var arrayList = new List<dynamic>();
            char xkey = 'm';
            List<string> ykeys = new List<string>();
            List<string> labels = new List<string>();

            var sales =_salesService.GetAllSalesByCurrentYear().GroupBy(g => g.InvoiceDate.Month).Select(g => new { year = g.FirstOrDefault().InvoiceDate.Year, month = g.FirstOrDefault().InvoiceDate.Month, saleAmount = g.Sum(x => ((x.TotalPrice - BankAccountController.DiscountCalculator(x.DiscountAmount, x.DiscountType, x.TotalPrice).DiscountValue) + x.TotalVat)) });
            var purchase = _purchaseService.GetAllPurchasesByCurrentYear().GroupBy(g => g.PurchaseDate.Month).Select(g => new { year = g.FirstOrDefault().PurchaseDate.Year, month = g.FirstOrDefault().PurchaseDate.Month, purchaseAmount = g.Sum(x => x.TotalPrice), purchaseCost = g.Sum(x => x.PurchaseCosts.Sum(a => a.Amount)) });
            for (int i = 1; i <= DateTime.Now.Month; i++)
            {
                double currentSale = 0.0;
                double currentPurchase = 0.0;
                var currentSaleByMonth = sales.FirstOrDefault(gm => gm.year == currentDate.Year && gm.month.ToString() == i.ToString());
                if (currentSaleByMonth != null)
                {
                    currentSale = Math.Round(Convert.ToDouble(currentSaleByMonth.saleAmount), 2);
                }
                var currentPurchaseByMonth = purchase.FirstOrDefault(gm => gm.year == currentDate.Year && gm.month.ToString() == i.ToString());
                if (currentPurchaseByMonth != null)
                {
                    currentPurchase = Math.Round(Convert.ToDouble(currentPurchaseByMonth.purchaseAmount + currentPurchaseByMonth.purchaseCost), 2);
                }
                dynamic monthlyData = new { m = currentDate.Year + "-" + i, sales = currentSale, purchase = currentPurchase };

                arrayList.Add(monthlyData);
            }

            ykeys.Add("sales");
            labels.Add("Total Sales");
            ykeys.Add("purchase");
            labels.Add("Total Purchase");

            var data = new { xkey = xkey, ykeys = ykeys.ToArray(), labels = labels.ToArray(), arrayList = arrayList };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FindYearlyAnalytics(int? deptId)
        {

            var arrayList = new List<dynamic>();
            char xkey = 'y';
            List<string> ykeys = new List<string>();
            List<string> labels = new List<string>();
            var sales = _salesService.GetAllSales().ToList().GroupBy(g => g.InvoiceDate.Year).Select(g => new { year = g.FirstOrDefault().InvoiceDate.Year, saleAmount = g.Sum(x => ((x.TotalPrice - BankAccountController.DiscountCalculator(x.DiscountAmount, x.DiscountType, x.TotalPrice).DiscountValue) + x.TotalVat)) });
            var purchase = _purchaseService.GetAllPurchases().ToList().GroupBy(g => g.PurchaseDate.Year).Select(g => new { year = g.FirstOrDefault().PurchaseDate.Year, purchaseAmount = g.Sum(x => x.TotalPrice), purchaseCost = g.Sum(x => x.PurchaseCosts.Sum(a => a.Amount)) });
            var years = ProjectGlobalProperties.DeploymentYear;

            for (int i = years; i <= DateTime.Now.Year; i++)
            {
                double currentSale = 0.0;
                double currentPurchase = 0.0;
                var currentSaleByYear = sales.FirstOrDefault(gm => gm.year == i);
                if (currentSaleByYear != null)
                {
                    currentSale = Math.Round(Convert.ToDouble(currentSaleByYear.saleAmount), 2);
                }
                var currentPurchaseByYear = purchase.FirstOrDefault(gm => gm.year.ToString() == i.ToString());
                if (currentPurchaseByYear != null)
                {
                    currentPurchase = Math.Round(Convert.ToDouble(currentPurchaseByYear.purchaseAmount + currentPurchaseByYear.purchaseCost), 2);
                }

                dynamic yearlyData = new { y = i, sales = currentSale, purchase = currentPurchase };
                arrayList.Add(yearlyData);
            }


            ykeys.Add("sales");
            labels.Add("Total Sales");
            ykeys.Add("purchase");
            labels.Add("Total Purchase");
            var data = new { xkey = xkey, ykeys = ykeys.ToArray(), labels = labels.ToArray(), arrayList = arrayList };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
      
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stockService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
     [Authorize]
    public class DashboardController : Controller
    {

         [Roles("Global_SupAdmin")]
        public ActionResult Superadmin()
        {
            return View("../Home/DashboardSuperadmin");
        }
         [Roles("Global_Manager")]
         public ActionResult Manager()
         {
             return View("../Home/DashboardManager");
         }

          [Roles("Aqurium_Admin")]
          public ActionResult AquariumAdmin()
        {
            return View("../Home/DashboardShopAdmin");
        }
         [Roles("Aqurium_Staff")]
         public ActionResult AquariumStaff()
        {
            return View("../Home/DashboardAquariumStaff");
        }
         [Roles("Gamming_Zone_Admin")]
         public ActionResult GameAdmin()
         {
             return View("../Home/DashboardGameAdmin");
         }
         [Roles("Gamming_Zone_Staff")]
         public ActionResult GameStaff()
         {
             return View("../Home/DashboardGameStaff");
         }
         [Roles("3D_Zone_Admin")]
         public ActionResult Zone3DAdmin()
         {
             return View("../Home/Dashboard3DZoneAdmin");
         }
         [Roles("3D_Zone_Staff")]
         public ActionResult Zone3DStaff()
         {
             return View("../Home/Dashboard3DZoneStaff");
         }
         [Roles("Mirror_Tunnel_Admin")]
         public ActionResult MirrorAdmin()
         {
             return View("../Home/DashboardMirrorAdmin");
         }
         [Roles("Mirror_Tunnel_Staff")]
         public ActionResult MirrorStaff()
         {
             return View("../Home/DashboardMirrorStaff");
         }
         [Roles("Shop_Admin")]
        public ActionResult ShopAdmin()
        {
            return View("../Home/DashboardShopAdmin");
        }
          [Roles("Shop_Staff")]
         public ActionResult ShopStaff()
         {
             return View("../Home/DashboardShopStaff");
         }
          [Roles("Restaurant_Admin")]
          public ActionResult RestaurantAdmin()
          {
              return View("../Home/DashboardRestaurantAdmin");
          }
          [Roles("Restaurant_Staff")]
          public ActionResult RestaurantStaff()
          {
              return View("../Home/DashboardRestaurantStaff");
          }
    }


}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : December 2016
//=======================================================================================//