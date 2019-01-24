using System;
namespace EBSM.Repo
{
    public class GroupUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private GroupRepository _groupRepository { get; set; }

        public GroupUnitOfWork(WmsDbContext context)
        {
            db = context;
            _groupRepository = new GroupRepository(db);
        }

        public GroupRepository GroupRepository
        {
            get
            {
                return _groupRepository;
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
