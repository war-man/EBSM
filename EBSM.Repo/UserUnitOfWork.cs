using System;
namespace EBSM.Repo
{
    public class UserUnitOfWork: IDisposable
    {
        private WmsDbContext _context { get; set; }
        private UserRepository _userRepository { get; set; }

        public UserUnitOfWork(WmsDbContext context)
        {
            _context = context;
            _userRepository = new UserRepository(_context);
        }

        public UserRepository UserRepository
        {
            get
            {
                return _userRepository;
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
