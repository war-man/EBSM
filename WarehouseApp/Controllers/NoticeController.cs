using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;
using EBSM.Entities;
using EBSM.Services;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class NoticeController : Controller
    {
        private NoticeService _noticeService = new NoticeService();

        // GET: /Notice/
      
        public ActionResult Index(NoticeboardPaginationViewModel model)
        {
            
            var notices =_noticeService.GetAll();
          if (!User.IsInRole("Global_SupAdmin") || !User.IsInRole("Global_Manager"))
            {
                notices = notices.Where(x=>x.Status!=0).ToList().OrderByDescending(x => x.CreatedDate);
            }
            model.Notices = notices.ToPagedList(model.Page, model.PageSize);
            return View("../Notice/Index", model);
        } 
        // GET: /Notice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = _noticeService.GetNoticeById(id.Value);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        // GET: /Notice/Create
        public ActionResult NewNotice()
        {

            return View();
        }

        // POST: /Notice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewNotice( Notice notice)
        {
            if (ModelState.IsValid)
            {
                notice.Status = 1;
                notice.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                notice.CreatedDate = DateTime.Now;
                _noticeService.Save(notice, AuthenticatedUser.GetUserFromIdentity().UserId);
               
                return RedirectToAction("Index");
            }

           
            return View(notice);
        }

        // GET: /Notice/Edit/5
        public ActionResult EditNotice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = _noticeService.GetNoticeById(id.Value);
            if (notice == null)
            {
                return HttpNotFound();
            }
          
            return View(notice);
        }

        // POST: /Notice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNotice(Notice notice)
        {
            if (ModelState.IsValid)
            {
                Notice editNotice = _noticeService.GetNoticeById(notice.NoticeId); 
                editNotice.Heading = notice.Heading;
                editNotice.Description = notice.Description;
                editNotice.Status = notice.Status;
                editNotice.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                editNotice.UpdatedDate = DateTime.Now;
                _noticeService.Edit(notice, AuthenticatedUser.GetUserFromIdentity().UserId);
                //db.Entry(editNotice).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }

            return View(notice);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _noticeService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//