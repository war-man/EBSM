using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;

using System.Web.Security;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;
using EBSM.Entities;
using EBSM.Services;
namespace WarehouseApp.Controllers
{
    [Authorize]
    public class SalesOrderController : Controller
    {
        private SalesOrderService _salesOrderService = new SalesOrderService();
        private CustomerService _customerService = new CustomerService();
        private SalesmanService _salesmanService = new SalesmanService();
        private ProductService _productService = new ProductService();
        private TransactionAccountService _transactionAccountService = new TransactionAccountService();
        private const int InitialInvoiceNo = 1;
        #region list view
        [Roles("Global_SupAdmin,SalesOrder_Create,SalesOrder_Edit")]
        [OutputCache(Duration = 20)]
        public ActionResult Index(SalesOrderSearchViewModel model)
        {
            var fromDate = Convert.ToDateTime(model.OrderDateFrom);
            var toDate = Convert.ToDateTime(model.OrderDateTo);
            var salesOrders = _salesOrderService.GetAllSalesOrders(model.OrderNo, model.OrderDateFrom, model.OrderDateTo, model.CustomerId, model.Status);
            model.SalesOrders = salesOrders.ToPagedList(model.Page, model.PageSize);
            ViewBag.CustomerId = new SelectList(_customerService.GetAllCustomers(), "CustomerId", "FullName");
            return View("../Shop/SalesOrder/Index", model);
        }
        #endregion
        #region new sales order
        [Roles("Global_SupAdmin,SalesOrder_Create,SalesOrder_Edit")]
        [OutputCache(Duration = 120)]
        public ActionResult NewSalesOrder()
        {
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");
            ViewBag.SalesmanId = new SelectList(_customerService.GetAllCustomers(), "SalesmanId", "FullName");
            ViewBag.CustomerId = new SelectList(_salesmanService.GetAllSalesman(), "CustomerId", "FullName");
            return View("../Shop/SalesOrder/NewSalesOrder");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSalesOrder(InvoiceViewModel data)
        {
            if (ModelState.IsValid)
            {
                string fmt = "0000000.##";
                int lastInvoiceId = _salesOrderService.GetCount();
                
                #region Invoice save
                SalesOrder invoice = new SalesOrder()
                {
                    OrderDate = Convert.ToDateTime(data.InvoiceDate),
                    OrderNumber = "O" + TransactionController.BillingMonthString(data.InvoiceDate) + (lastInvoiceId + 1).ToString(fmt),
                    TotalQuantity = data.InvoiceProducts.Sum(x => x.Quantity),
                    TotalPrice = data.TotalPrice,
                    TotalVat = data.TotalVat,
                    DiscountType = data.DiscountType,
                    DiscountAmount = data.DiscountAmount,
                    TransactionMode = data.TransactionMode,
                    TransactionModeId = data.TransactionModeId,
                    CustomerId = data.CustomerId,
                    CustomerBranchId = data.CustomerBranchId,
                    //SalesmanId = data.SalesmanId,
                    AdvancePaid = data.PaidAmount ?? 0,
                    CashPaid = data.CashPaid ?? 0,
                    Note = data.Note,
                    Status = 1,
                    CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId,
                    CreatedDate = DateTime.Now
                };
                _salesOrderService.SaveSalesOrder(invoice, AuthenticatedUser.GetUserFromIdentity().UserId);
             
                #endregion
                #region order products
                var sl = 1;
                foreach (var item in data.InvoiceProducts)
                {
                    OrderProduct invoiceProduct = new OrderProduct()
                    {
                        OrderId = invoice.SalesOrderId,
                        ProductId = Convert.ToInt32(item.ProductId),
                        Barcode = item.Barcode,
                        Dp = Convert.ToDouble(item.UnitPrice),
                        //DiscountType = item.DiscountType,
                        //DiscountAmount = item.DiscountAmount,
                        TotalPrice = item.TotalPrice,
                        Quantity = item.Quantity,
                        Status = 1
                    };
                    _salesOrderService.SaveOrderProduct(invoiceProduct);
                    
                }
                #endregion
                #region transaction chanel
                //========add to account transaction as format DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue,currentUserId);============================================================================================================================
                TransactionController transaction = new TransactionController();
                double transactionAmount = data.PaidAmount ?? 0;
                if (transactionAmount > 0)
                {
                    transaction.DepositToAccount(data.TransactionMode, Convert.ToInt32(data.TransactionModeId), transactionAmount, Convert.ToDateTime(data.InvoiceDate), "SalesOrder", "SalesOrderId", invoice.SalesOrderId, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), "Advance Payment");
                }
                #endregion
                #region Order Export


                if (data.FormSubmitType == 3)
                {
                    ExportOrderChalanExcel(invoice.SalesOrderId);

                }

                #endregion

                return RedirectToAction("SalesOrderDetails", "SalesOrder", new { id = invoice.SalesOrderId });
            }
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");
            ViewBag.CustomerId = new SelectList(_customerService.GetAllCustomers(), "CustomerId", "FullName", data.CustomerId);
            return View("../Shop/SalesOrder/NewSalesOrder");
        }

