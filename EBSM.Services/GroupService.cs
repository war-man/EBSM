using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class GroupService
    {
        private WmsDbContext _context;
        private GroupUnitOfWork _groupUnitOfWork;

        public GroupService()
        {
            _context = new WmsDbContext();
            _groupUnitOfWork = new GroupUnitOfWork(_context);
        }
       
        public Group GetGroupById(int id)
        {
            return _groupUnitOfWork.GroupRepository.GetById(id);
        }
       
        public int Save(Group group, int? loggedInUserId)
        {
            _groupUnitOfWork.GroupRepository.Add(group);
            _groupUnitOfWork.Save(loggedInUserId.ToString());
            return group.GroupNameId;
        }
        public void Edit(Group group, int? loggedInUserId)
        {
            _groupUnitOfWork.GroupRepository.Edit(group);
            _groupUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Group> GetAllGroups()
        {
            return _groupUnitOfWork.GroupRepository.GetAll();
        }
        public IEnumerable<Group> GetAll(string GrpName)
        {
            return _groupUnitOfWork.GroupRepository.GetAll(GrpName);
        }
        public bool isExistGroup(string groupName)
        {
            return _groupUnitOfWork.GroupRepository.isExistGroup(groupName);
        }
            public void Dispose()
        {
            _groupUnitOfWork.Dispose();
        }
    }
}
