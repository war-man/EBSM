using System;
namespace EBSM.Repo
{
    public class ProductUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private ProductRepository _productRepository { get; set; }

        public ProductUnitOfWork(WmsDbContext context)
        {
            db = context;
            _productRepository = new ProductRepository(db);
        }

        public ProductRepository ProductRepository
        {
            get
            {
                return _productRepository;
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
