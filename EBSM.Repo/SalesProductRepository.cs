using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class SalesProductRepository
    {
        private WmsDbContext db;
        public SalesProductRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(InvoiceProduct item)
        {
            db.InvoiceProducts.Add(item);
        }
        public void Edit(InvoiceProduct item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public InvoiceProduct GetById(int id)
        {
            return db.InvoiceProducts.Find(id); 
        }
        public InvoiceProduct GetByInvoiceAndProductId(int invoiceId, int productId)
        {
            return db.InvoiceProducts.FirstOrDefault(x=>x.InvoiceId== invoiceId&&x.ProductId==productId);
        }
        public IEnumerable<InvoiceProduct> GetByInvoiceId(int invoiceId)
        {
            return db.InvoiceProducts.Where(x=>x.InvoiceId== invoiceId);
        }
        public IEnumerable<InvoiceProduct> GetAll()
        {
            return db.InvoiceProducts;
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(InvoiceProduct item)
        {
            db.InvoiceProducts.Remove(item);
        }
        //public IEnumerable<InvoiceProduct> GetInvoicesWithoutAnyBill(int customerId)
        //{
        //    return db.Invoices.Where(x => x.CustomerId == customerId && !x.InvoiceBills.Any());
        //}
        //public int GetCount()
        //{
        //    return db.Invoices.Count();
        //}
        //public IEnumerable<Invoice> GetAll(int? CustomerId, string InvoiceNo, string InvoiceDateFrom, string InvoiceDateTo,string TransactionMode,int? SalesmanId)
        //{
        //    var fromDate = string.IsNullOrEmpty(InvoiceDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(InvoiceDateFrom);
        //    var toDate = string.IsNullOrEmpty(InvoiceDateTo) ? DateTime.Now.Date : Convert.ToDateTime(InvoiceDateTo).AddDays(1);
        //    return db.Invoices.Where(x => (TransactionMode == null || x.TransactionMode.Equals(TransactionMode)) && (InvoiceNo == null || x.InvoiceNumber.StartsWith(InvoiceNo))
        //       && (InvoiceDateFrom == null || x.InvoiceDate >= fromDate) && (InvoiceDateTo == null || x.InvoiceDate < toDate)
        //        && (CustomerId == null || x.CustomerId == CustomerId) && (SalesmanId == null || x.SalesmanId == SalesmanId)).Include(o => o.Salesman).OrderByDescending(o => o.InvoiceDate).ThenByDescending(o => o.CreatedDate);
        //}


    }
}
