using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class DamageDismissRepository
    {
        private WmsDbContext db;
        public DamageDismissRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(DamageDismiss item)
        {
            db.DamageDismisses.Add(item);
        }
        public void Edit(DamageDismiss item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public DamageDismiss GetById(int id)
        {
            return db.DamageDismisses.Find(id); 
        }
        public IEnumerable<DamageDismiss> GetAll()
        {
            return db.DamageDismisses;
        }
        //public IEnumerable<DamageDismiss> GetAll(int? SelectedProductId, string ProductNameFull)
        //{
        //    return db.Damages.Where(x => (SelectedProductId == null || x.Stock.ProductId == SelectedProductId) && (ProductNameFull == null || (x.Stock.Product.ProductFullName.StartsWith(ProductNameFull) || x.Stock.Product.ProductFullName.Contains(" " + ProductNameFull)))).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.Stock.Product.ProductFullName);
        //}


    }
}
