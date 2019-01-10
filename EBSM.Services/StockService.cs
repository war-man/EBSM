using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class StockService
    {
        private WmsDbContext _context;
        private StockUnitOfWork _stockUnitOfWork;

        public StockService()
        {
            _context = new WmsDbContext();
            _stockUnitOfWork = new StockUnitOfWork(_context);
        }
       
        public IEnumerable<Stock> GetAll()
        {
            return _stockUnitOfWork.StockRepository.GetAll();
        }
        public IEnumerable<Stock> GetAll(int? SelectedProductId, string PName, string PCode, string StockLimitOut, int? StockZoneId)
        {
            return _stockUnitOfWork.StockRepository.GetAll(SelectedProductId, PName, PCode, StockLimitOut, StockZoneId);
        }
        public IEnumerable<Stock> GetAllExceptThis(int? stockIdNotIn)
        {
            return _stockUnitOfWork.StockRepository.GetAllExceptThis(stockIdNotIn);
        }
        public int Save(Stock stock, int? loggedInUserId)
        {
            _stockUnitOfWork.StockRepository.Add(stock);
            _stockUnitOfWork.Save(loggedInUserId.ToString());
            return stock.StockId;
        }
       
        public void Edit(Stock stock, int? loggedInUserId)
        {
            _stockUnitOfWork.StockRepository.Edit(stock);
            _stockUnitOfWork.Save(loggedInUserId.ToString());
        }
        public Stock GetById(int id)
        {
            return _stockUnitOfWork.StockRepository.GetById(id);
        }
        public Stock GetByProductId(int productId)
        {
            return _stockUnitOfWork.StockRepository.GetByProductId(productId);
        }
        public Stock GetByProductIdAndBarcode(int productId, string barcode)
        {
            return _stockUnitOfWork.StockRepository.GetByProductIdAndBarcode(productId, barcode);
        }
        public StockWarehouseRelation GetStockWarehouseRelation(int stockId, int zoneId)
        {
            return _stockUnitOfWork.StockWarehouseRelationRepository.GetStockWarehouseRelation(stockId, zoneId);
        }
        public int SaveWarehouseStockRlation(StockWarehouseRelation stockWarehouse)
        {
            _stockUnitOfWork.StockWarehouseRelationRepository.Add(stockWarehouse);
            _stockUnitOfWork.Save();
            return stockWarehouse.StockWarehouseRelationId;
        }
        public void EditWarehouseStockRlation(StockWarehouseRelation stockWarehouse)
        {
            _stockUnitOfWork.StockWarehouseRelationRepository.Edit(stockWarehouse);
            _stockUnitOfWork.Save();
        }
        public bool IsBarcodeExist(string barcode)
        {
           return  _stockUnitOfWork.StockRepository.IsBarcodeExist(barcode);
        }
        public bool IsBarcodeExist(string Barcode, string InitialBarcode)
        {
           return  _stockUnitOfWork.StockRepository.IsBarcodeExist(Barcode, InitialBarcode);
        }
        public void Dispose()
        {
            _stockUnitOfWork.Dispose();
        }
    }
}
