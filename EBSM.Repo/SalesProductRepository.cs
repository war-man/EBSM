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
        public IEnumerable<InvoiceProduct> GetAll(string fromDate2, string toDate2, int? SelectedProductId)
        {
            var fromDate = string.IsNullOrEmpty(fromDate2) ? DateTime.Now.Date : Convert.ToDateTime(fromDate2);
            var toDate = string.IsNullOrEmpty(toDate2) ? DateTime.Now.Date : Convert.ToDateTime(toDate2).AddDays(1);
            return db.InvoiceProducts.Where(x => (fromDate2 == null||x.ProductId == SelectedProductId) && (fromDate2 == null || x.Invoice.InvoiceDate >= fromDate) && (toDate2 == null || x.Invoice.InvoiceDate < toDate)).OrderByDescending(x => x.Invoice.InvoiceDate);
        }
        public IEnumerable<InvoiceProduct> GetAllByMonth(DateTime date)
        {
            return db.InvoiceProducts.Where(x => x.Invoice.InvoiceDate.Year == date.Year && x.Invoice.InvoiceDate.Month == date.Month);
        }


    }
}
