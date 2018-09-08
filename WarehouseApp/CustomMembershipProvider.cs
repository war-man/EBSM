using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WarehouseApp.Controllers;
using WarehouseApp.Models;


namespace WarehouseApp
{
    public class CustomMembershipProvider:MembershipProvider
    {
         public override MembershipUser CreateUser(string username, string password,
            string email, string passwordQuestion, string passwordAnswer,
            bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args =
                new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser user = GetUser(username, true);

            if (user == null)
            {
                User userObj = new User();
             
                //userObj.UserId = ;
                userObj.UserName = username;
                userObj.Password = password;
                //userObj.Password = GetMD5Hash(password);
                userObj.Email = email;
          

                UserRepo userRep = new UserRepo();
                userRep.RegisterUser(userObj);

                status = MembershipCreateStatus.Success;

                return GetUser(username, true);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            return null;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
            string newPasswordQuestion,
            string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            int userId = Convert.ToInt32(username.Split('|')[0]);
            UserRepo userRep = new UserRepo();
            var user = userRep.GetAllUsers().SingleOrDefault
                (u => u.UserId == userId);
            if (user != null)
            {
                MembershipUser memUser = new MembershipUser("CustomMembershipProvider",
                   user.UserName, user.UserId, user.Email,
                    string.Empty, string.Empty,
                    true, false, DateTime.MinValue,
                    DateTime.MinValue,
                    DateTime.MinValue,
                    DateTime.Now, DateTime.Now);
                return memUser;
            }
            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override bool ValidateUser(string username, string password)
        {
            string sha1Pswd = GetMD5Hash(password);
            UserRepo user = new UserRepo();
            var userObj = user.GetUserObjByUserName(username, sha1Pswd);
            if (userObj != null)
                return true;
            return false;
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            // In a real application, you will essentially have to return true
            // and implement the GetUserNameByEmail method to identify duplicates
            get { return false; }
        }

        public static string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

    }
   
    // check session timeout===============================================================
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckSessionTimeOutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Session != null)
            {
                if (context.Session.IsNewSession)
                {
                    string sessionCookie = context.Request.Headers["Cookie"];
                    if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET&#95;SessionId") >= 0))
                    {
                        FormsAuthentication.SignOut();
                        string redirectTo = "~/Home/Login";
                        if (!string.IsNullOrEmpty(context.Request.RawUrl))
                        {
                            redirectTo = string.Format("~/Home/Login?ReturnUrl={0}", HttpUtility.UrlEncode(context.Request.RawUrl));
                        }
                        filterContext.HttpContext.Response.Redirect(redirectTo, true);
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }

    // [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckConcurrentLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Session["sessionid"] == null)
                context.Session["sessionid"] = "empty";

            //LoginsController loginsCtlr = new LoginsController();

            //string userId = HttpContext.Current.User.Identity.GetUserId();
            if (LoginsController.IsYourLoginStillTrue(System.Web.HttpContext.Current.User.Identity.Name, context.Session["sessionid"].ToString()))
            {
                if (!LoginsController.IsUserLoggedOnElsewhere(System.Web.HttpContext.Current.User.Identity.Name, context.Session["sessionid"].ToString()))
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    // if it is being used elsewhere, update all their Logins records to LoggedIn = false, except for your session ID
                    LoginsController.LogEveryoneElseOut(System.Web.HttpContext.Current.User.Identity.Name, context.Session["sessionid"].ToString());
                    base.OnActionExecuting(filterContext);
                }
            }
            else
            {
                FormsAuthentication.SignOut();
                filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary{{ "controller", "User" },
                                                  { "action", "Login" }
 
                                                 });
            }

        }

    }
}