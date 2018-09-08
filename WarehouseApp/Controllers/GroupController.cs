using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;

using System.Web.Security;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;


namespace WarehouseApp.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult Index(GroupSearchViewModel model)
        {

            var groups = db.Groups.Where(x => (model.GrpName == null || x.GroupName.StartsWith(model.GrpName))).OrderBy(x => x.GroupName).ToList();
            model.Groups = groups.ToPagedList(model.Page, model.PageSize);
            return View("../Shop/Group/Index", model);
        } 

       
        public ActionResult AjaxCreateGroupName(Group group)
        {
            if (ModelState.IsValid)
            {
                var isExist = "No";
                var allGroups = db.Groups.Any(e => e.GroupName == group.GroupName);
                if (!allGroups)
                {
                    group.Status = 1;
                    group.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                    group.CreatedDate = DateTime.Now;
                    db.Groups.Add(group);
                     db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                     var data = new { group = group, isExist = isExist };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    isExist = "Yes";
                    var data = new { isExist = isExist };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateGroupName(int? id, string name)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            Group groupName = db.Groups.Find(id);
            if (groupName == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }
            groupName.GroupName = name;
            groupName.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
            groupName.UpdatedDate = DateTime.Now;
            db.Entry(groupName).State = EntityState.Modified;
             db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
            return Json(new { Result = "OK" });
        }
        public ActionResult UpdateGroupNameStatus(int? id, string tag)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            Group groupName = db.Groups.Find(id);
            if (groupName == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }
            if (tag == "inactive")
            {
                groupName.Status = 0;
               
            }
            else 
            {
                groupName.Status = 1;
               
            }

            groupName.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
            groupName.UpdatedDate = DateTime.Now;
            db.Entry(groupName).State = EntityState.Modified;
             db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
            return Json(new { Result = "OK" });
        }

      
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//