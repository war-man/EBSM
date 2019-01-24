using System;

using System.Data.Entity;
using System.Linq;
using System.Net;
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
    public class SupplierController : Controller
    {
        private SupplierService _supplierService = new SupplierService();

        // GET: /MUnit/
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult Index(SupplierSearchViewModel model)
        {
            var suppliers = _supplierService.GetAllSuppliers(model.SupplierName).ToList();
            model.Suppliers = suppliers.ToPagedList(model.Page, model.PageSize);
            return View("../Shop/Supplier/index", model);
        }
        // create of supplier   
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult Create()
        {
            return View("../Shop/Supplier/Create");
        }



        // save bank account 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.Balance = supplier.Balance??0;
                supplier.Status = 1;
                 supplier.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                supplier.CreatedDate = DateTime.Now;
                _supplierService.Save(supplier, AuthenticatedUser.GetUserFromIdentity().UserId);
               
                return RedirectToAction("index");
            }

            return View("../Shop/Supplier/Create");
        }

        // GET: /Product/EditBankAccount/5
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier =_supplierService.GetSupplierById(id.Value);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View("../Shop/Supplier/Edit", supplier);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                Supplier oldSupplier = _supplierService.GetSupplierById(supplier.SupplierId);
                oldSupplier.SupplierName = supplier.SupplierName;
                oldSupplier.SupplierType = supplier.SupplierType;
                oldSupplier.SupplierAddress = supplier.SupplierAddress;
                oldSupplier.ContactPersonName = supplier.ContactPersonName;
                oldSupplier.ContactNo = supplier.ContactNo;
                oldSupplier.Email = supplier.Email;
                oldSupplier.Position = supplier.Position;
                oldSupplier.Status = supplier.Status;
                oldSupplier.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                supplier.UpdatedDate = DateTime.Now;
                _supplierService.Edit(oldSupplier, AuthenticatedUser.GetUserFromIdentity().UserId);
               
                return RedirectToAction("index");
            }
            return View("../Shop/Supplier/Edit", supplier);
        }

        public void UpdateSupplierBalance(int? supplierId, double? remaining,int? currentUserId)
        {
            Supplier supplier = _supplierService.GetSupplierById(supplierId.Value);
            if (supplier != null)
            {
                supplier.Balance = (supplier.Balance ?? 0) + remaining;
                supplier.UpdatedBy = currentUserId;
                supplier.UpdatedDate = DateTime.Now;
                _supplierService.Edit(supplier, currentUserId);
            }
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult AddNewSupplier(Supplier supplier)
        {
            bool inserted = false;
            if (ModelState.IsValid)
            {
                supplier.SupplierType = supplier.SupplierType??3;
                supplier.Status = supplier.Status??1;
                supplier.Balance = supplier.Balance ?? 0;
                supplier.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                supplier.CreatedDate = DateTime.Now;
                supplier.SupplierId= _supplierService.Save(supplier, AuthenticatedUser.GetUserFromIdentity().UserId);
             
                inserted = true;
            }
            var data = new { inserted = inserted, supplier = supplier };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _supplierService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//