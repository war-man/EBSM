using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Schema;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using PagedList;
using Microsoft.Owin.Security.Provider;
using WarehouseApp.Models;
using EBSM.Entities;
using EBSM.Services;

namespace WarehouseApp.Controllers
{
    //================================Shop reporting Controller=====================================================================
    public class ShopReportController : Controller
    {
        private SalesService _salesService = new SalesService();
        private CompanyProfileService _companyProfileService = new CompanyProfileService();
        private SupplierService _supplierService = new SupplierService();
        private PurchaseService _purchaseService = new PurchaseService();
        private ProductService _productService = new ProductService();
        private TransactionService _transactionService = new TransactionService();
        [Roles("Global_SupAdmin,Report_View")]
        public ActionResult SalesReport()
        {
            ViewBag.ControllerName = "ShopReport";
            ViewBag.FormHeading = "Sales Report";
            ViewBag.ProductIdNullError = "";
            return View("../Report/Shop/SalesReport");
        }

        [HttpGet]
        public ActionResult SalesReportByDate(string date)
        {

            //var inputDate = Convert.ToDateTime(date);
            ViewBag.currentDate = Convert.ToDateTime(date);

            //var invoices = db.Invoices.Where(x => (model.TransactionMode == null || x.TransactionMode.Equals(model.TransactionMode))
            //   && (model.PurchaseDateFrom == null || x.InvoiceDate >= fromDate) && (model.PurchaseDateTo == null || x.InvoiceDate <= toDate)
            //    && (model.SalesmanId == null || x.SalesmanId == model.SalesmanId)).Include(o => o.Salesman).OrderByDescending(o => o.InvoiceDate).ThenByDescending(o => o.CreatedDate);

            var sales =_salesService.GetAllSales().AsQueryable();
            if (!String.IsNullOrEmpty(date.ToString()))
            {
                var selectedDate = Convert.ToDateTime(date);
                sales = (IOrderedQueryable<Invoice>)sales.Where(a => a.InvoiceDate.Year == selectedDate.Year && a.InvoiceDate.Month == selectedDate.Month && a.InvoiceDate.Day == selectedDate.Day);
            }
            TempData["SalesReportByDate"] = sales.ToList();

            ViewBag.CompanyInfo = _companyProfileService.GetComapnyProfile();
            return View("../Report/Shop/SalesReportByDate", sales);
        }

        [HttpGet]
        public ActionResult SalesReportByDateRange(string fromDate, string toDate)
        {


            ViewBag.currentFrom = Convert.ToDateTime(fromDate);
            ViewBag.currentTo = Convert.ToDateTime(toDate);
            var sales = _salesService.GetAllSales().AsQueryable();
            if (!String.IsNullOrEmpty(fromDate.ToString()) && !String.IsNullOrEmpty(toDate.ToString()))
            {
                var dateFrom = Convert.ToDateTime(fromDate);
                var dateTo = Convert.ToDateTime(toDate);
                sales = (IOrderedQueryable<Invoice>)sales.ToList().Where(a => a.InvoiceDate.Date >= dateFrom.Date && a.InvoiceDate.Date <= dateTo.Date).AsQueryable();

            }
            TempData["SalesReportByDateRange"] = sales.ToList();

            ViewBag.CompanyInfo = _companyProfileService.GetComapnyProfile();
            return View("../Report/Shop/SalesReportByDateRange", sales);
        }

