using System;
namespace EBSM.Repo
{
    public class ArticleTransferUnitOfWork : IDisposable
    {
        private WmsDbContext _context { get; set; }
        private ArticleTransferRepository _articleTransferRepository { get; set; }

        public ArticleTransferUnitOfWork(WmsDbContext context)
        {
            _context = context;
            _articleTransferRepository = new ArticleTransferRepository(_context);
        }

        public ArticleTransferRepository ArticleTransferRepository
        {
            get
            {
                return _articleTransferRepository;
            }
        }
        public void Save(string loggedInUserId)
        {
            _context.SaveChanges(loggedInUserId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
