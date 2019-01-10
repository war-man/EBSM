using System;
namespace EBSM.Repo
{
    public class TransactionAccountUnitOfWork : IDisposable
    {
        private WmsDbContext db { get; set; }
        private BankAccountRepository _bankAccountRepository { get; set; }
        private CashAccountRepository _cashAccountRepository { get; set; }
        private MobileBankingAccountRepository _mobileBankingAccountRepository { get; set; }

        public TransactionAccountUnitOfWork(WmsDbContext context)
        {
            db = context;
            _bankAccountRepository = new BankAccountRepository(db);
            _cashAccountRepository = new CashAccountRepository(db);
            _mobileBankingAccountRepository = new MobileBankingAccountRepository(db);
        }

        public BankAccountRepository BankAccountRepository
        {
            get
            {
                return _bankAccountRepository;
            }
        } public CashAccountRepository CashAccountRepository
        {
            get
            {
                return _cashAccountRepository;
            }
        }public MobileBankingAccountRepository MobileBankingAccountRepository
        {
            get
            {
                return _mobileBankingAccountRepository;
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
