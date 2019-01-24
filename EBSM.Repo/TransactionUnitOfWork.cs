using System;
namespace EBSM.Repo
{
    public class TransactionUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private TransactionRepository _transactionRepository { get; set; }

        public TransactionUnitOfWork(WmsDbContext context)
        {
            db = context;
            _transactionRepository = new TransactionRepository(db);
        }

        public TransactionRepository TransactionRepository
        {
            get
            {
                return _transactionRepository;
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
