using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WarehouseApp.Models;
using EBSM.Entities;
using EBSM.Services;
namespace WarehouseApp
{
    public class UserRepo
    {
        private UserService _userService = new UserService();

        //public User()
        //{
        //    string connectionString =
        //      ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
        //    context = new DataContext(connectionString);
        //    usersTable = context.GetTable<UserObj>();
        //}

        public User GetUserObjByUserName(string userName, string passWord)
        {
            var user = _userService.GetValidUserByPassword(userName, passWord);
            return user;
        }

        public User GetUserObjByUserName(string userName)
        {
            var user = _userService.GetUserByUsername(userName);
            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userService.GetAllUsers().AsEnumerable();
        }

        public int RegisterUser(User userObj)
        {
            User user = new User();
            user.UserName = userObj.UserName;
            user.Password = userObj.Password;
            user.Email = userObj.Email;
            return _userService.SaveUser(user); ;
        }
    }
}