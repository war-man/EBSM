using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class SalesService
    {
        private WmsDbContext _context;
        private SalesUnitOfWork _salesUnitOfWork;

        public SalesService()
        {
            _context = new WmsDbContext();
            _salesUnitOfWork = new SalesUnitOfWork(_context);
        }
       
        public Invoice GetInvoiceById(int id)
        {
            return _salesUnitOfWork.SalesRepository.GetById(id);
        }
       
        public int Save(Invoice invoice, int? loggedInUserId)
        {
            _salesUnitOfWork.SalesRepository.Add(invoice);
            _salesUnitOfWork.Save(loggedInUserId.ToString());
            return invoice.InvoiceId;
        }
        public void Edit(Invoice invoice, int? loggedInUserId)
        {
            _salesUnitOfWork.SalesRepository.Edit(invoice);
            _salesUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Invoice> GetAll()
        {
            return _salesUnitOfWork.SalesRepository.GetAll();
        }

        public IEnumerable<Invoice> GetAll(int? CustomerId, string InvoiceNo, string InvoiceDateFrom, string InvoiceDateTo, string TransactionMode, int? SalesmanId)
        {
            return _salesUnitOfWork.SalesRepository.GetAll(CustomerId, InvoiceNo, InvoiceDateFrom, InvoiceDateTo, TransactionMode, SalesmanId);
        }
        public IEnumerable<Invoice> GetInvoicesWithoutAnyBill(int customerId)
        {
            return _salesUnitOfWork.SalesRepository.GetInvoicesWithoutAnyBill(customerId);
        }
        public int GetCount()
        {
            return _salesUnitOfWork.SalesRepository.GetCount();
        }

        //Invoice products
        public InvoiceProduct GetByInvoiceAndProductId(int invoiceId, int productId)
        {
            return _salesUnitOfWork.SalesProductRepository.GetByInvoiceAndProductId(invoiceId, productId);
        }
        public IEnumerable<InvoiceProduct> GetProductsByInvoiceId(int invoiceId) {
            return _salesUnitOfWork.SalesProductRepository.GetByInvoiceId(invoiceId);
        }
        public int SaveInvoiceProduct(InvoiceProduct item)
        {
            _salesUnitOfWork.SalesProductRepository.Add(item);
            _salesUnitOfWork.Save();
            return item.InvoiceProductId;
        }
        public void EditInvoiceProduct(InvoiceProduct item)
        {
            _salesUnitOfWork.SalesProductRepository.Edit(item);
            _salesUnitOfWork.Save();
        }
        public void DeleteInvoiceProductFromDbById(int id, int? loggedInUserId)
        {
            _salesUnitOfWork.SalesProductRepository.DeleteFromDbById(id);
            _salesUnitOfWork.Save(loggedInUserId.ToString());

        }
        public void DeleteInvoiceProductFromDbByItem(InvoiceProduct item)
        {
            _salesUnitOfWork.SalesProductRepository.DeleteFromDbByItem(item);
            _salesUnitOfWork.Save();
        }
        public void Dispose()
        {
            _salesUnitOfWork.Dispose();
        }
    }
}
