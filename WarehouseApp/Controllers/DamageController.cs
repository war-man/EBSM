using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.Security;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;


namespace WarehouseApp.Controllers
{
    [Authorize]
    public class DamageController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

        // GET: /Damage/
        [Roles("Global_SupAdmin,Damage")]
        public ActionResult Index(DamageSearchViewModel model)
        {

            var damages = db.Damages.Where(x => (model.SelectedProductId == null || x.Stock.ProductId == model.SelectedProductId) && (model.ProductNameFull == null || (x.Stock.Product.ProductFullName.StartsWith(model.ProductNameFull) || x.Stock.Product.ProductFullName.Contains(" " + model.ProductNameFull)))).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.Stock.Product.ProductFullName).ToList();
            model.Damages = damages.ToPagedList(model.Page, model.PageSize);
            ViewBag.ZoneId = new SelectList(db.WarehouseZones, "ZoneId", "ZoneName");
            return View("../Shop/Damage/Index", model);

        }
        [Roles("Global_SupAdmin,Damage")]
        public ActionResult Create()
        {
         
            return View("../Shop/Damage/NewDamage");
        }

        [HttpPost]
  
        public ActionResult SaveDamage(DamageProductsViewModel data)
        {
            var result = "Error";
            if (ModelState.IsValid)
            {
                foreach (var damageItem in data.DamageProducts)
                { 
                Damage damage = new Damage()
                {
                    StockId = damageItem.StockId,
                    Quantity = damageItem.Quantity,
                    Note = damageItem.Note,
                    CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                    CreatedDate = DateTime.Now
                };
                db.Damages.Add(damage);
               db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                    //====update stock==============================================
                    StockController updateStock = new StockController();
                    if (damageItem.DefaultZoneId == null)
                    {
                        updateStock.RemoveFromStockByStockId(Convert.ToInt32(damage.StockId),
                            Convert.ToDouble(damage.Quantity),
                            Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    }
                    else
                    {
                        updateStock.RemoveFromStockByStockId(Convert.ToInt32(damage.StockId),
                            Convert.ToDouble(damage.Quantity), Convert.ToInt32(damageItem.DefaultZoneId),
                            Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    }
                    //====update damage stock==============================================
                    //DamageController updateDamageStock = new DamageController();
                        AddToDamageStock(damage.StockId, damage.Quantity, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));

                }
                return RedirectToAction("Index", "Damage");
            }

            result = "OK";
            var jsonData = new { result = result, returnUrl = @Url.Action("Index", "Damage") };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
    }

       

        // GET: /Damage/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)


        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Damage damage = db.Damages.Find(id);
        //    if (damage == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    List<Damage>damageList=new List<Damage>();
        //    damageList.Add(damage);
        //    DamageViewModel damages = new DamageViewModel();
        //    damages.DamageProducts = damageList;
        //    return View("../Shop/Damage/EditDamage", damages);
        //}

        // POST: /Damage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Damage damage)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var oldDamage = db.Damages.Find(damage.DamageId);
        //       // int? oldDamageQuantity = Convert.ToInt32(Request["oldQuantity"]);
        //        double? changeQuentity = damage.Quantity - oldDamage.Quantity;

        //        oldDamage.StockId = damage.StockId;
        //        oldDamage.Quantity = damage.Quantity;
        //        oldDamage.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
        //        oldDamage.UpdatedDate = DateTime.Now;
        //        db.Entry(oldDamage).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
        //        //====update stock==============================================
        //        StockController updateStock = new StockController();
        //        updateStock.RemoveFromStock(Convert.ToInt32(damage.Stock.ProductId), Convert.ToDouble(changeQuentity), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
        //        //====update damage stock==============================================
        //        DamageController updateDamageStock = new DamageController();
        //        updateDamageStock.AddToDamageStock(Convert.ToInt32(damage.Stock.ProductId), changeQuentity, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));

        //        return RedirectToAction("Index");
        //    }

         
        //    return View("../Shop/Damage/Edit",damage);
        //}
       

        [HttpPost]
        public ActionResult RetunDismiss(DamageDismissViewModel vmodel)
        {
            if (ModelState.IsValid) { 
            if (vmodel.ActionType == "dismiss")
            {
                DamageDismiss dismiss = new DamageDismiss()
                {
                    DismissDate = Convert.ToDateTime(vmodel.ActionDate),
                    Quantity = Convert.ToInt32(vmodel.Quantity),
                    DamageId = vmodel.DamageId,
               
                };
                db.DamageDismisses.Add(dismiss);

                //Update damage stock quantity =============================================
                RemoveFromDamageStock(Convert.ToInt32(vmodel.StockId), Convert.ToInt32(vmodel.Quantity), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
            }
            else
            {
                DamageReturn returnInfo = new DamageReturn()
                {
                    ReturnDate = Convert.ToDateTime(vmodel.ActionDate),
                    Quantity = Convert.ToInt32(vmodel.Quantity),
                    DamageId = vmodel.DamageId,
                };
                db.DamageReturns.Add(returnInfo);
                //Update damage stock quantity =============================================
                RemoveFromDamageStock(Convert.ToInt32(vmodel.StockId), Convert.ToInt32(vmodel.Quantity), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                //Update product stock quantity =============================================
                StockController updaStockController = new StockController();
                if (vmodel.ZoneId == null) { 
                updaStockController.AddToStockByStockId(Convert.ToInt32(vmodel.StockId), Convert.ToInt32(vmodel.Quantity), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                }
                else
                {
                updaStockController.AddToStockByStockId(Convert.ToInt32(vmodel.StockId), Convert.ToInt32(vmodel.Quantity),Convert.ToInt32(vmodel.ZoneId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                 }
            }
            db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                //Update damage quantity =============================================
            Damage damage = db.Damages.Find(vmodel.DamageId);
            damage.Quantity -= vmodel.Quantity;
            db.Entry(damage).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

            return RedirectToAction("Index", "Damage");
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        //=====Update Damage Stock procedure ========================================================================================
        public void AddToDamageStock(int stockId, double? quantity, int? currentUserId)
        {
            DamageStock damageSstock = db.DamageStocks.FirstOrDefault(x => x.StockId == stockId);

            if (damageSstock == null)
            {
                DamageStock newDamageStock = new DamageStock();
                newDamageStock.Quantity = quantity;
                newDamageStock.StockId = Convert.ToInt32(stockId);
                newDamageStock.Status = 1;
                newDamageStock.CreatedBy = currentUserId;

                newDamageStock.CreatedDate = DateTime.Now;
                db.DamageStocks.Add(newDamageStock);
            }
            else
            {
                damageSstock.Quantity = damageSstock.Quantity + quantity;
                damageSstock.UpdatedBy = currentUserId;
                damageSstock.UpdatedDate = DateTime.Now;
                db.Entry(damageSstock).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges(currentUserId.ToString());

        }
        public void RemoveFromDamageStock(int stockId, int? quantity, int? currentUserId)
        {
            DamageStock damageSstock = db.DamageStocks.FirstOrDefault(x => x.StockId == stockId);
            if (damageSstock != null)
            {
                damageSstock.Quantity = damageSstock.Quantity - quantity;
                damageSstock.UpdatedBy = currentUserId;
                damageSstock.CreatedDate = DateTime.Now;
                db.Entry(damageSstock).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges(currentUserId.ToString());
            }
        }


        public ActionResult GetDamage(int? damageId)
        {
            Damage damage = db.Damages.FirstOrDefault(d => d.DamageId == damageId);
            var data = new { DamageId = damage.DamageId, StockId = damage.StockId, ProductName = damage.Stock.Product.ProductFullName, Quantity = damage.Quantity, DefaultZoneId = damage.Stock.Product.DefaultZoneId };
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