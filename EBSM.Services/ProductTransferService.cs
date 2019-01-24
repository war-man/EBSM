using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class ProductTransferService
    {
        private WmsDbContext _context;
        private ProductTransferUnitOfWork _productTransferUnitOfWork;

        public ProductTransferService()
        {
            _context = new WmsDbContext();
            _productTransferUnitOfWork = new ProductTransferUnitOfWork(_context);
        }
       
        public TransferProduct GetArticleTransferById(int id)
        {
            return _productTransferUnitOfWork.ProductTransferRepository.GetById(id);
        }
       
        public int Save(TransferProduct transferProduct, int? loggedInUserId)
        {
            _productTransferUnitOfWork.ProductTransferRepository.Add(transferProduct);
            _productTransferUnitOfWork.Save(loggedInUserId.ToString());
            return transferProduct.ProductTransferId;
        }
        public void Edit(TransferProduct transferProduct, int? loggedInUserId)
        {
            _productTransferUnitOfWork.ProductTransferRepository.Edit(transferProduct);
            _productTransferUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<TransferProduct> GetAll()
        {
            return _productTransferUnitOfWork.ProductTransferRepository.GetAll();
        }
        public IEnumerable<TransferProduct> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        {
            return _productTransferUnitOfWork.ProductTransferRepository.GetAll(SelectedProductId, PName, TransferDateFrom, TransferDateTo);
        }
        public void Dispose()
        {
            _productTransferUnitOfWork.Dispose();
        }
    }
}
