using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class NoticeService
    {
        private WmsDbContext _context;
        private NoticeUnitOfWork _notificationUnitOfWork;

        public NoticeService()
        {
            _context = new WmsDbContext();
            _notificationUnitOfWork = new NoticeUnitOfWork(_context);
        }
       
        public Notice GetNoticeById(int id)
        {
            return _notificationUnitOfWork.NoticeRepository.GetById(id);
        }
       
        public int Save(Notice notice, int? loggedInUserId)
        {
            _notificationUnitOfWork.NoticeRepository.Add(notice);
            _notificationUnitOfWork.Save(loggedInUserId.ToString());
            return notice.NoticeId;
        }
        public void Edit(Notice notice, int? loggedInUserId)
        {
            _notificationUnitOfWork.NoticeRepository.Edit(notice);
            _notificationUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Notice> GetAll()
        {
            return _notificationUnitOfWork.NoticeRepository.GetAll();
        }
        public int GetCount()
        {
            return _notificationUnitOfWork.NoticeRepository.GetCount();
        }
        //public IEnumerable<Notice> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        //{
        //    return _notificationUnitOfWork.NoticeRepository.GetAll(SelectedProductId, PName, TransferDateFrom, TransferDateTo);
        //}
        public int LastDateNoticeCount()
        {
            return _notificationUnitOfWork.NoticeRepository.LastDateNoticeCount();
        }
        public DateTime LastDateOfNoticePublished()
        {
            return _notificationUnitOfWork.NoticeRepository.LastDateOfNoticePublished();
        }
        public void Dispose()
        {
            _notificationUnitOfWork.Dispose();
        }
    }
}
