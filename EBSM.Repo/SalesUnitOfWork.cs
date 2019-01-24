using System;
namespace EBSM.Repo
{
    public class SalesUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private SalesRepository _salesRepository { get; set; }
        private SalesProductRepository _salesProductRepository { get; set; }
        private OrderInvoiceRelationRepository _orderInvoiceRelationRepository { get; set; }

        public SalesUnitOfWork(WmsDbContext context)
        {
            db = context;
            _salesRepository = new SalesRepository(db);
            _salesProductRepository = new SalesProductRepository(db);
            _orderInvoiceRelationRepository = new OrderInvoiceRelationRepository(db);
        }

        public SalesRepository SalesRepository
        {
            get
            {
                return _salesRepository;
            }
        }public SalesProductRepository SalesProductRepository
        {
            get
            {
                return _salesProductRepository;
            }
        }public OrderInvoiceRelationRepository OrderInvoiceRelationRepository
        {
            get
            {
                return _orderInvoiceRelationRepository;
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
