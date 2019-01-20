using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class SalesRepository
    {
        private WmsDbContext db;
        public SalesRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Invoice invoice)
        {
            db.Invoices.Add(invoice);
        }
        public void Edit(Invoice invoice)
        {
            db.Entry(invoice).State = EntityState.Modified;
        }
        public Invoice GetById(int id)
        {
            return db.Invoices.Find(id); 
        }
        public IEnumerable<Invoice> GetAll()
        {
            return db.Invoices;
        }
        public IEnumerable<Invoice> GetInvoicesWithoutAnyBill(int customerId)
        {
            return db.Invoices.Where(x => x.CustomerId == customerId && !x.InvoiceBills.Any());
        }
        public int GetCount()
        {
            return db.Invoices.Count();
        }
        public IEnumerable<Invoice> GetAll(int? CustomerId, string InvoiceNo, string InvoiceDateFrom, string InvoiceDateTo,string TransactionMode,int? SalesmanId)
        {
            var fromDate = string.IsNullOrEmpty(InvoiceDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(InvoiceDateFrom);
            var toDate = string.IsNullOrEmpty(InvoiceDateTo) ? DateTime.Now.Date : Convert.ToDateTime(InvoiceDateTo).AddDays(1);
            return db.Invoices.Where(x => (TransactionMode == null || x.TransactionMode.Equals(TransactionMode)) && (InvoiceNo == null || x.InvoiceNumber.StartsWith(InvoiceNo))
               && (InvoiceDateFrom == null || x.InvoiceDate >= fromDate) && (InvoiceDateTo == null || x.InvoiceDate < toDate)
                && (CustomerId == null || x.CustomerId == CustomerId) && (SalesmanId == null || x.SalesmanId == SalesmanId)).Include(o => o.Salesman).OrderByDescending(o => o.InvoiceDate).ThenByDescending(o => o.CreatedDate);
        }


    }
}
