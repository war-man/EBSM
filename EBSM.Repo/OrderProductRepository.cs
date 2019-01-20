using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class OrderProductRepository
    {
        private WmsDbContext db;
        public OrderProductRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(OrderProduct item)
        {
            db.OrderProducts.Add(item);
        }
        public void Edit(OrderProduct item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public OrderProduct GetById(int id)
        {
            return db.OrderProducts.Find(id); 
        }
        public IEnumerable<OrderProduct> GetAll()
        {
            return db.OrderProducts;
        }
        //public IEnumerable<ArticleTransfer> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        //{
        //    var fromDate = string.IsNullOrEmpty(TransferDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateFrom);
        //    var toDate = string.IsNullOrEmpty(TransferDateTo) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateTo).AddDays(1);
        //    return db.ArticleTransfers.ToList().Where(x => (SelectedProductId == null || x.StockFrom.ProductId == SelectedProductId) && (PName == null || (x.StockFrom.Product.ProductFullName.StartsWith(PName) || x.StockFrom.Product.ProductFullName.Contains(" " + PName))) && (TransferDateFrom == null || x.TransferDate.Date >= fromDate) && (TransferDateTo == null || x.TransferDate.Date < toDate)).OrderByDescending(o => o.CreatedDate);
        //} 

    
    }
}
