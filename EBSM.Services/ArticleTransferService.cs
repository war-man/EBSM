using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class ArticleTransferService
    {
        private WmsDbContext _context;
        private ArticleTransferUnitOfWork _articleTransferUnitOfWork;

        public ArticleTransferService()
        {
            _context = new WmsDbContext();
            _articleTransferUnitOfWork = new ArticleTransferUnitOfWork(_context);
        }
       
        public ArticleTransfer GetArticleTransferById(int id)
        {
            return _articleTransferUnitOfWork.ArticleTransferRepository.GetById(id);
        }
       
        public int Save(ArticleTransfer articleTransfer, int? loggedInUserId)
        {
            _articleTransferUnitOfWork.ArticleTransferRepository.Add(articleTransfer);
            _articleTransferUnitOfWork.Save(loggedInUserId.ToString());
            return articleTransfer.ArticleTransferId;
        }
        public void Edit(ArticleTransfer articleTransfer, int? loggedInUserId)
        {
            _articleTransferUnitOfWork.ArticleTransferRepository.Edit(articleTransfer);
            _articleTransferUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<ArticleTransfer> GetAll()
        {
            return _articleTransferUnitOfWork.ArticleTransferRepository.GetAll();
        }
        public IEnumerable<ArticleTransfer> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        {
            return _articleTransferUnitOfWork.ArticleTransferRepository.GetAll(SelectedProductId, PName, TransferDateFrom, TransferDateTo);
        }
        public void Dispose()
        {
            _articleTransferUnitOfWork.Dispose();
        }
    }
}