        #endregion
        #region Order deatails
        [Roles("Global_SupAdmin,Sales_Create,Sales_Edit")]
        public ActionResult SalesOrderDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder invoice =_salesOrderService.GetSalesOrderById(id.Value);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View("../Shop/SalesOrder/OrderDetails", invoice);
        }
        #endregion

        #region sales order edit
        [Roles("Global_SupAdmin,SalesOrder_Edit")]
        public ActionResult EditSalesOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder invoice = _salesOrderService.GetSalesOrderById(id.Value);
            if (invoice == null)
            {
                return HttpNotFound();
            }

            InvoiceViewModel invoiceViewModel = new InvoiceViewModel();
            invoiceViewModel.OrderId = invoice.SalesOrderId;
            invoiceViewModel.InvoiceNumber = invoice.OrderNumber;
            invoiceViewModel.InvoiceDate = invoice.OrderDate.ToString("dd-MM-yyyy");
            invoiceViewModel.TotalQuantity = invoice.TotalQuantity;
            invoiceViewModel.TotalPrice = invoice.TotalPrice;
            invoiceViewModel.TransactionMode = invoice.TransactionMode;
            invoiceViewModel.TransactionModeId = invoice.TransactionModeId;

            invoiceViewModel.DiscountAmount = invoice.DiscountAmount;
            invoiceViewModel.DiscountType = invoice.DiscountType;

            invoiceViewModel.TotalVat = invoice.TotalVat;
            invoiceViewModel.CashPaid = invoice.CashPaid;
            invoiceViewModel.PaidAmount = invoice.AdvancePaid;

            invoiceViewModel.CustomerId = Convert.ToInt32(invoice.CustomerId);
            invoiceViewModel.Customer = invoice.Customer;
            invoiceViewModel.CustomerBranchId = invoice.CustomerBranchId;
            invoiceViewModel.CustomerBranch = invoice.CustomerBranch;



