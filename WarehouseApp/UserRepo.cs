using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WarehouseApp.Models;

namespace WarehouseApp
{
    public class UserRepo
    {
        private WmsDbContext db = new WmsDbContext();

        //public User()
        //{
        //    string connectionString =
        //      ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
        //    context = new DataContext(connectionString);
        //    usersTable = context.GetTable<UserObj>();
        //}

        public User GetUserObjByUserName(string userName, string passWord)
        {
            var user = db.Users.SingleOrDefault(
              u => u.UserName == userName && u.Password == passWord);
            return user;
        }

        public User GetUserObjByUserName(string userName)
        {
            var user = db.Users.SingleOrDefault(u => u.UserName == userName);
            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return db.Users.AsEnumerable();
        }

        public int RegisterUser(User userObj)
        {
            User user = new User();
            user.UserName = userObj.UserName;
            user.Password = userObj.Password;
            user.Email = userObj.Email;

            db.Users.Add(user);
            db.SaveChanges();

            return user.UserId;
        }
    }
}