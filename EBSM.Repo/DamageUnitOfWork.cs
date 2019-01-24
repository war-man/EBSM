using System;
namespace EBSM.Repo
{
    public class DamageUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private DamageRepository _damageRepository { get; set; }
        private DamageStockRepository _damageStockRepository { get; set; }
        private DamageDismissRepository _damageDismissRepository { get; set; }
        private DamageReturnRepository _damageReturnRepository { get; set; }

        public DamageUnitOfWork(WmsDbContext context)
        {
            db = context;
            _damageRepository = new DamageRepository(db);
            _damageStockRepository = new DamageStockRepository(db);
            _damageDismissRepository = new DamageDismissRepository(db);
            _damageReturnRepository = new DamageReturnRepository(db);
        }

        public DamageRepository DamageRepository
        {
            get
            {
                return _damageRepository;
            }
        }
        public DamageStockRepository DamageStockRepository
        {
            get
            {
                return _damageStockRepository;
            }
        }
        public DamageDismissRepository DamageDismissRepository
        {
            get
            {
                return _damageDismissRepository;
            }
        }
        public DamageReturnRepository DamageReturnRepository
        {
            get
            {
                return _damageReturnRepository;
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
