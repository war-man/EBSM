using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public void Edit(WarehouseZone warehouseZone)
        {
            db.Entry(warehouseZone).State = EntityState.Modified;
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
        }public IEnumerable<WarehouseZone> GetAll(string ZoneName)
        {
            return db.WarehouseZones.Where(x => (ZoneName == null || x.ZoneName.StartsWith(ZoneName))).OrderBy(x => x.ZoneName);
        }
        public bool IsNameUsed(string ZoneName, string InitialZoneName)
        {
            bool isNotExist = true;
            if (ZoneName != string.Empty && InitialZoneName == "undefined")
            {
                var isExist = db.WarehouseZones.Any(x => x.ZoneName.ToLower().Equals(ZoneName.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (ZoneName != string.Empty && InitialZoneName != "undefined")
            {
                var isExist = db.WarehouseZones.Any(x => x.ZoneName.ToLower() == ZoneName.ToLower() && x.ZoneName.ToLower() != InitialZoneName.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return isNotExist;
        }
        //ware house
        public Warehouse GetWarehouseByName(string name)
        {
            return db.Warehouses.FirstOrDefault(x => x.WarehouseName.ToLower().Contains(name.ToLower()));
        }
        public IEnumerable<Warehouse> GetAllWarehouses()
        {
            return db.Warehouses.Where(x => x.Status!=0);
        }
    }
}
