using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class ExpenseTypeRepository
    {
        private WmsDbContext db;
        public ExpenseTypeRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(ExpenseType item)
        {
            db.ExpenseTypes.Add(item);
        }
        public void Edit(ExpenseType item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public ExpenseType GetById(int id)
        {
            return db.ExpenseTypes.Find(id); 
        }
        public IEnumerable<ExpenseType> GetAll()
        {
            return db.ExpenseTypes;
        }
        public IEnumerable<ExpenseType> GetAllByParentId(int? parentId)
        {
            return db.ExpenseTypes.Where(x => x.ParentId == parentId && x.Status == 1);
        }
        public IEnumerable<ExpenseType> GetChildExpenseTypes()
        {
            return db.ExpenseTypes.Where(x => x.ParentId.HasValue & x.Status!=0);
        }
        public IEnumerable<ExpenseType> GetParentExpenseTypes()
        {
            return db.ExpenseTypes.Where(x => !x.ParentId.HasValue & x.Status != 0);
        }
        public bool ExistExpenseTypes(string typeName)
        {
            return db.ExpenseTypes.Any(e => e.TypeName == typeName);
        }
        //public IEnumerable<ArticleTransfer> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        //{
        //    var fromDate = string.IsNullOrEmpty(TransferDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateFrom);
        //    var toDate = string.IsNullOrEmpty(TransferDateTo) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateTo).AddDays(1);
        //    return db.ArticleTransfers.ToList().Where(x => (SelectedProductId == null || x.StockFrom.ProductId == SelectedProductId) && (PName == null || (x.StockFrom.Product.ProductFullName.StartsWith(PName) || x.StockFrom.Product.ProductFullName.Contains(" " + PName))) && (TransferDateFrom == null || x.TransferDate.Date >= fromDate) && (TransferDateTo == null || x.TransferDate.Date < toDate)).OrderByDescending(o => o.CreatedDate);
        //} 


    }
}
