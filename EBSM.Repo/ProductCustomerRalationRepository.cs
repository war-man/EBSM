using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class ProductCustomerRalationRepository
    {
        private WmsDbContext db;
        public ProductCustomerRalationRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(ProductCustomerRelation item)
        {
            db.ProductCustomerRelations.Add(item);
        }
        public void Edit(ProductCustomerRelation item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public ProductCustomerRelation GetById(int id)
        {
            return db.ProductCustomerRelations.Find(id); 
        }
        public IEnumerable<ProductCustomerRelation> GetAll()
        {
            return db.ProductCustomerRelations;
        }
        public IEnumerable<ProductCustomerRelation> GetAllByProductId(int productId)
        {
            return db.ProductCustomerRelations.Where(x => x.ProductId == productId).Include(x => x.Customer);
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(ProductCustomerRelation item)
        {
            db.ProductCustomerRelations.Remove(item);
        }
        //public IEnumerable<ArticleTransfer> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        //{
        //    var fromDate = string.IsNullOrEmpty(TransferDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateFrom);
        //    var toDate = string.IsNullOrEmpty(TransferDateTo) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateTo).AddDays(1);
        //    return db.ArticleTransfers.ToList().Where(x => (SelectedProductId == null || x.StockFrom.ProductId == SelectedProductId) && (PName == null || (x.StockFrom.Product.ProductFullName.StartsWith(PName) || x.StockFrom.Product.ProductFullName.Contains(" " + PName))) && (TransferDateFrom == null || x.TransferDate.Date >= fromDate) && (TransferDateTo == null || x.TransferDate.Date < toDate)).OrderByDescending(o => o.CreatedDate);
        //} 


    }
}
