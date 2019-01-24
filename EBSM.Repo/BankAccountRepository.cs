using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class BankAccountRepository
    {
        private WmsDbContext db;
        public BankAccountRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(BankAccount bankAccount)
        {
            db.BankAccounts.Add(bankAccount);
        }
        public void Edit(BankAccount bankAccount)
        {
            db.Entry(bankAccount).State = EntityState.Modified;
        }
        public BankAccount GetById(int id)
        {
            return db.BankAccounts.Find(id); 
        }
        public IEnumerable<BankAccount> GetAll()
        {
            return db.BankAccounts;
        } 

    
    }
}
