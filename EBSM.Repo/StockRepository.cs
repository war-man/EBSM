using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class StockRepository
    {
        private WmsDbContext db;
        public StockRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Stock stock)
        {
            db.Stocks.Add(stock);
        }
        public void Edit(Stock stock)
        {
            db.Entry(stock).State = EntityState.Modified;
        }
        public Stock GetById(int id)
        {
            return db.Stocks.Find(id); 
        }
        public IEnumerable<Stock> GetAll()
        {
            return db.Stocks;
        }
        public IEnumerable<Stock> GetAll(int? SelectedProductId, string PName, string PCode, string StockLimitOut, int? StockZoneId)
        {
             //var stocks=db.Stocks.Where(x => (SelectedProductId == null || x.ProductId == SelectedProductId) && (PName == null || (x.Product.ProductFullName.ToLower().StartsWith(PName.ToLower()) || x.Product.ProductFullName.ToLower().Contains(" " + PName.ToLower()))) && (PCode == null || x.Product.ProductCode.ToLower().StartsWith(PCode.ToLower())) && (StockLimitOut == null || x.Sum(y => y.TotalQuantity) <= x.First().Product.MinStockLimit) && (StockZoneId == null || x.First().StockWarehouseRelations.Any(y => y.ZoneId == StockZoneId))).OrderBy(o => o.First().Product.ProductFullName);
             var stockGroups=db.Stocks.GroupBy(x => x.ProductId).ToList().Where(x => (SelectedProductId == null || x.First().ProductId == SelectedProductId) && (PName == null || (x.First().Product.ProductFullName.ToLower().StartsWith(PName.ToLower()) || x.First().Product.ProductFullName.ToLower().Contains(" " + PName.ToLower()))) && (PCode == null || x.First().Product.ProductCode.ToLower().StartsWith(PCode.ToLower())) && (StockLimitOut == null || x.Sum(y => y.TotalQuantity) <= x.First().Product.MinStockLimit) && (StockZoneId == null || x.First().StockWarehouseRelations.Any(y => y.ZoneId == StockZoneId))).OrderBy(o => o.First().Product.ProductFullName);
            List<Stock> stocks = stockGroups.Select(x => new Stock { StockId = x.First().StockId, ProductId = x.First().ProductId, Product = x.First().Product, PurchasePrice = x.First().PurchasePrice, SalePrice = x.First().SalePrice, TotalQuantity = x.Sum(y => y.TotalQuantity) }).ToList();
            return stocks;
        }
        public IEnumerable<Stock> GetAllExceptThis(int? stockIdNotIn)
        {
            return db.Stocks.Where(x => x.StockId != stockIdNotIn).ToList(); ;
        }
        public Stock GetByProductId(int productId)
        {
            return db.Stocks.FirstOrDefault(x=>x.ProductId==productId);
        }
        public Stock GetByProductIdAndBarcode(int productId,string barcode)
        {
            return db.Stocks.FirstOrDefault(x => x.ProductId == productId && x.Barcode.ToLower() == barcode.ToLower());
        }
        public bool IsBarcodeExist(string barcode)
        {
            return db.Stocks.Any(x => x.Barcode.ToLower() == barcode.ToLower());
        }
        public bool IsBarcodeExist(string Barcode, string InitialBarcode)
        {
            bool isNotExist = true;
            if (Barcode != string.Empty && InitialBarcode == "undefined")
            {
                var isExist = db.Stocks.Any(x => x.Barcode.ToLower().Equals(Barcode.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (Barcode != string.Empty && InitialBarcode != "undefined")
            {
                var isExist = db.Stocks.Any(x => x.Barcode.ToLower() == Barcode.ToLower() && x.Barcode.ToLower() != InitialBarcode.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return isNotExist;
        }
    
}
}
