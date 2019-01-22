using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class SupplierRepository
    {
        private WmsDbContext db;
        public SupplierRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Supplier item)
        {
            db.Suppliers.Add(item);
        }
        public void Edit(Supplier item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Supplier GetById(int id)
        {
            return db.Suppliers.Find(id); 
        }
        public IEnumerable<Supplier> GetAll()
        {
            return db.Suppliers;
        }
        public IEnumerable<Supplier> GetAll(string SupplierName)
        {
            return db.Suppliers.Where(x => (SupplierName == null || x.SupplierName.StartsWith(SupplierName))).OrderBy(x => x.SupplierName);
        }
        public IEnumerable<Supplier> GetAllManufecturer()
        {
            return db.Suppliers.Where(x => x.Status != 0 && x.SupplierType != 2).OrderBy(x => x.SupplierName);
        }
        public IEnumerable<Supplier> GetAllDistributor()
        {
            return db.Suppliers.Where(x => x.Status != 0 && x.SupplierType != 1).OrderBy(x => x.SupplierName);
      
        }
    }
}
