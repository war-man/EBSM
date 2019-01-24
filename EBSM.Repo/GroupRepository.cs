using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class GroupRepository
    {
        private WmsDbContext db;
        public GroupRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Group item)
        {
            db.Groups.Add(item);
        }
        public void Edit(Group item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Group GetById(int id)
        {
            return db.Groups.Find(id);
        }
        public IEnumerable<Group> GetAll()
        {
            return db.Groups;
        }
        public IEnumerable<Group> GetAll(string GrpName)
        {
            return db.Groups.Where(x => (GrpName == null || x.GroupName.StartsWith(GrpName))).OrderBy(x => x.GroupName);
        }

        public bool isExistGroup(string groupName)
        {
        return db.Groups.Any(e => e.GroupName == groupName);
        }
        
}
}
