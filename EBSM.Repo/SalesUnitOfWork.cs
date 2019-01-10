using System;
namespace EBSM.Repo
{
    public class SalesUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private SalesRepository _salesRepository { get; set; }

        public SalesUnitOfWork(WmsDbContext context)
        {
            db = context;
            _salesRepository = new SalesRepository(db);
        }

        public SalesRepository SalesRepository
        {
            get
            {
                return _salesRepository;
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
