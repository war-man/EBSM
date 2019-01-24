﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class NoticeRepository
    {
        private WmsDbContext db;
        public NoticeRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Notice item)
        {
            db.Notices.Add(item);
        }
        public void Edit(Notice item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Notice GetById(int id)
        {
            return db.Notices.Find(id); 
        }
        public IEnumerable<Notice> GetAll()
        {
            return db.Notices.Where(x => x.Status != 0).OrderByDescending(x => x.CreatedDate);
        }
        public int GetCount()
        {
            return db.Notices.Count(x => x.Status != 0);
        } public int LastDateNoticeCount()
        {
            return db.Notices.Where(x => x.Status != 0).GroupBy(t => t.CreatedDate.Date).First().ToList().Count;
        }
        public DateTime LastDateOfNoticePublished()
        {
            return db.Notices.Where(x => x.Status != 0).GroupBy(t => t.CreatedDate.Date).First().ToList().First().CreatedDate;
        }
        //public IEnumerable<Notice> GetAll(int? SelectedProductId, string PName, string TransferDateFrom, string TransferDateTo)
        //{
        //    var fromDate = string.IsNullOrEmpty(TransferDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateFrom);
        //    var toDate = string.IsNullOrEmpty(TransferDateTo) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateTo).AddDays(1);
        //    return db.ArticleTransfers.ToList().Where(x => (SelectedProductId == null || x.StockFrom.ProductId == SelectedProductId) && (PName == null || (x.StockFrom.Product.ProductFullName.StartsWith(PName) || x.StockFrom.Product.ProductFullName.Contains(" " + PName))) && (TransferDateFrom == null || x.TransferDate.Date >= fromDate) && (TransferDateTo == null || x.TransferDate.Date < toDate)).OrderByDescending(o => o.CreatedDate);
        //} 

    
    }
}
