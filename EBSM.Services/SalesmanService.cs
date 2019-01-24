using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class SalesmanService
    {
        private WmsDbContext _context;
        private SalesmanUnitOfWork _salesmanUnitOfWork;

        public SalesmanService()
        {
            _context = new WmsDbContext();
            _salesmanUnitOfWork = new SalesmanUnitOfWork(_context);
        }
       
        public Salesman GetSalesmanById(int id)
        {
            return _salesmanUnitOfWork.SalesmanRepository.GetById(id);
        }
       
        public int Save(Salesman salesman, int? loggedInUserId)
        {
            _salesmanUnitOfWork.SalesmanRepository.Add(salesman);
            _salesmanUnitOfWork.Save(loggedInUserId.ToString());
            return salesman.SalesmanId;
        }
        public void Edit(Salesman salesman, int? loggedInUserId)
        {
            _salesmanUnitOfWork.SalesmanRepository.Edit(salesman);
            _salesmanUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Salesman> GetAllSalesman()
        {
            return _salesmanUnitOfWork.SalesmanRepository.GetAll();
        }
        public IEnumerable<Salesman> GetAllSalesman(string SalesmanName)
        {
            return _salesmanUnitOfWork.SalesmanRepository.GetAll(SalesmanName);
        }
        public void DeleteSalesman(Salesman salesman)
        {
            _salesmanUnitOfWork.SalesmanRepository.DeleteFromDbByItem(salesman);
            _salesmanUnitOfWork.Save();
        }
        public void Dispose()
        {
            _salesmanUnitOfWork.Dispose();
        }
    }
}
