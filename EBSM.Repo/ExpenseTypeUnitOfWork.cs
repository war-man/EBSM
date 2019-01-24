using System;
namespace EBSM.Repo
{
    public class ExpenseTypeUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private ExpenseTypeRepository _expenseTypeRepository { get; set; }

        public ExpenseTypeUnitOfWork(WmsDbContext context)
        {
            db = context;
            _expenseTypeRepository = new ExpenseTypeRepository(db);
        }

        public ExpenseTypeRepository ExpenseTypeRepository
        {
            get
            {
                return _expenseTypeRepository;
            }
        }
        public void Save()
        {
            db.SaveChanges("");
        }
        public void Save(string loggedInUserId)
        {
            db.SaveChanges(loggedInUserId);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
