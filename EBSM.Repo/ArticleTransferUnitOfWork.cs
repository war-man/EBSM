using System;
namespace EBSM.Repo
{
    public class ArticleTransferUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private ArticleTransferRepository _articleTransferRepository { get; set; }

        public ArticleTransferUnitOfWork(WmsDbContext context)
        {
            db = context;
            _articleTransferRepository = new ArticleTransferRepository(db);
        }

        public ArticleTransferRepository ArticleTransferRepository
        {
            get
            {
                return _articleTransferRepository;
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
