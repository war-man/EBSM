using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class ProductService
    {
        private WmsDbContext _context;
        private ProductUnitOfWork _productUnitOfWork;

        public ProductService()
        {
            _context = new WmsDbContext();
            _productUnitOfWork = new ProductUnitOfWork(_context);
        }
        public IEnumerable<Product> GetActiveProducts()
        {
            return _productUnitOfWork.ProductRepository.GetActiveProducts();
        }
           
        public Product GetProductrById(int id)
        {
            return _productUnitOfWork.ProductRepository.GetById(id);
        }
       
        public int Save(Product product, int? loggedInUserId)
        {
            _productUnitOfWork.ProductRepository.Add(product);
            _productUnitOfWork.Save(loggedInUserId.ToString());
            return product.ProductId;
        }
        public void Edit(Product product, int? loggedInUserId)
        {
            _productUnitOfWork.ProductRepository.Edit(product);
            _productUnitOfWork.Save(loggedInUserId.ToString());
        }

        //public IEnumerable<User> GetAllCardNotAssignedEmployee()
        //{
        //    return _employeeUnitOfWork.EmployeeRepository.GetAllCardNotAssignedEmployee();
        //}
        //public void DeleteCostCenter(int id)
        //{
        //    _costCenterUnitOfWork.CostCenterRepository.DeleteById(id);
        //    _costCenterUnitOfWork.Save();
        //}
        //public bool IsCostCenterExist(string CostCenterName, string InitialCostCenterName)
        //{
        //    return _costCenterUnitOfWork.CostCenterRepository.IsCostCenterExist(CostCenterName, InitialCostCenterName);
        //}
        public void Dispose()
        {
            _productUnitOfWork.Dispose();
        }
    }
}
