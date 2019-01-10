using EBSM.Entities;
using System;
namespace EBSM.Repo
{
    public class StockUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private StockRepository _stockRepository { get; set; }
        private StockWarehouseRelationRepository _stockWarehouseRelationRepository { get; set; }

        public StockUnitOfWork(WmsDbContext context)
        {
            db = context;
            _stockRepository = new StockRepository(db);
            _stockWarehouseRelationRepository = new StockWarehouseRelationRepository(db);
        }

        public StockRepository StockRepository
        {
            get
            {
                return _stockRepository;
            }
        }
        public StockWarehouseRelationRepository StockWarehouseRelationRepository
        {
            get
            {
                return _stockWarehouseRelationRepository;
            }
        }
        public void Save()
        {
            db.SaveChanges("");
        }
        public void Save(string loggedInUserId)
        {
            db.SaveChanges(loggedInUserId);
        }
        
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
