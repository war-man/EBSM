using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class WarehouseZoneService
    {
        private WmsDbContext _context;
        private WarehouseZoneUnitOfWork _warehouseZoneUnitOfWork;

        public WarehouseZoneService()
        {
            _context = new WmsDbContext();
            _warehouseZoneUnitOfWork = new WarehouseZoneUnitOfWork(_context);
        }
        public IEnumerable<WarehouseZone> GetAllWarehouseZone()
        {
            return _warehouseZoneUnitOfWork.WarehouseZoneRepository.GetAll();
        }
           
        //public ArticleTransfer GetUserById(int id)
        //{
        //    return _articleTransferUnitOfWork.ArticleTransferRepository.GetById(id);
        //}
       
        //public int Save(ArticleTransfer articleTransfer, int? loggedInUserId)
        //{
        //    _articleTransferUnitOfWork.ArticleTransferRepository.Add(articleTransfer);
        //    _articleTransferUnitOfWork.Save(loggedInUserId.ToString());
        //    return articleTransfer.ArticleTransferId;
        //}

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
            _warehouseZoneUnitOfWork.Dispose();
        }
    }
}
