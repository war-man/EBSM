using System;
namespace EBSM.Repo
{
    public class SupplierUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private SupplierRepository _supplierRepository { get; set; }

        public SupplierUnitOfWork(WmsDbContext context)
        {
            db = context;
            _supplierRepository = new SupplierRepository(db);
        }

        public SupplierRepository SupplierRepository
        {
            get
            {
                return _supplierRepository;
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
