using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class BillRepository
    {
        private WmsDbContext db;
        public BillRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Bill bill)
        {
            db.Bills.Add(bill);
        }
        public void Edit(Bill bill)
        {
            db.Entry(bill).State = EntityState.Modified;
        }
        public Bill GetById(int id)
        {
            return db.Bills.Find(id); 
        }
        public IEnumerable<Bill> GetAll()
        {
            return db.Bills;
        }
        public IEnumerable<Bill> GetAll(string BillNo, string BillDateFrom, string BillDateTo, int? Customer)
        {
            var fromDate = string.IsNullOrEmpty(BillDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(BillDateFrom);
            var toDate = string.IsNullOrEmpty(BillDateTo) ? DateTime.Now.Date : Convert.ToDateTime(BillDateTo).AddDays(1);
            return db.Bills.Where(x => (BillDateFrom == null || x.BillDate >= fromDate) && (BillDateTo == null || x.BillDate <= toDate) && (BillNo == null || (x.BillNo.StartsWith(BillNo) || x.BillNo.Contains(BillNo))) && (Customer == null || x.CustomerId == Customer)).OrderByDescending(o => o.BillDate).ThenByDescending(o => o.CreatedDate);
        }
        public int GetCount()
        {
            return db.Bills.Count();
        }

    }
}
