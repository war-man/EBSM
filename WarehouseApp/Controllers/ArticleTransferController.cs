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
    #region article transfer
    [Authorize]
    public class ArticleTransferController : Controller
    {
        private ArticleTransferService _articleTransferService = new ArticleTransferService();
        private StockService _stockService = new StockService();
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
                    articleTransfer.ArticleTransferId = _articleTransferService.Save(articleTransfer, AuthenticatedUser.GetUserFromIdentity().UserId);
               // db.ArticleTransfers.Add(articleTransfer);
               //db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                    //====update stock==============================================
                    StockController updateStock = new StockController();
                    var fromStok = _stockService.GetById(articleTransfer.FromStockId);
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

                    var toStock = _stockService.GetById(articleTransfer.ToStockId);
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
                _articleTransferService.Dispose();
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