        [HttpGet]
        //public ActionResult SalesProductReportByDateRange(string fromDate2, string toDate2, string product)
        public ActionResult SalesProductReportByDateRange(string fromDate2, string toDate2, int? SelectedProductId)
        {

            if (SelectedProductId != null)
            //if (product != null)
            {
           
                var invoiceProducts =_salesService.GetAllInvoiceProducts(fromDate2, toDate2, SelectedProductId);

                //var invoiceProducts = db.InvoiceProducts.ToList().Where(x =>  (x.Product.ProductFullName.ToLower().StartsWith(product.ToLower()) || x.Product.ProductFullName.ToLower().Contains(" " + product.ToLower())) && (fromDate2 == null || x.Invoice.InvoiceDate >= Convert.ToDateTime(fromDate2)) && (toDate2 == null || x.Invoice.InvoiceDate <= Convert.ToDateTime(toDate2))).OrderByDescending(x => x.Invoice.InvoiceDate).ToList();
                TempData["SalesProductReportByDateRange"] = invoiceProducts;
                ViewBag.CompanyInfo = _companyProfileService.GetComapnyProfile();
                return View("../Report/Shop/SalesProductReportByDateRange", invoiceProducts);
            }

            ViewBag.ProductIdNullError = "Please Select Product.";
            return View("../Report/Shop/SalesReport");
        }
        [Roles("Global_SupAdmin,Report_View")]
        public ActionResult PurchaseReport()
        {
            ViewBag.ControllerName = "ShopReport";
            ViewBag.FormHeading = "Purchase Report";
            ViewBag.ProductIdNullError = "";
            ViewBag.SupplierId = new SelectList(_supplierService.GetAllDistributor(), "SupplierId", "SupplierName");
            return View("../Report/Shop/PurchaseReport");
        }

        [HttpGet]
        public ActionResult PurchaseReportByDate(string date, int? SupplierId)
        {

            //var inputDate = Convert.ToDateTime(date);
            ViewBag.currentDate = Convert.ToDateTime(date);
            //var invoices = db.Invoices.Where(x => (model.TransactionMode == null || x.TransactionMode.Equals(model.TransactionMode))
            //   && (model.PurchaseDateFrom == null || x.InvoiceDate >= fromDate) && (model.PurchaseDateTo == null || x.InvoiceDate <= toDate)
            //    && (model.SalesmanId == null || x.SalesmanId == model.SalesmanId)).Include(o => o.Salesman).OrderByDescending(o => o.InvoiceDate).ThenByDescending(o => o.CreatedDate);
            ViewBag.SupplierName = "";
            var purchases = _purchaseService.GetAllPurchases().AsQueryable();
            if (!String.IsNullOrEmpty(date.ToString()))
            {
                var selectedDate = Convert.ToDateTime(date);
                purchases = (IOrderedQueryable<Purchase>)purchases.Where(a => a.PurchaseDate.Year == selectedDate.Year && a.PurchaseDate.Month == selectedDate.Month && a.PurchaseDate.Day == selectedDate.Day);
            }
            if (!String.IsNullOrEmpty(SupplierId.ToString()))
            {

                purchases = (IOrderedQueryable<Purchase>)purchases.Where(x => x.SupplierId == SupplierId);
                ViewBag.SupplierName =_supplierService.GetSupplierById(SupplierId.Value).SupplierName;
            }
            TempData["PurchaseReportByDate"] = purchases.ToList();

            ViewBag.CompanyInfo =_companyProfileService.GetComapnyProfile();
            return View("../Report/Shop/PurchaseReportByDate", purchases);
        }

        [HttpGet]
        public ActionResult PurchaseReportByDateRange(string fromDate, string toDate, int? SupplierId)
        {


            ViewBag.currentFrom = Convert.ToDateTime(fromDate);
            ViewBag.currentTo = Convert.ToDateTime(toDate);
            ViewBag.SupplierName = "";
            var purchases = _purchaseService.GetAllPurchases().AsQueryable();
            if (!String.IsNullOrEmpty(fromDate.ToString()) && !String.IsNullOrEmpty(toDate.ToString()))
            {
                var dateFrom = Convert.ToDateTime(fromDate);
                var dateTo = Convert.ToDateTime(toDate);
                purchases = (IOrderedQueryable<Purchase>)purchases.ToList().Where(a => a.PurchaseDate.Date >= dateFrom.Date && a.PurchaseDate.Date <= dateTo.Date).AsQueryable();

            }
            if (!String.IsNullOrEmpty(SupplierId.ToString()))
            {

                purchases = (IOrderedQueryable<Purchase>)purchases.Where(x => x.SupplierId == SupplierId);
                ViewBag.SupplierName = _supplierService.GetSupplierById(SupplierId.Value).SupplierName;
            }
            var purlist = purchases.ToList();
            TempData["PurchaseReportByDateRange"] = purlist;

            ViewBag.CompanyInfo = _companyProfileService.GetComapnyProfile();
            return View("../Report/Shop/PurchaseReportByDateRange", purchases);
        }

