using System;
namespace EBSM.Repo
{
    public class CustomerUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private CustomerRepository _customerRepository { get; set; }

        public CustomerUnitOfWork(WmsDbContext context)
        {
            db = context;
            _customerRepository = new CustomerRepository(db);
        }

        public CustomerRepository CustomerRepository
        {
            get
            {
                return _customerRepository;
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
