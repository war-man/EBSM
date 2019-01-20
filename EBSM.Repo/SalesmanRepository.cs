using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class SalesmanRepository
    {
        private WmsDbContext db;
        public SalesmanRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Salesman item)
        {
            db.Salesman.Add(item);
        }
        public void Edit(Salesman item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Salesman GetById(int id)
        {
            return db.Salesman.Find(id); 
        }
        public IEnumerable<Salesman> GetAll()
        {
            return db.Salesman.OrderBy(x => x.FullName);
        }
        //public IEnumerable<Salesman> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        //{
        //    var fromDate = string.IsNullOrEmpty(TransferDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateFrom);
        //    var toDate = string.IsNullOrEmpty(TransferDateTo) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateTo).AddDays(1);
        //    return db.ArticleTransfers.ToList().Where(x => (SelectedProductId == null || x.StockFrom.ProductId == SelectedProductId) && (PName == null || (x.StockFrom.Product.ProductFullName.StartsWith(PName) || x.StockFrom.Product.ProductFullName.Contains(" " + PName))) && (TransferDateFrom == null || x.TransferDate.Date >= fromDate) && (TransferDateTo == null || x.TransferDate.Date < toDate)).OrderByDescending(o => o.CreatedDate);
        //} 

    
    }
}
