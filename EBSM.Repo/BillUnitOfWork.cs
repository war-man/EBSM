using System;
namespace EBSM.Repo
{
    public class BillUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private BillRepository _billRepository { get; set; }

        public BillUnitOfWork(WmsDbContext context)
        {
            db = context;
            _billRepository = new BillRepository(db);
        }

        public BillRepository BillRepository
        {
            get
            {
                return _billRepository;
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
