using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class UserService
    {
        private WmsDbContext _context;
        private UserUnitOfWork _userUnitOfWork;

        public UserService()
        {
            _context = new WmsDbContext();
            _userUnitOfWork = new UserUnitOfWork(_context);
        }

        public IEnumerable<User> GetAllUsers(string SearchString)
        {
            return _userUnitOfWork.UserRepository.GetAll(SearchString);
        }
        public User GetUserById(int id)
        {
            return _userUnitOfWork.UserRepository.GetById(id);
        }
       
        public void SaveUser(User user, int? loggedInUserId)
        {
            var newUser = new User
            {
                //CardNo = card.CardNo,
                //CardAcc = card.CardAcc,
                Status = 1,
                CreatedBy = loggedInUserId,
                CreatedDate = DateTime.Now,
            };

            _userUnitOfWork.UserRepository.Add(newUser);
            _userUnitOfWork.Save(loggedInUserId.ToString());
        }
        public void EditUser(User user, int? loggedInUserId)
        {
            //var costcenter = GetCostCenter(costCenter.Id);
            //costcenter.CostCenterName = costCenter.CostCenterName;
            //costcenter.DepartmentId = costCenter.DepartmentId;
            //costcenter.UpdatedAt = costCenter.UpdatedAt;
            //costcenter.UpdatedBy = costCenter.UpdatedBy;
            //_costCenterUnitOfWork.Save();
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
            _userUnitOfWork.Dispose();
        }
    }
}
