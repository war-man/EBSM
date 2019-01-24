using System;
namespace EBSM.Repo
{
    public class UserUnitOfWork: IDisposable
    {
        private WmsDbContext db { get; set; }
        private UserRepository _userRepository { get; set; }

        public UserUnitOfWork(WmsDbContext context)
        {
            db = context;
            _userRepository = new UserRepository(db);
        }

        public UserRepository UserRepository
        {
            get
            {
                return _userRepository;
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
