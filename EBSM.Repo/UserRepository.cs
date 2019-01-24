using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
        public void Edit(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }
        public User GetById(int id)
        {
            return db.Users.Find(id); ;
        }
        public IEnumerable<User> GetAll()
        {
            return db.Users.Where(x => x.Status != 0).Include(x => x.Role).OrderBy(x => x.FullName);
        }
        public IEnumerable<User> GetAll(string SearchString)
        {
            return db.Users.Where(x => x.Status != 0 && (SearchString == null || x.FullName.Contains(SearchString))).Include(x=>x.Role).OrderBy(x => x.FullName);
        }
        public User GetUserByUsername(string username)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName.ToUpper() == username.ToUpper() && u.Status != 0);
            return user;

        }
        public bool CheckUsernameIsValid(string username)
        {
            return db.Users.Any(u => u.UserName.ToUpper() == username.ToUpper() && u.Status != 0);
        }
        public User GetValidUserByPassword(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName.ToUpper() == username.ToUpper() && u.Status != 0 && u.Password.Equals(password));
            return user;

        }
        
        public bool IsEmailExist(string Email, string InitialEmail)
        {
            bool isNotExist = true;
            if (Email != string.Empty && InitialEmail == "undefined")
            {
                var isExist = db.Users.Any(x => x.Status != 0 && x.Email.ToLower().Equals(Email.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (Email != string.Empty && InitialEmail != "undefined")
            {
                var isExist = db.Users.Any(x => x.Status != 0 && x.Email.ToLower() == Email.ToLower() && x.Email.ToLower() != InitialEmail.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return isNotExist;
        }

        public User GetUserById(int id)
        {
            return db.Users.Find(id);
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

        ///logins record 
        public void SaveUserLoginRecord(Logins login)
        {
            db.Logins.Add(login);
        }

        public bool IsYourLoginStillTrue(string userId, string sid)
        {
         

                //IEnumerable<Logins> logins = (from i in context.Logins
                //                              where i.LoggedIn == true &&
                //                              i.UserId == userId && i.SessionId == sid
                //                              select i).AsEnumerable();
                var logins = db.Logins.Where(i => i.LoggedIn == true && i.UserId == userId && i.SessionId == sid);
           
            return logins.Any();
            
        }

        public  bool IsUserLoggedOnElsewhere(string userId, string sid)
        {
            

                //IEnumerable<Logins> logins = (from i in context.Logins
                //                              where i.LoggedIn == true &&
                //                              i.UserId == userId && i.SessionId != sid
                //                              select i).AsEnumerable();
                var logins = db.Logins.Where(i => i.LoggedIn == true && i.UserId == userId && i.SessionId != sid);
            return logins.Any();
        }

        public  void LogEveryoneElseOut(string userId, string sid)
        {
         
                //IEnumerable<Logins> logins = (from i in context.Logins
                //                              where i.LoggedIn == true &&
                //                              i.UserId == userId &&
                //                              i.SessionId != sid // need to filter by user ID
                //                              select i).AsEnumerable();
                var logins = db.Logins.Where(i => i.LoggedIn == true && i.UserId == userId && i.SessionId != sid);

            foreach (Logins item in logins)
            {
                item.LoggedIn = false;
            }

            db.SaveChanges();
        }
    }
}
