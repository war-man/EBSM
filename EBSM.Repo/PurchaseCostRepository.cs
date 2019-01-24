using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class PurchaseCostRepository
    {
        private WmsDbContext db;
        public PurchaseCostRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(PurchaseCost item)
        {
            db.PurchaseCosts.Add(item);
        }
        public void Edit(PurchaseCost item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public PurchaseCost GetById(int id)
        {
            return db.PurchaseCosts.Find(id); 
        }
        public IEnumerable<PurchaseCost> GetAll()
        {
            return db.PurchaseCosts;
        }
        public PurchaseCost GetByPurchaseId(int purchaseId)
        {
            return db.PurchaseCosts.FirstOrDefault(x => x.PurchaseId == purchaseId);
        }
        public IEnumerable<PurchaseCost> GetAllByPurchaseId(int purchaseId)
        {
            return db.PurchaseCosts.Where(x => x.PurchaseId == purchaseId);
        }
        //public IEnumerable<Purchase> GetAll(string PurchaseNo,  string PurchaseDateFrom, string PurchaseDateTo, int? SupplierId,string TransactionMode)
        //{
        //    var fromDate = string.IsNullOrEmpty(PurchaseDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(PurchaseDateFrom);
        //    var toDate = string.IsNullOrEmpty(PurchaseDateTo) ? DateTime.Now.Date : Convert.ToDateTime(PurchaseDateTo).AddDays(1);
        //    return db.Purchases.Where(x => (PurchaseNo == null || x.PurchaseNumber.StartsWith(PurchaseNo))
        //         && (PurchaseDateFrom == null || x.PurchaseDate >= fromDate) && (PurchaseDateTo == null || x.PurchaseDate < toDate)
        //          && (SupplierId == null || x.SupplierId == SupplierId) && (TransactionMode == null || x.TransactionMode.Equals(TransactionMode))).Include(o => o.Supplier).OrderByDescending(o => o.PurchaseDate).ThenByDescending(o => o.CreatedDate);
        //}
        public int GetCount()
        {
            return db.PurchaseCosts.Count();
        }

    }
}
