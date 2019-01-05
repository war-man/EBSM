using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class ArticleTransferService
    {
        private WmsDbContext _context;
        private ArticleTransferUnitOfWork _articleTransferUnitOfWork;

        public ArticleTransferService()
        {
            _context = new WmsDbContext();
            _articleTransferUnitOfWork = new ArticleTransferUnitOfWork(_context);
        }
        public IEnumerable<ArticleTransfer> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        {
            return _articleTransferUnitOfWork.ArticleTransferRepository.GetAll(SelectedProductId, PName, TransferDateFrom, TransferDateTo);
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
            _articleTransferUnitOfWork.Dispose();
        }
    }
}
