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
        } public IEnumerable<WarehouseZone> GetAllWarehouseZone(string ZoneName)
        {
            return _warehouseZoneUnitOfWork.WarehouseZoneRepository.GetAll(ZoneName);
        }
        public WarehouseZone GetWarehouseZoneByName(string name)
        {
            return _warehouseZoneUnitOfWork.WarehouseZoneRepository.GetWarehouseZoneByName(name);
        }
        public WarehouseZone GetWarehouseZoneById(int id)
        {
            return _warehouseZoneUnitOfWork.WarehouseZoneRepository.GetById(id);
        }

        public int Save(WarehouseZone warehouseZone, int? loggedInUserId)
        {
            _warehouseZoneUnitOfWork.WarehouseZoneRepository.Add(warehouseZone);
            _warehouseZoneUnitOfWork.Save(loggedInUserId.ToString());
            return warehouseZone.ZoneId;
        }public void Edit(WarehouseZone warehouseZone, int? loggedInUserId)
        {
            _warehouseZoneUnitOfWork.WarehouseZoneRepository.Edit(warehouseZone);
            _warehouseZoneUnitOfWork.Save(loggedInUserId.ToString());
        }
        public bool IsNameUsed(string ZoneName, string InitialZoneName)
        { return _warehouseZoneUnitOfWork.WarehouseZoneRepository.IsNameUsed(ZoneName, InitialZoneName); }
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

            //ware house=============
            public IEnumerable<Warehouse> GetAllWarehouses()
        {
            return _warehouseZoneUnitOfWork.WarehouseZoneRepository.GetAllWarehouses();
        }public Warehouse GetWarehouseByName(string name)
        {
            return _warehouseZoneUnitOfWork.WarehouseZoneRepository.GetWarehouseByName(name);
        }
        public void Dispose()
        {
            _warehouseZoneUnitOfWork.Dispose();
        }
    }
}
