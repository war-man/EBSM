using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class PurchaseService
    {
        private WmsDbContext _context;
        private PurchaseUnitOfWork _purchaseUnitOfWork;

        public PurchaseService()
        {
            _context = new WmsDbContext();
            _purchaseUnitOfWork = new PurchaseUnitOfWork(_context);
        }
       
        public Purchase GetPurchaseById(int id)
        {
            return _purchaseUnitOfWork.PurchaseRepository.GetById(id);
        }
       
        public int Save(Purchase purchase, int? loggedInUserId)
        {
            _purchaseUnitOfWork.PurchaseRepository.Add(purchase);
            _purchaseUnitOfWork.Save(loggedInUserId.ToString());
            return purchase.PurchaseId;
        }
        public void Edit(Purchase purchase, int? loggedInUserId)
        {
            _purchaseUnitOfWork.PurchaseRepository.Edit(purchase);
            _purchaseUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Purchase> GetAllPurchases()
        {
            return _purchaseUnitOfWork.PurchaseRepository.GetAll();
        }
        public IEnumerable<Purchase> GetAllPurchases(string PurchaseNo, string PurchaseDateFrom, string PurchaseDateTo, int? SupplierId, string TransactionMode)
        {
            return _purchaseUnitOfWork.PurchaseRepository.GetAll( PurchaseNo,  PurchaseDateFrom,  PurchaseDateTo,  SupplierId,  TransactionMode);
        }
        public int GetCount()
        {
            return _purchaseUnitOfWork.PurchaseRepository.GetCount();
        }
        public void DeletePurchase(Purchase purchase)
        {
            _purchaseUnitOfWork.PurchaseRepository.DeleteFromDbByItem(purchase);
            _purchaseUnitOfWork.Save();
        }
        public IEnumerable<Purchase> GetAllPurchasesByYear(int year)
        {
            return _purchaseUnitOfWork.PurchaseRepository.GetAllPurchasesByYear(year);
        }
        public IEnumerable<Purchase> GetAllPurchasesByCurrentYear()
        {
            return _purchaseUnitOfWork.PurchaseRepository.GetAllPurchasesByYear(DateTime.Now.Year);
        }
        //Purchase cost======================
        public PurchaseCost GetCostByPurchaseId(int purchaseId)
        {
            return _purchaseUnitOfWork.PurchaseCostRepository.GetByPurchaseId(purchaseId);
        } public IEnumerable<PurchaseCost> GetAllCostByPurchaseId(int purchaseId)
        {
            return _purchaseUnitOfWork.PurchaseCostRepository.GetAllByPurchaseId(purchaseId);
        }
        public int SavePurchaseCost(PurchaseCost purchaseCost, int? loggedInUserId)
        {
            _purchaseUnitOfWork.PurchaseCostRepository.Add(purchaseCost);
            _purchaseUnitOfWork.Save(loggedInUserId.ToString());
            return purchaseCost.PurchaseCostId;
        }
        public void EditPurchaseCost(PurchaseCost purchaseCost, int? loggedInUserId)
        {
            _purchaseUnitOfWork.PurchaseCostRepository.Edit(purchaseCost);
            _purchaseUnitOfWork.Save(loggedInUserId.ToString());
        }
        //Purchase Products======================
        public IEnumerable<PurchaseProduct> GetAllPurchaseProducts(string product, string fromDate2, string toDate2)
        {
            return _purchaseUnitOfWork.PurchaseProductRepository.GetAll(product,fromDate2, toDate2);
        
    }
    public void SavePurchaseProduct(PurchaseProduct purchaseProduct)
        {
            _purchaseUnitOfWork.PurchaseProductRepository.Add(purchaseProduct);
            _purchaseUnitOfWork.Save();
        }
        public void SavePurchaseProductList(IEnumerable<PurchaseProduct> purchaseProductList)
        {
            foreach (var item in purchaseProductList)
            {
                _purchaseUnitOfWork.PurchaseProductRepository.Add(item);
            }
            _purchaseUnitOfWork.Save();
        }
        public IEnumerable<PurchaseProduct> GetAllProductByPurchaseId(int purchaseId)
        {
            return _purchaseUnitOfWork.PurchaseProductRepository.GetAllByPurchaseId(purchaseId);
        }
        public void DeletePurchaseProduct(PurchaseProduct purchaseProduct)
        {
             _purchaseUnitOfWork.PurchaseProductRepository.DeleteFromDbByItem(purchaseProduct);
            _purchaseUnitOfWork.Save();
        }
        public void DeletePurchaseProductList(IEnumerable<PurchaseProduct> purchaseProductList)
        {
            foreach (var item in purchaseProductList)
            {
                _purchaseUnitOfWork.PurchaseProductRepository.DeleteFromDbByItem(item);
            }
            _purchaseUnitOfWork.Save();
        }
        //Purchase payment
        public int SavePurchasePayment(PurchasePayment purchasePayment, int? loggedInUserId)
        {
            _purchaseUnitOfWork.PurchasePaymentRepository.Add(purchasePayment);
            _purchaseUnitOfWork.Save(loggedInUserId.ToString());
            return purchasePayment.PurchasePaymentId;
        }
        public void EditPurchasePayment(PurchasePayment purchasePayment, int? loggedInUserId)
        {
            _purchaseUnitOfWork.PurchasePaymentRepository.Edit(purchasePayment);
            _purchaseUnitOfWork.Save(loggedInUserId.ToString());
        }
        public void Dispose()
        {
            _purchaseUnitOfWork.Dispose();
        }
    }
}
