using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class PurchaseRepository
    {
        private WmsDbContext db;
        public PurchaseRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Purchase item)
        {
            db.Purchases.Add(item);
        }
        public void Edit(Purchase item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Purchase GetById(int id)
        {
            return db.Purchases.Find(id); 
        }
        public IEnumerable<Purchase> GetAll()
        {
            return db.Purchases;
        }
        public IEnumerable<Purchase> GetAll(string PurchaseNo,  string PurchaseDateFrom, string PurchaseDateTo, int? SupplierId,string TransactionMode)
        {
            var fromDate = string.IsNullOrEmpty(PurchaseDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(PurchaseDateFrom);
            var toDate = string.IsNullOrEmpty(PurchaseDateTo) ? DateTime.Now.Date : Convert.ToDateTime(PurchaseDateTo).AddDays(1);
            return db.Purchases.Where(x => (PurchaseNo == null || x.PurchaseNumber.StartsWith(PurchaseNo))
                 && (PurchaseDateFrom == null || x.PurchaseDate >= fromDate) && (PurchaseDateTo == null || x.PurchaseDate < toDate)
                  && (SupplierId == null || x.SupplierId == SupplierId) && (TransactionMode == null || x.TransactionMode.Equals(TransactionMode))).Include(o => o.Supplier).OrderByDescending(o => o.PurchaseDate).ThenByDescending(o => o.CreatedDate);
        }
        public int GetCount()
        {
            return db.Purchases.Count();
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(Purchase item)
        {
            db.Purchases.Remove(item);
        }
    }
}
