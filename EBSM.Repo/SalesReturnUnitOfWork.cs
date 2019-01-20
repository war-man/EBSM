using System;
namespace EBSM.Repo
{
    public class SalesReturnUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private SalesReturnRepository _salesReturnRepository { get; set; }
        private SalesReturnProductRepository _salesReturnProductRepository { get; set; }

        public SalesReturnUnitOfWork(WmsDbContext context)
        {
            db = context;
            _salesReturnRepository = new SalesReturnRepository(db);
            _salesReturnProductRepository = new SalesReturnProductRepository(db);
        }

        public SalesReturnRepository SalesReturnRepository
        {
            get
            {
                return _salesReturnRepository;
            }
        }
        public SalesReturnProductRepository SalesReturnProductRepository
        {
            get
            {
                return _salesReturnProductRepository;
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
