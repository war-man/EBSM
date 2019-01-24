using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class PurchasePaymentRepository
    {
        private WmsDbContext db;
        public PurchasePaymentRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(PurchasePayment item)
        {
            db.PurchasePayments.Add(item);
        }
        public void Edit(PurchasePayment item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public PurchasePayment GetById(int id)
        {
            return db.PurchasePayments.Find(id); 
        }
        public IEnumerable<PurchasePayment> GetAll()
        {
            return db.PurchasePayments;
        }
        public PurchasePayment GetByPurchaseId(int purchaseId)
        {
            return db.PurchasePayments.FirstOrDefault(x => x.PurchaseId == purchaseId);
        }
        public IEnumerable<PurchasePayment> GetAllByPurchaseId(int purchaseId)
        {
            return db.PurchasePayments.Where(x => x.PurchaseId == purchaseId);
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
            return db.PurchasePayments.Count();
        }

    }
}
