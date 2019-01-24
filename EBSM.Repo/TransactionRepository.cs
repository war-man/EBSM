using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class TransactionRepository
    {
        private WmsDbContext db;
        public TransactionRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Transaction item)
        {
            db.Transactions.Add(item);
        }
        public void Edit(Transaction item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Transaction GetById(int id)
        {
            return db.Transactions.Find(id); 
        }
        public IEnumerable<Transaction> GetAll()
        {
            return db.Transactions;
        }public IEnumerable<Transaction> GetAll(string TransactionType,string TransactionMode,string TransactionDateFrom, string TransactionDateTo)
        {
            var fromDate = string.IsNullOrEmpty(TransactionDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(TransactionDateFrom);
            var toDate = string.IsNullOrEmpty(TransactionDateTo) ? DateTime.Now.Date : Convert.ToDateTime(TransactionDateTo).AddDays(1);
            return db.Transactions.ToList().Where(x => (x.TableName == "Cash" || x.TableName == "BankAccounts" || x.TableName == "MobileBangking") && (TransactionType == null || x.TypeOfTransaction == TransactionType) && (fromDate == null || x.TransactionDate.Date >= fromDate) && (toDate == null || x.TransactionDate.Date < toDate)
                && (String.IsNullOrEmpty(TransactionMode) || x.TransactionMode == TransactionMode)).OrderByDescending(x => x.CreatedDate);
        }
        public IEnumerable<Transaction> GetAll( string TransactionDateFrom, string TransactionDateTo, string TransactionType, string TransactionMode, string TransactionTable)
        {
            var fromDate = string.IsNullOrEmpty(TransactionDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(TransactionDateFrom);
            var toDate = string.IsNullOrEmpty(TransactionDateTo) ? DateTime.Now.Date : Convert.ToDateTime(TransactionDateTo).AddDays(1);
            return db.Transactions.Where(x => (TransactionDateFrom == null || x.TransactionDate >= fromDate) && (TransactionDateTo == null || x.TransactionDate <= toDate)
                 && (TransactionType == null || x.TypeOfTransaction == TransactionType) && (TransactionMode == null || x.TransactionMode == TransactionMode) && (TransactionTable == null || x.TableName == TransactionTable)).OrderByDescending(x => x.CreatedDate);
        }
        public void DepositToAccount(string transactionMode, int accountId, double amount, DateTime transactionDate, string tableName, string primaryKeyName, int primaryKeyValue, int? currentUserId, string transactionHead)
        {

            if (transactionMode == "Cash")
            {
                var cash = db.Cash.FirstOrDefault(x => x.CashId == accountId);
                if (cash != null)
                {
                    cash.Balance += amount;
                    cash.UpdatedBy = currentUserId;
                    cash.CreatedDate = DateTime.Now;
                    db.Entry(cash).State = System.Data.Entity.EntityState.Modified;
                }

            }
            else if (transactionMode == "Bank")
            {
                var bankAcc = db.BankAccounts.FirstOrDefault(x => x.BankId == accountId);
                if (bankAcc != null)
                {
                    bankAcc.Balance += amount;
                    bankAcc.UpdatedBy = currentUserId;
                    bankAcc.CreatedDate = DateTime.Now;
                    db.Entry(bankAcc).State = System.Data.Entity.EntityState.Modified;
                }

            }
            else if (transactionMode == "Mobile")
            {
                var mobAcc = db.MobileBangking.FirstOrDefault(x => x.AccountId == accountId);
                if (mobAcc != null)
                {
                    mobAcc.Balance += amount;
                    mobAcc.UpdatedBy = currentUserId;
                    mobAcc.CreatedDate = DateTime.Now;
                    db.Entry(mobAcc).State = System.Data.Entity.EntityState.Modified;
                }
            }

            db.SaveChanges(currentUserId.ToString());
          
        }
        public void WithdrawFromAccount(string transactionMode, int accountId, double amount, DateTime transactionDate, string tableName, string primaryKeyName, int primaryKeyValue, int? currentUserId, string transactionHead)
        {
            if (transactionMode == "Cash")
            {
                var cash = db.Cash.FirstOrDefault(x => x.CashId == accountId);
                if (cash != null)
                {
                    cash.Balance -= amount;
                    cash.UpdatedBy = currentUserId;
                    cash.CreatedDate = DateTime.Now;
                    db.Entry(cash).State = System.Data.Entity.EntityState.Modified;
                }

            }
            else if (transactionMode == "Bank")
            {
                var bankAcc = db.BankAccounts.FirstOrDefault(x => x.BankId == accountId);
                if (bankAcc != null)
                {
                    bankAcc.Balance -= amount;
                    bankAcc.UpdatedBy = currentUserId;
                    bankAcc.CreatedDate = DateTime.Now;
                    db.Entry(bankAcc).State = System.Data.Entity.EntityState.Modified;
                }

            }
            else if (transactionMode == "Mobile")
            {
                var mobAcc = db.MobileBangking.FirstOrDefault(x => x.AccountId == accountId);
                if (mobAcc != null)
                {
                    mobAcc.Balance -= amount;
                    mobAcc.UpdatedBy = currentUserId;
                    mobAcc.CreatedDate = DateTime.Now;
                    db.Entry(mobAcc).State = System.Data.Entity.EntityState.Modified;
                }
            }
            db.SaveChanges(currentUserId.ToString());
           
        }
    }
}
