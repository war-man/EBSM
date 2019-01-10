using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class ProductRepository
    {
        private WmsDbContext db;
        public ProductRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Product product)
        {
            db.Products.Add(product);
        }
        public void Edit(Product product)
        {
            db.Entry(product).State = EntityState.Modified;
        }
        public Product GetById(int id)
        {
            return db.Products.Find(id); 
        }
        public IEnumerable<Product> GetActiveProducts()
        {
            return db.Products.Where(x => x.Status != 0).OrderBy(x => x.ProductFullName);
        }
        public IEnumerable<ArticleTransfer> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        {
            var fromDate = string.IsNullOrEmpty(TransferDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateFrom);
            var toDate = string.IsNullOrEmpty(TransferDateTo) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateTo).AddDays(1);
            return db.ArticleTransfers.ToList().Where(x => (SelectedProductId == null || x.StockFrom.ProductId == SelectedProductId) && (PName == null || (x.StockFrom.Product.ProductFullName.StartsWith(PName) || x.StockFrom.Product.ProductFullName.Contains(" " + PName))) && (TransferDateFrom == null || x.TransferDate.Date >= fromDate) && (TransferDateTo == null || x.TransferDate.Date < toDate)).OrderByDescending(o => o.CreatedDate);
        } 

    
    }
}
