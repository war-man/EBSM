using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class BillInvoicesRepository
    {
        private WmsDbContext db;
        public BillInvoicesRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(InvoiceBill item)
        {
            db.InvoiceBills.Add(item);
        }
        public void Edit(InvoiceBill item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public InvoiceBill GetById(int id)
        {
            return db.InvoiceBills.Find(id); 
        }
        public IEnumerable<InvoiceBill> GetAll()
        {
            return db.InvoiceBills;
        }
       
        public int GetCount()
        {
            return db.InvoiceBills.Count();
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(InvoiceBill item)
        {
            db.InvoiceBills.Remove(item);
        }
    }
}
