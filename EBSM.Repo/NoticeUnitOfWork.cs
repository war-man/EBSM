using System;
namespace EBSM.Repo
{
    public class NoticeUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private NoticeRepository _noticeRepository { get; set; }

        public NoticeUnitOfWork(WmsDbContext context)
        {
            db = context;
            _noticeRepository = new NoticeRepository(db);
        }

        public NoticeRepository NoticeRepository
        {
            get
            {
                return _noticeRepository;
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
