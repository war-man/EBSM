using System;

using System.Linq;

using System.Web.Mvc;

using PagedList;

using System.Web.Security;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;


namespace WarehouseApp.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private WmsDbContext db = new WmsDbContext();
        [Roles("Global_SupAdmin")]
        [OutputCache(Duration = 30)]
        public ActionResult Index(TransactionSearchViewModel model)
        {
            var fromDate = Convert.ToDateTime(model.TransactionDateFrom);
            var toDate = Convert.ToDateTime(model.TransactionDateTo);
            var transactions = db.Transactions.Where(x => (model.TransactionDateFrom == null || x.TransactionDate >= fromDate) && (model.TransactionDateTo == null || x.TransactionDate <= toDate)
                 && (model.TransactionType == null || x.TypeOfTransaction == model.TransactionType) && (model.TransactionMode == null || x.TransactionMode == model.TransactionMode) && (model.TransactionTable == null || x.TableName == model.TransactionTable)).OrderByDescending(x => x.CreatedDate).ToList();
            model.Transactions = transactions.ToPagedList(model.Page, model.PageSize);


            ViewBag.TransactionTable = new SelectList(db.Transactions.GroupBy(x => x.TableName).Select(x => new { TableName = x.FirstOrDefault().TableName }).ToList(), "TableName", "TableName");
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text");

            return View("../Shop/Transaction/Index", model);
        } 

        public void TransactionDeposit(string transactionMode, int accountId, double amount, DateTime transactionDate, string tableName, string primaryKeyName, int primaryKeyValue, int? currentUserId, string transactionHead)
        {
         
            Transaction transaction=new Transaction()
            {
                TransactionHead=transactionHead,
                TransactionMode = transactionMode,
                TransactionModeId = accountId,
                Amount = amount,
                TransactionDate = transactionDate,
                TableName = tableName,
                PrimaryKeyName = primaryKeyName,
                PrimaryKeyValue = primaryKeyValue,
                TypeOfTransaction = "deposit",
                CreatedBy = currentUserId,
                CreatedDate = DateTime.Now,
            };
            db.Transactions.Add(transaction);
            db.SaveChanges(currentUserId.ToString());
        }
        public void TransactionWithdraw(string transactionMode, int accountId, double amount, DateTime transactionDate, string tableName, string primaryKeyName, int primaryKeyValue, int? currentUserId,string transactionHead)
        {
       
            Transaction transaction = new Transaction()
            {
                TransactionHead = transactionHead,
                TransactionMode = transactionMode,
                TransactionModeId = accountId,
                Amount = amount,
                TransactionDate = transactionDate,
                TableName = tableName,
                PrimaryKeyName = primaryKeyName,
                PrimaryKeyValue = primaryKeyValue,
                TypeOfTransaction = "withdraw",
                CreatedBy =currentUserId,
                CreatedDate = DateTime.Now,
            };
            db.Transactions.Add(transaction);
            db.SaveChanges(currentUserId.ToString());
        }

        //====deposit to acccount function==============================================================================================================
        public void DepositToAccount(string transactionMode, int accountId, double amount, DateTime transactionDate, string tableName, string primaryKeyName, int primaryKeyValue, int? currentUserId, string transactionHead)
        {
     
            if (transactionMode == "Cash")
            {
                var cash = db.Cash.FirstOrDefault(x => x.CashId == accountId);
                if (cash != null)
                {
                    cash.Balance += amount;
                    cash.UpdatedBy =currentUserId;
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
            TransactionDeposit(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue, currentUserId,transactionHead );
            
        }

        //withdraw from account functtion================================================================================================================
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
            TransactionWithdraw(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue, currentUserId, transactionHead);
        }

        public static string BillingMonthString(string date)
        {
            
            string[] months = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };
               
            string result = "";
            var currentDate = Convert.ToDateTime(date);
            result = months[currentDate.Month-1] + currentDate.ToString("yy");
            return result;
        }

        //public static double FindVat(string deptName)
        //{
        //     var cx = new WmsDbContext();
        //    double vatPercentage = 0;
        //    Department department = cx.Departments.FirstOrDefault(x=>x.DepartmentName.ToLower()==deptName.ToLower());
        //    if (department != null)
        //    {
        //        Vat vat = cx.Vats.FirstOrDefault(x => x.DepartmentId == department.DepartmentId);
        //        if (vat != null)
        //        {
        //            vatPercentage = vat.VatPercentage;
        //        }
        //    }
        //    return vatPercentage;
        //}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//