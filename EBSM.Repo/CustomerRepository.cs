using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class CustomerRepository
    {
        private WmsDbContext db;
        public CustomerRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Customer customer)
        {
            db.Customers.Add(customer);
        }
        public void Edit(Customer customer)
        {
            db.Entry(customer).State = EntityState.Modified;
        }
        public Customer GetById(int id)
        {
            return db.Customers.Find(id); 
        }
        public IEnumerable<Customer> GetAll()
        {
            return db.Customers.OrderBy(x=>x.FullName);
        }
        public IEnumerable<Customer> GetAll(string CustomerName, string ContactNo)
        {
            return db.Customers.Where(x => (CustomerName == null || x.FullName.StartsWith(CustomerName)) && (ContactNo == null || x.ContactNo.StartsWith(ContactNo))).OrderBy(x => x.FullName);
        }
        public IEnumerable<Customer> GetCustomersByName(string term)
        {
            return db.Customers.Where(p => (p.FullName.StartsWith(term) || p.FullName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.FullName);
        }

    }
}
