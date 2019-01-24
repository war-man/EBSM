using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class ProductCategoryRepository
    {
        private WmsDbContext db;
        public ProductCategoryRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(ProductCategory item)
        {
            db.ProductCategories.Add(item);
        }
        public void Edit(ProductCategory item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public ProductCategory GetById(int id)
        {
            return db.ProductCategories.Find(id); 
        }
        public IEnumerable<ProductCategory> GetAll()
        {
            return db.ProductCategories;
        }
        public IEnumerable<ProductCategory> GetAllByProductId(int producId)
        {
            return db.ProductCategories.Where(x => x.ProductId == producId).Include(x => x.Category);
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(ProductCategory item)
        {
            db.ProductCategories.Remove(item);
        }


    }
}
