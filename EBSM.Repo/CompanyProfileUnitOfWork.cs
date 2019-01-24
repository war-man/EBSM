using System;
namespace EBSM.Repo
{
    public class CompanyProfileUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private CompnayProfileRepository _compnayProfileRepository { get; set; }

        public CompanyProfileUnitOfWork(WmsDbContext context)
        {
            db = context;
            _compnayProfileRepository = new CompnayProfileRepository(db);
        }

        public CompnayProfileRepository CompnayProfileRepository
        {
            get
            {
                return _compnayProfileRepository;
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
