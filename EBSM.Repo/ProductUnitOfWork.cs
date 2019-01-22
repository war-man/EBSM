using System;
namespace EBSM.Repo
{
    public class ProductUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private ProductRepository _productRepository { get; set; }
        private ProductAttributeRelationRepository _productAttributeRelationRepository { get; set; }
        private ProductCategoryRepository _productCategoryRepository { get; set; }
        private ProductCustomerRalationRepository _productCustomerRalationRepository { get; set; }
       

        public ProductUnitOfWork(WmsDbContext context)
        {
            db = context;
            _productRepository = new ProductRepository(db);
            _productAttributeRelationRepository = new ProductAttributeRelationRepository(db);
            _productCategoryRepository = new ProductCategoryRepository(db);
            _productCustomerRalationRepository = new ProductCustomerRalationRepository(db);
        }

        public ProductRepository ProductRepository
        {
            get
            {
                return _productRepository;
            }
        } public ProductAttributeRelationRepository ProductAttributeRelationRepository
        {
            get
            {
                return _productAttributeRelationRepository;
            }
        } public ProductCategoryRepository ProductCategoryRepository
        {
            get
            {
                return _productCategoryRepository;
            }
        }public ProductCustomerRalationRepository ProductCustomerRalationRepository
        {
            get
            {
                return _productCustomerRalationRepository;
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
