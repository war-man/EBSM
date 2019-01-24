using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
   public class UserRoleRepository
   {
       private WmsDbContext db;
       public UserRoleRepository(WmsDbContext context)
        {
            db = context;
        }
        public IEnumerable<Role> GetAllRoles()
        {
            return db.Roles.Where(x => x.Status == 1);
        }
        public int GetCount()
        {
            return db.Roles.Count(x => x.Status == 1);
        }
        public Role GetById(int id)
        {
            return db.Roles.Find(id);
        }
        public Role GetRoleByRoleName(string name)
        {
            return db.Roles.FirstOrDefault(e => e.RoleName.ToLower() == name.ToLower());
        }
        public void Add(Role userRole)
        {
            db.Roles.Add(userRole);
        }
        public void Edit(Role userRole)
        {
            db.Entry(userRole).State = EntityState.Modified;
        }
        public bool IsRoleNameExist(string RoleName, string InitialRoleName)
       {
           bool isNotExist = true;
           if (RoleName != string.Empty && InitialRoleName == "undefined")
           {
               var isExist = db.Roles.Any(x => x.Status != 0 && x.RoleName.ToLower().Equals(RoleName.ToLower()));
               if (isExist)
               {
                   isNotExist = false;
               }
           }
           if (RoleName != string.Empty && InitialRoleName != "undefined")
           {
               var isExist = db.Roles.Any(x => x.Status!= 0 && x.RoleName.ToLower() == RoleName.ToLower() && x.RoleName.ToLower() != InitialRoleName.ToLower());
               if (isExist)
               {
                   isNotExist = false;
               }
           }
           return isNotExist;
       }

        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(Role item)
        {
            db.Roles.Remove(item);
        }
        public string[] GetRolesById(int id)
        {
            string[] roleTasks = (from a in db.RoleTasks
                                  join b in db.Users on a.RoleId equals b.RoleId
                                  where b.UserId.Equals(id)
                                  select a.Task).ToArray<string>();
            return roleTasks;

        }
        public string[] GetRolesByUserName(string userName)
        {
            string[] roleTasks = (from a in db.RoleTasks
                                  join b in db.Users on a.RoleId equals b.RoleId
                                  where b.UserName.Equals(userName)
                                  select a.Task).ToArray<string>();
            return roleTasks;

        }
    }
   public class RoleTaskRepository
   {
       private WmsDbContext db;
       public RoleTaskRepository(WmsDbContext context)
       {
           db = context;
       }
        public void Add(RoleTask item)
        {
            db.RoleTasks.Add(item);
        }
        //public void Edit(RoleTask item)
        //{
        //    db.Entry(item).State = EntityState.Modified;
        //}
        public RoleTask GetById(int id)
        {
            return db.RoleTasks.Find(id);
        }
        public IEnumerable<RoleTask> GetAll()
        {
            return db.RoleTasks;
        }
        public IEnumerable<RoleTask> GetAllByPurchaseId(int RoleId)
        {
            return db.RoleTasks.Where(x => x.RoleId == RoleId);
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(RoleTask item)
        {
            db.RoleTasks.Remove(item);
        }
   
    }  
}
