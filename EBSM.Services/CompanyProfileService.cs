using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class CompanyProfileService
    {
        private WmsDbContext _context;
        private CompanyProfileUnitOfWork _companyProfileUnitOfWork;

        public CompanyProfileService()
        {
            _context = new WmsDbContext();
            _companyProfileUnitOfWork = new CompanyProfileUnitOfWork(_context);
        }
        public CompanyProfile GetComapnyProfile()
        {
            return _companyProfileUnitOfWork.CompnayProfileRepository.GetAll().FirstOrDefault();
        }
        public CompanyProfile GetCompanyProfileById(int id)
        {
            return _companyProfileUnitOfWork.CompnayProfileRepository.GetById(id);
        }
       
        public int Save(CompanyProfile companyProfile, int? loggedInUserId)
        {
            _companyProfileUnitOfWork.CompnayProfileRepository.Add(companyProfile);
            _companyProfileUnitOfWork.Save(loggedInUserId.ToString());
            return companyProfile.CompanyId;
        }
        public void Edit(CompanyProfile companyProfile, int? loggedInUserId)
        {
            _companyProfileUnitOfWork.CompnayProfileRepository.Edit(companyProfile);
            _companyProfileUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<CompanyProfile> GetAll()
        {
            return _companyProfileUnitOfWork.CompnayProfileRepository.GetAll();
        }
        //public IEnumerable<ArticleTransfer> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        //{
        //    return _articleTransferUnitOfWork.ArticleTransferRepository.GetAll(SelectedProductId, PName, TransferDateFrom, TransferDateTo);
        //}
        public void Dispose()
        {
            _companyProfileUnitOfWork.Dispose();
        }
    }
}
