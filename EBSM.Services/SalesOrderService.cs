using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class SalesOrderService
    {
        private WmsDbContext _context;
        private SalesOrderUnitOfWork _salesOrderUnitOfWork;

        public SalesOrderService()
        {
            _context = new WmsDbContext();
            _salesOrderUnitOfWork = new SalesOrderUnitOfWork(_context);
        }
       
        public SalesOrder GetSalesOrderById(int id)
        {
            return _salesOrderUnitOfWork.SalesOrderRepository.GetById(id);
        }
       
        public int Save(SalesOrder salesOrder, int? loggedInUserId)
        {
            _salesOrderUnitOfWork.SalesOrderRepository.Add(salesOrder);
            _salesOrderUnitOfWork.Save(loggedInUserId.ToString());
            return salesOrder.SalesOrderId;
        }
        public void Edit(SalesOrder salesOrder, int? loggedInUserId)
        {
            _salesOrderUnitOfWork.SalesOrderRepository.Edit(salesOrder);
            _salesOrderUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<SalesOrder> GetAll()
        {
            return _salesOrderUnitOfWork.SalesOrderRepository.GetAll();
        }
        //public IEnumerable<SalesOrder> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        //{
        //    return _articleTransferUnitOfWork.ArticleTransferRepository.GetAll(SelectedProductId, PName, TransferDateFrom, TransferDateTo);
        //}

        public int SaveOrderInvoice(InvoiceOrder orderInvoice, int? loggedInUserId)
        {
            _salesOrderUnitOfWork.OrderInvoiceRelationRepository.Add(orderInvoice);
            _salesOrderUnitOfWork.Save(loggedInUserId.ToString());
            return orderInvoice.InvoiceOrderId;
        }
        public void Dispose()
        {
            _salesOrderUnitOfWork.Dispose();
        }
    }
}
