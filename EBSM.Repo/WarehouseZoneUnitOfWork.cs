using System;
namespace EBSM.Repo
{
    public class WarehouseZoneUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private WarehouseZoneRepository _warehouseZoneRepository { get; set; }

        public WarehouseZoneUnitOfWork(WmsDbContext context)
        {
            db = context;
            _warehouseZoneRepository = new WarehouseZoneRepository(db);
        }

        public WarehouseZoneRepository WarehouseZoneRepository
        {
            get
            {
                return _warehouseZoneRepository;
            }
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
