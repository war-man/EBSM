using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class CustomerProjectRepository
    {
        private WmsDbContext db;
        public CustomerProjectRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(CustomerProject item)
        {
            db.CustomerProjects.Add(item);
        }
        public void Edit(CustomerProject item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public CustomerProject GetById(int id)
        {
            return db.CustomerProjects.Find(id); 
        }
        public IEnumerable<CustomerProject> GetAll()
        {
            return db.CustomerProjects.OrderBy(x=>x.ProjectName);
        }
        public IEnumerable<CustomerProject> GetAll(int? CustomerId)
        {
            return db.CustomerProjects.Where(p => p.CustomerId == CustomerId  && p.Status != 0).OrderBy(p => p.ProjectName);
        }
        public IEnumerable<CustomerProject> GetAll(int? CustomerId, string term)
        {
            return db.CustomerProjects.Where(p => p.CustomerId == CustomerId && (p.ProjectName.StartsWith(term) || p.ProjectName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.ProjectName);
        }


    }
}
