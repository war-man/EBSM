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
        public IEnumerable<Salesman> GetAll(string SalesmanName)
        {

            return db.Salesman.Where(x => (SalesmanName == null || x.FullName.StartsWith(SalesmanName))).OrderBy(x => x.FullName);
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(Salesman item)
        {
            db.Salesman.Remove(item);
        }

    }
}
