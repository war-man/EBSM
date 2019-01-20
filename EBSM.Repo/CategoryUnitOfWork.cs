using System;
namespace EBSM.Repo
{
    public class CategoryUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private CategoryRepository _categoryRepository { get; set; }

        public CategoryUnitOfWork(WmsDbContext context)
        {
            db = context;
            _categoryRepository = new CategoryRepository(db);
        }

        public CategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository;
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
