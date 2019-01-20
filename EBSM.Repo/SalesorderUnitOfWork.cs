using System;
namespace EBSM.Repo
{
    public class SalesOrderUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private SalesOrderRepository _salesOrderRepository { get; set; }
        private OrderProductRepository _orderProductRepository { get; set; }
        private OrderInvoiceRelationRepository _orderInvoiceRelationRepository { get; set; }

        public SalesOrderUnitOfWork(WmsDbContext context)
        {
            db = context;
            _salesOrderRepository = new SalesOrderRepository(db);
        }

        public SalesOrderRepository SalesOrderRepository
        {
            get
            {
                return _salesOrderRepository;
            }
        } public OrderProductRepository OrderProductRepository
        {
            get
            {
                return _orderProductRepository;
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
