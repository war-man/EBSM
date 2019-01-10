using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;
using EBSM.Services;
using EBSM.Entities;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class BillController : Controller
    {
        private BillService _billService = new BillService();
        private CustomerService _customerService = new CustomerService();
        private SalesService _salesService = new SalesService();
        private const int InitialBillNo = 1;
        #region Bill list view
        [OutputCache(Duration = 30)]
        public ActionResult Index(BillSearchViewModel model)
        {

            var fromDate = Convert.ToDateTime(model.BillDateFrom);
            var toDate = Convert.ToDateTime(model.BillDateTo);
            var bills = _billService.GetAll(model.BillNo, model.BillDateFrom, model.BillDateTo, model.Customer);
            model.Bills = bills.ToPagedList(model.Page, model.PageSize);

            ViewBag.CustomerDrpDownList = new SelectList(_customerService.GetAllCustomers(), "CustomerId", "FullName");
            return View("../Shop/Bill/Index", model);
       
        }
        #endregion
        #region bill creation
        public ActionResult NewBill(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = _customerService.GetCustomerById(id.Value); 
            if (customer == null)
            {
                return HttpNotFound();
            }
           // var invoicesWithoutBill =_salesService.GetInvoicesWithoutAnyBill(customer.CustomerId).ToList();
            ViewBag.InvoiceWithoutBill = _salesService.GetInvoicesWithoutAnyBill(customer.CustomerId).ToList();
            ViewBag.Customer = customer;
           
            return View("../Shop/Bill/NewBill");
        }

        [HttpPost]
        public ActionResult NewBill(BillInvoicesViewModel billInvoices)
        {
            if (ModelState.IsValid)
            {
               
                Bill bill = new Bill();

                bill.BillDate = Convert.ToDateTime(billInvoices.BillDate);
                bill.BillNo = "B/" + (InitialBillNo + _billService.GetCount()) +"/"+ DateTime.Now.ToString("yy");
                bill.BillAmount = billInvoices.Bill.BillAmount;
                bill.CustomerId = billInvoices.Bill.CustomerId;
                bill.Status = 1;
                bill.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                bill.CreatedDate = DateTime.Now;

                List<InvoiceBill> invoiceBillRelations=new List<InvoiceBill>();
                foreach (var item in billInvoices.InvoiceCheckboxes.Where(x=>x.IsChecked))
                {
                    InvoiceBill invoiceBillRelation=new InvoiceBill()
                    {
                        InvoiceId = item.InvoiceId,
                        CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId,
                        CreatedDate = DateTime.Now
                    };

                    invoiceBillRelations.Add(invoiceBillRelation);
                }
                bill.InvoiceBills = invoiceBillRelations;
                _billService.Save(bill, AuthenticatedUser.GetUserFromIdentity().UserId);

                 return RedirectToAction("BillDetails", "Bill", new { id = bill .BillId});
            }


            ViewBag.InvoiceWithoutBill = _salesService.GetInvoicesWithoutAnyBill(billInvoices.Bill.Customer.CustomerId).ToList();
            ViewBag.Customer = _customerService.GetCustomerById(billInvoices.Bill.Customer.CustomerId);
            return View("../Shop/Bill/NewBill");
        }
        #endregion
        #region billl edit
        [Roles("Global_SupAdmin,Bill_Edit")]
 public ActionResult EditBill(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = _billService.GetBillById(id.Value);
            if (bill == null)
            {
                return HttpNotFound();
            }

            BillInvoicesViewModel billInvoices = new BillInvoicesViewModel();
     billInvoices.Bill = bill;
     billInvoices.BillDate = string.Format("{0:dd-MM-yyyy}",bill.BillDate);
     List<InvoiceCheckboxViewModel>checkboxes=new List<InvoiceCheckboxViewModel>();
    
     List<InvoiceBill> invoicesWithBill = bill.InvoiceBills.ToList();
     
     if (invoicesWithBill.Any()) {
         foreach (var item in invoicesWithBill)
     {
         InvoiceCheckboxViewModel invoiceCheckbox = new InvoiceCheckboxViewModel()
         {
             InvoiceId = item.InvoiceId,
             Invoice = item.Invoice,
             IsChecked = true,
         };
         checkboxes.Add(invoiceCheckbox);
     }
     }
     List<Invoice> invoicesWithoutBill = db.Invoices.Where(x => x.CustomerId == bill.CustomerId && !x.InvoiceBills.Any()).ToList();
            if (invoicesWithoutBill.Any())
            {
                foreach (var item in invoicesWithoutBill)
                {
                    InvoiceCheckboxViewModel invoiceCheckbox = new InvoiceCheckboxViewModel()
                    {
                        InvoiceId = item.InvoiceId,
                        Invoice = item
                    };
                    checkboxes.Add(invoiceCheckbox);
                   
                }
            }
            billInvoices.InvoiceCheckboxes = checkboxes;
            return View("../Shop/Bill/EditBill", billInvoices);
        }
  

        [HttpPost]
 public ActionResult EditBill(BillInvoicesViewModel billInvoices)
        {
            if (ModelState.IsValid)
            {
                Bill bill = db.Bills.Find(billInvoices.Bill.BillId);
                //remove================================================
                List<InvoiceBill> removeInvoiceBills = bill.InvoiceBills.ToList();
                foreach (var removeItem in removeInvoiceBills)
                {
                    db.InvoiceBills.Remove(removeItem);
                }
                db.SaveChanges("");

                bill.BillDate = Convert.ToDateTime(billInvoices.BillDate);
                bill.BillAmount = billInvoices.Bill.BillAmount;
                bill.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                bill.UpdatedDate = DateTime.Now;

                
                //addd==============================================
                List<InvoiceBill> invoiceBillRelations=new List<InvoiceBill>();
                foreach (var item in billInvoices.InvoiceCheckboxes.Where(x=>x.IsChecked))
                {
                    InvoiceBill invoiceBillRelation=new InvoiceBill()
                    {
                        InvoiceId = item.InvoiceId,
                        CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                        CreatedDate = DateTime.Now
                    };

                    invoiceBillRelations.Add(invoiceBillRelation);
                }
                bill.InvoiceBills = invoiceBillRelations;

                db.Entry(bill);
                db.SaveChanges("");
                 return RedirectToAction("Index", "Bill");
            }
           
    
            ViewBag.InvoiceWithoutBill = db.Invoices.Where(x => x.CustomerId == billInvoices.Bill.Customer.CustomerId && !x.InvoiceBills.Any()).ToList();
            ViewBag.Customer = db.Customers.FirstOrDefault(x => x.CustomerId == billInvoices.Bill.Customer.CustomerId);
            return View("../Shop/Bill/EditBill", billInvoices);
        }
        #endregion
        #region customer bill payment
        [HttpPost]
        public ActionResult CustomerBillPayment(CustomerDetailsViewModel customerDetails)
        {
            Customer customer = db.Customers.FirstOrDefault(x => x.CustomerId == customerDetails.CustomerPayment.CustomerId);
            customerDetails.Customer = customer;
            double customerPaid = customerDetails.CustomerPayment.PaidAmount;
            if (ModelState.IsValid)
            {
                List<Invoice> dueInvoices = customer.Invoices.Where(x => ((x.TotalPrice - BankAccountController.DiscountCalculator(x.DiscountAmount, x.DiscountType, x.TotalPrice).DiscountValue) + x.TotalVat) > x.PaidAmount).OrderBy(x => x.InvoiceId).ToList();
                List<Invoice> paidInvoices =customer.Invoices.Where(x =>x.PaidAmount>0).OrderByDescending(x=>x.InvoiceId).ToList();
                var customerBalance = customer.PreviousBalance??0;
                if (customerBalance > 0 && customerPaid > 0)
                {
                    if (customerPaid > customerBalance)
                    {
                        customer.PreviousBalance -= customerBalance;
                        customerPaid -= customerBalance;
                    }
                    else
                    {
                        customer.PreviousBalance -= customerPaid;
                        customerPaid -= customerPaid;
                    }
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                }
                if (customerPaid > 0 && dueInvoices.Any())
                {

                    foreach (var dueInvoice in dueInvoices)
                    {
                        if (dueInvoice != null && customerPaid > 0)
                        {
                            var gTotal = (dueInvoice.TotalPrice -
                                          BankAccountController.DiscountCalculator(dueInvoice.DiscountAmount,
                                              dueInvoice.DiscountType, dueInvoice.TotalPrice).DiscountValue) +
                                         dueInvoice.TotalVat;
                            var totalDue = gTotal - dueInvoice.PaidAmount;
                            if (customerPaid > totalDue)
                            {
                                dueInvoice.PaidAmount += totalDue;
                                customerPaid -= Convert.ToDouble(totalDue);
                            }
                            else
                            {
                                dueInvoice.PaidAmount += customerPaid;
                                customerPaid -= customerPaid;
                            }
                            db.Entry(dueInvoice).State = EntityState.Modified;
                            db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                        }
                    }
                }
                if (customerPaid < 0 && paidInvoices.Any())
                {
                    double adjAmount = Math.Abs(customerPaid);
                      foreach (var paidInvoice in paidInvoices)
                    {
                        if (paidInvoice != null && adjAmount > 0)
                        {
                        if (adjAmount < paidInvoice.PaidAmount)
                        {
                            paidInvoice.PaidAmount -= adjAmount;
                            adjAmount -= adjAmount;
                        }
                        else
                        {
                            paidInvoice.PaidAmount -= paidInvoice.PaidAmount;
                            adjAmount -= Convert.ToDouble(paidInvoice.PaidAmount);
                        }
                        db.Entry(paidInvoice).State = EntityState.Modified;
                        db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                    }
                } } 
                //Transaction occurance===============================
                TransactionController transaction = new TransactionController();
                transaction.DepositToAccount(customerDetails.CustomerPayment.TransactionMode, customerDetails.CustomerPayment.TransactionModeId, customerDetails.CustomerPayment.PaidAmount, Convert.ToDateTime(customerDetails.CustomerPayment.PaymentDate), "Customers", "CustomerId", customerDetails.CustomerPayment.CustomerId, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),customerDetails.CustomerPayment.Note);
               
                ViewBag.PaymentMessage = "Payment successfully done!";
                ViewBag.CustomerId = customer.CustomerId;
                return View("../Customer/PaymentSuccess");
            }

            ViewBag.PaymentMessage = "Error";
            var grandTotal = customer.Invoices.Sum(x => (x.TotalPrice - BankAccountController.DiscountCalculator(x.DiscountAmount, x.DiscountType, x.TotalPrice).DiscountValue) + x.TotalVat);
            ViewBag.TotalDue = grandTotal - customer.Invoices.Sum(x => x.PaidAmount);

            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");
            return View("../Customer/Details", customerDetails);
        }
        #endregion
        #region bill details
        public ActionResult BillDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            StringBuilder refChalans=new StringBuilder("");
            StringBuilder req=new StringBuilder("");
            StringBuilder po=new StringBuilder("");
            if (bill.InvoiceBills.Any())
            {
                foreach (var chalan in bill.InvoiceBills)
                {
                    refChalans.Append(chalan.Invoice.InvoiceNumber);
                    refChalans.Append(", Dt: ");
                    refChalans.Append(chalan.Invoice.InvoiceDate.ToString("dd-MM-yyyy"));
                    refChalans.Append(", ");
                }

                foreach (var chalan in bill.InvoiceBills.GroupBy(x => x.Invoice.WorkOrderNumber))
                {
                    if (!string.IsNullOrEmpty(chalan.First().Invoice.WorkOrderNumber))
                    {
                        po.Append(chalan.First().Invoice.WorkOrderNumber);
                        if (!string.IsNullOrEmpty(chalan.First().Invoice.WorkOrderDate.ToString()))
                        {
                            po.Append(", Dt: ");
                            po.Append(Convert.ToDateTime(chalan.First().Invoice.WorkOrderDate).ToString("dd-MM-yyyy"));
                        }

                        po.Append(", ");
                    }
                }
            }
            
            ViewBag.CompanyInfo = db.CompanyProfiles.FirstOrDefault();
            ViewBag.RefChalans = refChalans;
            ViewBag.ReqNo = req;
            ViewBag.PoNo = po;
            var grandTotal = bill.InvoiceBills.Sum(x => (x.Invoice.TotalPrice - BankAccountController.DiscountCalculator(x.Invoice.DiscountAmount, x.Invoice.DiscountType, x.Invoice.TotalPrice).DiscountValue) + x.Invoice.TotalVat);
            ViewBag.BillAmount = grandTotal;
            ViewBag.BillAmountInWord = SettingsController.NumberToCurrencyTextBdt(Convert.ToDouble(grandTotal));
            return View("../Shop/Bill/BillDetails", bill);
        }
        #endregion 
        #region helper module
        public int SaveInvoiceBill(Invoice invoice, int currentUserId)
        {
            Bill bill = new Bill();

            bill.BillDate = Convert.ToDateTime(invoice.InvoiceDate);
            bill.BillNo = "B/" + (BillController.InitialBillNo + db.Bills.Count()) + "/" +
                          DateTime.Now.ToString("yy");
            bill.BillAmount = Convert.ToDouble(invoice.TotalPrice);
            bill.BillAmount =
                Convert.ToDouble((invoice.TotalPrice -
                                  BankAccountController.DiscountCalculator(invoice.DiscountAmount, invoice.DiscountType,
                                      invoice.TotalPrice).DiscountValue) + invoice.TotalVat);
            bill.CustomerId = Convert.ToInt32(invoice.CustomerId);
            bill.Status = 1;
            bill.CreatedBy = currentUserId;
            bill.CreatedDate = DateTime.Now;

            List<InvoiceBill> invoiceBillRelations = new List<InvoiceBill>();

            InvoiceBill invoiceBillRelation = new InvoiceBill()
            {
                InvoiceId = invoice.InvoiceId,
                CreatedBy = currentUserId,
                CreatedDate = DateTime.Now
            };

            invoiceBillRelations.Add(invoiceBillRelation);

            bill.InvoiceBills = invoiceBillRelations;
            db.Bills.Add(bill);
            db.SaveChanges(currentUserId.ToString());
            return bill.BillId;
        }
        #endregion
        #region di*spose
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
//Creation Date : May 2017
//=======================================================================================//