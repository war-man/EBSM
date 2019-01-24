using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class MobileBankingAccountRepository
    {
        private WmsDbContext db;
        public MobileBankingAccountRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(MobileBanking mobileBanking)
        {
            db.MobileBangking.Add(mobileBanking);
        }
        public void Edit(MobileBanking mobileBanking)
        {
            db.Entry(mobileBanking).State = EntityState.Modified;
        }
        public MobileBanking GetById(int id)
        {
            return db.MobileBangking.Find(id); 
        }
        public IEnumerable<MobileBanking> GetAll()
        {
            return db.MobileBangking;
        } 

    
    }
}
