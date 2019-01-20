using System;
using System.Linq;
using System.Net;
using System.Text;
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
    public class ExpenseController : Controller
    {
        private ExpenseService _expenseService = new ExpenseService();
        private ExpenseTypeService _expenseTypeService = new ExpenseTypeService();
        private TransactionAccountService _transactionAccountService = new TransactionAccountService();

         [Roles("Global_SupAdmin,Expenses_Entry,Expenses_Edit")]
         [OutputCache(Duration = 30)]
        public ActionResult Index(ExpenseSearchViewModel model)
        {
            var expenses = _expenseService.GetAll(model.ExpenseTypeId,model.ExpenseDateFrom,model.ExpenseDateTo).ToList();
            model.Expenses = expenses.ToPagedList(model.Page, model.PageSize);

            ViewBag.TotalExpense = expenses.Sum(x => x.Amount);
            ViewBag.ExpenseTypeId = new SelectList(_expenseTypeService.GetChildExpenseTypes(), "ExpenseTypeId", "TypeName");

            return View("../Shop/Expense/Index", model);
        } 

        // GET: 
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult ExpenseTypeList()
        {
            var expenseTypes = _expenseTypeService.GetAll().ToList();
            return View("../Shop/Expense/ExpenseTypeList", expenseTypes);
        }
        

       public ActionResult CreateExpenseType()
        {
            return View("../Shop/Expense/ConfExpenseType");
        }

        // POST: 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateExpenseType(ExpenseType expenseType)
        {
            if (ModelState.IsValid)
            {
                expenseType.Status = 1;
                expenseType.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                expenseType.CreatedDate = DateTime.Now;
                _expenseTypeService.Save(expenseType, AuthenticatedUser.GetUserFromIdentity().UserId);
                //db.ExpenseTypes.Add(expenseType);
                // db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("ExpenseTypeList");
            }

            return View("../Shop/Expense/ConfExpenseType", expenseType);
        }
        public ActionResult CreateExpenseName(ExpenseType expenseType)
        {
            if (ModelState.IsValid)
            {
                var isExist = "No";
                var existExpenseTypes = _expenseTypeService.ExistExpenseTypes(expenseType.TypeName);
                if (!existExpenseTypes)
                {

                    expenseType.Status = 1;
                    expenseType.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                    expenseType.CreatedDate = DateTime.Now;
                    expenseType.ExpenseTypeId = _expenseTypeService.Save(expenseType,AuthenticatedUser.GetUserFromIdentity().UserId);
                 
                    var data = new {expenseType = expenseType, isExist = isExist};
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    isExist = "Yes";
                    var data = new { isExist = isExist };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxCreateExpenseType(ExpenseType expenseType)
        {
            if (ModelState.IsValid)
            {
                var isExist = "No";
                var existExpenseTypes = _expenseTypeService.ExistExpenseTypes(expenseType.TypeName);
                if (!existExpenseTypes)
                {
                    expenseType.Status = 1;
                    expenseType.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                    expenseType.CreatedDate = DateTime.Now;
                    expenseType.ExpenseTypeId= _expenseTypeService.Save(expenseType, AuthenticatedUser.GetUserFromIdentity().UserId);
                    //db.ExpenseTypes.Add(expenseType);
                    // db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                    var data = new { expenseType = expenseType, isExist = isExist };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    isExist = "Yes";
                    var data = new { isExist = isExist };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateExpenseType(int? id, string name)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            ExpenseType expenseType = _expenseTypeService.GetExpenseTypeById(id.Value);
            if (expenseType == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }
            expenseType.TypeName = name;
            expenseType.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
            expenseType.UpdatedDate = DateTime.Now;
            _expenseTypeService.Edit(expenseType, AuthenticatedUser.GetUserFromIdentity().UserId);
           
            return Json(new { Result = "OK" });
        }
        public ActionResult UpdateExpenseTypeStatus(int? id, string tag)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            ExpenseType expenseType = _expenseTypeService.GetExpenseTypeById(id.Value);
            if (expenseType == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }
            if (tag == "inactive")
            {
                expenseType.Status = 0;
                if (expenseType.ExpenseTypes.Count > 0)
                {
                    foreach (var i in expenseType.ExpenseTypes)
                    {
                        i.Status = 0;
                    }
                }
            }
            else 
            {
                expenseType.Status = 1;
                if (expenseType.ExpenseTypes.Count > 0)
                {
                    foreach (var i in expenseType.ExpenseTypes)
                    {
                        i.Status = 1;
                    }
                }
            }

            expenseType.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
            expenseType.UpdatedDate = DateTime.Now;
            _expenseTypeService.Edit(expenseType, AuthenticatedUser.GetUserFromIdentity().UserId);
            //db.Entry(expenseType).State = System.Data.Entity.EntityState.Modified;
            // db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
            return Json(new { Result = "OK" });
        }

        protected ActionResult CreateExpenseName(int expenseTypeId)
        {
            ExpenseType exp = _expenseTypeService.GetExpenseTypeById(expenseTypeId);
            return View("../Shop/Expense/ConfExpenseType", exp);
        }

        [Roles("Global_SupAdmin,Expenses_Entry,Expenses_Edit")]
        public ActionResult CreateExpense()
        {
            ViewBag.ExpenseTypeParent = new SelectList(_expenseTypeService.GetParentExpenseTypes(), "ExpenseTypeId", "TypeName");
            ViewBag.ExpenseTypeId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text");
            ViewBag.TransactionModeId = new SelectList(Enumerable.Empty<SelectListItem>());
            return View("../Shop/Expense/Create");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
         public ActionResult CreateExpense(ExpenseViewModel objExpense)
        {
            if (ModelState.IsValid)
            {
                Expense expense = new Expense();

                expense.ExpenseTypeId = objExpense.ExpenseTypeId;
                expense.ExpenseBy = objExpense.ExpenseBy;
                expense.Amount = objExpense.Amount;
                expense.Description = objExpense.Description;
                expense.ExpenseDate = !String.IsNullOrEmpty(objExpense.ExpenseDate)? Convert.ToDateTime(objExpense.ExpenseDate): (DateTime?) null;
                expense.TransactionMode = objExpense.TransactionMode;
                expense.TransactionModeId = objExpense.TransactionModeId;
                expense.Status = 1;
                expense.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                expense.CreatedDate = DateTime.Now;
                _expenseService.Save(expense, AuthenticatedUser.GetUserFromIdentity().UserId);
                //db.Expenses.Add(expense);
                //db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                TransactionController transaction = new TransactionController();

                //========add to account transaction as format DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue,currentUserId);============================================================================================================================
                string expenseName =_expenseTypeService.GetExpenseTypeById(expense.ExpenseTypeId).TypeName;
                transaction.WithdrawFromAccount(expense.TransactionMode, Convert.ToInt32(expense.TransactionModeId), Convert.ToDouble(expense.Amount), Convert.ToDateTime(expense.ExpenseDate), "Expenses", "ExpenseId", Convert.ToInt32(expense.ExpenseId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), expenseName);
               
                return RedirectToAction("Index", "Expense");
            }
            var expenseType =Convert.ToInt32(Request["ExpenseTypeParent"]);

            ViewBag.ExpenseTypeParent = new SelectList(_expenseTypeService.GetParentExpenseTypes(), "ExpenseTypeId", "TypeName", expenseType);
            ViewBag.ExpenseTypeId = new SelectList(_expenseTypeService.GetAllByParentId(expenseType), "ExpenseTypeId", "TypeName",objExpense.ExpenseTypeId);
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text",objExpense.TransactionMode);
            if (objExpense.TransactionMode == "Cash")
            {
                ViewBag.TransactionModeId = new SelectList(_transactionAccountService.GetAllCashAccounts().Where(x => x.Status == 1), "CashId", "CashName", objExpense.TransactionModeId);   
            }
            else if (objExpense.TransactionMode == "Bank")
            {
                ViewBag.TransactionModeId = new SelectList(_transactionAccountService.GetAllBankAccounts().Where(x => x.Status == 1).Select(x => new { bankId = x.BankId, account = x.BankName + "-" + x.Branch + "-" + x.AccountNo }), "bankId", "account", objExpense.TransactionModeId);   
            }
            else if (objExpense.TransactionMode == "Mobile")
            {
                ViewBag.TransactionModeId = new SelectList(_transactionAccountService.GetAllMobileBankingAccounts().Where(x => x.Status == 1), "AccountId", "AccountName", objExpense.TransactionModeId);   
            }
            return View("../Shop/Expense/Create", objExpense);

        }
        [Roles("Global_SupAdmin,Expenses_Edit")]
        public ActionResult EditExpense(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = _expenseService.GetExpenseById(id.Value);
            if (expense == null)
            {
                return HttpNotFound();
            }
            ExpenseViewModel expenseModel=new ExpenseViewModel()
            {
                ExpenseId = expense.ExpenseId,
                ExpenseTypeId = expense.ExpenseTypeId,
                ExpenseBy = expense.ExpenseBy,
                Amount = expense.Amount,
                Description = expense.Description,
                ExpenseDate = string.Format("{0:dd-MM-yyyy}", expense.ExpenseDate),
                TransactionMode = expense.TransactionMode,
                TransactionModeId = Convert.ToInt32(expense.TransactionModeId),
                Status = expense.Status,
            };
            var expenseType = Convert.ToInt32(expense.ExpenseType.ParentType.ExpenseTypeId);

            ViewBag.ExpenseTypeParent = new SelectList(_expenseTypeService.GetParentExpenseTypes(), "ExpenseTypeId", "TypeName", expenseType);
            ViewBag.ExpenseTypeId = new SelectList(_expenseTypeService.GetAllByParentId(expenseType), "ExpenseTypeId", "TypeName", expenseModel.ExpenseTypeId);
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", expenseModel.TransactionMode);
            if (expenseModel.TransactionMode == "Cash")
            {
                ViewBag.TransactionModeId = new SelectList(_transactionAccountService.GetAllCashAccounts().Where(x => x.Status == 1), "CashId", "CashName", expenseModel.TransactionModeId);
            }
            else if (expenseModel.TransactionMode == "Bank")
            {
                ViewBag.TransactionModeId = new SelectList(_transactionAccountService.GetAllBankAccounts().Where(x => x.Status == 1).Select(x => new { bankId = x.BankId, account = x.BankName + "-" + x.Branch + "-" + x.AccountNo }), "bankId", "account", expenseModel.TransactionModeId);
            }
            else if (expenseModel.TransactionMode == "Mobile")
            {
                ViewBag.TransactionModeId = new SelectList(_transactionAccountService.GetAllMobileBankingAccounts().Where(x => x.Status == 1), "AccountId", "AccountName", expenseModel.TransactionModeId);
            }
            return View("../Shop/Expense/Edit", expenseModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditExpense(ExpenseViewModel objExpense)
        {
            if (ModelState.IsValid)
            {
                Expense expense = _expenseService.GetExpenseById(objExpense.ExpenseId.Value);
                var oldAmount = expense.Amount;
                expense.ExpenseTypeId = objExpense.ExpenseTypeId;
                expense.ExpenseBy = objExpense.ExpenseBy;
                expense.Amount = objExpense.Amount;
                expense.Description = objExpense.Description;
                expense.ExpenseDate = !String.IsNullOrEmpty(objExpense.ExpenseDate) ? Convert.ToDateTime(objExpense.ExpenseDate) : (DateTime?)null;
                expense.TransactionMode = objExpense.TransactionMode;
                expense.TransactionModeId = objExpense.TransactionModeId;
                expense.Status = objExpense.Status;
                expense.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                expense.UpdatedDate = DateTime.Now;
                _expenseService.Edit(expense, AuthenticatedUser.GetUserFromIdentity().UserId);
                //db.Entry(expense).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());


                TransactionController transaction = new TransactionController();

                //========add to account transaction as format DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue,currentUserId);============================================================================================================================
                if ((expense.Amount - oldAmount) != 0) {
                    transaction.WithdrawFromAccount(expense.TransactionMode, Convert.ToInt32(expense.TransactionModeId), Convert.ToDouble(expense.Amount - oldAmount), Convert.ToDateTime(expense.ExpenseDate), "Expenses", "ExpenseId", Convert.ToInt32(expense.ExpenseId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), expense.ExpenseType.TypeName);
                }
                return RedirectToAction("Index");
            }
            var expenseType = Convert.ToInt32(Request["ExpenseTypeParent"]);

            ViewBag.ExpenseTypeParent = new SelectList(_expenseTypeService.GetParentExpenseTypes(), "ExpenseTypeId", "TypeName", expenseType);
            ViewBag.ExpenseTypeId = new SelectList(_expenseTypeService.GetAllByParentId(expenseType), "ExpenseTypeId", "TypeName", objExpense.ExpenseTypeId);
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", objExpense.TransactionMode);
            if (objExpense.TransactionMode == "Cash")
            {
                ViewBag.TransactionModeId = new SelectList(_transactionAccountService.GetAllCashAccounts().Where(x => x.Status == 1), "CashId", "CashName", objExpense.TransactionModeId);
            }
            else if (objExpense.TransactionMode == "Bank")
            {
                ViewBag.TransactionModeId = new SelectList(_transactionAccountService.GetAllBankAccounts().Where(x => x.Status == 1).Select(x => new { bankId = x.BankId, account = x.BankName + "-" + x.Branch + "-" + x.AccountNo }), "bankId", "account", objExpense.TransactionModeId);
            }
            else if (objExpense.TransactionMode == "Mobile")
            {
                ViewBag.TransactionModeId = new SelectList(_transactionAccountService.GetAllMobileBankingAccounts().Where(x => x.Status == 1), "AccountId", "AccountName", objExpense.TransactionModeId);
            }
           return View("../Shop/Expense/Edit", objExpense);
        }
        public ActionResult GetExpenseNameByTypeId(int? typeId)
        {
            var expenseNames =_expenseTypeService.GetAllByParentId(typeId).ToList();
            StringBuilder selectListString = new StringBuilder();
            selectListString.Append("<option value=''>--Select--</option>");
            foreach (var item in expenseNames)
            {
                selectListString.Append("<option value='" + item.ExpenseTypeId + "'>" + item.TypeName + "</option>");
            }
            var data = new { selectListString = selectListString.ToString() };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _expenseTypeService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Al Monsur
//Creation Date : January 2017
//=======================================================================================//