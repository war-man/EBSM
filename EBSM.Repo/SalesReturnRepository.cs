using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class SalesReturnRepository
    {
        private WmsDbContext db;
        public SalesReturnRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Return item)
        {
            db.Returns.Add(item);
        }
        public void Edit(Return item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Return GetById(int id)
        {
            return db.Returns.Find(id); 
        }
       
        public IEnumerable<Return> GetAll()
        {
            return db.Returns;
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(Return item)
        {
            db.Returns.Remove(item);
        }
        //public IEnumerable<InvoiceProduct> GetInvoicesWithoutAnyBill(int customerId)
        //{
        //    return db.Invoices.Where(x => x.CustomerId == customerId && !x.InvoiceBills.Any());
        //}
        //public int GetCount()
        //{
        //    return db.Invoices.Count();
        //}
        public IEnumerable<Return> GetAll(int? CustomerId, string InvoiceNo, string ReturnDateFrom, string ReturnDateTo)
        {
            var fromDate = string.IsNullOrEmpty(ReturnDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(ReturnDateFrom);
            var toDate = string.IsNullOrEmpty(ReturnDateTo) ? DateTime.Now.Date : Convert.ToDateTime(ReturnDateTo).AddDays(1);
            return db.Returns.Where(x => (InvoiceNo == null || x.Invoice.InvoiceNumber.StartsWith(InvoiceNo))
                && (ReturnDateFrom == null || x.CreatedDate >= fromDate) && (ReturnDateTo == null || x.CreatedDate < toDate)
                 && (CustomerId == null || x.Invoice.CustomerId == CustomerId)).Include(x => x.Invoice).OrderByDescending(o => o.CreatedDate);
        }


    }
}
