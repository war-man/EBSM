using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Caching;
using WarehouseApp.Models;
using EBSM.Entities;
using EBSM.Services;

namespace WarehouseApp
{
    public class CustomRoleProvider:RoleProvider
    {
        private int _cacheTimeoutInMinute = 60;
        private UserRoleService _userRoleService;

        public CustomRoleProvider()
        {
            _userRoleService = new UserRoleService();
        }
        public override bool IsUserInRole(string userId, string roleName)
        {
            var userRoles = GetRolesForUser(userId);
            return userRoles.Contains(roleName);

        }


        //public override string[] GetRolesForUser(string username)
        //{
        //    if (!HttpContext.Current.User.Identity.IsAuthenticated)
        //    {
        //        return null;
        //    }
        //    //check cache
        //    //var cacheKey = string.Format("{0_role}", username);
        //    //if (HttpRuntime.Cache[cacheKey] != null)
        //    //{
        //    //    return (string[]) HttpRuntime.Cache[cacheKey];
        //    //}
        //    string[] roles = new string[] { };
        //    using (RfwDbContext db = new RfwDbContext())
        //    {
        //        roles = (from a in db.Roles
        //                 join b in db.Users on a.RoleId equals b.RoleId 
        //                 where b.UserName.Equals(username)
        //                 select a.RoleName).ToArray<string>();
        //        //if (roles.Count() > 0)
        //        //{
        //        //    HttpRuntime.Cache.Insert(cacheKey,roles,null,DateTime.Now.AddMinutes(_cacheTimeoutInMinute), Cache.NoSlidingExpiration);
        //        //}

        //    }
        //    return roles;
        //}

        public override string[] GetRolesForUser(string userid)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }
            //check cache
            //var cacheKey = string.Format("{0_role}", username);
            //if (HttpRuntime.Cache[cacheKey] != null)
            //{
            //    return (string[]) HttpRuntime.Cache[cacheKey];
            //}
            string[] RoleTasks = new string[] { };
            int id = Convert.ToInt32(userid.Split('|')[0]);

            //using (WmsDbContext db = new WmsDbContext())
            //{
            //    RoleTasks = (from a in db.RoleTasks
            //                 join b in db.Users on new {  a.RoleId } equals new {  b.RoleId }
            //                 where b.UserId.Equals(id)
            //                 select a.Task).ToArray<string>();
            //var user = db.Users.FirstOrDefault(u => u.UserName == username);
            //deptRoles =
            //    db.DeptRoleConfigs.Where(x => x.DepartmentId == user.DepartmentId && x.RoleId == user.RoleId)
            //        .Select(x => x.DepartmentRole)
            //        .ToArray<string>();
            //if (roles.Count() > 0)
            //{
            //    HttpRuntime.Cache.Insert(cacheKey,roles,null,DateTime.Now.AddMinutes(_cacheTimeoutInMinute), Cache.NoSlidingExpiration);
            //}

