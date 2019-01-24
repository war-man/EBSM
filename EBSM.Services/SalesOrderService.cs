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
       
        public int SaveSalesOrder(SalesOrder salesOrder, int? loggedInUserId)
        {
            _salesOrderUnitOfWork.SalesOrderRepository.Add(salesOrder);
            _salesOrderUnitOfWork.Save(loggedInUserId.ToString());
            return salesOrder.SalesOrderId;
        }
        public void EditSalesOrder(SalesOrder salesOrder, int? loggedInUserId)
        {
            _salesOrderUnitOfWork.SalesOrderRepository.Edit(salesOrder);
            _salesOrderUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<SalesOrder> GetAllSalesOrders()
        {
            return _salesOrderUnitOfWork.SalesOrderRepository.GetAll();
        }
        public IEnumerable<SalesOrder> GetAllSalesOrders(string OrderNo, string OrderDateFrom, string OrderDateTo, int? CustomerId, byte? Status)
        {
            return _salesOrderUnitOfWork.SalesOrderRepository.GetAll( OrderNo,  OrderDateFrom,  OrderDateTo,  CustomerId,  Status);
        }
        public int GetCount()
        {
            return _salesOrderUnitOfWork.SalesOrderRepository.GetCount();
        }public int GetPendingOrdersCount()
        {
            return _salesOrderUnitOfWork.SalesOrderRepository.GetPendingOrdersCount();
        }
        public int SaveOrderInvoice(InvoiceOrder orderInvoice, int? loggedInUserId)
        {
            _salesOrderUnitOfWork.OrderInvoiceRelationRepository.Add(orderInvoice);
            _salesOrderUnitOfWork.Save(loggedInUserId.ToString());
            return orderInvoice.InvoiceOrderId;
        }
        public void SaveOrderProduct(OrderProduct orderProduct)
        {
            _salesOrderUnitOfWork.OrderProductRepository.Add(orderProduct);
            _salesOrderUnitOfWork.Save();

        }
        public IEnumerable<OrderProduct> GetAllOrderProductsByOrderId(int orderId)
        {
            return _salesOrderUnitOfWork.OrderProductRepository.GetAllOrderProductsByOrderId(orderId);
        }
        public void DeleteOrderProductList(IEnumerable<OrderProduct> orderProductList)
        {
            foreach(var item in orderProductList)
            {
                _salesOrderUnitOfWork.OrderProductRepository.DeleteFromDbByItem(item);
            }
            _salesOrderUnitOfWork.Save();

        }

        public void Dispose()
        {
            _salesOrderUnitOfWork.Dispose();
        }
    }
}
