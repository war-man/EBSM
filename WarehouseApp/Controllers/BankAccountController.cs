using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;
using System.Data.Entity;
using NPOI.SS.Formula.Functions;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private WmsDbContext db = new WmsDbContext();
        #region list view
        [Roles("Global_SupAdmin,Configuration,Account_Add")]
        public ActionResult Index()
        {
            var cashAccounts = db.Cash.ToList();
            var bankAccounts = db.BankAccounts.ToList();
            var mobileBankings = db.MobileBangking.ToList();
        var allPriceModels = new Tuple<List<Cash>,List<BankAccount>, List<MobileBanking>>(cashAccounts, bankAccounts,mobileBankings);
         return View("../BankAccount/Index", allPriceModels); 
        }

       // list of bank account 
        public ActionResult BankAccountList()
        {            
            return View("../BankAccount/BankAccountList", db.BankAccounts.ToList());
        }

        // List of Mobile Account
        public ActionResult MobileAccountList()
        {
            return View("../BankAccount/MobileAccountList", db.MobileBangking.ToList());
        }

        public ActionResult CashAccount()
        {
            return View("../BankAccount/CashAccount", db.Cash.ToList());
        }
        #endregion
        #region acccount create
        // create of Bank Account       
     [Roles("Global_SupAdmin,Configuration,Account_Add")]
        public ActionResult CreateBankAccount()
        {
            return View("../BankAccount/CreateBankAccount");
        }


        // create Mobile acount 

       
        // save bank account 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBankAccount(BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                bankAccount.Balance = bankAccount.Balance ?? 0;
                bankAccount.Status = 1;
                bankAccount.CreatedDate = DateTime.Now;
                bankAccount.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                db.BankAccounts.Add(bankAccount);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }

            return View("../BankAccount/CreateBankAccount");
        }

         [Roles("Global_SupAdmin,Configuration,Account_Add")]
        public ActionResult CreateMobileAccount()
        {
            return View("../BankAccount/CreateMobileAccount");
        }

        // save Mobile account 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMobileAccount(MobileBanking mobileBanking)
        {
            if (ModelState.IsValid)
            {
                mobileBanking.Balance = mobileBanking.Balance ?? 0;
                mobileBanking.Status = 1;
                mobileBanking.CreatedDate = DateTime.Now;
                mobileBanking.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                db.MobileBangking.Add(mobileBanking);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }

            return View("../BankAccount/CreateMobileAccount");
        }
        #endregion
        #region edit account

        // GET: /Product/EditBankAccount/5
         [Roles("Global_SupAdmin,Configuration,Account_Add")]
        public ActionResult EditBankAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }          
            return View("../BankAccount/EditBankAccount", bankAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBankAccount(BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                BankAccount oldBankAcc = db.BankAccounts.Find(bankAccount.BankId);
                oldBankAcc.AccountName = bankAccount.AccountName;
                oldBankAcc.AccountNo = bankAccount.AccountNo;
                oldBankAcc.BankName = bankAccount.BankName;
                oldBankAcc.Branch = bankAccount.Branch;
                oldBankAcc.Status = bankAccount.Status;
                oldBankAcc.UpdatedDate = DateTime.Now;
                oldBankAcc.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                db.Entry(oldBankAcc).State = EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }
            return View("../bankAccount/EditBankAccount", bankAccount);
        }
        // GET: /Product/EditMobileAccount/5
         [Roles("Global_SupAdmin,Configuration,Account_Add")]
        public ActionResult EditMobileAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MobileBanking mobileAccount = db.MobileBangking.Find(id);
            if (mobileAccount == null)
            {
                return HttpNotFound();
            }
            return View("../BankAccount/EditMobileAccount", mobileAccount);
        }

      
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMobileAccount(MobileBanking mobileBanking)
        {
            if (ModelState.IsValid)
            {
                MobileBanking oldMobileAcc = db.MobileBangking.Find(mobileBanking.AccountId);
                oldMobileAcc.AccountName = mobileBanking.AccountName;
                oldMobileAcc.AccountNumber = mobileBanking.AccountNumber;
                oldMobileAcc.AccountOwner = mobileBanking.AccountOwner;
                oldMobileAcc.Status = mobileBanking.Status;
                oldMobileAcc.UpdatedDate = DateTime.Now;
                oldMobileAcc.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                db.Entry(oldMobileAcc).State = EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }
            return View("../bankAccount/EditMobileAccount", mobileBanking);
        }
        #endregion
        #region withdraw deposit
        [Roles("Global_SupAdmin,Withdraw_Deposit")]
        public ActionResult WithdrawDeposit()
        {
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text");
            ViewBag.TransactionModeId = new SelectList(Enumerable.Empty<SelectListItem>());
            return View("../BankAccount/WithdrawDeposit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WithdrawDeposit(WithdrawDepositViewModel model)
        {
            if (ModelState.IsValid)
            {

                string tableName = "";
                string primaryKeyName = "";
                if (model.TransactionMode == "Cash")
                {
                    tableName = "Cash";
                    primaryKeyName = "CashId";

                }
                else if (model.TransactionMode == "Bank")
                {
                    tableName = "BankAccounts";
                    primaryKeyName = "BankId";

                }
                else if (model.TransactionMode == "Mobile")
                {
                    tableName = "MobileBangking";
                    primaryKeyName = "AccountId";

                }

                //========add to account transaction as format DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue,currentUserId);============================================================================================================================
                TransactionController transaction = new TransactionController();
                if (model.TransactionType == "deposit")
                {

                    transaction.DepositToAccount(model.TransactionMode, Convert.ToInt32(model.TransactionModeId),
                        model.Amount, DateTime.Now, tableName, primaryKeyName, Convert.ToInt32(model.TransactionModeId),
                        Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),"Manual Deposit");
                }
                else if (model.TransactionType == "withdraw")
                {
                    var accountBalance =
                        AllAccountByMode(model.TransactionMode)
                            .FirstOrDefault(x => x.Id == model.TransactionModeId)
                            .Balance;
                    if (model.Amount > accountBalance)
                    {
                        ModelState.AddModelError("", "Account balance is not sufficient.");
                        ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value",
                            "Text", model.TransactionMode);
                        ViewBag.TransactionModeId =
                            new SelectList(BankAccountController.AllAccountByMode(model.TransactionMode), "Id", "Name",
                                model.TransactionModeId);
                        return View("../BankAccount/WithdrawDeposit", model);
                    
                }
                else
                {
                    transaction.WithdrawFromAccount(model.TransactionMode, Convert.ToInt32(model.TransactionModeId), model.Amount, DateTime.Now, tableName, primaryKeyName, Convert.ToInt32(model.TransactionModeId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), "Manual Deposit");
                }
            }
                return TransactionSuccess(model.TransactionType);
            }
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", model.TransactionMode);
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode(model.TransactionMode), "Id", "Name", model.TransactionModeId);
            return View("../BankAccount/WithdrawDeposit", model);
        }
        #endregion
        #region helper modules
        private ActionResult TransactionSuccess(string transactionType)
        {
           
                if (transactionType == "deposit")
                {

                    ViewBag.TransactionSuccessMessage = "Deposit successfully completed";
                }
                else if (transactionType== "withdraw")
                {
                    ViewBag.TransactionSuccessMessage = "Withdraw successfully completed";
                }
            return View("../BankAccount/TransactionSuccess");
        }
       public static List<SelectListItem> TransactionModes()

    {
        List<SelectListItem> transactionModes = new List<SelectListItem>();
        transactionModes.Add(new SelectListItem() { Text = "Select..", Value = string.Empty });
        transactionModes.Add(new SelectListItem() { Text = "Cash", Value = "Cash" });
        transactionModes.Add(new SelectListItem() { Text = "Bank", Value = "Bank" });
        transactionModes.Add(new SelectListItem() { Text = "Mobile Banking", Value = "Mobile" });

           return transactionModes;
    }

       public static List<AccountsView> AllAccountByMode(string transactionMode)
       {
           var cx =new WmsDbContext();
           List<AccountsView>accounts= new List<AccountsView>();
            if (transactionMode == "Cash")
            {
                accounts =
                    cx.Cash
                        .Select(p => new AccountsView { Id = p.CashId, Name = p.CashName, Balance = p.Balance,Status = p.Status})
                        .ToList();

               
            }
            else if (transactionMode == "Bank")
            {

                accounts = cx.BankAccounts.Select(p => new AccountsView { Id = p.BankId, Name = p.BankName + "-" + p.Branch + "-" + p.AccountNo, Balance = p.Balance, Status = p.Status }).ToList();
               
            }
            else if (transactionMode == "Mobile")
            {

                accounts = cx.MobileBangking.Select(p => new AccountsView { Id = p.AccountId, Name = p.AccountName + "-" + p.AccountNumber, Balance = p.Balance, Status = p.Status }).ToList();
               
            }

           return accounts;
       }

        public ActionResult GetAccountByMode(string transactionMode)
        {
            StringBuilder selectListString = new StringBuilder();
 
            var transaction = AllAccountByMode(transactionMode).Where(p => p.Status == 1).ToList();
            if (transactionMode == "Cash")
            {
                
                foreach (var item in transaction)
                {
                    selectListString.Append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                }

            }
            else 
            {
                selectListString.Append("<option value=''>--Select--</option>");
               
                foreach (var item in transaction)
                {
                    selectListString.Append("<option value='" + item.Id + "'>" + item.Name + "</option>");
                }

            }
           
            var data = new { selectListString = selectListString.ToString() };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public static Discount DiscountCalculator(double? discountAmount, string discountType, double? amount)
        {
            Discount discount=new Discount();
            double discountAmountWhenPercent = Math.Round(Convert.ToDouble(amount * (discountAmount / 100)), 2);
            discount.DiscountValue = discountType == "Flat" ? discountAmount: discountAmountWhenPercent;
            discount.DiscountValueShow = discountType == "Flat" ? string.Format("{0:0.00}",discountAmount)  : discountAmountWhenPercent.ToString("0.00") + "  (" + discountAmount + "%)";
            return discount;
        }
        #endregion
        #region dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
    
}

//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//