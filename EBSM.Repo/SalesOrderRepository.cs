using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class SalesOrderRepository
    {
        private WmsDbContext db;
        public SalesOrderRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(SalesOrder item)
        {
            db.SalesOrders.Add(item);
        }
        public void Edit(SalesOrder item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public SalesOrder GetById(int id)
        {
            return db.SalesOrders.Find(id); 
        }
        public IEnumerable<SalesOrder> GetAll()
        {
            return db.SalesOrders;
        }
        public IEnumerable<SalesOrder> GetAll(string OrderNo, string OrderDateFrom, string OrderDateTo, int? CustomerId, byte? Status)
        {
            var fromDate = string.IsNullOrEmpty(OrderDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(OrderDateFrom);
            var toDate = string.IsNullOrEmpty(OrderDateTo) ? DateTime.Now.Date : Convert.ToDateTime(OrderDateTo).AddDays(1);
            return db.SalesOrders.Where(x => (OrderNo == null || x.OrderNumber.StartsWith(OrderNo))
                && (OrderDateFrom == null || x.OrderDate >= fromDate) && (OrderDateTo == null || x.OrderDate < toDate)
                 && (CustomerId == null || x.CustomerId == CustomerId) && (Status == null || x.Status == Status)).OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.CreatedDate);
        }
        public int GetCount()
        {
            return db.SalesOrders.Count();
        } public int GetPendingOrdersCount()
        {
            return db.SalesOrders.Count(t => t.Status == 1);
        }

    }
}
