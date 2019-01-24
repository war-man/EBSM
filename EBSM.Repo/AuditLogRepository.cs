using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class AuditLogRepository
    {
        private WmsDbContext db;
        public AuditLogRepository(WmsDbContext context)
        {
            db = context;
        }
        //public void Add(AuditLog item)
        //{
        //    db.ArticleTransfers.Add(item);
        //}
        //public void Edit(AuditLog item)
        //{
        //    db.Entry(item).State = EntityState.Modified;
        //}
        public AuditLog GetById(Guid id)
        {
            return db.AuditLogs.Find(id); 
        }
        public IEnumerable<object> GetAllAuditTables()
        {
            var auditTables= db.AuditLogs.GroupBy(x => x.TableName).Select(x => new { TableName = x.FirstOrDefault().TableName });
            return auditTables;
        }
        public IEnumerable<AuditLog> GetAll()
        {
            return db.AuditLogs;
        }
        public IEnumerable<AuditLog> GetAll(string AuditDateFrom, string AuditDateTo, string EventType, string AuditTable)
        {
            var fromDate = string.IsNullOrEmpty(AuditDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(AuditDateFrom);
            var toDate = string.IsNullOrEmpty(AuditDateTo) ? DateTime.Now.Date : Convert.ToDateTime(AuditDateTo).AddDays(1);
            return db.AuditLogs.Where(x => (AuditDateFrom == null || x.UpdatedDate >= fromDate) && (AuditDateTo == null || x.UpdatedDate < toDate)
                 && (EventType == null || x.EventType == EventType) && (AuditTable == null || x.TableName == AuditTable)).OrderByDescending(x => x.UpdatedDate);
        } 

    
    }
}
