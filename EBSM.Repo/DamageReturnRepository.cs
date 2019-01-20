using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class DamageReturnRepository
    {
        private WmsDbContext db;
        public DamageReturnRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(DamageReturn item)
        {
            db.DamageReturns.Add(item);
        }
        public void Edit(DamageReturn item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public DamageReturn GetById(int id)
        {
            return db.DamageReturns.Find(id); 
        }
        public IEnumerable<DamageReturn> GetAll()
        {
            return db.DamageReturns;
        }
        //public IEnumerable<DamageDismiss> GetAll(int? SelectedProductId, string ProductNameFull)
        //{
        //    return db.Damages.Where(x => (SelectedProductId == null || x.Stock.ProductId == SelectedProductId) && (ProductNameFull == null || (x.Stock.Product.ProductFullName.StartsWith(ProductNameFull) || x.Stock.Product.ProductFullName.Contains(" " + ProductNameFull)))).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.Stock.Product.ProductFullName);
        //}


    }
}
