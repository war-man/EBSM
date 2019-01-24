using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class DamageService
    {
        private WmsDbContext _context;
        private DamageUnitOfWork _damageUnitOfWork;

        public DamageService()
        {
            _context = new WmsDbContext();
            _damageUnitOfWork = new DamageUnitOfWork(_context);
        }
       
        public Damage GetDamageById(int id)
        {
            return _damageUnitOfWork.DamageRepository.GetById(id);
        }
       
        public int Save(Damage damage, int? loggedInUserId)
        {
            _damageUnitOfWork.DamageRepository.Add(damage);
            _damageUnitOfWork.Save(loggedInUserId.ToString());
            return damage.DamageId;
        }
        public void Edit(Damage damage, int? loggedInUserId)
        {
            _damageUnitOfWork.DamageRepository.Edit(damage);
            _damageUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Damage> GetAll()
        {
            return _damageUnitOfWork.DamageRepository.GetAll();
        }
        public IEnumerable<Damage> GetAll(int? SelectedProductId, string ProductNameFull)
        {
            return _damageUnitOfWork.DamageRepository.GetAll(SelectedProductId, ProductNameFull);
        }

        //Damage stock  =============

        public DamageStock GetDamageStockByStockId(int stockId)
        {
            return _damageUnitOfWork.DamageStockRepository.GetByStockId(stockId);
        }
        public IEnumerable<DamageStock> GetAllDamageStocks()
        {
            return _damageUnitOfWork.DamageStockRepository.GetAll();
        }
        public int SaveDamageStock(DamageStock damageStock, int? loggedInUserId)
        {
            _damageUnitOfWork.DamageStockRepository.Add(damageStock);
            _damageUnitOfWork.Save(loggedInUserId.ToString());
            return damageStock.DamageStockId;
        }
        public void EditDamageStock(DamageStock damageStock, int? loggedInUserId)
        {
            _damageUnitOfWork.DamageStockRepository.Edit(damageStock);
            _damageUnitOfWork.Save(loggedInUserId.ToString());
        }
        //Damage Dismiss   =============
        //public DamageStock GetReturnStockByStockId(int stockId)
        //{
        //    return _damageUnitOfWork.DamageStockRepository.GetByStockId(stockId);
        //}
        public int SaveDamageDismiss(DamageDismiss damageDismiss, int? loggedInUserId)
        {
            _damageUnitOfWork.DamageDismissRepository.Add(damageDismiss);
            _damageUnitOfWork.Save(loggedInUserId.ToString());
            return damageDismiss.DamageDismissId;
        }
        public void EditDamageDismiss(DamageDismiss damageDismiss, int? loggedInUserId)
        {
            _damageUnitOfWork.DamageDismissRepository.Edit(damageDismiss);
            _damageUnitOfWork.Save(loggedInUserId.ToString());
        }
        //Damage Return   =============
        //public DamageStock GetDamageStockByStockId(int stockId)
        //{
        //    return _damageUnitOfWork.DamageStockRepository.GetByStockId(stockId);
        //}
        public int SaveDamageReturn(DamageReturn damageReturn, int? loggedInUserId)
        {
            _damageUnitOfWork.DamageReturnRepository.Add(damageReturn);
            _damageUnitOfWork.Save(loggedInUserId.ToString());
            return damageReturn.DamageReturnId;
        }
        public void EditDamageReturn(DamageReturn damageReturn, int? loggedInUserId)
        {
            _damageUnitOfWork.DamageReturnRepository.Edit(damageReturn);
            _damageUnitOfWork.Save(loggedInUserId.ToString());
        }
        public void Dispose()
        {
            _damageUnitOfWork.Dispose();
        }
    }
}
