
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
using EBSM.Entities;
using EBSM.Services;
namespace WarehouseApp.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private CustomerService _customerService= new CustomerService();
  

        // GET: /Customer/
        [Roles("Global_SupAdmin,Customer_View")]
        [OutputCache(Duration = 10)]
        public ActionResult Index(CustomerSearchViewModel model)
        {
            var customers = _customerService.GetAllCustomers(model.CustomerName,model.ContactNo).ToList();
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
            Customer customer = _customerService.GetCustomerById(id.Value);
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
                customer.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                customer.Status = 1;
                _customerService.Save(customer, AuthenticatedUser.GetUserFromIdentity().UserId);
  
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
                customer.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                customer.CreatedDate = DateTime.Now;
                _customerService.Save(customer, AuthenticatedUser.GetUserFromIdentity().UserId);
               
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
            Customer customer = _customerService.GetCustomerById(id.Value);
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
                Customer oldCustomer = _customerService.GetCustomerById(customer.CustomerId);
                oldCustomer.FullName = customer.FullName;
                oldCustomer.Address = customer.Address;
                oldCustomer.ContactNo = customer.ContactNo;
                oldCustomer.Email = customer.Email;
                oldCustomer.UpdatedDate = DateTime.Now;
                oldCustomer.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                _customerService.Edit(customer, AuthenticatedUser.GetUserFromIdentity().UserId);
               
                return RedirectToAction("Details", new { id = customer.CustomerId });
            }
            return View("../Customer/EditCustomerInfo", customer);
        }
        [Roles("Global_SupAdmin,Customer_View")]
        public ActionResult CreateCustomerProject(int customerId)
        {

            ViewBag.Customer = _customerService.GetCustomerById(customerId);
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
                customerProject.CreatedBy =AuthenticatedUser.GetUserFromIdentity().UserId;
                customerProject.CreatedDate = DateTime.Now;
                _customerService.SaveProject(customerProject, AuthenticatedUser.GetUserFromIdentity().UserId);
              
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
                customerBranch.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                customerBranch.CreatedDate = DateTime.Now;
                _customerService.SaveProject(customerBranch, AuthenticatedUser.GetUserFromIdentity().UserId);
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
            CustomerProject customerProject = _customerService.GetCustomerProjectById(id.Value);
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
                CustomerProject oldCustomerProject = _customerService.GetCustomerProjectById(customerProject.CustomerProjectId);
                oldCustomerProject.ProjectName = customerProject.ProjectName;
                oldCustomerProject.ProjectAddress = customerProject.ProjectAddress;
                oldCustomerProject.ProjectPhoneNo = customerProject.ProjectPhoneNo;
                oldCustomerProject.ProjectDescription = customerProject.ProjectDescription;
                oldCustomerProject.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                _customerService.EditProject(oldCustomerProject, AuthenticatedUser.GetUserFromIdentity().UserId);
              
                return RedirectToAction("Details", new { id = oldCustomerProject.CustomerId });
            }
            return View("../Customer/EditCustomerProject", customerProject);
        }
        public JsonResult GetCustomerByContNum(string contNum)
        {
            bool hasValue = true;
            string customerName = string.Empty;
            var customer =_customerService.GetAllCustomers(customerName,contNum).Select(c => new { c.CustomerId, c.FullName, c.ContactNo, c.Address, c.Email, c.Age, c.Gender, c.Status });
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
       
            var customer = _customerService.GetAllCustomers().Where(c => c.CustomerId == CustomerId).Select(c => new { c.CustomerId, c.FullName, c.ContactNo, c.Address, c.Email, c.Age, c.Gender, c.Status });

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
            var customerProjects = _customerService.GetAllCustomerProjects().Where(c => c.CustomerProjectId == CustomerProjectId).Select(c => new { c.CustomerId, c.ProjectName, c.ProjectAddress, c.ProjectPhoneNo, c.ProjectDescription });

            if (!customerProjects.Any())
            {
                hasValue = false;
            }
            var data = new { hasValue = hasValue, customerProjects = customerProjects };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerList(string term)
        {
            var customers = _customerService.GetAllCustomersByName(term).Select(x => new { CustomerId = x.CustomerId, CustomerName = x.FullName, CustomerContactNo = x.ContactNo, CustomerEmail = x.Email, CustomerAddress = x.Address }).ToList();
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerProjects(int? cid, string term)
        {
            var customerProjects = _customerService.GetAllCustomerProjects(cid, term).Select(x => new { CustomerProjectId = x.CustomerProjectId, ProjectName = x.ProjectName, ProjectAddress = x.ProjectAddress, ProjectPhoneNo = x.ProjectPhoneNo, ProjectDescription = x.ProjectDescription }).ToList();
            return Json(customerProjects, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerProjectsByCustomerId(int? cid)
        {
            StringBuilder selectListString = new StringBuilder();

            var customerProjects = _customerService.GetAllCustomerProjectsByCustomerId(cid).ToList();
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
                _customerService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//