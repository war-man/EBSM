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
        public IEnumerable<Customer> GetCustomersByName(string CustomerName)
        {
            return _customerUnitOfWork.CustomerRepository.GetCustomersByName(CustomerName);
        }
        public IEnumerable<Customer> GetAllCustomers(string CustomerName, string ContactNo)
        {
            return _customerUnitOfWork.CustomerRepository.GetAll(CustomerName, ContactNo);
        }
        public CustomerProject GetCustomerProjectById(int id)
        {
            return _customerUnitOfWork.CustomerProjectRepository.GetById(id);
        }

        public int SaveProject(CustomerProject customerProject, int? loggedInUserId)
        {
            _customerUnitOfWork.CustomerProjectRepository.Add(customerProject);
            _customerUnitOfWork.Save(loggedInUserId.ToString());
            return customerProject.CustomerProjectId;
        }
        public void EditProject(CustomerProject customerProject, int? loggedInUserId)
        {
            _customerUnitOfWork.CustomerProjectRepository.Edit(customerProject);
            _customerUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<CustomerProject> GetAllCustomerProjects()
        {
            return _customerUnitOfWork.CustomerProjectRepository.GetAll();
        }
        public IEnumerable<CustomerProject> GetAllCustomerProjectsByCustomerId(int? CustomerId)
        {
            return _customerUnitOfWork.CustomerProjectRepository.GetAll(CustomerId);
        }
        public IEnumerable<CustomerProject> GetAllCustomerProjects(int? CustomerId, string Name)
        {
            return _customerUnitOfWork.CustomerProjectRepository.GetAll(CustomerId, Name);
        }
        public void Dispose()
        {
            _customerUnitOfWork.Dispose();
        }
    }
}
