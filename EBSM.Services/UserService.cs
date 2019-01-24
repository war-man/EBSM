using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class UserService
    {
        private WmsDbContext _context;
        private UserUnitOfWork _userUnitOfWork;

        public UserService()
        {
            _context = new WmsDbContext();
            _userUnitOfWork = new UserUnitOfWork(_context);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userUnitOfWork.UserRepository.GetAll();
        }public IEnumerable<User> GetAllUsers(string SearchString)
        {
            return _userUnitOfWork.UserRepository.GetAll(SearchString);
        }
        public User GetUserById(int id)
        {
            return _userUnitOfWork.UserRepository.GetById(id);
        }
        public int SaveUser(User user)
        {
            _userUnitOfWork.UserRepository.Add(user);
            _userUnitOfWork.Save();
            return user.UserId;
        }
        public int SaveUser(User user, int? loggedInUserId)
        {
            _userUnitOfWork.UserRepository.Add(user);
            _userUnitOfWork.Save(loggedInUserId.ToString());
            return user.UserId;
        }
        public void EditUser(User user)
        {
            _userUnitOfWork.UserRepository.Edit(user);
            _userUnitOfWork.Save();
        }
        public void EditUser(User user, int? loggedInUserId)
        {
            _userUnitOfWork.UserRepository.Edit(user);
            _userUnitOfWork.Save(loggedInUserId.ToString());
        }
       
        //public void DeleteCostCenter(int id)
        //{
        //    _costCenterUnitOfWork.CostCenterRepository.DeleteById(id);
        //    _costCenterUnitOfWork.Save();
        //}
       
        //public IEnumerable<User> GetAllUser()
        //{
        //    return _userUnitOfWork.UserRepository.GetAll();
        //}

      
        public User GetUserByUsername(string username)
        {
            return _userUnitOfWork.UserRepository.GetUserByUsername(username);
        }
        public bool CheckUsernameIsValid(string username)
        {
            return _userUnitOfWork.UserRepository.CheckUsernameIsValid(username);
        }
        public User GetValidUserByPassword(string username, string password)
        {
            return _userUnitOfWork.UserRepository.GetValidUserByPassword(username, password);
        }
        public bool IsUserNameExist(string UserName, string InitialUserName)
        {
            return _userUnitOfWork.UserRepository.IsUserNameExist(UserName, InitialUserName);
        }
        public bool IsEmailExist(string Email, string InitialEmail)
        {
            return _userUnitOfWork.UserRepository.IsEmailExist(Email, InitialEmail);
        }
      
            //logins record
            public void SaveUserLoginRecord(string UserId, string SessionId, bool LoggedIn)
        {
            Logins login = new Logins
            {
                UserId = UserId.ToLower(),
                SessionId = SessionId,
                LoggedIn = LoggedIn,
                LoggedInDateTime = DateTime.Now
            };
            _userUnitOfWork.UserRepository.SaveUserLoginRecord(login);
            _userUnitOfWork.Save();
        }
        public bool IsYourLoginStillTrue(string userId, string sid)
        {
            return _userUnitOfWork.UserRepository.IsYourLoginStillTrue(userId, sid);
        }
        public bool IsUserLoggedOnElsewhere(string userId, string sid)
        {
            return _userUnitOfWork.UserRepository.IsUserLoggedOnElsewhere(userId, sid); }
        public void LogEveryoneElseOut(string userId, string sid)
        {
            _userUnitOfWork.UserRepository.LogEveryoneElseOut(userId, sid); }

        public void Dispose()
        {
            _userUnitOfWork.Dispose();
        }
    }
}
