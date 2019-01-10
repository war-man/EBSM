using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class CustomerService
    {
        private WmsDbContext _context;
        private CustomerUnitOfWork _customerUnitOfWork;

        public CustomerService()
        {
            _context = new WmsDbContext();
            _customerUnitOfWork = new CustomerUnitOfWork(_context);
        }
       
        public Customer GetCustomerById(int id)
        {
            return _customerUnitOfWork.CustomerRepository.GetById(id);
        }
       
        public int Save(Customer customer, int? loggedInUserId)
        {
            _customerUnitOfWork.CustomerRepository.Add(customer);
            _customerUnitOfWork.Save(loggedInUserId.ToString());
            return customer.CustomerId;
        }
        public void Edit(Customer customer, int? loggedInUserId)
        {
            _customerUnitOfWork.CustomerRepository.Edit(customer);
            _customerUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customerUnitOfWork.CustomerRepository.GetAll();
        }
        //public IEnumerable<ArticleTransfer> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        //{
        //    return _articleTransferUnitOfWork.ArticleTransferRepository.GetAll(SelectedProductId, PName, TransferDateFrom, TransferDateTo);
        //}
        public void Dispose()
        {
            _customerUnitOfWork.Dispose();
        }
    }
}
