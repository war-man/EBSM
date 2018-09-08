using System;
using System.Collections.Generic;

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
    public class WarehouseZoneController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

        #region warehouse zone list view

        [Roles("Global_SupAdmin,Configuration,Warehouse_Zone_Creation")]
        public ActionResult Index(ZoneSearchViewModel model)
        {

            var warehouseZones = db.WarehouseZones.Where(x => (model.ZoneName == null || x.ZoneName.StartsWith(model.ZoneName))).OrderBy(x => x.ZoneName).ToList();
            model.WarehouseZones = warehouseZones.ToPagedList(model.Page, model.PageSize);
            ViewBag.WarehouseId = new SelectList(db.Warehouses.Where(x => x.Status != 0).OrderBy(x => x.WarehouseName), "WarehouseId", "WarehouseName", db.Warehouses.FirstOrDefault(y=>y.IsDefault).WarehouseId);

            return View("../Shop/WarehouseZone/Index", model);
        }
        #endregion
        #region create warehouse zone
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WarehouseZone zone)
        {
            if (ModelState.IsValid)
            {
               
                zone.Status = 1;
                zone.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                zone.CreatedDate = DateTime.Now;
                db.WarehouseZones.Add(zone);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }
            ViewBag.WarehouseId = new SelectList(db.Warehouses.Where(x => x.Status != 0).OrderBy(x => x.WarehouseName), "WarehouseId", "WarehouseName", db.Warehouses.FirstOrDefault(y => y.IsDefault).WarehouseId);
            return View("../Shop/WarehouseZone/Create", zone);
        }
        #endregion
        #region edit warehouse zone
        [Roles("Global_SupAdmin,Configuration,Warehouse_Zone_Creation")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarehouseZone warehouseZone = db.WarehouseZones.Find(id);
            if (warehouseZone == null)
            {
                return HttpNotFound();
            }

            return View("../Shop/WarehouseZone/Edit", warehouseZone);
        }

 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WarehouseZone warehouseZone)
        {
            if (ModelState.IsValid)
            {
                WarehouseZone zone = db.WarehouseZones.Find(warehouseZone.ZoneId);
                zone.ZoneName = warehouseZone.ZoneName;
                zone.Status = warehouseZone.Status;
                zone.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                zone.UpdatedDate = DateTime.Now;
                db.Entry(zone).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }

            return View("../Shop/WarehouseZone/Edit", warehouseZone);
        }
        #endregion
        #region helper modules
        public JsonResult IsNameUsed(string ZoneName, string InitialZoneName)
        {
            bool isNotExist = true;
            if (ZoneName != string.Empty && InitialZoneName == "undefined")
            {
                var isExist = db.WarehouseZones.Any(x => x.ZoneName.ToLower().Equals(ZoneName.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (ZoneName != string.Empty && InitialZoneName != "undefined")
            {
                var isExist = db.WarehouseZones.Any(x => x.ZoneName.ToLower() == ZoneName.ToLower() && x.ZoneName.ToLower() != InitialZoneName.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZoneSelectListByProductId(int? productId)
        {
            StringBuilder selectListString = new StringBuilder();

            var allZone = db.WarehouseZones.ToList();
            int productDefaultZoneId = 0;
            if (productId != null)
            {
                var product = db.Products.FirstOrDefault(x => x.ProductId == productId);
                if (product != null)
                {
                    productDefaultZoneId = Convert.ToInt32(product.DefaultZoneId);
                }
            }
           
            foreach (var item in allZone)
            {
                string optionString = item.ZoneId == productDefaultZoneId
                    ? "<option selected='selected' value='" + item.ZoneId + "'>" + item.ZoneName + "</option>"
                    : "<option  value='" + item.ZoneId + "'>" + item.ZoneName + "</option>";
                selectListString.Append(optionString);
                }

                var data = new { selectListString = selectListString.ToString() };
                return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetZoneSelectListByStockId(int? stockId)
        {
            StringBuilder selectListString = new StringBuilder();

            var allZone = db.StockWarehouseRelations.Where(x => (x.StockId == stockId)&&(x.Quantity!=0)).Select(x=>x.WarehouseZone).ToList();
            //int productDefaultZoneId = 0;
            //if (productId != null)
            //{
            //    productDefaultZoneId = Convert.ToInt32(db.Products.FirstOrDefault(x => x.ProductId == productId).DefaultZoneId);
            //}
            foreach (var item in allZone)
            {
                //string optionString = item.ZoneId == productDefaultZoneId
                //    ? "<option selected='selected' value='" + item.ZoneId + "'>" + item.ZoneName + "</option>"
                //    : "<option  value='" + item.ZoneId + "'>" + item.ZoneName + "</option>"; 
                string optionString ="<option  value='" + item.ZoneId + "'>" + item.ZoneName + "</option>";
                selectListString.Append(optionString);
            }

            var data = new { selectListString = selectListString.ToString() };
            return Json(data, JsonRequestBehavior.AllowGet);
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