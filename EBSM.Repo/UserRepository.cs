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
        private WmsDbContext db;
        public UserRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(User user)
        {
            db.Users.Add(user);
        }
        public User GetById(int id)
        {
            return db.Users.Find(id); ;
        }
        public IEnumerable<User> GetAll(string SearchString)
        {
            return db.Users.Where(x => x.Status != 0 && (SearchString == null || x.FullName.Contains(SearchString))).OrderBy(x => x.FullName);
        }



        public bool IsUserNameExist(string UserName, string InitialUserName)
        {
            bool isNotExist = true;
            if (UserName != string.Empty && InitialUserName == "undefined")
            {
                var isExist = db.Users.Any(x => x.Status != 0 && x.UserName.ToLower().Equals(UserName.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (UserName != string.Empty && InitialUserName != "undefined")
            {
                var isExist = db.Users.Any(x => x.Status != 0 && x.UserName.ToLower() == UserName.ToLower() && x.UserName.ToLower() != InitialUserName.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return isNotExist;
        }
    }
}
