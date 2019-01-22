using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class WarehouseZoneRepository
    {
        private WmsDbContext db;
        public WarehouseZoneRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(WarehouseZone warehouseZone)
        {
            db.WarehouseZones.Add(warehouseZone);
        }
        public WarehouseZone GetById(int id)
        {
            return db.WarehouseZones.Find(id); 
        }
        public WarehouseZone GetWarehouseZoneByName(string name)
        {
            return db.WarehouseZones.FirstOrDefault(x => x.ZoneName.ToLower().Contains(name.ToLower()));
        }
        public IEnumerable<WarehouseZone> GetAll()
        {
            return db.WarehouseZones.Where(x => x.Status != 0).OrderBy(x => x.ZoneName);
        }
        //ware house
        public Warehouse GetWarehouseByName(string name)
        {
            return db.Warehouses.FirstOrDefault(x => x.WarehouseName.ToLower().Contains(name.ToLower()));
        }
    }
}
