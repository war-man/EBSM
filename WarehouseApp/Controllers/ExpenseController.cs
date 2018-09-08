using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using PagedList;

using System.Web.Security;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;


namespace WarehouseApp.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

         [Roles("Global_SupAdmin,Expenses_Entry,Expenses_Edit")]
         [OutputCache(Duration = 30)]
        public ActionResult Index(ExpenseSearchViewModel model)
        {
            var fromDate = Convert.ToDateTime(model.ExpenseDateFrom);
            var toDate = Convert.ToDateTime(model.ExpenseDateTo);
            var expenses = db.Expenses.Where(x => (model.ExpenseDateFrom == null || x.ExpenseDate >= fromDate) && (model.ExpenseDateTo == null || x.ExpenseDate <= toDate)
                 && (model.ExpenseTypeId == null || x.ExpenseTypeId == model.ExpenseTypeId)).OrderByDescending(x => x.ExpenseDate).ToList();
            model.Expenses = expenses.ToPagedList(model.Page, model.PageSize);

            ViewBag.TotalExpense = expenses.Sum(x => x.Amount);
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes.Where(x => x.ParentId.HasValue), "ExpenseTypeId", "TypeName");

            return View("../Shop/Expense/Index", model);
        } 

        // GET: 
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult ExpenseTypeList()
        {
            var expenseTypes = db.ExpenseTypes.ToList();
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
                expenseType.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                expenseType.CreatedDate = DateTime.Now;
                db.ExpenseTypes.Add(expenseType);
                 db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("ExpenseTypeList");
            }

            return View("../Shop/Expense/ConfExpenseType", expenseType);
        }
        public ActionResult CreateExpenseName(ExpenseType expenseType)
        {
            if (ModelState.IsValid)
            {
                var isExist = "No";
                var allExpenseTypes = db.ExpenseTypes.Any(e=>e.TypeName==expenseType.TypeName);
                if (!allExpenseTypes)
                {

                    expenseType.Status = 1;
                    expenseType.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                    expenseType.CreatedDate = DateTime.Now;
                    db.ExpenseTypes.Add(expenseType);
                     db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
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
                var allExpenseTypes = db.ExpenseTypes.Any(e => e.TypeName == expenseType.TypeName);
                if (!allExpenseTypes)
                {
                    expenseType.Status = 1;
                    expenseType.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                    expenseType.CreatedDate = DateTime.Now;
                    db.ExpenseTypes.Add(expenseType);
                     db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
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
            ExpenseType expenseType = db.ExpenseTypes.Find(id);
            if (expenseType == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }
            expenseType.TypeName = name;
            expenseType.UpdatedBy=Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
            expenseType.UpdatedDate = DateTime.Now;
            db.Entry(expenseType).State = System.Data.Entity.EntityState.Modified;
             db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
            return Json(new { Result = "OK" });
        }
        public ActionResult UpdateExpenseTypeStatus(int? id, string tag)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            ExpenseType expenseType = db.ExpenseTypes.Find(id);
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
            db.Entry(expenseType).State = System.Data.Entity.EntityState.Modified;
             db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
            return Json(new { Result = "OK" });
        }

        protected ActionResult CreateExpenseName(int expenseTypeId)
        {
            ExpenseType exp = db.ExpenseTypes.Find(expenseTypeId);
            return View("../Shop/Expense/ConfExpenseType", exp);
        }

        [Roles("Global_SupAdmin,Expenses_Entry,Expenses_Edit")]
        public ActionResult CreateExpense()
        {
            ViewBag.ExpenseTypeParent = new SelectList(db.ExpenseTypes.Where(x => !x.ParentId.HasValue), "ExpenseTypeId", "TypeName");
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
                expense.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                expense.CreatedDate = DateTime.Now;
                db.Expenses.Add(expense);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                TransactionController transaction = new TransactionController();

                //========add to account transaction as format DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue,currentUserId);============================================================================================================================
                string expenseName = db.ExpenseTypes.Find(expense.ExpenseTypeId).TypeName;
                transaction.WithdrawFromAccount(expense.TransactionMode, Convert.ToInt32(expense.TransactionModeId), Convert.ToDouble(expense.Amount), Convert.ToDateTime(expense.ExpenseDate), "Expenses", "ExpenseId", Convert.ToInt32(expense.ExpenseId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), expenseName);
               
                return RedirectToAction("Index", "Expense");
            }
            var expenseType =Convert.ToInt32(Request["ExpenseTypeParent"]);

            ViewBag.ExpenseTypeParent = new SelectList(db.ExpenseTypes.Where(x => !x.ParentId.HasValue), "ExpenseTypeId", "TypeName", expenseType);
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes.Where(x => x.ParentId == expenseType && x.Status==1), "ExpenseTypeId", "TypeName",objExpense.ExpenseTypeId);
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text",objExpense.TransactionMode);
            if (objExpense.TransactionMode == "Cash")
            {
                ViewBag.TransactionModeId = new SelectList(db.Cash.Where(x => x.Status == 1), "CashId", "CashName", objExpense.TransactionModeId);   
            }
            else if (objExpense.TransactionMode == "Bank")
            {
                ViewBag.TransactionModeId = new SelectList(db.BankAccounts.Where(x => x.Status == 1).Select(x => new { bankId = x.BankId, account = x.BankName + "-" + x.Branch + "-" + x.AccountNo }), "bankId", "account", objExpense.TransactionModeId);   
            }
            else if (objExpense.TransactionMode == "Mobile")
            {
                ViewBag.TransactionModeId = new SelectList(db.MobileBangking.Where(x => x.Status == 1), "AccountId", "AccountName", objExpense.TransactionModeId);   
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
            Expense expense = db.Expenses.Find(id);
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

            ViewBag.ExpenseTypeParent = new SelectList(db.ExpenseTypes.Where(x => !x.ParentId.HasValue), "ExpenseTypeId", "TypeName", expenseType);
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes.Where(x => x.ParentId == expenseType && x.Status == 1), "ExpenseTypeId", "TypeName", expenseModel.ExpenseTypeId);
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", expenseModel.TransactionMode);
            if (expenseModel.TransactionMode == "Cash")
            {
                ViewBag.TransactionModeId = new SelectList(db.Cash.Where(x => x.Status == 1), "CashId", "CashName", expenseModel.TransactionModeId);
            }
            else if (expenseModel.TransactionMode == "Bank")
            {
                ViewBag.TransactionModeId = new SelectList(db.BankAccounts.Where(x => x.Status == 1).Select(x => new { bankId = x.BankId, account = x.BankName + "-" + x.Branch + "-" + x.AccountNo }), "bankId", "account", expenseModel.TransactionModeId);
            }
            else if (expenseModel.TransactionMode == "Mobile")
            {
                ViewBag.TransactionModeId = new SelectList(db.MobileBangking.Where(x => x.Status == 1), "AccountId", "AccountName", expenseModel.TransactionModeId);
            }
            return View("../Shop/Expense/Edit", expenseModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditExpense(ExpenseViewModel objExpense)
        {
            if (ModelState.IsValid)
            {
                Expense expense = db.Expenses.Find(objExpense.ExpenseId);
                var oldAmount = expense.Amount;
                expense.ExpenseTypeId = objExpense.ExpenseTypeId;
                expense.ExpenseBy = objExpense.ExpenseBy;
                expense.Amount = objExpense.Amount;
                expense.Description = objExpense.Description;
                expense.ExpenseDate = !String.IsNullOrEmpty(objExpense.ExpenseDate) ? Convert.ToDateTime(objExpense.ExpenseDate) : (DateTime?)null;
                expense.TransactionMode = objExpense.TransactionMode;
                expense.TransactionModeId = objExpense.TransactionModeId;
                expense.Status = objExpense.Status;
                expense.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                expense.UpdatedDate = DateTime.Now;
                db.Entry(expense).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());


                TransactionController transaction = new TransactionController();

                //========add to account transaction as format DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue,currentUserId);============================================================================================================================
                if ((expense.Amount - oldAmount) != 0) {
                    transaction.WithdrawFromAccount(expense.TransactionMode, Convert.ToInt32(expense.TransactionModeId), Convert.ToDouble(expense.Amount - oldAmount), Convert.ToDateTime(expense.ExpenseDate), "Expenses", "ExpenseId", Convert.ToInt32(expense.ExpenseId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), expense.ExpenseType.TypeName);
                }
                return RedirectToAction("Index");
            }
            var expenseType = Convert.ToInt32(Request["ExpenseTypeParent"]);

            ViewBag.ExpenseTypeParent = new SelectList(db.ExpenseTypes.Where(x => !x.ParentId.HasValue), "ExpenseTypeId", "TypeName", expenseType);
            ViewBag.ExpenseTypeId = new SelectList(db.ExpenseTypes.Where(x => x.ParentId == expenseType && x.Status == 1), "ExpenseTypeId", "TypeName", objExpense.ExpenseTypeId);
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", objExpense.TransactionMode);
            if (objExpense.TransactionMode == "Cash")
            {
                ViewBag.TransactionModeId = new SelectList(db.Cash.Where(x => x.Status == 1), "CashId", "CashName", objExpense.TransactionModeId);
            }
            else if (objExpense.TransactionMode == "Bank")
            {
                ViewBag.TransactionModeId = new SelectList(db.BankAccounts.Where(x => x.Status == 1).Select(x => new { bankId = x.BankId, account = x.BankName + "-" + x.Branch + "-" + x.AccountNo }), "bankId", "account", objExpense.TransactionModeId);
            }
            else if (objExpense.TransactionMode == "Mobile")
            {
                ViewBag.TransactionModeId = new SelectList(db.MobileBangking.Where(x => x.Status == 1), "AccountId", "AccountName", objExpense.TransactionModeId);
            }
           return View("../Shop/Expense/Edit", objExpense);
        }
        public ActionResult GetExpenseNameByTypeId(int? typeId)
        {
            var expenseNames = db.ExpenseTypes.Where(x => x.ParentId == typeId && x.Status == 1).ToList();
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Al Monsur
//Creation Date : January 2017
//=======================================================================================//