        [HttpGet]
        //public ActionResult PurchaseProductReportByDateRange(string fromDate2, string toDate2, int? SelectedProductId)
        public ActionResult PurchaseProductReportByDateRange(string fromDate2, string toDate2, string product)
        {

            //if (SelectedProductId != null)
            if (product != null)
            {
                ViewBag.currentFrom = Convert.ToDateTime(fromDate2);
                ViewBag.currentTo = Convert.ToDateTime(toDate2);
                // var purchaseProducts = db.PurchaseProducts.ToList().Where(x => (x.ProductId == SelectedProductId) && (fromDate2 == null || x.Purchase.PurchaseDate >= Convert.ToDateTime(fromDate2)) && (toDate2 == null || x.Purchase.PurchaseDate <= Convert.ToDateTime(toDate2))).OrderByDescending(x => x.Purchase.PurchaseDate).ToList();
                var purchaseProducts =_purchaseService.GetAllPurchaseProducts( fromDate2,  toDate2,  product).ToList();
                TempData["PurchaseProductReportByDateRange"] = purchaseProducts;
                ViewBag.CompanyInfo = _companyProfileService.GetComapnyProfile();
                return View("../Report/Shop/PurchaseProductReportByDateRange", purchaseProducts);
            }

            ViewBag.ProductIdNullError = "Please Select Product.";
            ViewBag.SupplierId = new SelectList(_supplierService.GetAllDistributor(), "SupplierId", "SupplierName");
            return View("../Report/Shop/PurchaseReport");
        }
        public ActionResult ExpiryRange()
        {
            ViewBag.ControllerName = "ShopReport";
            ViewBag.FormHeading = "Expiry Date Within";
            return View("../Report/Shop/ExpiryRange");
        }
        [HttpGet]
        public ActionResult ExpiryWithinReportByDate(string date)
        {

            //var inputDate = Convert.ToDateTime(date);
            ViewBag.currentDate = Convert.ToDateTime(date);
            //var invoices = db.Invoices.Where(x => (model.TransactionMode == null || x.TransactionMode.Equals(model.TransactionMode))
            //   && (model.PurchaseDateFrom == null || x.InvoiceDate >= fromDate) && (model.PurchaseDateTo == null || x.InvoiceDate <= toDate)
            //    && (model.SalesmanId == null || x.SalesmanId == model.SalesmanId)).Include(o => o.Salesman).OrderByDescending(o => o.InvoiceDate).ThenByDescending(o => o.CreatedDate);

            var products = _productService.GetAllNotExpiredProducts();
            if (!String.IsNullOrEmpty(date.ToString()))
            {
                var selectedDate = Convert.ToDateTime(date);
                products = (IOrderedQueryable<Product>)products.Where(a => a.ExpiryDate <= selectedDate);
            }
            TempData["ExpiryWithinReportByDate"] = products.ToList();

            ViewBag.CompanyInfo = _companyProfileService.GetComapnyProfile();
            return View("../Report/Shop/ExpiryWithinReportByDate", products);
        }
        [Roles("Global_SupAdmin,Report_View")]
        public ActionResult DepositWithdrawReport()
        {
            ViewBag.ControllerName = "ShopReport";
            ViewBag.FormHeading = "Deposit - Withdraw  Report";
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text");
            return View("../Report/Shop/DepositWithdrawReport");
        }


        [HttpGet]
        public ActionResult DepositWithdrawReportByDateRange(string fromDate, string toDate, string TransactionType, string TransactionMode)
        {


            ViewBag.currentFrom = Convert.ToDateTime(fromDate);
            ViewBag.currentTo = Convert.ToDateTime(toDate);
            var transactions = _transactionService.GetAllTransactions( fromDate,  toDate,  TransactionType,  TransactionMode).ToList();

            TempData["DepositWithdrawReportByDateRange"] = transactions.ToList();

            ViewBag.TransacionType = TransactionType;
            ViewBag.CompanyInfo = _companyProfileService.GetComapnyProfile();
            return View("../Report/Shop/DepositWithdrawReportByDateRange", transactions);
        }
    }

