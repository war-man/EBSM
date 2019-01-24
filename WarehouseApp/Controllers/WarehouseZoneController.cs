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
using EBSM.Entities;
using EBSM.Services;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class WarehouseZoneController : Controller
    {
        private WarehouseZoneService _warehouseZoneService = new WarehouseZoneService();
        private ProductService _productService = new ProductService();
        private StockService _stockService = new StockService();

        #region warehouse zone list view

        [Roles("Global_SupAdmin,Configuration,Warehouse_Zone_Creation")]
        public ActionResult Index(ZoneSearchViewModel model)
        {

            var warehouseZones =_warehouseZoneService.GetAllWarehouseZone(model.ZoneName).ToList();
            model.WarehouseZones = warehouseZones.ToPagedList(model.Page, model.PageSize);
            ViewBag.WarehouseId = new SelectList(_warehouseZoneService.GetAllWarehouses(), "WarehouseId", "WarehouseName", _warehouseZoneService.GetWarehouseByName("default warehouse").WarehouseId);

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
                zone.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                zone.CreatedDate = DateTime.Now;
                _warehouseZoneService.Save(zone,AuthenticatedUser.GetUserFromIdentity().UserId);
               
                return RedirectToAction("Index");
            }
            ViewBag.WarehouseId = new SelectList(_warehouseZoneService.GetAllWarehouses(), "WarehouseId", "WarehouseName", _warehouseZoneService.GetWarehouseByName("default warehouse").WarehouseId);
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
            WarehouseZone warehouseZone =_warehouseZoneService.GetWarehouseZoneById(id.Value);
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
                WarehouseZone zone = _warehouseZoneService.GetWarehouseZoneById(warehouseZone.ZoneId);
                zone.ZoneName = warehouseZone.ZoneName;
                zone.Status = warehouseZone.Status;
                zone.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                zone.UpdatedDate = DateTime.Now;
                _warehouseZoneService.Edit(zone, AuthenticatedUser.GetUserFromIdentity().UserId);
                return RedirectToAction("Index");
            }

            return View("../Shop/WarehouseZone/Edit", warehouseZone);
        }
        #endregion
        #region helper modules
        public JsonResult IsNameUsed(string ZoneName, string InitialZoneName)
        {
            bool isNotExist = _warehouseZoneService.IsNameUsed(ZoneName, InitialZoneName);
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZoneSelectListByProductId(int? productId)
        {
            StringBuilder selectListString = new StringBuilder();

            var allZone = _warehouseZoneService.GetAllWarehouseZone();
            int productDefaultZoneId = 0;
            if (productId != null)
            {
                var product = _productService.GetProductById(productId.Value);
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

            var allZone = _stockService.GetStockWarehouseRelationByStockId(stockId.Value).Select(x=>x.WarehouseZone).ToList();
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
                _warehouseZoneService.Dispose();
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