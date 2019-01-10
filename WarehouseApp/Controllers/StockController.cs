using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using PagedList;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;
using WebGrease.Css.Extensions;
using EBSM.Services;
using EBSM.Entities;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        private StockService _stockService = new StockService();
        private WarehouseZoneService _warehouseZoneService = new WarehouseZoneService();

        #region stock list view
         [Roles("Global_SupAdmin,Stock_View")]
         [OutputCache(Duration = 20)]
        public ActionResult Index(StockSearchViewModel model)
        {

            //List<Stock> stocks = db.Stocks.Where(x => (model.SelectedProductId == null || x.ProductId == model.SelectedProductId) && (model.ProductName == null || (x.Product.ProductFullName.StartsWith(model.ProductName) || x.Product.ProductFullName.Contains(" " + model.ProductName)))
            //    && (model.StockLimitOut == null || x.TotalQuantity<= x.Product.MinStockLimit)).Include(x => x.Product).OrderBy(o => o.TotalQuantity).ThenBy(o => o.Product.ProductFullName).ToList();
            var stockGroups = _stockService.GetAll().GroupBy(x => x.ProductId).ToList().Where(x => (model.SelectedProductId == null || x.First().ProductId == model.SelectedProductId) && (model.PName == null || (x.First().Product.ProductFullName.ToLower().StartsWith(model.PName.ToLower()) || x.First().Product.ProductFullName.ToLower().Contains(" " + model.PName.ToLower()))) && (model.PCode == null || x.First().Product.ProductCode.ToLower().StartsWith(model.PCode.ToLower())) && (model.StockLimitOut == null || x.Sum(y => y.TotalQuantity) <= x.First().Product.MinStockLimit)&& (model.StockZoneId == null || x.First().StockWarehouseRelations.Any(y=>y.ZoneId==model.StockZoneId))).OrderBy(o => o.First().Product.ProductFullName);
            List<Stock> stocks = stockGroups.Select(x => new Stock { StockId = x.First().StockId, ProductId = x.First().ProductId, Product = x.First().Product, PurchasePrice = x.First().PurchasePrice, SalePrice = x.First().SalePrice, TotalQuantity = x.Sum(y => y.TotalQuantity) }).ToList();
            ViewBag.Inventory = stocks.Sum(x => x.TotalQuantity);
            ViewBag.InventoryPurchasedValue = stocks.Sum(x => x.TotalQuantity * x.Product.Tp);
            ViewBag.InventorySalesValue = stocks.Sum(x => x.TotalQuantity * x.Product.Dp);
            model.Stocks = stocks.ToPagedList(model.Page, model.PageSize);
            model.SelectedProductId = null;
            // zone wise stock show======================================
            #region zone wise stock showing
            Dictionary<int,string> zoneWiseStockList=new Dictionary<int,string>();
             foreach (var itemGroup in stockGroups)
             {
                 var stockWarehouseRelationList = new List<StockWarehouseRelation>();
                 string zoneWiseStock = "";
                foreach (var item in itemGroup)
                {
                  stockWarehouseRelationList.AddRange(item.StockWarehouseRelations);
                //     foreach (var stockWarehouseRelation in item.StockWarehouseRelations)
                //{
                //    stockWarehouseRelationList.Add(stockWarehouseRelation);
                //}
                    
                }
                foreach (var zoneWiseItem in stockWarehouseRelationList.GroupBy(x => x.ZoneId))
                 {
                    if(zoneWiseItem.Sum(x=>x.Quantity)!=0)
                    {
                        zoneWiseStock += zoneWiseItem.First().WarehouseZone.ZoneName + " " +zoneWiseItem.Sum(x => x.Quantity) + ", ";
                    }
                 }
                  zoneWiseStockList.Add(itemGroup.First().StockId,zoneWiseStock.Trim(',',' '));
            }
            ViewBag.ZoneWiseStockList = zoneWiseStockList;
            #endregion
            ViewBag.StockZoneId = new SelectList(_warehouseZoneService.GetAll(), "ZoneId", "ZoneName");

            return View("../Shop/Stock/Index", model);
       
        }
        #endregion
         //**************************************start to Add stock code **********************************************************************************************************************

        // add to stock without barcode -------------------------------
        #region add to stock without barcode
        public void AddToStock(int productId, double quantity, int? currentUserId)
        {
            Stock stock = _stockService.GetByProductId(productId);
            
            if (stock == null)
            {
                stock = new Stock()
                {
                TotalQuantity = quantity,
                ProductId = Convert.ToInt32(productId),
                Status = 1,
                CreatedBy = currentUserId,
                CreatedDate = DateTime.Now};
                _stockService.Save(stock, currentUserId);
            }
            else
            {
                stock.TotalQuantity = stock.TotalQuantity + quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock, currentUserId);
               // db.Entry(stock).State = EntityState.Modified;
            }
            //db.SaveChanges(currentUserId.ToString());
           
        }
        #endregion
        // add to stock with barcode -------------------------------
        #region add to stock with barcode
        public void AddToStock(int productId, double quantity, int? currentUserId,string barcode)
        {
            Stock stock = _stockService.GetByProductIdAndBarcode(productId, barcode);
            
            if (stock == null)
            {
                stock = new Stock()
                {
                    TotalQuantity = quantity,
                    ProductId = Convert.ToInt32(productId),
                    Barcode = barcode,
                    Status = 1,
                    CreatedBy = currentUserId,
                    CreatedDate = DateTime.Now
                };
                _stockService.Save(stock, currentUserId);
            }
            else
            {
                stock.TotalQuantity = stock.TotalQuantity + quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock, currentUserId);
            }
           // db.SaveChanges(currentUserId.ToString());
           
        }
        #endregion
        // add to stock by stockId -------------------------------
        #region add to stock by stockId
        public void AddToStockByStockId(int stockId, double quantity, int? currentUserId)
        {
            Stock stock = _stockService.GetById(stockId); 

            if (stock != null)
            {
                stock.TotalQuantity = stock.TotalQuantity + quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock, currentUserId);
                //db.Entry(stock).State = EntityState.Modified;
                //db.SaveChanges(currentUserId.ToString());
            }


        }
        #endregion
        //**************************************end Add to stock code **********************************************************************************************************************

        //**************************************start Add to stock with Warehouse Zone code **********************************************************************************************************************
        // add to stock in warehouse zone without barcode -------------------------------
        #region add to stock in warehouse zone without barcode
        public void AddToStock(int productId, double quantity, int zoneId, int? currentUserId)
        {
            Stock stock = _stockService.GetByProductId(productId);

            if (stock == null)
            {
                stock = new Stock()
                {
                    ProductId = Convert.ToInt32(productId),
                    TotalQuantity = quantity,
                    Status = 1,
                    CreatedBy = currentUserId,
                    CreatedDate = DateTime.Now
                };
                _stockService.Save(stock, currentUserId);
                
            }
            else
            {
                stock.TotalQuantity = stock.TotalQuantity + quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock, currentUserId);
                
            }
           // db.SaveChanges(currentUserId.ToString());

            AddToStockWarehouseRetaion(stock.StockId, quantity, zoneId);
        }
        #endregion 
        // add to stock in warehouse zone with barcode -------------------------------
        #region add to stock in warehouse zone with barcode
        public void AddToStock(int productId, double quantity, int zoneId, int? currentUserId, string barcode)
        {
            Stock stock = _stockService.GetByProductIdAndBarcode(productId, barcode);

            if (stock == null)
            {
                stock = new Stock()
                {
                    ProductId = Convert.ToInt32(productId),
                    TotalQuantity = quantity,
            
                    Barcode = barcode,
                    Status = 1,
                    CreatedBy = currentUserId,
                    CreatedDate = DateTime.Now
                };
                _stockService.Save(stock, currentUserId);
               
            }
            else
            {
                stock.TotalQuantity = stock.TotalQuantity + quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock, currentUserId);
            }
            //db.SaveChanges(currentUserId.ToString());

            AddToStockWarehouseRetaion(stock.StockId, quantity, zoneId);

        }
        #endregion
        // add to stock  by stockId in warehouse zone -------------------------------
        #region add to stock  by stockId in warehouse zone
        public void AddToStockByStockId(int stockId, double quantity, int zoneId,int? currentUserId)
        {
            Stock stock = _stockService.GetById(stockId);

            if (stock != null)
            {
                stock.TotalQuantity = stock.TotalQuantity + quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock, currentUserId);

                AddToStockWarehouseRetaion(stock.StockId, quantity, zoneId);
            }


        }
        #endregion

        // add to stock  warehouse zone relation -------------------------------
        #region add to stock  warehouse zone relation
        public void AddToStockWarehouseRetaion(int stockId, double quantity, int zoneId)
        {
            StockWarehouseRelation stockWarehouse = _stockService.GetStockWarehouseRelation(stockId,zoneId);

            if (stockWarehouse == null)
            {
                stockWarehouse = new StockWarehouseRelation()
                {
                    StockId = stockId,
                    ZoneId = zoneId,
                    Quantity = quantity,
                    
                };
                _stockService.SaveWarehouseStockRlation(stockWarehouse);
            }
            else
            {
                stockWarehouse.StockId = stockId;
                stockWarehouse.ZoneId = zoneId;
                stockWarehouse.Quantity += quantity;
                _stockService.EditWarehouseStockRlation(stockWarehouse);
                
            }
        }
        #endregion
        //**************************************End Add to stock with Warehouse Zone code **********************************************************************************************************************
        //**************************************start remove from stock code **********************************************************************************************************************

        // remove from stock without barcode -------------------------------
        #region remove from stock without barcode
        public void RemoveFromStock(int productId, double quantity, int? currentUserId)
        {
            Stock stock = _stockService.GetByProductId(productId);
            if (stock != null)
            {
                stock.TotalQuantity = stock.TotalQuantity - quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock,currentUserId);

            }
        }
        #endregion
        // remove from stock with barcode -------------------------------
        #region remove from stock with barcode
        public void RemoveFromStock(int productId, double quantity, int? currentUserId, string barcode)
        {
            Stock stock = _stockService.GetByProductIdAndBarcode(productId, barcode); 
            if (stock != null)
            {
                stock.TotalQuantity = stock.TotalQuantity - quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock,currentUserId);
            }
        }
        #endregion
        // remove from stock by stockId -------------------------------
        #region remove from stock by stockId
        public void RemoveFromStockByStockId(int stockId, double quantity,  int? currentUserId)
        {
            Stock stock = _stockService.GetById(stockId);
            if (stock != null)
            {
                stock.TotalQuantity = stock.TotalQuantity - quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock, currentUserId);
            }
        }
        #endregion
        //**************************************end remove from stock code **********************************************************************************************************************
        //**************************************start remove from stock with wharehouse zone code **********************************************************************************************************************

        // remove from stock without barcode with wharehouse zone-------------------------------
        #region remove from stock without barcode with wharehouse zone
        public void RemoveFromStock(int productId, double quantity, int zoneId, int? currentUserId)
        {
            Stock stock = _stockService.GetByProductId(productId);
            if (stock != null)
            {
                stock.TotalQuantity = stock.TotalQuantity - quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock, currentUserId);
                RemoveFromStockWarehouse(stock.StockId, quantity, zoneId);
            }
        }
        #endregion
        // remove from stock with barcode with wharehouse zone-------------------------------
        #region remove from stock with barcode with wharehouse zone
        public void RemoveFromStock(int productId, double quantity, int zoneId, int? currentUserId, string barcode)
        {
            Stock stock = _stockService.GetByProductIdAndBarcode(productId, barcode);
            if (stock != null)
            {
                stock.TotalQuantity = stock.TotalQuantity - quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock, currentUserId);

                RemoveFromStockWarehouse(stock.StockId, quantity, zoneId);
            }
        }
        #endregion
        // remove from stock with stockId with wharehouse zone-------------------------------
        #region remove from stock with stockId with wharehouse zone
        public void RemoveFromStockByStockId(int stockId, double quantity,int zoneId, int? currentUserId)
        {
            Stock stock = _stockService.GetById(stockId);
            if (stock != null)
            {
                stock.TotalQuantity = stock.TotalQuantity - quantity;
                stock.UpdatedBy = currentUserId;
                stock.UpdatedDate = DateTime.Now;
                _stockService.Edit(stock, currentUserId);

                RemoveFromStockWarehouse(stock.StockId, quantity,zoneId);
            }
        }
        #endregion
        //**************************************end remove from stock with wharehouse zone code **********************************************************************************************************************

        // remove from stock warehouse relation -------------------------------
        #region remove from stock warehouse relation
        public void RemoveFromStockWarehouse(int stockId, double quantity, int zoneId)
        {
            StockWarehouseRelation stockWarehouse = _stockService.GetStockWarehouseRelation(stockId, zoneId);

            if (stockWarehouse == null)
            {
                stockWarehouse = new StockWarehouseRelation()
                {
                    StockId = stockId,
                    ZoneId = zoneId,
                    Quantity = 0,
                };
                stockWarehouse.Quantity -= quantity;
                _stockService.SaveWarehouseStockRlation(stockWarehouse);
               
            }
            else
            {
                stockWarehouse.StockId = stockId;
                stockWarehouse.ZoneId = zoneId;
                stockWarehouse.Quantity -= quantity;
                _stockService.EditWarehouseStockRlation(stockWarehouse);
            }
        }
        #endregion
        #region stock select list
        public JsonResult GetStockSelectList(int? stockIdNotIn)
        {
            StringBuilder selectListString = new StringBuilder();

            var allStock = _stockService.GetAllExceptThis(stockIdNotIn);
            //int productDefaultZoneId = 0;
            //if (stockIdNotIn != null)
            //{
            //    productDefaultZoneId = Convert.ToInt32(db.Products.FirstOrDefault(x => x.ProductId == productId).DefaultZoneId);
            //}
            foreach (var item in allStock)
            {
                //string optionString = item.ZoneId == productDefaultZoneId
                //    ? "<option selected='selected' value='" + item.ZoneId + "'>" + item.ZoneName + "</option>"
                //    : "<option  value='" + item.ZoneId + "'>" + item.ZoneName + "</option>";
                string optionString =  "<option  value='" + item.StockId + "'>" + item.Product.ProductFullName + "</option>";
                selectListString.Append(optionString);
            }

            var data = new { selectListString = selectListString.ToString() };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stockService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//