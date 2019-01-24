using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class ExpenseRepository
    {
        private WmsDbContext db;
        public ExpenseRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Expense item)
        {
            db.Expenses.Add(item);
        }
        public void Edit(Expense item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Expense GetById(int id)
        {
            return db.Expenses.Find(id); 
        }
        public IEnumerable<Expense> GetAll()
        {
            return db.Expenses;
        }
        public IEnumerable<Expense> GetAll(int? ExpenseTypeId, string ExpenseDateFrom, string ExpenseDateTo)
        {
            var fromDate = string.IsNullOrEmpty(ExpenseDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(ExpenseDateFrom);
            var toDate = string.IsNullOrEmpty(ExpenseDateTo) ? DateTime.Now.Date : Convert.ToDateTime(ExpenseDateTo).AddDays(1);
            return db.Expenses.Where(x => (ExpenseDateFrom == null || x.ExpenseDate >= fromDate) && (ExpenseDateTo == null || x.ExpenseDate < toDate)
                 && (ExpenseTypeId == null || x.ExpenseTypeId == ExpenseTypeId)).OrderByDescending(x => x.ExpenseDate);
        }


    }
}
