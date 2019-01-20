using System;
namespace EBSM.Repo
{
    public class SalesmanUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private SalesmanRepository _salesmanRepository { get; set; }

        public SalesmanUnitOfWork(WmsDbContext context)
        {
            db = context;
            _salesmanRepository = new SalesmanRepository(db);
        }

        public SalesmanRepository SalesmanRepository
        {
            get
            {
                return _salesmanRepository;
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
