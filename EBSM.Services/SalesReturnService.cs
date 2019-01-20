using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class SalesReturnService
    {
        private WmsDbContext _context;
        private SalesReturnUnitOfWork _salesReturnUnitOfWork;

        public SalesReturnService()
        {
            _context = new WmsDbContext();
            _salesReturnUnitOfWork = new SalesReturnUnitOfWork(_context);
        }
       
        public Return GetSalesReturnById(int id)
        {
            return _salesReturnUnitOfWork.SalesReturnRepository.GetById(id);
        }
       
        public int Save(Return item, int? loggedInUserId)
        {
            _salesReturnUnitOfWork.SalesReturnRepository.Add(item);
            _salesReturnUnitOfWork.Save(loggedInUserId.ToString());
            return item.ReturnId;
        }
        public void Edit(Return item, int? loggedInUserId)
        {
            _salesReturnUnitOfWork.SalesReturnRepository.Edit(item);
            _salesReturnUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Return> GetAll()
        {
            return _salesReturnUnitOfWork.SalesReturnRepository.GetAll();
        }
        public IEnumerable<Return> GetAll(int? CustomerId, string InvoiceNo, string ReturnDateFrom, string ReturnDateTo)
        {
            return _salesReturnUnitOfWork.SalesReturnRepository.GetAll(CustomerId, InvoiceNo, ReturnDateFrom, ReturnDateTo);
        }

        //return products============

        public int SaveReturnProduct(ReturnProduct item)
        {
            _salesReturnUnitOfWork.SalesReturnProductRepository.Add(item);
            _salesReturnUnitOfWork.Save();
            return item.ReturnProductId;
        }
        public void EditReturnProduct(ReturnProduct item)
        {
            _salesReturnUnitOfWork.SalesReturnProductRepository.Edit(item);
            _salesReturnUnitOfWork.Save();
        }
        public void Dispose()
        {
            _salesReturnUnitOfWork.Dispose();
        }
    }
}