            if (invoice.OrderProducts.Count > 0)
            {
                List<InvoiceProductViewModel> invoiceProductViewModels = new List<InvoiceProductViewModel>();
                foreach (var item in invoice.OrderProducts)
                {
                    InvoiceProductViewModel invoiceProductVm = new InvoiceProductViewModel()
                   {
                       InvoiceId = item.OrderId,
                       ProductId = item.ProductId,
                       Product = item.Product,
                       Quantity = item.Quantity,
                       UnitPrice = item.Dp,
                       TotalPrice = item.TotalPrice,
                       Vat = item.Product.Vat ?? 0,
                       Barcode = item.Barcode
                   };
                    invoiceProductViewModels.Add(invoiceProductVm);
                }
                invoiceViewModel.InvoiceProducts = invoiceProductViewModels;
            }

            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", invoice.TransactionMode);
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode(invoice.TransactionMode), "Id", "Name", invoice.TransactionModeId);

            ViewBag.CustomerId = new SelectList(_customerService.GetAllCustomers(), "CustomerId", "FullName", invoice.CustomerId);
            ViewBag.CustomerBranchId = new SelectList(_customerService.GetAllCustomerProjectsByCustomerId(invoice.CustomerId).OrderBy(x => x.ProjectName), "CustomerProjectId", "ProjectName", invoice.CustomerBranchId);
            return View("../Shop/SalesOrder/EditSalesOrder", invoiceViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSalesOrder(InvoiceViewModel data)
        {
            //string result = "Error";
            if (ModelState.IsValid)
            {
                var invoice = _salesOrderService.GetSalesOrderById(data.OrderId.Value); 
                if (invoice == null)
                {
                    return HttpNotFound();
                }
                Customer customer = new Customer();
                int? customerId = data.CustomerId;

                // calculation of old transaction amount===============
                var oldTotalPrice = invoice.TotalPrice;
                var oldDiscountAmount = invoice.DiscountAmount;
                var oldDiscountType = invoice.DiscountType;
                var oldVat = invoice.TotalVat;
                double oldTransactionAmount = invoice.AdvancePaid ?? 0;

                // assigning new value ==========================================
                invoice.OrderDate = Convert.ToDateTime(data.InvoiceDate);
                invoice.TotalQuantity = data.InvoiceProducts.Sum(x => x.Quantity);
                invoice.TotalPrice = data.TotalPrice;
                invoice.TotalVat = data.TotalVat;
                invoice.DiscountType = data.DiscountType;
                invoice.DiscountAmount = data.DiscountAmount;
                invoice.TransactionMode = data.TransactionMode;
                invoice.TransactionModeId = data.TransactionModeId;
                invoice.CashPaid = data.CashPaid ?? 0;
                invoice.AdvancePaid = data.PaidAmount ?? 0;
                invoice.Note = data.Note;
                invoice.Status = 1;
                invoice.UpdatedBy =AuthenticatedUser.GetUserFromIdentity().UserId;
                invoice.UpdatedDate = DateTime.Now;

                if (customer.FullName != null)
                {
                    invoice.CustomerId = customer.CustomerId;
                }
                _salesOrderService.EditSalesOrder(invoice, AuthenticatedUser.GetUserFromIdentity().UserId);
               
                //====remove previous invoice products=======================
                var invoicePrdcts =_salesOrderService.GetAllOrderProductsByOrderId(invoice.SalesOrderId).ToList();
                if (invoicePrdcts.Count > 0)
                {
                    _salesOrderService.DeleteOrderProductList(invoicePrdcts);
                   
                }
                //====Add new invoice products=======================
                var sl = 1;
                foreach (var item in data.InvoiceProducts)
                {
                    OrderProduct orderProduct = new OrderProduct()
                    {
                        OrderId = invoice.SalesOrderId,
                        ProductId = Convert.ToInt32(item.ProductId),
                        Barcode = item.Barcode,
                        Dp = Convert.ToDouble(item.UnitPrice),
                        TotalPrice = item.TotalPrice,
                        Quantity = item.Quantity,
                        Status = 1
                    };
                    _salesOrderService.SaveOrderProduct(orderProduct);
                   

                }
                //========add to account transaction as format DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue,currentUserId);============================================================================================================================
                TransactionController transaction = new TransactionController();
                double transactionAmount = data.PaidAmount ?? 0;

                if (Convert.ToDouble(transactionAmount) - Convert.ToDouble(oldTransactionAmount) != 0)
                {
                    transaction.DepositToAccount(data.TransactionMode, Convert.ToInt32(data.TransactionModeId), (Convert.ToDouble(transactionAmount) - Convert.ToDouble(oldTransactionAmount)), Convert.ToDateTime(data.InvoiceDate), "SalesOrders", "SalesOrderId", invoice.SalesOrderId, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), "Advance Payment");
                }

                else if (data.FormSubmitType == 3)
                {
                    ExportOrderChalanExcel(invoice.SalesOrderId);
                }

                return RedirectToAction("SalesOrderDetails", "SalesOrder", new { id = invoice.SalesOrderId });
            }

            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", data.TransactionMode);
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode(data.TransactionMode), "Id", "Name", data.TransactionModeId);

            ViewBag.CustomerId = new SelectList(_customerService.GetAllCustomers(), "CustomerId", "FullName", data.CustomerId);
            ViewBag.CustomerBranchId = new SelectList(_customerService.GetAllCustomerProjectsByCustomerId(data.CustomerId), "CustomerProjectId", "ProjectName", data.CustomerBranchId);
            return View("../Shop/SalesOrder/EditSalesOrder", data);

        }
        #endregion
        #region order to sales

        public ActionResult OrderToSales(int orderId)
        {

            SalesOrder invoice = _salesOrderService.GetSalesOrderById(orderId);
            if (invoice == null)
            {
                return HttpNotFound();
            }

            InvoiceViewModel invoiceViewModel = new InvoiceViewModel
            {
                OrderId = invoice.SalesOrderId,
                TotalQuantity = invoice.TotalQuantity,
                TotalPrice = invoice.TotalPrice,
                TransactionMode = invoice.TransactionMode,
                TransactionModeId = invoice.TransactionModeId,
                DiscountAmount = invoice.DiscountAmount,
                DiscountType = invoice.DiscountType,
                TotalVat = invoice.TotalVat,
                PaidAmount = invoice.AdvancePaid,
                CashPaid = invoice.CashPaid,
                CustomerId = Convert.ToInt32(invoice.CustomerId),
                Customer = invoice.Customer,
                CustomerBranchId = invoice.CustomerBranchId,
                CustomerBranch = invoice.CustomerBranch,
            };




            if (invoice.OrderProducts.Count > 0)
            {
                List<InvoiceProductViewModel> invoiceProductViewModels = new List<InvoiceProductViewModel>();
                foreach (var item in invoice.OrderProducts)
                {
                    InvoiceProductViewModel invoiceProductVm = new InvoiceProductViewModel()
                    {
                        //InvoiceId = item.InvoiceId,
                        //Invoice = item.Invoice,
                        ProductId = item.ProductId,
                        Product = item.Product,
                        Quantity = item.Quantity,
                        UnitPrice = item.Dp,
                        TotalPrice = item.TotalPrice,
                        Vat = item.Product.Vat ?? 0,
                        DiscountAmount = 0,
                        DiscountType = "Flat",
                        Barcode = item.Barcode
                    };
                    invoiceProductViewModels.Add(invoiceProductVm);
                }
                invoiceViewModel.InvoiceProducts = invoiceProductViewModels;
            }

            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", invoice.TransactionMode);
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode(invoice.TransactionMode), "Id", "Name", invoice.TransactionModeId);


            ViewBag.CustomerId = new SelectList(_customerService.GetAllCustomers(), "CustomerId", "FullName", invoice.CustomerId);
            ViewBag.CustomerBranchId = new SelectList(_customerService.GetAllCustomerProjectsByCustomerId(invoice.CustomerId), "CustomerProjectId", "ProjectName", invoice.CustomerBranchId);
            return View("../Shop/Invoice/OrderToSales", invoiceViewModel);

        }
        #endregion
        #region export order
        public void ExportOrderChalanExcel(int id)
        {

            var invoice =_salesOrderService.GetSalesOrderById(id);
            var companyProfile = SettingsController.CompanyInfo();

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add(invoice.OrderNumber);
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
            //Header of table  
            int recordIndex = 1;
            workSheet.Row(recordIndex).Height = 25;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;
            workSheet.Row(recordIndex).Style.Font.Size = 15;
            workSheet.Cells[recordIndex, 2].Value = companyProfile.CompanyName;
            workSheet.Cells[recordIndex, 2, recordIndex, 6].Merge = true;
            recordIndex++;
            //workSheet.Row(recordIndex).Height = 20;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;
            workSheet.Row(recordIndex).Style.WrapText = true;
            workSheet.Cells[recordIndex, 2].Value = companyProfile.CompanyAddress;
            workSheet.Cells[recordIndex, 2, recordIndex, 6].Merge = true;

            recordIndex++;
            //workSheet.Row(recordIndex).Height = 20;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;
            workSheet.Cells[recordIndex, 2].Value = "Cell: " + companyProfile.Phone;
            workSheet.Cells[recordIndex, 2, recordIndex, 6].Merge = true;
            recordIndex++;
            // workSheet.Row(recordIndex).Height = 20;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;
            workSheet.Cells[recordIndex, 2].Value = "Email:" + companyProfile.Email;
            workSheet.Cells[recordIndex, 2, recordIndex, 6].Merge = true;
            recordIndex += 2;
            workSheet.Row(recordIndex).Height = 20;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;
            workSheet.Row(recordIndex).Style.Font.Size = 14;
            workSheet.Cells[recordIndex, 2].Value = "Sales Order";
            workSheet.Cells[recordIndex, 2, recordIndex, 6].Merge = true;
            recordIndex++;

            //workSheet.Row(recordIndex).Height=20;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;
            workSheet.Cells[recordIndex, 2].Value = "Order No: " + invoice.OrderNumber + "       Date: " + invoice.OrderDate.ToString("dd-MM-yyyy");
            workSheet.Cells[recordIndex, 2, recordIndex, 6].Merge = true;
            recordIndex++;

            //workSheet.Row(recordIndex).Height = 20;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;

            workSheet.Cells[recordIndex, 2].Value = "Customer: " + invoice.Customer.FullName;
            workSheet.Cells[recordIndex, 2, recordIndex, 6].Merge = true;
            recordIndex++;
            // workSheet.Row(recordIndex).Height = 20;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;
            workSheet.Row(recordIndex).Style.WrapText = true;
            workSheet.Cells[recordIndex, 2].Value = "Address: " + invoice.Customer.Address;
            workSheet.Cells[recordIndex, 2, recordIndex, 6].Merge = true;
            recordIndex += 2;
            //workSheet.Row(recordIndex).Height = 20;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;

            workSheet.Cells[recordIndex, 1].Value = "SL";
            workSheet.Cells[recordIndex, 2].Value = "Code";
            workSheet.Cells[recordIndex, 3].Value = "Description";
            workSheet.Cells[recordIndex, 4].Value = "Quantity";
            workSheet.Cells[recordIndex, 5].Value = "Stock Zone";
            workSheet.Cells[recordIndex, 6].Value = "Remarks";
            using (ExcelRange Rng = workSheet.Cells[recordIndex, 1, recordIndex, 6])
            {
                Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Rng.Style.Font.Bold = true;
                Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                Rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                Rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                Rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                Rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                Rng.Style.WrapText = true;
            }
            //Body of table  
            recordIndex++;

            foreach (var item in invoice.OrderProducts)
            {

                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 11).ToString();
                workSheet.Cells[recordIndex, 2].Value = item.Product.CustomerOptions.Any(x => x.CustomerId == invoice.CustomerId && !string.IsNullOrEmpty(x.ProductCode)) ?
                    item.Product.CustomerOptions.FirstOrDefault(x => x.CustomerId == invoice.CustomerId).ProductCode : "";
                workSheet.Cells[recordIndex, 3].Value = item.Product.CustomerOptions.Any(x => x.CustomerId == invoice.CustomerId && !string.IsNullOrEmpty(x.ProductDescription)) ?
                    item.Product.CustomerOptions.FirstOrDefault(x => x.CustomerId == invoice.CustomerId).ProductDescription : item.Product.ProductFullName;
                workSheet.Cells[recordIndex, 4].Value = item.Quantity;
                workSheet.Cells[recordIndex, 5].Value = item.Product.WarehouseZone.ZoneName;

                recordIndex++;
            }
            workSheet.Row(recordIndex).Style.Font.Bold = true;
            workSheet.Cells[recordIndex, 1].Value = "Total";
            workSheet.Cells[recordIndex, 4].Value = invoice.TotalQuantity;
            recordIndex++;


            workSheet.Column(1).AutoFit();
            workSheet.Column(2).AutoFit();
            workSheet.Column(3).AutoFit();
            workSheet.Column(4).AutoFit();
            string excelName = "Chalan_" + invoice.OrderNumber;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
        #endregion
        #region po import from excel
        [Roles("Global_SupAdmin,SalesOrder_Create,SalesOrder_Edit")]
        [OutputCache(Duration = 120)]
        public ActionResult PoImport()
        {
            ViewBag.CustomerId = new SelectList(_customerService.GetAllCustomers(), "CustomerId", "FullName");
            return View("../Shop/SalesOrder/PoImport");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PoImport(PoImportViewModel model)
        {
            if (ModelState.IsValid)
            {
                string ExcuteMsg = string.Empty;
                int NumberOfColume = 0;
                try
                {
                    //HttpPostedFileBase file = Request.Files["ProductExcel"];
                    HttpPostedFileBase file = model.PoExcelFile;
                    //Extaintion Check
                    if (model.PoExcelFile.FileName.EndsWith("xls") || file.FileName.EndsWith("xlsx") ||
                        file.FileName.EndsWith("XLS") ||
                        file.FileName.EndsWith("XLSX"))
                    {
                        //Null Exp Check
                        if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                        {
                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                            List<PoProductViewModel> poProductImportList = new List<PoProductViewModel>();


                            using (var package = new ExcelPackage(file.InputStream))
                            {
                                var currentSheet = package.Workbook.Worksheets;
                                var workSheet = currentSheet.First();
                                var noOfCol = workSheet.Dimension.End.Column;
                                var noOfRow = workSheet.Dimension.End.Row;

                                #region iterat excel to list

                                for (int rowIterator = 4; rowIterator <= noOfRow; rowIterator++)
                                {
                                    PoProductViewModel productImport = new PoProductViewModel
                                    {
                                        ProductCode = workSheet.Cells[rowIterator, 2].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 2].Value.ToString(),
                                        ProductName = workSheet.Cells[rowIterator, 3].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 3].Value.ToString(),

                                        Quantity = workSheet.Cells[rowIterator, 4].Value == null
                                            ? (double?)null
                                            : Convert.ToDouble(workSheet.Cells[rowIterator, 4].Value),

                                        RetailPrice = workSheet.Cells[rowIterator, 5].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 5].Value), 2),


                                    };
                                    if (productImport.ProductCode != null && productImport.Quantity != null && _productService.GetAllByProductByCodeAndCustomer(model.CustomerId, productImport.ProductCode).Any())
                                    {
                                        poProductImportList.Add(productImport);
                                    }

                                }
                            }

                                #endregion

                            //List data saving
                            if (poProductImportList.Count > 0)
                            {
                                string fmt = "000000000.##";
                                int lastInvoiceId = _salesOrderService.GetCount();

                                #region Invoice save

                                SalesOrder salesOrder = new SalesOrder()
                                {
                                    OrderDate = Convert.ToDateTime(model.PoDate),
                                    OrderNumber =
                                        "O" + TransactionController.BillingMonthString(model.PoDate) +
                                        (lastInvoiceId + 1).ToString(fmt),
                                    TotalQuantity = poProductImportList.Sum(x => x.Quantity),
                                    TotalPrice = poProductImportList.Sum(x => (x.Quantity * x.RetailPrice)),
                                    TotalVat = 0,
                                    DiscountType = "Flat",
                                    DiscountAmount = 0,
                                    CustomerId = model.CustomerId,
                                    CustomerBranchId = model.CustomerBranchId,
                                    TransactionMode = "Cash",
                                    TransactionModeId = _transactionAccountService.GetAllCashAccounts().Any() ? _transactionAccountService.GetAllCashAccounts().FirstOrDefault().CashId : (int?)null,
                                    AdvancePaid = 0,
                                    CashPaid = 0,
                                    Status = 1,
                                    CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId,
                                    CreatedDate = DateTime.Now
                                };
                            _salesOrderService.SaveSalesOrder(salesOrder, AuthenticatedUser.GetUserFromIdentity().UserId);
                               
                                #endregion
                                foreach (var item in poProductImportList)
                                {
                                    var customerProduct = _productService.GetAllByProductByCodeAndCustomer(salesOrder.CustomerId.Value, item.ProductCode).FirstOrDefault();
                                        
                                    OrderProduct orderProduct = new OrderProduct
                                    {
                                        ProductId = customerProduct.ProductId,
                                        OrderId = salesOrder.SalesOrderId,
                                        Quantity = Convert.ToDouble(item.Quantity),
                                        Dp = item.RetailPrice ?? 0,
                                        TotalPrice = item.Quantity * item.RetailPrice,
                                        Status = 1,
                                        CreatedBy =AuthenticatedUser.GetUserFromIdentity().UserId,
                                        CreatedDate = DateTime.Now,
                                    };
                                    _salesOrderService.SaveOrderProduct(orderProduct);
                                    
                                }
                            
                                //if (db.OrderProducts.Any(x => x.OrderId == salesOrder.SalesOrderId))
                                //{
                                //    salesOrder.TotalQuantity =
                                //        db.OrderProducts.Where(x => x.OrderId == salesOrder.SalesOrderId)
                                //            .Sum(x => x.Quantity);
                                //    salesOrder.TotalPrice =
                                //        db.OrderProducts.Where(x => x.OrderId == salesOrder.SalesOrderId)
                                //            .Sum(x => (x.Dp*x.Quantity));
                                //    db.Entry(salesOrder).State = EntityState.Modified;
                                //    db.SaveChanges("");
                                //}
                                ExcuteMsg = "Excel Input Successfully" + "  Number of Colume :" + NumberOfColume;
                                return RedirectToAction("SalesOrderDetails", "SalesOrder", new { id = salesOrder.SalesOrderId });

                            }
                        }

                    }
                    else
                    {
                        ViewBag.Error = "File type is incorrect <br>";

                    }
                }
                catch (Exception r)
                {
                    ExcuteMsg = r.Message;
                }
                ViewBag.ExcuteMsg = ExcuteMsg;
            }
            ViewBag.CustomerId = new SelectList(_customerService.GetAllCustomers(), "CustomerId", "FullName", model.CustomerId);
            return View("../Shop/SalesOrder/PoImport", model);
        }
        #endregion
        #region deispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _salesOrderService.Dispose();
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}

//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//