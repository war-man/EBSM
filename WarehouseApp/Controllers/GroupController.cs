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
using EBSM.Entities;
using EBSM.Services;


namespace WarehouseApp.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private GroupService _groupService = new GroupService();

        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult Index(GroupSearchViewModel model)
        {

            var groups = _groupService.GetAll(model.GrpName).ToList();
            model.Groups = groups.ToPagedList(model.Page, model.PageSize);
            return View("../Shop/Group/Index", model);
        } 

       
        public ActionResult AjaxCreateGroupName(Group group)
        {
            if (ModelState.IsValid)
            {
                var isExist = "No";
                var isExistGroup = _groupService.isExistGroup(group.GroupName); 
                if (!isExistGroup)
                {
                    group.Status = 1;
                    group.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                    group.CreatedDate = DateTime.Now;
                    group.GroupNameId=_groupService.Save(group,AuthenticatedUser.GetUserFromIdentity().UserId);
                    
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
            Group groupName =_groupService.GetGroupById(id.Value);
            if (groupName == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }
            groupName.GroupName = name;
            groupName.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
            groupName.UpdatedDate = DateTime.Now;
            _groupService.Edit(groupName, AuthenticatedUser.GetUserFromIdentity().UserId);
            
            return Json(new { Result = "OK" });
        }
        public ActionResult UpdateGroupNameStatus(int? id, string tag)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            Group groupName = _groupService.GetGroupById(id.Value);
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

            groupName.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
            groupName.UpdatedDate = DateTime.Now;
            _groupService.Edit(groupName, AuthenticatedUser.GetUserFromIdentity().UserId);

            return Json(new { Result = "OK" });
        }

      
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _groupService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//