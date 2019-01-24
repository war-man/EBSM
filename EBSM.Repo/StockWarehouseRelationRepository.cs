using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class StockWarehouseRelationRepository
    {
        private WmsDbContext db;
        public StockWarehouseRelationRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(StockWarehouseRelation stockWarehouse)
        {
            db.StockWarehouseRelations.Add(stockWarehouse);
        }
        public void Edit(StockWarehouseRelation stockWarehouse)
        {
            db.Entry(stockWarehouse).State = EntityState.Modified;
        }
        public StockWarehouseRelation GetStockWarehouseRelation(int stockId, int zoneId)
        {
            return db.StockWarehouseRelations.FirstOrDefault(x => x.StockId == stockId && x.ZoneId == zoneId);
        }
        public IEnumerable<StockWarehouseRelation> GetStockWarehouseRelationByStockId(int stockId)
        {
            return db.StockWarehouseRelations.Where(x => x.StockId == stockId && x.Quantity != 0);
        }
    }
}
