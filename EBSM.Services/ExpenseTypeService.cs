using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class ExpenseTypeService
    {
        private WmsDbContext _context;
        private ExpenseTypeUnitOfWork _expenseTypeUnitOfWork;

        public ExpenseTypeService()
        {
            _context = new WmsDbContext();
            _expenseTypeUnitOfWork = new ExpenseTypeUnitOfWork(_context);
        }
       
        public ExpenseType GetExpenseTypeById(int id)
        {
            return _expenseTypeUnitOfWork.ExpenseTypeRepository.GetById(id);
        }
       
        public int Save(ExpenseType expenseType, int? loggedInUserId)
        {
            _expenseTypeUnitOfWork.ExpenseTypeRepository.Add(expenseType);
            _expenseTypeUnitOfWork.Save(loggedInUserId.ToString());
            return expenseType.ExpenseTypeId;
        }
        public void Edit(ExpenseType expenseType, int? loggedInUserId)
        {
            _expenseTypeUnitOfWork.ExpenseTypeRepository.Edit(expenseType);
            _expenseTypeUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<ExpenseType> GetAll()
        {
            return _expenseTypeUnitOfWork.ExpenseTypeRepository.GetAll();
        }
        public IEnumerable<ExpenseType> GetAllByParentId(int? parentId)
        {
            return _expenseTypeUnitOfWork.ExpenseTypeRepository.GetAllByParentId(parentId);
        }
            public IEnumerable<ExpenseType> GetChildExpenseTypes()
        {
            return _expenseTypeUnitOfWork.ExpenseTypeRepository.GetChildExpenseTypes();
        }
        public IEnumerable<ExpenseType> GetParentExpenseTypes()
        {
            return _expenseTypeUnitOfWork.ExpenseTypeRepository.GetParentExpenseTypes();
        }
        public bool ExistExpenseTypes(string typeName)
        {
            return _expenseTypeUnitOfWork.ExpenseTypeRepository.ExistExpenseTypes(typeName);
        }
        //public IEnumerable<Expense> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        //{
        //    return _expenseUnitOfWork.ExpenseRepository.GetAll(SelectedProductId, PName, TransferDateFrom, TransferDateTo);
        //}
        public void Dispose()
        {
            _expenseTypeUnitOfWork.Dispose();
        }
    }
}