    //================================Controller for exporting=====================================================================
    public class ExportController : Controller
    {
        // private WmsDbContext db = new WmsDbContext();
        private ProductService _productService = new ProductService();
        public void ExportDailySalesReport()
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Invoice No", typeof(string));
            dt.Columns.Add("Total Quantity", typeof(double));
            dt.Columns.Add("Sales Amount", typeof(double));
            dt.Columns.Add("VAT", typeof(double));
            dt.Columns.Add("Total", typeof(double));
            dt.Columns.Add("Salesman", typeof(string));
            List<Invoice> data = (List<Invoice>)TempData["SalesReportByDate"];
            foreach (var item in data)
            {
                double? tatalSales = item.TotalPrice - BankAccountController.DiscountCalculator(item.DiscountAmount, item.DiscountType, item.TotalPrice).DiscountValue;
                var salesman = item.Salesman != null ? item.Salesman.FullName : " ";
                var total = tatalSales + item.TotalVat;
                dt.Rows.Add(item.InvoiceDate.ToString("dd-MM-yyyy"), item.InvoiceNumber, item.TotalQuantity, tatalSales, item.TotalVat, total, salesman);
            }
            //Export export = new Export();
            //export.WriteExcelWithNPOI(dt, "ContactNPOI", "xlsx");
            WriteExcelWithNPOI(dt, "Daily_Sales_Report", "xlsx");
        }

        public void ExportDateRangeSalesReport()
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Total Quantity", typeof(string));
            dt.Columns.Add("Total Sales Amount", typeof(string));
            dt.Columns.Add("Total VAT", typeof(double));
            dt.Columns.Add("Total", typeof(double));

