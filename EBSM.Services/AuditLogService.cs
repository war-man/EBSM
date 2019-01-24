using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class AuditLogService
    {
        private WmsDbContext _context;
        private AuditLogUnitOfWork _auditLogUnitOfWork;

        public AuditLogService()
        {
            _context = new WmsDbContext();
            _auditLogUnitOfWork = new AuditLogUnitOfWork(_context);
        }
       
        public AuditLog GetAuditLogById(Guid id)
        {
            return _auditLogUnitOfWork.AuditLogRepository.GetById(id);
        }
       
       
        public IEnumerable<AuditLog> GetAllAuditLog()
        {
            return _auditLogUnitOfWork.AuditLogRepository.GetAll();
        }
        public IEnumerable<AuditLog> GetAllAuditLog(string AuditDateFrom, string AuditDateTo, string EventType, string AuditTable)
        {
            return _auditLogUnitOfWork.AuditLogRepository.GetAll(AuditDateFrom, AuditDateTo, EventType, AuditTable);
        }
        public IEnumerable<AuditLog> GetAllAuditTables()
        {
            return _auditLogUnitOfWork.AuditLogRepository.GetAll();
        }
        public void Dispose()
        {
            _auditLogUnitOfWork.Dispose();
        }
    }
}
