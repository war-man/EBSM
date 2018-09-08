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

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

        // GET: /MUnit/
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult Index(SupplierSearchViewModel model)
        {
            var suppliers = db.Suppliers.Where(x => (model.SupplierName == null || x.SupplierName.StartsWith(model.SupplierName))).OrderBy(x => x.SupplierName).ToList();
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
                 supplier.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                supplier.CreatedDate = DateTime.Now;
                db.Suppliers.Add(supplier);

                 db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
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
            Supplier supplier = db.Suppliers.Find(id);
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
                Supplier oldSupplier = db.Suppliers.Find(supplier.SupplierId);
                oldSupplier.SupplierName = supplier.SupplierName;
                oldSupplier.SupplierType = supplier.SupplierType;
                oldSupplier.SupplierAddress = supplier.SupplierAddress;
                oldSupplier.ContactPersonName = supplier.ContactPersonName;
                oldSupplier.ContactNo = supplier.ContactNo;
                oldSupplier.Email = supplier.Email;
                oldSupplier.Position = supplier.Position;
                oldSupplier.Status = supplier.Status;
                oldSupplier.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                supplier.UpdatedDate = DateTime.Now;
                db.Entry(oldSupplier).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("index");
            }
            return View("../Shop/Supplier/Edit", supplier);
        }

        public void UpdateSupplierBalance(int? supplierId, double? remaining,int? currentUserId)
        {
            Supplier supplier = db.Suppliers.FirstOrDefault(x => x.SupplierId == supplierId);
            if (supplier != null)
            {
                supplier.Balance = (supplier.Balance ?? 0) + remaining;
                supplier.UpdatedBy = currentUserId;
                supplier.UpdatedDate = DateTime.Now;
                db.Entry(supplier).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges(currentUserId.ToString());
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
                supplier.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                supplier.CreatedDate = DateTime.Now;
                db.Suppliers.Add(supplier);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                inserted = true;
            }
            var data = new { inserted = inserted, supplier = supplier };
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