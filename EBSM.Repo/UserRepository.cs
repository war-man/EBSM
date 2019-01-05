using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class UserRepository
    {
        private WmsDbContext _context;
        public UserRepository(WmsDbContext context)
        {
            _context = context;
        }
        public void Add(User user)
        {
            _context.Users.Add(user);
        }
        public User GetById(int id)
        {
            return _context.Users.Find(id); ;
        }
        public IEnumerable<User> GetAll(string SearchString)
        {
            return _context.Users.Where(x => x.Status != 0 && (SearchString == null || x.FullName.Contains(SearchString))).OrderBy(x => x.FullName);
        } 

    
       
        //public bool IsCostCenterExist(string CostCenterName, string InitialCostCenterName)
        //{
        //    bool isNotExist = true;
        //    if (CostCenterName != string.Empty && InitialCostCenterName == "undefined")
        //    {
        //        var isExist = _context.CostCenters.Any(x => x.Status != 0 && x.CostCenterName.ToLower().Equals(CostCenterName.ToLower()));
        //        if (isExist)
        //        {
        //            isNotExist = false;
        //        }
        //    }
        //    if (CostCenterName != string.Empty && InitialCostCenterName != "undefined")
        //    {
        //        var isExist = _context.CostCenters.Any(x => x.Status != 0 && x.CostCenterName.ToLower() == CostCenterName.ToLower() && x.CostCenterName.ToLower() != InitialCostCenterName.ToLower());
        //        if (isExist)
        //        {
        //            isNotExist = false;
        //        }
        //    }
        //    return isNotExist;
        //}
    }
}
