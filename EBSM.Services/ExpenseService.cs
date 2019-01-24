using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class ExpenseService
    {
        private WmsDbContext _context;
        private ExpenseUnitOfWork _expenseUnitOfWork;

        public ExpenseService()
        {
            _context = new WmsDbContext();
            _expenseUnitOfWork = new ExpenseUnitOfWork(_context);
        }
       
        public Expense GetExpenseById(int id)
        {
            return _expenseUnitOfWork.ExpenseRepository.GetById(id);
        }
       
        public int Save(Expense expense, int? loggedInUserId)
        {
            _expenseUnitOfWork.ExpenseRepository.Add(expense);
            _expenseUnitOfWork.Save(loggedInUserId.ToString());
            return expense.ExpenseId;
        }
        public void Edit(Expense expense, int? loggedInUserId)
        {
            _expenseUnitOfWork.ExpenseRepository.Edit(expense);
            _expenseUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Expense> GetAll()
        {
            return _expenseUnitOfWork.ExpenseRepository.GetAll();
        }
        public IEnumerable<Expense> GetAll(int? ExpenseTypeId, string ExpenseDateFrom, string ExpenseDateTo)
        {
            
            return _expenseUnitOfWork.ExpenseRepository.GetAll(ExpenseTypeId, ExpenseDateFrom, ExpenseDateTo);
        }
        public void Dispose()
        {
            _expenseUnitOfWork.Dispose();
        }
    }
}
