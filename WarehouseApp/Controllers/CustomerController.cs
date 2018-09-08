
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

        // GET: /Customer/
        [Roles("Global_SupAdmin,Customer_View")]
        [OutputCache(Duration = 10)]
        public ActionResult Index(CustomerSearchViewModel model)
        {

            var customers = db.Customers.Where(x => (model.CustomerName == null || x.FullName.StartsWith(model.CustomerName)) && (model.ContactNo == null || x.ContactNo.StartsWith(model.ContactNo))).OrderBy(x => x.FullName).ToList();
            model.Customers = customers.ToPagedList(model.Page, model.PageSize);
            return View( model);
        } 
        // GET: /Customer/Details/5
        [Roles("Global_SupAdmin,Customer_View")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            CustomerDetailsViewModel customerDetails = new CustomerDetailsViewModel();
            customerDetails.Customer = customer;
            var customerBalance = customer.PreviousBalance ?? 0;
            var grandTotal = customerBalance + customer.Invoices.Sum(x => (x.TotalPrice - BankAccountController.DiscountCalculator(x.DiscountAmount, x.DiscountType, x.TotalPrice).DiscountValue) + x.TotalVat);
            ViewBag.TotalDue = grandTotal - customer.Invoices.Sum(x => x.PaidAmount);

            ViewBag.PaymentMessage = "";
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");
            return View(customerDetails);
        }
        // GET: /Customer/Create
        [Roles("Global_SupAdmin,Customer_View")]
        public ActionResult Create()
        {
            return View("../Customer/Create");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreatedDate = DateTime.Now;
                customer.Status = 1;
                db.Customers.Add(customer);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }

            return View("../Customer/Create", customer);
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public JsonResult AddNewCustomer(Customer customer)
        {
            bool inserted = false;
            if (ModelState.IsValid)
            {
                customer.Status = 1;
                customer.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                customer.CreatedDate = DateTime.Now;
                db.Customers.Add(customer);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                inserted = true;
            }
            var data = new { inserted = inserted, customer = customer };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        // GET: /Customer/Edit/5

        [Roles("Global_SupAdmin,Customer_View")]
        public ActionResult EditCustomerInfo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View("../Customer/EditCustomerInfo", customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomerInfo(Customer customer)
        {
            if (ModelState.IsValid)
            {
                Customer oldCustomer = db.Customers.Find(customer.CustomerId);
                oldCustomer.FullName = customer.FullName;
                oldCustomer.Address = customer.Address;
                oldCustomer.ContactNo = customer.ContactNo;
                oldCustomer.Email = customer.Email;
                oldCustomer.UpdatedDate = DateTime.Now;
                oldCustomer.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                db.Entry(oldCustomer).State = EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Details", new { id = customer.CustomerId });
            }
            return View("../Customer/EditCustomerInfo", customer);
        }
        [Roles("Global_SupAdmin,Customer_View")]
        public ActionResult CreateCustomerProject(int customerId)
        {

            ViewBag.Customer = db.Customers.Find(customerId);
            var customerBranch = new CustomerProject
            {
                CustomerId = customerId,
            };
            return View("../Customer/CreateProject", customerBranch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomerProject(CustomerProject customerProject)
        {
            if (ModelState.IsValid)
            {
                customerProject.Status = 1;
                customerProject.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                customerProject.CreatedDate = DateTime.Now;
                db.CustomerProjects.Add(customerProject);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Details", "Customer", new { id = customerProject .CustomerId});
            }

            return View("../Customer/CreateProject", customerProject);
        } 
        
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public JsonResult AddCustomerBranch(CustomerProject customerBranch)
        {
            bool inserted = false;
            if (ModelState.IsValid)
            {
                customerBranch.Status = 1;
                customerBranch.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                customerBranch.CreatedDate = DateTime.Now;
                db.CustomerProjects.Add(customerBranch);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                inserted = true;
            }
            var data = new { inserted = inserted, customerBranch = customerBranch };
            return Json(data, JsonRequestBehavior.AllowGet);
           
        }
        [Roles("Global_SupAdmin,Customer_View")]
        public ActionResult EditCustomerProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerProject customerProject = db.CustomerProjects.Find(id);
            if (customerProject == null)
            {
                return HttpNotFound();
            }
            return View("../Customer/EditCustomerProject", customerProject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomerProject(CustomerProject customerProject)
        {
            if (ModelState.IsValid)
            {
                CustomerProject oldCustomerProject = db.CustomerProjects.Find(customerProject.CustomerProjectId);
                oldCustomerProject.ProjectName = customerProject.ProjectName;
                oldCustomerProject.ProjectAddress = customerProject.ProjectAddress;
                oldCustomerProject.ProjectPhoneNo = customerProject.ProjectPhoneNo;
                oldCustomerProject.ProjectDescription = customerProject.ProjectDescription;
                oldCustomerProject.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                db.Entry(oldCustomerProject).State = EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Details", new { id = oldCustomerProject.CustomerId });
            }
            return View("../Customer/EditCustomerProject", customerProject);
        }
        public JsonResult GetCustomerByContNum(string contNum)
        {
            bool hasValue = true;
            var customer = db.Customers.Where(c => c.ContactNo == contNum).Select(c => new { c.CustomerId, c.FullName, c.ContactNo, c.Address, c.Email, c.Age, c.Gender, c.Status });
            if (!customer.Any())
            {
                hasValue = false;
            }

            var data = new { hasValue = hasValue, customer = customer };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerInfoById(int? CustomerId)
        {
            bool hasValue = true;
            var customer = db.Customers.Where(c => c.CustomerId == CustomerId).Select(c => new { c.CustomerId, c.FullName, c.ContactNo, c.Address, c.Email, c.Age, c.Gender, c.Status });

            if (!customer.Any())
            {
                hasValue = false;
            }
            var data = new { hasValue = hasValue, customer = customer };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerProjectInfoLoadById(int? CustomerProjectId)
        {
            bool hasValue = true;
            var customerProjects = db.CustomerProjects.Where(c => c.CustomerProjectId == CustomerProjectId).Select(c => new { c.CustomerId, c.ProjectName, c.ProjectAddress, c.ProjectPhoneNo, c.ProjectDescription });

            if (!customerProjects.Any())
            {
                hasValue = false;
            }
            var data = new { hasValue = hasValue, customerProjects = customerProjects };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerList(string term)
        {
            var customers = db.Customers.Where(p => (p.FullName.StartsWith(term) || p.FullName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.FullName).Select(x => new { CustomerId = x.CustomerId, CustomerName = x.FullName, CustomerContactNo = x.ContactNo, CustomerEmail = x.Email, CustomerAddress = x.Address }).ToList();
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerProjects(int? cid, string term)
        {
            var customerProjects = db.CustomerProjects.Where(p => p.CustomerId == cid && (p.ProjectName.StartsWith(term) || p.ProjectName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.ProjectName).Select(x => new { CustomerProjectId = x.CustomerProjectId, ProjectName = x.ProjectName, ProjectAddress = x.ProjectAddress, ProjectPhoneNo = x.ProjectPhoneNo, ProjectDescription = x.ProjectDescription }).ToList();
            return Json(customerProjects, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerProjectsByCustomerId(int? cid)
        {
            StringBuilder selectListString = new StringBuilder();

            var customerProjects = db.CustomerProjects.Where(p => p.CustomerId == cid && p.Status != 0).OrderBy(p => p.ProjectName).ToList();
            selectListString.Append("<option value=''>--Select--</option>");
            foreach (var item in customerProjects)
            {
                selectListString.Append("<option value='" + item.CustomerProjectId + "'>" + item.ProjectName + "</option>");
            }
            var data = new { selectListString = selectListString.ToString() };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//