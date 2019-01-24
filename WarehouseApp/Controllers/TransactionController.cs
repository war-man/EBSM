using System;

using System.Linq;

using System.Web.Mvc;

using PagedList;

using System.Web.Security;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;
using EBSM.Entities;
using EBSM.Services;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private TransactionService _transactionService = new TransactionService();
        [Roles("Global_SupAdmin")]
        [OutputCache(Duration = 30)]
        public ActionResult Index(TransactionSearchViewModel model)
        {
            var fromDate = Convert.ToDateTime(model.TransactionDateFrom);
            var toDate = Convert.ToDateTime(model.TransactionDateTo);
            var transactions = _transactionService.GetAllTransactions(model.TransactionDateFrom, model.TransactionDateTo, model.TransactionType, model.TransactionMode, model.TransactionTable).ToList();
            model.Transactions = transactions.ToPagedList(model.Page, model.PageSize);


            ViewBag.TransactionTable = new SelectList(_transactionService.GetAllTransactions().GroupBy(x => x.TableName).Select(x => new { TableName = x.FirstOrDefault().TableName }).ToList(), "TableName", "TableName");
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
            _transactionService.SaveTransaction(transaction, currentUserId);

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
            _transactionService.SaveTransaction(transaction, currentUserId);
        }

        //====deposit to acccount function==============================================================================================================
        public void DepositToAccount(string transactionMode, int accountId, double amount, DateTime transactionDate, string tableName, string primaryKeyName, int primaryKeyValue, int? currentUserId, string transactionHead)
        {
            _transactionService.DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue, currentUserId, transactionHead);
            TransactionWithdraw(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue, currentUserId, transactionHead);
        }
        //withdraw from account functtion================================================================================================================
        public void WithdrawFromAccount(string transactionMode, int accountId, double amount, DateTime transactionDate, string tableName, string primaryKeyName, int primaryKeyValue, int? currentUserId, string transactionHead)
        {
            _transactionService.WithdrawFromAccount( transactionMode,  accountId,  amount,  transactionDate,  tableName,  primaryKeyName,  primaryKeyValue,  currentUserId,  transactionHead);
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
                _transactionService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//