            // }
            RoleTasks= _userRoleService.GetRolesById(id);
            return RoleTasks;
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }

    }
    // Roles authorization=========================================
    public class RolesAttribute : AuthorizeAttribute
    {
        // Multiple roles authorization=========================================

        public RolesAttribute(params string[] roles)
        
        {
            Roles = String.Join(",", roles);

        }
        // Handle Unauthorized Request =========================================

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // The user is not authenticated
                base.HandleUnauthorizedRequest(filterContext);
            }else if (!this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
            {
                // The user is not in any of the listed roles => 
                // show the unauthorized view
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/403_Unauthorized.cshtml"
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

         //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    //    {
    //        if (filterContext.HttpContext.User.Identity.IsAuthenticated)
    //        {
    //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Area = "", Controller = "Error", Action = "Unauthorized" }));
    //        }
    //        else
    //        {
    //            base.HandleUnauthorizedRequest(filterContext);
    //        }
    //    }
    }
  
    
    public class RoleTaskCheckBoxModel
    {

        public string TaskName { get; set; }
        public string TaskNameDisplay { get; set; }
        public bool IsChecked { get; set; }


        public static List<RoleTaskCheckBoxModel> TaskNames = new List<RoleTaskCheckBoxModel>
        {
            new RoleTaskCheckBoxModel {TaskName="Role_Creation", TaskNameDisplay="Role Creation"},
            new RoleTaskCheckBoxModel {TaskName="User_Creation", TaskNameDisplay="User Creation"},
            new RoleTaskCheckBoxModel {TaskName="Purchase_Create", TaskNameDisplay="Purchase Create"},
            new RoleTaskCheckBoxModel {TaskName="StockIn", TaskNameDisplay="Stock In"},
             new RoleTaskCheckBoxModel {TaskName="SalesOrder_Create", TaskNameDisplay="Sales Order Create"},
            new RoleTaskCheckBoxModel {TaskName="SalesOrder_Edit", TaskNameDisplay="Sales Order Edit"},
            new RoleTaskCheckBoxModel {TaskName="Sales_Create", TaskNameDisplay="Sales Create"},
            new RoleTaskCheckBoxModel {TaskName="Sales_Edit", TaskNameDisplay="Sales Edit"},
            new RoleTaskCheckBoxModel {TaskName="Sales_Return", TaskNameDisplay="Sales Return"},
            new RoleTaskCheckBoxModel {TaskName="Configuration", TaskNameDisplay="Configuration"},
            new RoleTaskCheckBoxModel {TaskName="Stock_View", TaskNameDisplay="Stock View"},
            new RoleTaskCheckBoxModel {TaskName="Damage", TaskNameDisplay="Damage"},
            new RoleTaskCheckBoxModel {TaskName="Expenses_Entry", TaskNameDisplay="Expenses Entry"},
            new RoleTaskCheckBoxModel {TaskName="Expenses_Edit", TaskNameDisplay="Expenses Edit"},
            new RoleTaskCheckBoxModel {TaskName="Db_Backup", TaskNameDisplay="DB Backup"},
            new RoleTaskCheckBoxModel {TaskName="Withdraw_Deposit", TaskNameDisplay="Withdraw Deposit"},
            new RoleTaskCheckBoxModel {TaskName="Customer_View", TaskNameDisplay="Customer View"},
            new RoleTaskCheckBoxModel {TaskName="Report_View", TaskNameDisplay="Report View"},
            new RoleTaskCheckBoxModel {TaskName="Company_Profile_Edit", TaskNameDisplay="Company Profile Edit"},
            new RoleTaskCheckBoxModel {TaskName="Article_Transfer", TaskNameDisplay="Article Transfer"},
            new RoleTaskCheckBoxModel {TaskName="Product_Transfer", TaskNameDisplay="Product Transfer"},
            new RoleTaskCheckBoxModel {TaskName="Warehouse_Zone_Creation", TaskNameDisplay="Warehouse Zone Creation"},
            new RoleTaskCheckBoxModel {TaskName="Account_Add", TaskNameDisplay="Account Add"},
            new RoleTaskCheckBoxModel {TaskName="Bill_Edit", TaskNameDisplay="Bill Edit"},
            new RoleTaskCheckBoxModel {TaskName="Barcode_Generate", TaskNameDisplay="Barcode Generate"},
            new RoleTaskCheckBoxModel {TaskName="Customer_Due_Payment", TaskNameDisplay="Customer Due Payment"},
            new RoleTaskCheckBoxModel {TaskName="Customer_Price_Setup", TaskNameDisplay="Customer Price Setup"},
        };

        //    public static Dictionary<String, String> TaskNames = new Dictionary<String, String>()
        //    {
        //        {"UserCreation", "User Creation"},
        //        {"PurchaseCreate", "Purchase Create"},
        //        {"PurchaseEdit", "Purchase Edit"},
        //        {"PurchasePrice", "Purchase Price"},
        //        {"SalesCreate", "Sales Create"},
        //        {"SalesEdit", "Sales Edit"},
        //        {"Configuration", "Configuration"},
        //        {"StockView", "Stock View"},
        //        {"Damage", "Damage"},
        //        {"ExpensesEntry", "Expenses Entry"},
        //        {"Report View", "Report View"},
        //        {"DbBackup", "DB Backup"},
        //        {"WithdrawDeposit", "Withdraw Deposit"},
        //        {"CustomerView", "Customer View"},
        //        {"CompanyProfileEdit", "Company Profile Edit"},


        //    };
    }
}
