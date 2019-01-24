using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class SupplierService
    {
        private WmsDbContext _context;
        private SupplierUnitOfWork _supplierUnitOfWork;

        public SupplierService()
        {
            _context = new WmsDbContext();
            _supplierUnitOfWork = new SupplierUnitOfWork(_context);
        }
       
        public Supplier GetSupplierById(int id)
        {
            return _supplierUnitOfWork.SupplierRepository.GetById(id);
        }
       
        public int Save(Supplier supplier, int? loggedInUserId)
        {
            _supplierUnitOfWork.SupplierRepository.Add(supplier);
            _supplierUnitOfWork.Save(loggedInUserId.ToString());
            return supplier.SupplierId;
        }
        public void Edit(Supplier supplier, int? loggedInUserId)
        {
            _supplierUnitOfWork.SupplierRepository.Edit(supplier);
            _supplierUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return _supplierUnitOfWork.SupplierRepository.GetAll();
        }
        public IEnumerable<Supplier> GetAllSuppliers(string SupplierName)
        {
            return _supplierUnitOfWork.SupplierRepository.GetAll(SupplierName);
        }
        public IEnumerable<Supplier> GetAllManufecturer()
        {
            return _supplierUnitOfWork.SupplierRepository.GetAllManufecturer();
        }public IEnumerable<Supplier> GetAllDistributor()
        {
            return _supplierUnitOfWork.SupplierRepository.GetAllDistributor();
        }
        public void Dispose()
        {
            _supplierUnitOfWork.Dispose();
        }
    }
}
