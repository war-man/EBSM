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
using EBSM.Services;

namespace WarehouseApp.Controllers
{
    #region article transfer
    [Authorize]
    public class ArticleTransferController : Controller
    {
        private ArticleTransferService _articleTransferService = new ArticleTransferService();
        [Roles("Global_SupAdmin,Article_Transfer")]
        public ActionResult Index(ArticleTransferSearchViewModel model)
        {

            var articleTransferList = _articleTransferService.GetAll(model.SelectedProductId,model.PName,model.TransferDateFrom,model.TransferDateTo).ToList();
            model.ArticleTransfers = articleTransferList.ToPagedList(model.Page, model.PageSize);
           
            return View("../Shop/ArticleTransfer/ArticleTransferList", model);

        }
     
        [Roles("Global_SupAdmin,Article_Transfer")]
        public ActionResult CreateArticleTransfer()
        {
            return View("../Shop/ArticleTransfer/CreateArticleTransfer");
        }

        [HttpPost]
        public ActionResult SaveArticleTransfer(ArticleTransfersViewModel data)
        {
            var result = "Error";
            if (ModelState.IsValid)
            {
                foreach (var item in data.ArticleTransfers)
                { 
                var articleTransfer = new ArticleTransfer
                {
                    TransferDate = DateTime.Now,
                    FromStockId = item.FromStockId,
                    ToStockId = item.ToStockId,
                    TransferQuantity = item.TransferQuantity,
                    CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                    CreatedDate = DateTime.Now
                };
                db.ArticleTransfers.Add(articleTransfer);
               db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                    //====update stock==============================================
                    StockController updateStock = new StockController();
                    var fromStok = db.Stocks.Find(articleTransfer.FromStockId);
                    if (fromStok.Product.DefaultZoneId == null)
                    {
                        updateStock.RemoveFromStockByStockId(Convert.ToInt32(articleTransfer.FromStockId),
                            Convert.ToDouble(articleTransfer.TransferQuantity),
                            Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    }
                    else
                    {
                        updateStock.RemoveFromStockByStockId(Convert.ToInt32(articleTransfer.FromStockId),
                            Convert.ToDouble(articleTransfer.TransferQuantity), Convert.ToInt32(fromStok.Product.DefaultZoneId),
                            Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    }

                    var toStock = db.Stocks.Find(articleTransfer.ToStockId);
                    if (toStock.Product.DefaultZoneId == null)
                    {
                        updateStock.AddToStockByStockId(Convert.ToInt32(articleTransfer.ToStockId),
                            Convert.ToDouble(articleTransfer.TransferQuantity),
                            Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    }
                    else
                    {
                        updateStock.AddToStockByStockId(Convert.ToInt32(articleTransfer.ToStockId),
                            Convert.ToDouble(articleTransfer.TransferQuantity), Convert.ToInt32(toStock.Product.DefaultZoneId),
                            Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    }
                    
                }
                return RedirectToAction("CreateArticleTransfer", "ArticleTransfer");
            }

            result = "OK";
            var jsonData = new { result = result, returnUrl = @Url.Action("Index", "Damage") };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
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
    #endregion
    #region product transfer
    [Authorize]
    public class TransferProductsController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

        // GET: /Damage/
        [Roles("Global_SupAdmin,Product_Transfer")]
        public ActionResult Index(ProductTransferSearchViewModel model)
        {
            var fromDate = Convert.ToDateTime(model.TransferDateFrom);
            var toDate = Convert.ToDateTime(model.TransferDateTo);
            var productTransferList = db.TransferProducts.ToList().Where(x => (model.SelectedProductId == null || x.Stock.ProductId == model.SelectedProductId) && (model.PName == null || (x.Stock.Product.ProductFullName.StartsWith(model.PName) || x.Stock.Product.ProductFullName.Contains(" " + model.PName))) && (model.TransferDateFrom == null || x.TransferDate.Date >= fromDate) && (model.TransferDateTo == null || x.TransferDate.Date <= toDate)).OrderByDescending(o => o.CreatedDate);
            model.ProductTransfers = productTransferList.ToPagedList(model.Page, model.PageSize);

            return View("../Shop/ArticleTransfer/ProductTransferList", model);

        }
        [Roles("Global_SupAdmin,Product_Transfer")]
        public ActionResult CreateProductTransfer()
        {
            // ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View("../Shop/ArticleTransfer/CreateProductTransfer");
        }

        [HttpPost]
        //public ActionResult SaveDamage(List<Damage>data)
        public ActionResult SaveProductTransfer(TransferProductsViewModel data)
        {
            var result = "Error";
            if (ModelState.IsValid)
            {
                foreach (var item in data.TransferProducts)
                {
                    var transferProduct = new TransferProduct
                    {
                        TransferDate = DateTime.Now,
                        StockId = item.StockId,
                        ZoneFromId = item.ZoneFromId,
                        ZoneToId = item.ZoneToId,
                        TransferQuantity = item.TransferQuantity,
                        CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                        CreatedDate = DateTime.Now
                    };
                    db.TransferProducts.Add(transferProduct);
                    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                    //====update stock==============================================
                    StockController updateStock = new StockController();
                    var stock = db.Stocks.Find(transferProduct.StockId);
                    if (string.IsNullOrEmpty(stock.Barcode))
                    {
                        updateStock.RemoveFromStock(Convert.ToInt32(stock.ProductId), item.TransferQuantity, Convert.ToInt32(item.ZoneFromId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    }
                    else
                    {
                        updateStock.RemoveFromStock(Convert.ToInt32(stock.ProductId), item.TransferQuantity, Convert.ToInt32(item.ZoneFromId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), stock.Barcode);
                    }
if (string.IsNullOrEmpty(stock.Barcode))
                    {
                        updateStock.AddToStock(Convert.ToInt32(stock.ProductId), item.TransferQuantity, Convert.ToInt32(item.ZoneToId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    }
                    else
                    {
                        updateStock.AddToStock(Convert.ToInt32(stock.ProductId), item.TransferQuantity, Convert.ToInt32(item.ZoneToId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), stock.Barcode);
                    }

                   
                }
                return RedirectToAction("CreateProductTransfer", "TransferProducts");
            }

            result = "OK";
            var jsonData = new { result = result, returnUrl = @Url.Action("Index", "Damage") };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
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
#endregion
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//