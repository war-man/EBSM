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
        public IEnumerable<Invoice> GetAllSales()
        {
            return _salesUnitOfWork.SalesRepository.GetAll();
        }

        public IEnumerable<Invoice> GetAllSales(int? CustomerId, string InvoiceNo, string InvoiceDateFrom, string InvoiceDateTo, string TransactionMode, int? SalesmanId)
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
        public IEnumerable<Invoice> GetAllSalesByDate(DateTime date)
        {
            return _salesUnitOfWork.SalesRepository.GetAllSalesByDate(date);
        }
        public IEnumerable<Invoice> GetAllTodaySales()
        {
            return _salesUnitOfWork.SalesRepository.GetAllSalesByDate(DateTime.Now);
        }
        public double GetSalesAmountByDate(DateTime date)
        {
            
            return _salesUnitOfWork.SalesRepository.GetSalesAmountByDate(date);
        }
        public double GetTodaysSalesAmount()
        {
            return _salesUnitOfWork.SalesRepository.GetSalesAmountByDate(DateTime.Now);
        }
        public IEnumerable<Invoice> GetAllSalesByMonth(DateTime date)
        {
            return _salesUnitOfWork.SalesRepository.GetAllSalesByMonth(date);
        }
        public IEnumerable<Invoice> GetAllSalesByCurrentMonth()
        {
            return _salesUnitOfWork.SalesRepository.GetAllSalesByMonth(DateTime.Now);
        } public IEnumerable<Invoice> GetAllSalesByYear(int year)
        {
            return _salesUnitOfWork.SalesRepository.GetAllSalesByYear(year);
        }
        public IEnumerable<Invoice> GetAllSalesByCurrentYear()
        {
            return _salesUnitOfWork.SalesRepository.GetAllSalesByYear(DateTime.Now.Year);
        }
        //Invoice products
        public IEnumerable<InvoiceProduct> GetAllInvoiceProducts(string fromDate2, string toDate2, int? SelectedProductId)
        {
            return _salesUnitOfWork.SalesProductRepository.GetAll( fromDate2,  toDate2,  SelectedProductId);
        }
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
        //public IEnumerable<InvoiceProduct> GetAllByDate(DateTime date)
        //{
        //    return _salesUnitOfWork.SalesProductRepository.GetAllByDate(date);
        //}
        public IEnumerable<InvoiceProduct> GetTopSalesProduct()
        {
            return _salesUnitOfWork.SalesProductRepository.GetAllByMonth(DateTime.Now);
        }
        public void Dispose()
        {
            _salesUnitOfWork.Dispose();
        }
    }
}
