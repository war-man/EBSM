using System;
namespace EBSM.Repo
{
    public class ExpenseUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private ExpenseRepository _expenseRepository { get; set; }

        public ExpenseUnitOfWork(WmsDbContext context)
        {
            db = context;
            _expenseRepository = new ExpenseRepository(db);
        }

        public ExpenseRepository ExpenseRepository
        {
            get
            {
                return _expenseRepository;
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
