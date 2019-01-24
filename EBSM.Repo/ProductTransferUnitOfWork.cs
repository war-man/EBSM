using System;
namespace EBSM.Repo
{
    public class ProductTransferUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private ProductTransferRepository _productTransferRepository { get; set; }

        public ProductTransferUnitOfWork(WmsDbContext context)
        {
            db = context;
            _productTransferRepository = new ProductTransferRepository(db);
        }

        public ProductTransferRepository ProductTransferRepository
        {
            get
            {
                return _productTransferRepository;
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
