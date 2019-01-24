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
using EBSM.Entities;

namespace WarehouseApp.Controllers
{

    #region product transfer
    [Authorize]
    public class TransferProductsController : Controller
    {
        private ProductTransferService _productTransferService = new ProductTransferService();
        private StockService _stockService = new StockService();

        // GET: /Damage/
        [Roles("Global_SupAdmin,Product_Transfer")]
        public ActionResult Index(ProductTransferSearchViewModel model)
        {
            var fromDate = Convert.ToDateTime(model.TransferDateFrom);
            var toDate = Convert.ToDateTime(model.TransferDateTo);
            var productTransferList = _productTransferService.GetAll(model.SelectedProductId, model.PName, model.TransferDateFrom, model.TransferDateTo);
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
                        CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId,
                        CreatedDate = DateTime.Now
                    };
                    _productTransferService.Save(transferProduct, AuthenticatedUser.GetUserFromIdentity().UserId);
             

                    //====update stock==============================================
                    StockController updateStock = new StockController();
                    var stock = _stockService.GetById(transferProduct.StockId);
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
                _productTransferService.Dispose();
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