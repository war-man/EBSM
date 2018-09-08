using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;

using System.Web.Security;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private WmsDbContext db = new WmsDbContext();
        private const int InitialInvoiceNo = 1;
        #region list view
        [Roles("Global_SupAdmin,Sales_Create,Sales_Edit")]
         [OutputCache(Duration = 20)]
        public ActionResult Index(InvoiceSearchViewModel model)
        {
            var fromDate = Convert.ToDateTime(model.InvoiceDateFrom);
            var toDate = Convert.ToDateTime(model.InvoiceDateTo);
            var invoices = db.Invoices.Where(x => (model.TransactionMode == null || x.TransactionMode.Equals(model.TransactionMode))&&(model.InvoiceNo == null || x.InvoiceNumber.StartsWith(model.InvoiceNo))
                && (model.InvoiceDateFrom == null || x.InvoiceDate >= fromDate) && (model.InvoiceDateTo == null || x.InvoiceDate <= toDate)
                 && (model.CustomerId == null || x.CustomerId == model.CustomerId) && (model.SalesmanId == null || x.SalesmanId == model.SalesmanId)).Include(o => o.Salesman).OrderByDescending(o => o.InvoiceDate).ThenByDescending(o => o.CreatedDate);
            model.Invoices = invoices.ToPagedList(model.Page, model.PageSize);

            ViewBag.SalesmanId = new SelectList(db.Salesman.OrderBy(x=>x.FullName), "SalesmanId", "FullName");
            ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.Status != 0).OrderBy(x => x.FullName), "CustomerId", "FullName");
            return View("../Shop/Invoice/Index", model);
        }
        #endregion
        #region new sales
        [Roles("Global_SupAdmin,Sales_Create,Sales_Edit")]
         [OutputCache(Duration = 120)]
        public ActionResult NewSales()
        {
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text","Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id","Name");
            ViewBag.SalesmanId = new SelectList(db.Salesman.OrderBy(x => x.FullName), "SalesmanId", "FullName");
            ViewBag.CustomerId = new SelectList(db.Customers.OrderBy(x => x.FullName), "CustomerId", "FullName");
            return View("../Shop/Invoice/NewSales");

       }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveInvoice(InvoiceViewModel data)
        {
            if (ModelState.IsValid)
            {
                InvoicePrintModel invoicePm = new InvoicePrintModel();
                List<ProductPrintModel> itemPmList = new List<ProductPrintModel>();

                int billId = 0;
                string fmt = "0000000.##";
                int lastInvoiceId = 0;
                var invoices = db.Invoices.ToList();
                if (invoices.Count > 0)
                {
                    lastInvoiceId = invoices.OrderByDescending(x => x.InvoiceId).FirstOrDefault().InvoiceId;
                }
                #region Invoice save
                Invoice invoice = new Invoice()
                {
                    InvoiceDate = Convert.ToDateTime(data.InvoiceDate),
                    InvoiceNumber = "S" + TransactionController.BillingMonthString(data.InvoiceDate) + (lastInvoiceId + 1).ToString(fmt),
                    TotalQuantity = data.InvoiceProducts.Sum(x => x.Quantity),
                    TotalPrice = data.TotalPrice,
                    TotalVat = data.TotalVat,
                    DiscountType = data.DiscountType,
                    DiscountAmount = data.DiscountAmount,
                    TransactionMode = data.TransactionMode,
                    TransactionModeId = data.TransactionModeId,
                    CustomerId = data.CustomerId,
                    CustomerBranchId = data.CustomerBranchId,
                    SalesmanId = data.SalesmanId,
                    PaidAmount = data.PaidAmount ?? 0,
                    CashPaid = data.CashPaid ?? 0,
                    Note = data.Note,
                    Status = 1,
                    CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                    CreatedDate = DateTime.Now
                };
                db.Invoices.Add(invoice);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                #endregion
                #region invoice products update and stok update
                var sl = 1;
                foreach (var item in data.InvoiceProducts)
                {
                    InvoiceProduct invoiceProduct = new InvoiceProduct()
                    {
                        InvoiceId = invoice.InvoiceId,
                        ProductId = Convert.ToInt32(item.ProductId),
                        Barcode =item.Barcode,
                        Dp = Convert.ToDouble(item.UnitPrice),
                        DiscountType = item.DiscountType,
                        DiscountAmount = item.DiscountAmount,
                        TotalPrice = item.TotalPrice,
                        Quantity = item.Quantity,
                        Status = 1
                    };
                    db.InvoiceProducts.Add(invoiceProduct);
                    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                    //====update stock==============================================
                    #region stock update
                    StockController updateStock = new StockController();
                    if (string.IsNullOrEmpty(item.Barcode))
                    {
                        if (item.DefaultZoneId == null)
                        {
                            updateStock.RemoveFromStock(Convert.ToInt32(invoiceProduct.ProductId), item.Quantity,
                                Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                        }
                        else 
                        {
                            updateStock.RemoveFromStock(Convert.ToInt32(invoiceProduct.ProductId), item.Quantity,
                                Convert.ToInt32(item.DefaultZoneId),
                                Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                        }
                    }
                    else
                    {
                        if (item.DefaultZoneId == null)
                        {
                            updateStock.RemoveFromStock(Convert.ToInt32(invoiceProduct.ProductId), item.Quantity,
                                Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                                item.Barcode);
                        }
                        else
                        {
                            updateStock.RemoveFromStock(Convert.ToInt32(invoiceProduct.ProductId), item.Quantity,
                                Convert.ToInt32(item.DefaultZoneId),
                                Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), item.Barcode);
                        }
                    }
                    #endregion
                    //====add to printing model==============================================
                    if (data.FormSubmitType == 2) { 
                    ProductPrintModel productItem = new ProductPrintModel()
                    {
                        Sl = sl,

                        ItemName = db.Products.Find(invoiceProduct.ProductId).ProductFullName,
                        ItemCode = db.Products.Find(invoiceProduct.ProductId).ProductCode,
                        UnitPrice = invoiceProduct.Dp,
                        Quantity = invoiceProduct.Quantity,
                        Total = invoiceProduct.TotalPrice
                    };
                    sl++;
                    itemPmList.Add(productItem);
                    }
                }
                #endregion
                #region transaction chanel
                //========add to account transaction as format DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue,currentUserId);============================================================================================================================
                TransactionController transaction = new TransactionController();
                double transactionAmount = 0;
                if (String.IsNullOrEmpty(data.DiscountAmount.ToString()) == false && String.IsNullOrEmpty(data.DiscountType.ToString()) == false)
                {
                    transactionAmount = Convert.ToDouble((data.TotalPrice - BankAccountController.DiscountCalculator(data.DiscountAmount, data.DiscountType, data.TotalPrice).DiscountValue) + data.TotalVat);
                }
                else
                {
                    transactionAmount = Convert.ToDouble(data.TotalPrice + data.TotalVat);
                }

                transaction.DepositToAccount(data.TransactionMode, Convert.ToInt32(data.TransactionModeId), transactionAmount, Convert.ToDateTime(data.InvoiceDate), "Invoices", "InvoiceId", invoice.InvoiceId, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),"Sells Payment");
                #endregion
                #region sales to order

                if (data.OrderId != null)
                {
                    SaveInvoiceOrderRelation(invoice.InvoiceId,Convert.ToInt32(data.OrderId));
                }
                #endregion
                #region Invoice Printing
                //----------------------------invoice recipt printing-------------------------------------------------------------
                if (data.FormSubmitType == 2)
                {
                    var discountAmount =BankAccountController.DiscountCalculator(invoice.DiscountAmount, invoice.DiscountType,invoice.TotalPrice).DiscountValue;
                    invoicePm.InvoiceNumber = invoice.InvoiceNumber;
                    invoicePm.InvoiceDateTime = string.Format("{0:dd-MMM-yyyy hh:mm tt}", invoice.CreatedDate);
                    invoicePm.NetTotal = Convert.ToDouble(invoice.TotalPrice);
                    invoicePm.DiscountAmount = Convert.ToDouble(invoice.DiscountAmount);
                    invoicePm.DiscountType = invoice.DiscountType;
                    invoicePm.VatAmount = Convert.ToDouble(invoice.TotalVat);
                    //int ShopDeptId =db.Departments.FirstOrDefault(x => x.DepartmentName.Equals("Shop") && x.Status != 0).DepartmentId;
                    //invoicePm.VatPercentage = Convert.ToDouble(db.Vats.FirstOrDefault(x => x.DepartmentId == ShopDeptId).VatPercentage);
                    invoicePm.Total = Convert.ToDouble((invoicePm.NetTotal - discountAmount) + invoicePm.VatAmount);
                    invoicePm.CashierName = Membership.GetUser(User.Identity.Name, true).UserName;
                    invoicePm.Items = itemPmList;
                    invoicePm.CashPaid = Convert.ToDouble(invoice.CashPaid);
                    invoicePm.ReturnAmount = Convert.ToDouble(invoice.CashPaid) - invoicePm.Total;
                    TicketPrintController invoicePrint = new TicketPrintController();
                    invoicePrint.PrintInvoice(invoicePm);
                    return RedirectToAction("NewSales", "Invoice");
                }
                else if (data.FormSubmitType == 3)
                {
                    return RedirectToAction("InvoicePrint", "Invoice", new {id=invoice.InvoiceId});
                }else if (data.FormSubmitType == 4)
                {
                    var billController = new BillController();
                   
                    billId = billController.SaveInvoiceBill(invoice, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    return RedirectToAction("BillDetails", "Bill", new { id = billId });
                }

                #endregion
                return RedirectToAction("Index", "Invoice");
                
            }
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");
            ViewBag.SalesmanId = new SelectList(db.Salesman.OrderBy(x => x.FullName), "SalesmanId", "FullName");
            ViewBag.CustomerId = new SelectList(db.Customers.OrderBy(x => x.FullName), "CustomerId", "FullName",data.CustomerId);
            return View("../Shop/Invoice/NewSales");
        }

        #endregion
        #region invoice deatails
         [Roles("Global_SupAdmin,Sales_Create,Sales_Edit")]
        public ActionResult InvoiceDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View("../Shop/Invoice/InvoiceDetails",invoice);
        }
        #endregion
        #region invoice print
         [Roles("Global_SupAdmin,Sales_Create,Sales_Edit")]
        public ActionResult InvoicePrint(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }

            return View("../Shop/Invoice/InvoicePrint", invoice);
        }
        #endregion
        #region sales edit
         [Roles("Global_SupAdmin,Sales_Edit")]
        public ActionResult EditSales(int? id){
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
           
            InvoiceViewModel invoiceViewModel=new InvoiceViewModel();
            invoiceViewModel.InvoiceId = invoice.InvoiceId;
            invoiceViewModel.InvoiceNumber = invoice.InvoiceNumber;
            invoiceViewModel.InvoiceDate = invoice.InvoiceDate.ToString("dd-MM-yyyy");
            invoiceViewModel.TotalQuantity = invoice.TotalQuantity;
            invoiceViewModel.TotalPrice = invoice.TotalPrice;
            invoiceViewModel.TransactionMode = invoice.TransactionMode;
            invoiceViewModel.TransactionModeId = invoice.TransactionModeId;
           
            invoiceViewModel.DiscountAmount = invoice.DiscountAmount;
            invoiceViewModel.DiscountType = invoice.DiscountType;
            invoiceViewModel.SalesmanId = invoice.SalesmanId;
            invoiceViewModel.TotalVat = invoice.TotalVat;
            invoiceViewModel.CashPaid = invoice.CashPaid;
            invoiceViewModel.PaidAmount = invoice.PaidAmount;
            invoiceViewModel.CashPaid = invoice.CashPaid;
           
                invoiceViewModel.CustomerId = Convert.ToInt32(invoice.CustomerId);
                invoiceViewModel.Customer = invoice.Customer;
                invoiceViewModel.CustomerBranchId = invoice.CustomerBranchId;
                invoiceViewModel.CustomerBranch = invoice.CustomerBranch;
             
                
            
            if (invoice.InvoiceProducts.Count > 0)
            { 
                List<InvoiceProductViewModel> invoiceProductViewModels=new List<InvoiceProductViewModel>();
             foreach(var item in invoice.InvoiceProducts){
                 InvoiceProductViewModel invoiceProductVm = new InvoiceProductViewModel()
                {
                    InvoiceId = item.InvoiceId,
                    Invoice = item.Invoice,
                    ProductId = item.ProductId,
                    Product = item.Product,
                    Quantity = item.Quantity,
                    UnitPrice = item.Dp,
                    TotalPrice = item.TotalPrice,
                    Vat = item.Product.Vat??0,
                    DiscountAmount = item.DiscountAmount??0,
                    DiscountType = item.DiscountType,
                    Barcode = item.Barcode
                };
                invoiceProductViewModels.Add(invoiceProductVm);
            }
                invoiceViewModel.InvoiceProducts = invoiceProductViewModels;
            }
       
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", invoice.TransactionMode);
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode(invoice.TransactionMode), "Id", "Name",invoice.TransactionModeId);
            ViewBag.SalesmanId = new SelectList(db.Salesman.OrderBy(x => x.FullName), "SalesmanId", "FullName", invoice.SalesmanId);
    
            ViewBag.CustomerId = new SelectList(db.Customers.OrderBy(x => x.FullName), "CustomerId", "FullName",invoice.CustomerId);
            ViewBag.CustomerBranchId = new SelectList(db.CustomerProjects.Where(x => x.CustomerId == invoice.CustomerId).OrderBy(x => x.ProjectName), "CustomerProjectId", "ProjectName", invoice.CustomerBranchId);
            return View("../Shop/Invoice/EditSales", invoiceViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSales(InvoiceViewModel data)
        {
            //string result = "Error";
            if (ModelState.IsValid)
            {
                InvoicePrintModel invoicePm = new InvoicePrintModel();
                List<ProductPrintModel> itemPmList = new List<ProductPrintModel>();
                Invoice invoice = db.Invoices.Find(data.InvoiceId);
                if (invoice == null)
                {
                    return HttpNotFound();
                }
                Customer customer = new Customer();
                int? customerId = data.CustomerId;
               
                // calculation of old transaction amount===============
                var oldTotalPrice = invoice.TotalPrice;
                var oldDiscountAmount=invoice.DiscountAmount;
                var oldDiscountType = invoice.DiscountType;
                var oldVat = invoice.TotalVat;
                double oldTransactionAmount = 0;
                if (!String.IsNullOrEmpty(data.DiscountAmount.ToString()) && !String.IsNullOrEmpty(data.DiscountType.ToString()))
                {
                    oldTransactionAmount = (Convert.ToDouble(oldTotalPrice) - Convert.ToDouble(BankAccountController.DiscountCalculator(oldDiscountAmount, oldDiscountType, oldTotalPrice).DiscountValue)) + Convert.ToDouble(oldVat);
                }
                else
                {
                    oldTransactionAmount = Convert.ToDouble(oldTotalPrice) + Convert.ToDouble(oldVat);
                }
                // assigning new value ==========================================
                invoice.InvoiceDate = Convert.ToDateTime(data.InvoiceDate);
                invoice.TotalQuantity = data.InvoiceProducts.Sum(x => x.Quantity);
                invoice.TotalPrice = data.TotalPrice;
                invoice.TotalVat = data.TotalVat;
                invoice.DiscountType = data.DiscountType;
                invoice.DiscountAmount = data.DiscountAmount;
                invoice.TransactionMode = data.TransactionMode;
                invoice.TransactionModeId = data.TransactionModeId;

                invoice.CashPaid = data.CashPaid ?? 0;
                invoice.Note = data.Note;
                invoice.Status = 1;

                invoice.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                invoice.UpdatedDate = DateTime.Now;
               
                if (customer.FullName != null)
                {
                    invoice.CustomerId = customer.CustomerId;
                }
                if (data.SalesmanId != null)
                {
                    invoice.SalesmanId = data.SalesmanId;
                }
                db.Entry(invoice).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                StockController updateStock = new StockController();

                //====remove previous invoice products=======================
                var invoicePrdcts = db.InvoiceProducts.Where(x => x.InvoiceId == invoice.InvoiceId).ToList();
                if (invoicePrdcts.Count > 0)
                {
                    foreach (var removeItem in invoicePrdcts)
                    {
                        //====update stock==============================================

                        if (string.IsNullOrEmpty(removeItem.Barcode))
                        {
                            if (removeItem.Product.DefaultZoneId == null)
                            {
                                updateStock.AddToStock(Convert.ToInt32(removeItem.ProductId), removeItem.Quantity,
                                Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                            }
                            else
                            {
                                updateStock.AddToStock(Convert.ToInt32(removeItem.ProductId), removeItem.Quantity,
                                    Convert.ToInt32(removeItem.Product.DefaultZoneId),
                                    Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                            }
                        }
                        else
                        {
                            if (removeItem.Product.DefaultZoneId == null)
                            {
                                updateStock.AddToStock(Convert.ToInt32(removeItem.ProductId), removeItem.Quantity,
                              Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), removeItem.Barcode);
                            }
                            else
                            {
                                updateStock.AddToStock(Convert.ToInt32(removeItem.ProductId), removeItem.Quantity,
                                    Convert.ToInt32(removeItem.Product.DefaultZoneId),
                                    Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), removeItem.Barcode);
                            }
                        }

                      
                     
                        db.InvoiceProducts.Remove(removeItem);
                    }
                    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                }
                //====Add new invoice products=======================
                var sl = 1;
                foreach (var item in data.InvoiceProducts)
                {
                    InvoiceProduct invoiceProduct = new InvoiceProduct()
                    {
                        InvoiceId = invoice.InvoiceId,
                        ProductId = Convert.ToInt32(item.ProductId),
                        Barcode = item.Barcode,
                        Dp = Convert.ToDouble(item.UnitPrice),
                        DiscountType = item.DiscountType,
                        DiscountAmount = item.DiscountAmount,
                        TotalPrice = item.TotalPrice,
                        Quantity = item.Quantity,
                        Status = 1


                        
                    };
                    db.InvoiceProducts.Add(invoiceProduct);
                    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                    //====update stock==============================================
                    if (string.IsNullOrEmpty(item.Barcode))
                    {
                        if (item.DefaultZoneId == null)
                        {
                            updateStock.RemoveFromStock(Convert.ToInt32(invoiceProduct.ProductId), item.Quantity,
                                Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                        }
                        else
                        {
                            updateStock.RemoveFromStock(Convert.ToInt32(invoiceProduct.ProductId), item.Quantity,
                                Convert.ToInt32(item.DefaultZoneId),
                                Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                        }
                    }
                    else
                    {
                        if (item.DefaultZoneId == null)
                        {
                            updateStock.RemoveFromStock(Convert.ToInt32(invoiceProduct.ProductId), item.Quantity,
                                Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                                item.Barcode);
                        }
                        else
                        {
                            updateStock.RemoveFromStock(Convert.ToInt32(invoiceProduct.ProductId), item.Quantity,
                                Convert.ToInt32(item.DefaultZoneId),
                                Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), item.Barcode);
                        }
                    }

             
                    //====add to printing model==============================================
                    if (data.FormSubmitType == 2)
                    {
                        ProductPrintModel productItem = new ProductPrintModel()
                        {
                            Sl = sl,

                            ItemName = db.Products.Find(invoiceProduct.ProductId).ProductFullName,
                            ItemCode = db.Products.Find(invoiceProduct.ProductId).ProductCode,
                            UnitPrice = invoiceProduct.Dp,
                            Quantity = invoiceProduct.Quantity,
                            Total = invoiceProduct.TotalPrice
                        };
                        sl++;
                        itemPmList.Add(productItem);
                    }
                }
                //========add to account transaction as format DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue,currentUserId);============================================================================================================================
                TransactionController transaction = new TransactionController();
                double transactionAmount = 0;
                if (!String.IsNullOrEmpty(data.DiscountAmount.ToString()) && !String.IsNullOrEmpty(data.DiscountType.ToString()))
                {
                    transactionAmount = Convert.ToDouble((data.TotalPrice - BankAccountController.DiscountCalculator(data.DiscountAmount, data.DiscountType, data.TotalPrice).DiscountValue) + data.TotalVat);
                }
                else
                {
                    transactionAmount = Convert.ToDouble(data.TotalPrice) + Convert.ToDouble(data.TotalVat);
                }
                if (Convert.ToDouble(transactionAmount) - Convert.ToDouble(oldTransactionAmount) != 0) {
                    transaction.DepositToAccount(data.TransactionMode, Convert.ToInt32(data.TransactionModeId), (Convert.ToDouble(transactionAmount) - Convert.ToDouble(oldTransactionAmount)), Convert.ToDateTime(data.InvoiceDate), "Invoices", "InvoiceId", invoice.InvoiceId, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), "Sells Payment");
                }
                //----------------------------invoice printing-------------------------------------------------------------
                if (data.FormSubmitType == 2)
                {
                    var discountAmount =
                        BankAccountController.DiscountCalculator(invoice.DiscountAmount, invoice.DiscountType,
                            invoice.TotalPrice).DiscountValue;
                    invoicePm.InvoiceNumber = invoice.InvoiceNumber;
                    invoicePm.InvoiceDateTime = string.Format("{0:dd-MMM-yyyy hh:mm tt}", invoice.CreatedDate);
                    invoicePm.NetTotal = Convert.ToDouble(invoice.TotalPrice);
                    invoicePm.DiscountAmount = Convert.ToDouble(invoice.DiscountAmount);
                    invoicePm.DiscountType = invoice.DiscountType;
                    invoicePm.VatAmount = Convert.ToDouble(invoice.TotalVat);
                    invoicePm.Total = Convert.ToDouble((invoicePm.NetTotal - discountAmount) + invoicePm.VatAmount);
                    invoicePm.CashierName = Membership.GetUser(User.Identity.Name, true).UserName;
                    invoicePm.Items = itemPmList;
                    invoicePm.CashPaid = Convert.ToDouble(invoice.CashPaid);
                    invoicePm.ReturnAmount = Convert.ToDouble(invoice.CashPaid) - invoicePm.Total;
                    TicketPrintController invoicePrint = new TicketPrintController();
                    invoicePrint.PrintInvoice(invoicePm);
                }

                else if (data.FormSubmitType == 3)
                {
                    return RedirectToAction("InvoicePrint", "Invoice", new { id = invoice.InvoiceId });
                }

                return RedirectToAction("Index", "Invoice");
            }
          
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", data.TransactionMode);
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode(data.TransactionMode), "Id", "Name", data.TransactionModeId);
            ViewBag.SalesmanId = new SelectList(db.Salesman.OrderBy(x => x.FullName), "SalesmanId", "FullName", data.SalesmanId);
            ViewBag.CustomerId = new SelectList(db.Customers.OrderBy(x => x.FullName), "CustomerId", "FullName", data.CustomerId);
            ViewBag.CustomerBranchId = new SelectList(db.CustomerProjects.Where(x => x.CustomerId == data.CustomerId).OrderBy(x => x.ProjectName), "CustomerProjectId", "ProjectName", data.CustomerBranchId);
            return View("../Shop/Invoice/EditSales", data);

        }
        #endregion
        #region export chalan
        public void ExportChalanExcel(int id)
        {
           
            Invoice invoice = db.Invoices.Find(id);
            var companyProfile = SettingsController.CompanyInfo();

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add(invoice.InvoiceNumber);
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
            workSheet.Cells[recordIndex, 2].Value ="Cell: "+ companyProfile.Phone;
            workSheet.Cells[recordIndex, 2, recordIndex, 6].Merge = true;
            recordIndex++;
           // workSheet.Row(recordIndex).Height = 20;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;
            workSheet.Cells[recordIndex, 2].Value = "Email:"+companyProfile.Email;
            workSheet.Cells[recordIndex, 2, recordIndex, 6].Merge = true;
            recordIndex += 2;
            workSheet.Row(recordIndex).Height = 20;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;
            workSheet.Row(recordIndex).Style.Font.Size = 14;
            workSheet.Cells[recordIndex, 2].Value = "Delivery Chalan ";
            workSheet.Cells[recordIndex, 2, recordIndex, 6].Merge = true;
            recordIndex++;
           
            //workSheet.Row(recordIndex).Height=20;
            workSheet.Row(recordIndex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(recordIndex).Style.Font.Bold = true;
            workSheet.Cells[recordIndex, 2].Value = "Inovice No: " + invoice.InvoiceNumber+"       Date: " + invoice.InvoiceDate.ToString("dd-MM-yyyy");
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

            foreach (var item in invoice.InvoiceProducts)
            {

                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 11).ToString();
                workSheet.Cells[recordIndex, 2].Value = item.Product.CustomerOptions.Any(x => x.CustomerId == invoice.CustomerId)?
                    item.Product.CustomerOptions.FirstOrDefault(x => x.CustomerId == invoice.CustomerId).ProductCode:item.Product.ProductCode;
                workSheet.Cells[recordIndex, 3].Value = item.Product.CustomerOptions.Any(x => x.CustomerId == invoice.CustomerId) ?
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
            string excelName = "Chalan_" + invoice.InvoiceNumber;
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
        #region  sales return
        #region sales return list view

        [Roles("Global_SupAdmin,Sales_Return")]
        [OutputCache(Duration = 10)]
        public ActionResult LoadSalesReturn(SalesReturnSearchViewModel model)
        {
            var fromDate = Convert.ToDateTime(model.ReturnDateFrom);
            var toDate = Convert.ToDateTime(model.ReturnDateTo);
            var returns = db.Returns.Where(x =>  (model.InvoiceNo == null || x.Invoice.InvoiceNumber.StartsWith(model.InvoiceNo))
                && (model.ReturnDateFrom == null || x.CreatedDate >= fromDate) && (model.ReturnDateTo == null || x.CreatedDate <= toDate)
                 && (model.CustomerId == null || x.Invoice.CustomerId == model.CustomerId)).Include(x=>x.Invoice).OrderByDescending(o => o.CreatedDate);
            model.SalesReturns = returns.ToPagedList(model.Page, model.PageSize);

            ViewBag.SalesmanId = new SelectList(db.Salesman.OrderBy(x => x.FullName), "SalesmanId", "FullName");
            ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.Status != 0).OrderBy(x => x.FullName), "CustomerId", "FullName");
            return View("../Shop/Invoice/SalesReturnList", model);
        }
        #endregion
        #region create sales return
        [Roles("Global_SupAdmin,Sales_Return")]
        public ActionResult SalesReturn(int? invoiceId)
        {
            if (invoiceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(invoiceId);
            if (invoice == null)
            {
                return HttpNotFound();
            }

            SalesReturnViewModel returnViewModel = new SalesReturnViewModel
            {
                InvoiceId = invoice.InvoiceId,
                Invoice = invoice,
                TotalQuantity = 0,
                TotalReturn = 0,
            };
            
            if (invoice.InvoiceProducts.Count > 0)
            {
                List<ReturnProductViewModel> returnProductList = new List<ReturnProductViewModel>();
                foreach (var item in invoice.InvoiceProducts)
                {
                    ReturnProductViewModel returnProductVm = new ReturnProductViewModel()
                    {
                        
                        
                        ProductId = item.ProductId,
                        Product = item.Product,
                        InvoiceProductId = item.InvoiceProductId,
                        InvoiceProduct = item,
                        UnitPrice = item.Dp,
                        ReturnQty = 0,
                       ReturnAmount = 0,
                        Barcode = item.Barcode
                    };
                    returnProductList.Add(returnProductVm);
                }
                returnViewModel.ReturnProducts = returnProductList;
            }

            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", invoice.TransactionMode);
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode(invoice.TransactionMode), "Id", "Name", invoice.TransactionModeId);

            return View("../Shop/Invoice/SalesReturn", returnViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalesReturn(SalesReturnViewModel data)
        {
            //string result = "Error";
            if (ModelState.IsValid && data.ReturnProducts.Any(x => x.ReturnQty > 0))
            {
                // assigning new value ==========================================
         
                Return salesReturn = new Return
                {
                    InvoiceId=data.InvoiceId,
                    TotalQuantity = data.ReturnProducts.Sum(x => x.ReturnQty),
                    TotalPrice=data.TotalReturn,
                    Status = 1,
                    CreatedDate = Convert.ToDateTime(data.ReturnDate),
                    CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey)
                };
                db.Returns.Add(salesReturn);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                StockController updateStock = new StockController();

                
                //====Add new return products=======================
                var sl = 1;
                foreach (var item in data.ReturnProducts.Where(x=>x.ReturnQty>0))
                {
                    ReturnProduct returnProduct = new ReturnProduct
                    {
                        ReturnId = salesReturn.ReturnId,
                        ProductId = Convert.ToInt32(item.ProductId),
                        //Barcode = item.Barcode,
                        UnitPrice = Convert.ToDouble(item.UnitPrice),
                        TotalPrice = item.ReturnAmount,
                        Quantity = Convert.ToDouble(item.ReturnQty),
                        ZoneId=item.ZoneId,
                        Status = 1,
                    };
                    db.ReturnProducts.Add(returnProduct);
                    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                    //====update stock==============================================
                    if (string.IsNullOrEmpty(item.Barcode))
                    {
                        if (item.DefaultZoneId == null)
                        {
                            updateStock.AddToStock(Convert.ToInt32(item.ProductId), Convert.ToDouble(item.Quantity),
                            Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                        }
                        else
                        {
                            updateStock.AddToStock(Convert.ToInt32(item.ProductId), Convert.ToDouble(item.Quantity),
                                Convert.ToInt32(item.DefaultZoneId),
                                Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                        }
                    }
                    else
                    {
                        if (item.DefaultZoneId == null)
                        {
                            updateStock.AddToStock(Convert.ToInt32(item.ProductId), Convert.ToDouble(item.Quantity),
                          Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), item.Barcode);
                        }
                        else
                        {
                            updateStock.AddToStock(Convert.ToInt32(item.ProductId), Convert.ToDouble(item.Quantity),
                                Convert.ToInt32(item.DefaultZoneId),
                                Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), item.Barcode);
                        }
                    }

                    //remove product quantity from invoice product
                    RemoveInvoiceProduct(data.InvoiceId, Convert.ToInt32(item.ProductId), Convert.ToDouble(item.ReturnQty), Convert.ToDouble(item.ReturnAmount));

                }

                // Invoice update  ---------------------
                Invoice invoice = db.Invoices.FirstOrDefault(x => x.InvoiceId == data.InvoiceId);
                invoice.TotalQuantity = invoice.TotalQuantity -salesReturn.TotalQuantity;
                invoice.TotalPrice = invoice.TotalPrice - salesReturn.TotalPrice;
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                return RedirectToAction("Index", "Invoice");
            }

            return View("../Shop/Invoice/SalesReturn", data);

        }
        #endregion
        #region sales return details
         [Roles("Global_SupAdmin,Sales_Return")]
        public ActionResult ReturnDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Return salesReturn = db.Returns.Find(id);
            if (salesReturn == null)
            {
                return HttpNotFound();
            }
            return View("../Shop/Invoice/ReturnDetails", salesReturn);
        }
        #endregion
        #endregion
        #region helping modules
        public static void SaveInvoiceOrderRelation( int invoiceId, int orderId)
        {
            using (var cx = new WmsDbContext()) { 
            var invoiceOrder = new InvoiceOrder
            {
                InvoiceId = invoiceId,
                OrderId = orderId
            };
            cx.InvoiceOrders.Add(invoiceOrder);
            cx.SaveChanges("");

            var order = cx.SalesOrders.Find(orderId);
            order.Status = 2;
            cx.Entry(order).State = EntityState.Modified;
            cx.SaveChanges("");
            }

        }
        public static void RemoveInvoiceProduct(int invoiceId, int productId, double returnQty, double returnAmount)
    {
        using (var cx = new WmsDbContext())
        {
            var invoiceProduct = cx.InvoiceProducts.FirstOrDefault(x => x.InvoiceId == invoiceId && x.ProductId == productId);
            if (invoiceProduct != null)
            {
                invoiceProduct.Quantity = invoiceProduct.Quantity - returnQty;
                invoiceProduct.TotalPrice = invoiceProduct.TotalPrice - returnAmount;
                cx.Entry(invoiceProduct).State = EntityState.Modified;
                cx.SaveChanges("");
            } 
        }
    }
        #endregion
        #region dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
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