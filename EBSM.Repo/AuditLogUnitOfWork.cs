using System;
namespace EBSM.Repo
{
    public class AuditLogUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private AuditLogRepository _auditLogRepository { get; set; }

        public AuditLogUnitOfWork(WmsDbContext context)
        {
            db = context;
            _auditLogRepository = new AuditLogRepository(db);
        }

        public AuditLogRepository AuditLogRepository
        {
            get
            {
                return _auditLogRepository;
            }
        }
        public void Save()
        {
            db.SaveChanges("");
        }
        public void Save(string loggedInUserId)
        {
            db.SaveChanges(loggedInUserId);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
