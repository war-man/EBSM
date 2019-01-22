using System;
namespace EBSM.Repo
{
    public class PurchaseUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private PurchaseRepository _purchaseRepository { get; set; }
        private PurchaseCostRepository _purchaseCostRepository { get; set; }
        private PurchaseProductRepository _purchaseProductRepository { get; set; }
        private PurchasePaymentRepository _purchasePaymentRepository { get; set; }
        public PurchaseUnitOfWork(WmsDbContext context)
        {
            db = context;
            _purchaseRepository = new PurchaseRepository(db);
            _purchaseCostRepository = new PurchaseCostRepository(db);
            _purchaseProductRepository = new PurchaseProductRepository(db);
            _purchasePaymentRepository = new PurchasePaymentRepository(db);
          
        }

        public PurchaseRepository PurchaseRepository
        {
            get
            {
                return _purchaseRepository;
            }
        }
        public PurchaseCostRepository PurchaseCostRepository
        {
            get
            {
                return _purchaseCostRepository;
            }
        }
        public PurchaseProductRepository PurchaseProductRepository
        {
            get
            {
                return _purchaseProductRepository;
            }
        }
        public PurchasePaymentRepository PurchasePaymentRepository
        {
            get
            {
                return _purchasePaymentRepository;
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
