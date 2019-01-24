using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class UserRoleService
    {
        private WmsDbContext _context;
        private UserRoleUnitOfWork _roleUnitOfWork;
        //private RoleTaskCheckBoxModel _roleTaskCheckBoxModel;

        public UserRoleService()
        {
            _context = new WmsDbContext();
            _roleUnitOfWork = new UserRoleUnitOfWork(_context);
          // _roleTaskCheckBoxModel=new RoleTaskCheckBoxModel();

        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _roleUnitOfWork.RoleRepository.GetAllRoles();
        }

        public Role GetRoleByid(int id)
        {
            return _roleUnitOfWork.RoleRepository.GetById(id);
        }

        public void DeleteRole(int id,int authorizeId)
        {
            _roleUnitOfWork.RoleRepository.DeleteFromDbById(id);
            _roleUnitOfWork.Save(authorizeId.ToString());
        }

        public int GetCount()
        {

            return _roleUnitOfWork.RoleRepository.GetCount();

        }
        public void SaveRole(Role role, int authorizeId)
        {
            _roleUnitOfWork.RoleRepository.Add(role);
            _roleUnitOfWork.Save(authorizeId.ToString());

        }
        public void AddRole(string roleName, List<RoleTask> rolePermissionList,int authorizeId)
        {
            
            var role = new Role()
            {
                RoleName = roleName,
                Status = 1,
                RoleTasks = rolePermissionList
            };
            
            _roleUnitOfWork.RoleRepository.Add(role);
            _roleUnitOfWork.Save(authorizeId.ToString());
           
        }
        public void EditRole(Role role, int authorizeId)
        {
            _roleUnitOfWork.RoleRepository.Edit(role);
            _roleUnitOfWork.Save(authorizeId.ToString());
        }
        public void EditRole(int id, string roleName, List<RoleTask> rolePermissionList, int authorizeId)
        {
            var role = _roleUnitOfWork.RoleRepository.GetById(id);
            role.RoleName = roleName;
            var rolePermissions = role.RoleTasks.ToList();
            foreach (var removePermission in rolePermissions)
            {
                _roleUnitOfWork.RoleTaskRepository.DeleteFromDbByItem(removePermission);
            }
            _roleUnitOfWork.Save(authorizeId.ToString());
            role.RoleTasks = rolePermissionList;
            _roleUnitOfWork.Save(authorizeId.ToString());

        }
        public bool IsRoleNameExist(string RoleName, string InitialRoleName)
        {
            return _roleUnitOfWork.RoleRepository.IsRoleNameExist(RoleName, InitialRoleName);
        }

        public Role GetUserRoleByName(string name)
        {
            return _roleUnitOfWork.RoleRepository.GetRoleByRoleName(name);
        }
        public string[] GetRolesById(int id)
        {
            return _roleUnitOfWork.RoleRepository.GetRolesById(id);

        }
        public string[] GetRolesByUserName(string userName)
        { return _roleUnitOfWork.RoleRepository.GetRolesByUserName(userName); 
}
        public void DeleteRoleTasks(IEnumerable<RoleTask>roleTasks)
        {
            foreach (var removeItem in roleTasks)
            {
                _roleUnitOfWork.RoleTaskRepository.DeleteFromDbByItem(removeItem);
            }
            _roleUnitOfWork.Save();
        }
    }
   
   
}
