using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class ProductTransferRepository
    {
        private WmsDbContext db;
        public ProductTransferRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(TransferProduct item)
        {
            db.TransferProducts.Add(item);
        }
        public void Edit(TransferProduct item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public TransferProduct GetById(int id)
        {
            return db.TransferProducts.Find(id); 
        }
        public IEnumerable<TransferProduct> GetAll()
        {
            return db.TransferProducts;
        }
        public IEnumerable<TransferProduct> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        {
            var fromDate = string.IsNullOrEmpty(TransferDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateFrom);
            var toDate = string.IsNullOrEmpty(TransferDateTo) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateTo).AddDays(1);
            return db.TransferProducts.ToList().Where(x => (SelectedProductId == null || x.Stock.ProductId == SelectedProductId) && (PName == null || (x.Stock.Product.ProductFullName.StartsWith(PName) || x.Stock.Product.ProductFullName.Contains(" " + PName))) && (TransferDateFrom == null || x.TransferDate.Date >= fromDate) && (TransferDateTo == null || x.TransferDate.Date < toDate)).OrderByDescending(o => o.CreatedDate);
        }


    }
}
