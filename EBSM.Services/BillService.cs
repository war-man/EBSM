using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class BillService
    {
        private WmsDbContext _context;
        private BillUnitOfWork _billUnitOfWork;

        public BillService()
        {
            _context = new WmsDbContext();
            _billUnitOfWork = new BillUnitOfWork(_context);
        }
       
        public Bill GetBillById(int id)
        {
            return _billUnitOfWork.BillRepository.GetById(id);
        }
       
        public int Save(Bill bill, int? loggedInUserId)
        {
            _billUnitOfWork.BillRepository.Add(bill);
            _billUnitOfWork.Save(loggedInUserId.ToString());
            return bill.BillId;
        }
        public void Edit(Bill bill, int? loggedInUserId)
        {
            _billUnitOfWork.BillRepository.Edit(bill);
            _billUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Bill> GetAll()
        {
            return _billUnitOfWork.BillRepository.GetAll();
        } public IEnumerable<Bill> GetAll(string BillNo, string BillDateFrom, string BillDateTo, int? Customer)
        {
            return _billUnitOfWork.BillRepository.GetAll(BillNo, BillDateFrom, BillDateTo, Customer);
        }
        public int GetCount()
        {
            return _billUnitOfWork.BillRepository.GetCount();
        }
        public void Dispose()
        {
            _billUnitOfWork.Dispose();
        }
    }
}
