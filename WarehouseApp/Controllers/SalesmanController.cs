using System;
using System.Collections.Generic;

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
    public class SalesmanController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

        // GET: /Salesman/
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult Index(SalsemanSearchViewModel model)
        {

            var salesmanList = db.Salesman.Where(x => (model.SalesmanName == null || x.FullName.StartsWith(model.SalesmanName))).OrderBy(x => x.FullName).ToList();
            model.SalesmanList = salesmanList.ToPagedList(model.Page, model.PageSize);
            return View("../Shop/Salesman/Index", model);
        } 
        public ActionResult LoadAllSalesman()
        {
            var salesmanList = db.Salesman.OrderBy(s=>s.FullName).ToList();
            return Json(new{data = salesmanList}, JsonRequestBehavior.AllowGet);
        }

        
        // GET: /Salesman/Create
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult Create()
        {
            return View("../Shop/Salesman/Create");
        }

        // POST: /Salesman/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Salesman salesman)
        {
            if (ModelState.IsValid)
            {
                salesman.CreatedDate = DateTime.Now;
                salesman.Status = 1;
                db.Salesman.Add(salesman);
                 db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }

            return View("../Shop/Salesman/Create",salesman);
        }

        // GET: /Salesman/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salesman salesman = db.Salesman.Find(id);
            if (salesman == null)
            {
                return HttpNotFound();
            }
            return View("../Shop/Salesman/Edit",salesman);
        }

        // GET: /Salesman/Edit/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salesman salesman = db.Salesman.Find(id);
            if (salesman == null)
            {
                return HttpNotFound();
            }
            return View("../Shop/Salesman/Details", salesman);
        }

        // POST: /Salesman/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Salesman salesman)
        {
            if (ModelState.IsValid)
            {
                salesman.UpdatedDate = DateTime.Now;
                db.Entry(salesman).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("../Shop/Salesman/Edit", salesman);
        }

        // GET: /DailyFxRate/Delete/5
        public ActionResult DeleteRecord(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            Salesman salesman = db.Salesman.Find(id);
            if (salesman == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }
            db.Salesman.Remove(salesman);
             db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
            return Json(new { Result = "OK" });
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