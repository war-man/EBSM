using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class CashAccountRepository
    {
        private WmsDbContext db;
        public CashAccountRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Cash cash)
        {
            db.Cash.Add(cash);
        }
        public void Edit(Cash cash)
        {
            db.Entry(cash).State = EntityState.Modified;
        }
        public Cash GetById(int id)
        {
            return db.Cash.Find(id); 
        }
        public IEnumerable<Cash> GetAll()
        {
            return db.Cash;
        }


    }
}
