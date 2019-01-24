using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class TransactionAccountService
    {
        private WmsDbContext _context;
        private TransactionAccountUnitOfWork _transactionAccountUnitOfWork;

        public TransactionAccountService()
        {
            _context = new WmsDbContext();
            _transactionAccountUnitOfWork = new TransactionAccountUnitOfWork(_context);
        }
        public IEnumerable<BankAccount> GetAllBankAccounts()
        {
            return _transactionAccountUnitOfWork.BankAccountRepository.GetAll();
        }
        public IEnumerable<Cash> GetAllCashAccounts()
        {
            return _transactionAccountUnitOfWork.CashAccountRepository.GetAll();
        }
        public IEnumerable<MobileBanking> GetAllMobileBankingAccounts()
        {
            return _transactionAccountUnitOfWork.MobileBankingAccountRepository.GetAll();
        }
        public BankAccount GetBankAccountById(int id)
        {
            return _transactionAccountUnitOfWork.BankAccountRepository.GetById(id);
        }
        public Cash GetCashAccountById(int id)
        {
            return _transactionAccountUnitOfWork.CashAccountRepository.GetById(id);
        }
        public MobileBanking GetMobileBankingAccountById(int id)
        {
            return _transactionAccountUnitOfWork.MobileBankingAccountRepository.GetById(id);
        }
        public int SaveBankaccount(BankAccount bankAccount, int? loggedInUserId)
        {
            _transactionAccountUnitOfWork.BankAccountRepository.Add(bankAccount);
            _transactionAccountUnitOfWork.Save(loggedInUserId.ToString());
            return bankAccount.BankId;
        }
        public int SaveMobileBankAccount(MobileBanking mobileBanking, int? loggedInUserId)
        {
            _transactionAccountUnitOfWork.MobileBankingAccountRepository.Add(mobileBanking);
            _transactionAccountUnitOfWork.Save(loggedInUserId.ToString());
            return mobileBanking.AccountId;
        }
        public void EditBankaccount(BankAccount bankAccount, int? loggedInUserId)
        {
            _transactionAccountUnitOfWork.BankAccountRepository.Edit(bankAccount);
            _transactionAccountUnitOfWork.Save(loggedInUserId.ToString());
          
        }
        public void EditMobileBankAccount(MobileBanking mobileBanking, int? loggedInUserId)
        {
            _transactionAccountUnitOfWork.MobileBankingAccountRepository.Edit(mobileBanking);
            _transactionAccountUnitOfWork.Save(loggedInUserId.ToString());
           
        }
        //public IEnumerable<User> GetAllCardNotAssignedEmployee()
        //{
        //    return _employeeUnitOfWork.EmployeeRepository.GetAllCardNotAssignedEmployee();
        //}
        //public void DeleteCostCenter(int id)
        //{
        //    _costCenterUnitOfWork.CostCenterRepository.DeleteById(id);
        //    _costCenterUnitOfWork.Save();
        //}
        //public bool IsCostCenterExist(string CostCenterName, string InitialCostCenterName)
        //{
        //    return _costCenterUnitOfWork.CostCenterRepository.IsCostCenterExist(CostCenterName, InitialCostCenterName);
        //}
        public void Dispose()
        {
            _transactionAccountUnitOfWork.Dispose();
        }
    }
}
