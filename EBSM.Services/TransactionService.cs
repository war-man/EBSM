using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class TransactionService
    {
        private WmsDbContext _context;
        private TransactionUnitOfWork _transactionUnitOfWork;

        public TransactionService()
        {
            _context = new WmsDbContext();
            _transactionUnitOfWork = new TransactionUnitOfWork(_context);
        }
       
        public Transaction GetTransactionById(int id)
        {
            return _transactionUnitOfWork.TransactionRepository.GetById(id);
        }
       
        public int SaveTransaction(Transaction transaction, int? loggedInUserId)
        {
            _transactionUnitOfWork.TransactionRepository.Add(transaction);
            _transactionUnitOfWork.Save(loggedInUserId.ToString());
            return transaction.TransactionId;
        }
        public void EditTransaction(Transaction transaction, int? loggedInUserId)
        {
            _transactionUnitOfWork.TransactionRepository.Edit(transaction);
            _transactionUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Transaction> GetAllTransactions()
        {
            return _transactionUnitOfWork.TransactionRepository.GetAll();
        }
        public IEnumerable<Transaction> GetAllTransactions(string TransactionType, string TransactionMode, string TransactionDateFrom, string TransactionDateTo)
        {
            return _transactionUnitOfWork.TransactionRepository.GetAll( TransactionType,  TransactionMode,  TransactionDateFrom,  TransactionDateTo);
        }public IEnumerable<Transaction> GetAllTransactions(string TransactionDateFrom, string TransactionDateTo, string TransactionType, string TransactionMode, string TransactionTable)
        {
            return _transactionUnitOfWork.TransactionRepository.GetAll(TransactionDateFrom, TransactionDateTo, TransactionType, TransactionMode, TransactionTable);
        }
        public void DepositToAccount(string transactionMode, int accountId, double amount, DateTime transactionDate, string tableName, string primaryKeyName, int primaryKeyValue, int? currentUserId, string transactionHead)
        {
            _transactionUnitOfWork.TransactionRepository.DepositToAccount( transactionMode,  accountId,  amount,  transactionDate,  tableName,  primaryKeyName,  primaryKeyValue,   currentUserId,  transactionHead);
        }
        public void WithdrawFromAccount(string transactionMode, int accountId, double amount, DateTime transactionDate, string tableName, string primaryKeyName, int primaryKeyValue, int? currentUserId, string transactionHead)
        {
            _transactionUnitOfWork.TransactionRepository.WithdrawFromAccount( transactionMode,  accountId,  amount,  transactionDate,  tableName,  primaryKeyName,  primaryKeyValue,  currentUserId,  transactionHead);
        }
            public void Dispose()
        {
            _transactionUnitOfWork.Dispose();
        }
    }
}
