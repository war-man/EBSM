using System;
namespace EBSM.Repo
{
    public class UserRoleUnitOfWork : IDisposable
    {
        private WmsDbContext db{ get; set; }
        private UserRoleRepository _roleRepository { get; set; }
        private RoleTaskRepository _roleTaskRepository { get; set; }

        public UserRoleUnitOfWork(WmsDbContext context)
        {
            db = context;
            _roleRepository = new UserRoleRepository(db);
            _roleTaskRepository = new RoleTaskRepository(db);
        }

        public UserRoleRepository RoleRepository
        {
            get
            {
                return _roleRepository;
            }
        }public RoleTaskRepository RoleTaskRepository
        {
            get
            {
                return _roleTaskRepository;
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
