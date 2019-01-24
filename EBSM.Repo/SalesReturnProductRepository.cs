using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class SalesReturnProductRepository
    {
        private WmsDbContext db;
        public SalesReturnProductRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(ReturnProduct item)
        {
            db.ReturnProducts.Add(item);
        }
        public void Edit(ReturnProduct item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public ReturnProduct GetById(int id)
        {
            return db.ReturnProducts.Find(id); 
        }
       
        public IEnumerable<ReturnProduct> GetAll()
        {
            return db.ReturnProducts;
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(ReturnProduct item)
        {
            db.ReturnProducts.Remove(item);
        }
 
        


    }
}
