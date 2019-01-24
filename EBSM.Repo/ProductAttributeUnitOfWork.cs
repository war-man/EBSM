using System;
namespace EBSM.Repo
{
    public class ProductAttributeUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private ProductAttributeRepository _productAttributeRepository { get; set; }
        private ProductAttributeSetRepository _productAttributeSetRepository { get; set; }
        private AttributeSetAttributeRepository _attributeSetAttributeRepository { get; set; }

        public ProductAttributeUnitOfWork(WmsDbContext context)
        {
            db = context;
            _productAttributeRepository = new ProductAttributeRepository(db);
            _productAttributeSetRepository = new ProductAttributeSetRepository(db);
            _attributeSetAttributeRepository = new AttributeSetAttributeRepository(db);
        }

        public ProductAttributeRepository ProductAttributeRepository
        {
            get
            {
                return _productAttributeRepository;
            }
        }
        public ProductAttributeSetRepository ProductAttributeSetRepository
        {
            get
            {
                return _productAttributeSetRepository;
            }
        }
        public AttributeSetAttributeRepository AttributeSetAttributeRepository
        {
            get
            {
                return _attributeSetAttributeRepository;
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
