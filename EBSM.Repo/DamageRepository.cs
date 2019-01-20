using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class DamageRepository
    {
        private WmsDbContext db;
        public DamageRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Damage item)
        {
            db.Damages.Add(item);
        }
        public void Edit(Damage item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Damage GetById(int id)
        {
            return db.Damages.Find(id); 
        }
        public IEnumerable<Damage> GetAll()
        {
            return db.Damages;
        }
        public IEnumerable<Damage> GetAll(int? SelectedProductId, string ProductNameFull)
        {
            return db.Damages.Where(x => (SelectedProductId == null || x.Stock.ProductId == SelectedProductId) && (ProductNameFull == null || (x.Stock.Product.ProductFullName.StartsWith(ProductNameFull) || x.Stock.Product.ProductFullName.Contains(" " + ProductNameFull)))).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.Stock.Product.ProductFullName);
        }


    }
}
