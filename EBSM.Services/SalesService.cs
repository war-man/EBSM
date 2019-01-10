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

        //public IEnumerable<ArticleTransfer> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        //{
        //    return _articleTransferUnitOfWork.ArticleTransferRepository.GetAll(SelectedProductId, PName, TransferDateFrom, TransferDateTo);
        //}
        public IEnumerable<Invoice> GetInvoicesWithoutAnyBill(int customerId)
        {
            return _salesUnitOfWork.SalesRepository.GetInvoicesWithoutAnyBill(customerId);
        }
        public void Dispose()
        {
            _salesUnitOfWork.Dispose();
        }
    }
}
