using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;

using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;
using EBSM.Services;
using EBSM.Entities;
namespace WarehouseApp.Controllers
{
    [Authorize]
    public class BatchController : Controller
    {
        private StockService _stockService = new StockService();
        private ProductService _productService = new ProductService();

        #region list view
        [Roles("Global_SupAdmin,Barcode_Generate")]
         [OutputCache(Duration = 30)]
        public ActionResult Index(BarcodeSearchViewModel model)
        {

            var barcodes = _stockService.GetAll().ToList().Where(x => (x.Barcode != null) && (model.PName == null || (x.Product.ProductFullName.ToLower().StartsWith(model.PName.ToLower()) || x.Product.ProductFullName.ToLower().Contains(" " + model.PName.ToLower())))
                    && (model.PCode == null || x.Product.ProductCode.ToLower().StartsWith(model.PCode.ToLower())) && (model.BCode == null || x.Barcode.ToLower().Equals(model.BCode.ToLower()))).OrderBy(x => x.Product.ProductFullName).ThenByDescending(x => x.CreatedDate).Select(x => new BarcodeViewModel { ProductId = (Int32)x.ProductId,Product = x.Product,Barcode = x.Barcode,BatchNo =x.BatchNo, Exp = String.Format("{0:dd-MM-yyyy}",x.Exp),PurchasePrice = x.PurchasePrice}).ToList();
            model.Barcodes = barcodes.ToPagedList(model.Page, model.PageSize);
            model.BarcodeModel = new BarcodeViewModel();
            ViewBag.ProductId = new SelectList(_productService.GetActiveProducts(), "ProductId", "ProductFullName");
   
            return View("../Shop/Batch/Index", model);
        }
        #endregion
        #region save exist barcode
        public ActionResult SaveBarcode()
        {
            ViewBag.ProductId = new SelectList(_productService.GetActiveProducts(), "ProductId", "ProductFullName");
            return View("../Shop/Batch/SaveExistBarcode");
        }

        [HttpPost]
        public ActionResult SaveBarcode(BarcodeViewModel barcodeModel)
        {
            if (ModelState.IsValid)
            {
                Stock newStock = new Stock()
                {
                    ProductId = barcodeModel.ProductId,
                    Barcode = barcodeModel.Barcode,
                    PurchasePrice = barcodeModel.PurchasePrice,
                    SalePrice = barcodeModel.SalePrice,
                    Exp = !String.IsNullOrEmpty(barcodeModel.Exp) ? Convert.ToDateTime(barcodeModel.Exp) : (DateTime?)null,
                    Mfg = !String.IsNullOrEmpty(barcodeModel.Mfg) ? Convert.ToDateTime(barcodeModel.Mfg) : (DateTime?)null,
                    TotalQuantity = 0,
                    Status = 1,
                    CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId,
                    CreatedDate = DateTime.Now,
                };
                _stockService.Save(newStock, AuthenticatedUser.GetUserFromIdentity().UserId);
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(_productService.GetActiveProducts(), "ProductId", "ProductFullName",barcodeModel.ProductId);
            return View("../Shop/Batch/SaveExistBarcode");
        }
        #endregion
        #region generate new barcode
        [Roles("Global_SupAdmin,Barcode_Generate")]
        public ActionResult GenerateBarcode()
        {
           
            return View("../Shop/Batch/GenerateBarcode");
        }
         [HttpPost]
        public ActionResult GenerateBarcode(BarcodeGenerateViewModel productsList)
         {
             
             foreach (var item in productsList.ProductBarcodes)
             {
                 //will call a recursive checking=============================================
                 string uniqueBarcode = item.ProductId+DateTime.Now.ToString("yyMdHHmm");
                 item.Barcode = _stockService.IsBarcodeExist(uniqueBarcode) ? uniqueBarcode + "D" : uniqueBarcode;
                 Stock newStock = new Stock()
                 {
                     ProductId = item.ProductId,
                     Barcode = item.Barcode,
                     PurchasePrice = item.PurchasePrice,
                     SalePrice = item.SalePrice,
                     Exp =!String.IsNullOrEmpty(item.Exp)? Convert.ToDateTime(item.Exp): (DateTime?) null,
                     Mfg =!String.IsNullOrEmpty(item.Mfg)? Convert.ToDateTime(item.Mfg): (DateTime?) null,
                     TotalQuantity = 0,
                     Status = 1,
                     CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId,
                     CreatedDate = DateTime.Now,
                 };
                _stockService.Save(newStock, AuthenticatedUser.GetUserFromIdentity().UserId);
                 item.Product = _productService.GetProductrById(item.ProductId);
             }
            
            return View("../Shop/Batch/PrintBarcode", productsList.ProductBarcodes);
        }
        #endregion
         #region print exist barcode
         public ActionResult PrintExistBarcodes()
         {

             return View("../Shop/Batch/PrintExistBarcodes");
         }
         [HttpPost]
         public ActionResult PrintExistBarcodes(ExistBarcodesPrintViewModel printBarcodeList)
        {
           
            return View("../Shop/Batch/PrintBarcode", printBarcodeList.ProductBarcodes);
        }
#endregion
         #region helper modules
         public JsonResult IsBarcodeExist(string Barcode, string InitialBarcode)
        {
            bool isNotExist = _stockService.IsBarcodeExist(Barcode, InitialBarcode);
           
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
        }
#endregion
         #region dispose
         protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stockService.Dispose();
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