            List<Invoice> data = (List<Invoice>)TempData["SalesReportByDateRange"];
            foreach (var item in data.GroupBy(d => d.InvoiceDate.Date))
            {
                var sales = Convert.ToDouble(item.Sum(x => x.TotalPrice - BankAccountController.DiscountCalculator(x.DiscountAmount, x.DiscountType, x.TotalPrice).DiscountValue));
                dt.Rows.Add(item.First().InvoiceDate.ToString("dd-MM-yyyy"), item.Sum(x => x.TotalQuantity), sales, item.Sum(x => x.TotalVat), sales + item.Sum(x => x.TotalVat));
            }
            //Export export = new Export();
            //export.WriteExcelWithNPOI(dt, "ContactNPOI", "xlsx");
            WriteExcelWithNPOI(dt, "Sales_DateRange_Report", "xlsx");
        }
        public void ExportDailyPurchaseReport()
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Purchase No", typeof(string));
            dt.Columns.Add("Total Quantity", typeof(double));
            dt.Columns.Add("Total Amount", typeof(double));
            dt.Columns.Add("Paid Amount", typeof(double));
            dt.Columns.Add("Due Amount", typeof(string));
            List<Purchase> data = (List<Purchase>)TempData["PurchaseReportByDate"];
            foreach (var item in data)
            {
                var tatalDue = item.TotalPrice - item.PaidAmount;
                dt.Rows.Add(item.PurchaseDate.ToString("dd-MM-yyyy"), item.PurchaseNumber, item.TotalQuantity, item.TotalPrice, item.PaidAmount, tatalDue);
            }
            //Export export = new Export();
            //export.WriteExcelWithNPOI(dt, "ContactNPOI", "xlsx");
            WriteExcelWithNPOI(dt, "Daily_Purchase_Report", "xlsx");
        }

        public void ExportDateRangePurchaseReport()
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Total Quantity", typeof(double));
            dt.Columns.Add("Total Amount", typeof(double));
            dt.Columns.Add("Paid Amount", typeof(double));
            dt.Columns.Add("Due Amount", typeof(string));

            List<Purchase> data = (List<Purchase>)TempData["PurchaseReportByDateRange"];
            foreach (var item in data.GroupBy(d => d.PurchaseDate.Date))
            {
                var due = item.Sum(x => x.TotalPrice) - item.Sum(x => x.PaidAmount);
                dt.Rows.Add(item.First().PurchaseDate.ToString("dd-MM-yyyy"), item.Sum(x => x.TotalQuantity), item.Sum(x => x.TotalPrice), item.Sum(x => x.PaidAmount), due);
            }
            //Export export = new Export();
            //export.WriteExcelWithNPOI(dt, "ContactNPOI", "xlsx");
            WriteExcelWithNPOI(dt, "Purchase_DateRange_Report", "xlsx");
        }

        public void ExportExpiryReport()
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("Expiry Date", typeof(string));
            dt.Columns.Add("Product Name", typeof(string));
            dt.Columns.Add("Product Code", typeof(string));
            dt.Columns.Add("Purchase Price", typeof(double));
            dt.Columns.Add("Retail Price", typeof(double));
            dt.Columns.Add("Stock Price", typeof(double));


            List<Product> expiryProducts = (List<Product>)TempData["ExpiryWithinReportByDate"];
            foreach (var item in expiryProducts)
            {
                dt.Rows.Add(String.Format("{0:dd-MM-yyyy}", item.ExpiryDate), item.ProductFullName, item.ProductCode, item.Tp, item.Dp, item.Stocks.Sum(x => x.TotalQuantity));
            }
            //Export export = new Export();
            //export.WriteExcelWithNPOI(dt, "ContactNPOI", "xlsx");
            WriteExcelWithNPOI(dt, "Expiry_Report", "xlsx");
        }


        public void ExportProductInExcel()
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("Product Code", typeof(string));
            dt.Columns.Add("Product Name", typeof(string));
            dt.Columns.Add("BP Value", typeof(string));
            dt.Columns.Add("Group Name", typeof(string));
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("Manufacturer", typeof(string));
            dt.Columns.Add("Purchase Price (Per Piece)", typeof(double));
            dt.Columns.Add("Retail Price (Per Piece)", typeof(double));
            dt.Columns.Add("Minimum Stock Warning", typeof(double));

            DateTime createdDate = Convert.ToDateTime("9/04/2017");
            List<Product> products = _productService.GetActiveProducts().ToList();
            foreach (var item in products)
            {
                var prAtt = _productService.GetAllAttributeByProductId(item.ProductId).ToList();
                var prCat = _productService.GetAllCategoriesByProductId(item.ProductId).ToList();
                dt.Rows.Add(String.IsNullOrEmpty(item.ProductCode) ? "" : item.ProductCode, String.IsNullOrEmpty(item.ProductName) ? "" : item.ProductName, prAtt.Count > 0 ? prAtt.First().Value : "", item.Group != null ? item.Group.GroupName : "", prCat.Count > 0 ? prCat.First().Category.CategoryName : "", item.Supplier != null ? item.Supplier.SupplierName : "", item.Tp != null ? item.Tp : 0, item.Dp != null ? item.Dp : 0, item.MinStockLimit != null ? item.MinStockLimit : 0);
            }
            //Export export = new Export();
            //export.WriteExcelWithNPOI(dt, "ContactNPOI", "xlsx");
            WriteExcelWithNPOI(dt, "Products", "xlsx");
        }
        //Write to excel====================================================================================
        public void WriteExcelWithNPOI(DataTable dt, String fileName, String extension)
        {

            IWorkbook workbook;

            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt.Columns.Count; j++)
            {

                ICell cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }
            string fileNameWithDate = fileName + " (" + DateTime.Now.ToString("dd-MM-yyyy") + ")";
            using (var exportData = new MemoryStream())
            {
                Response.Clear();
                workbook.Write(exportData);
                if (extension == "xlsx") //xlsx file format
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileNameWithDate + ".xlsx"));
                    Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")  //xls file format
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileNameWithDate + ".xls"));
                    Response.BinaryWrite(exportData.GetBuffer());
                }
                Response.End();
            }
        }
    }
}

////=======================================================================================//
////Author : Md. Mahid Choudhury
////Creation Date : January 2017
////=======================================================================================//