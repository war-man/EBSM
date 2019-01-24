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
            return db.Invoices.Include(x => x.Salesman).OrderByDescending(a => a.InvoiceDate).ThenByDescending(o => o.CreatedDate);
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
       
        public IEnumerable<Invoice> GetAllSalesByDate(DateTime date)
        {
            return db.Invoices.Include(t => t.Customer).Where(t => t.InvoiceDate.Year == date.Year && t.InvoiceDate.Month == date.Month && t.InvoiceDate.Day == date.Day &&t.Status!=0).OrderByDescending(t => t.InvoiceDate);
        }
        public IEnumerable<Invoice> GetAllSalesByMonth(DateTime date)
        {
            return db.Invoices.Where(g => g.InvoiceDate.Year == date.Year && g.InvoiceDate.Month == date.Month);
        }
        public IEnumerable<Invoice> GetAllSalesByYear(int year)
        {
            return db.Invoices.Where(g => g.InvoiceDate.Year == year);
        }
        public double GetSalesAmountByDate(DateTime date)
        {
            double totalAmount= GetAllSalesByDate(date).Any() ?(double) GetAllSalesByDate(date).Sum(x => x.TotalPrice) : 0;
            return totalAmount;
        }
    }
}
