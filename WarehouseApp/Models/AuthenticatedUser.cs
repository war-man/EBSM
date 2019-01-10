using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;


namespace WarehouseApp.Models
{
    [Authorize]
    public static class AuthenticatedUser
    {
        public static AuthenticatedUserModel GetUserFromIdentity()
        {
         
            //string[] strCUserDomain;
            //string strCUser, strCDomain;
            //var authenticatedUserData = System.Web.HttpContext.Current.User.Identity.Name;
           // strCUserDomain = System.Web.HttpContext.Current.User.Identity.Name.Split("\\".ToCharArray(), 2);
           // strCUser = strCUserDomain[1].ToString().ToUpper();
            //strCUser = new UserModel().GetUsernameFromFile("user_development"); 

            //var authenticatedUser = new Kaizoe2.Services.UserService().GetUserByUsername(strCUser);

            //string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            //HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            //FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
            //var authenticatedUserData = ticket.Name;
            var authenticatedUserData = System.Web.HttpContext.Current.User.Identity.Name;
            var authenticatedUserModel = new AuthenticatedUserModel
            {
                UserId = Convert.ToInt32(authenticatedUserData.Split('|')[0]),
                Username = authenticatedUserData.Split('|')[1],
                FullName = authenticatedUserData.Split('|')[2],
                ImageLink = (string.IsNullOrEmpty(authenticatedUserData.Split('|')[3]))
                    ? "~/Content/images/user.png"
                    : authenticatedUserData.Split('|')[3],
            };

            //var authenticatedUserModel = new AuthenticatedUserModel
            //{
            //    UserId = authenticatedUser.Id,
            //    Username = authenticatedUser.UserName,
            //    FullName = authenticatedUser.FullName,
            //    ImageLink = (string.IsNullOrEmpty(authenticatedUser.ImageFile)
            //        ? "~/Content/images/users/no-image.jpg"
            //        : authenticatedUser.ImageFile)
            //};
            return authenticatedUserModel;
        }
        public static AuthenticatedUserModel GetUserFromIdentity(IPrincipal user)
        {
            var authenticatedUserData = user == null ? System.Web.HttpContext.Current.User.Identity.Name : user.Identity.Name;
            var authenticatedUserModel = new AuthenticatedUserModel
            {
                UserId = Convert.ToInt32(authenticatedUserData.Split('|')[0]),
                Username = authenticatedUserData.Split('|')[1],
                FullName = authenticatedUserData.Split('|')[2],
                ImageLink = (string.IsNullOrEmpty(authenticatedUserData.Split('|')[3]))
                    ? "~/Content/images/users/no-image.jpg"
                    : authenticatedUserData.Split('|')[3],
            };
            return authenticatedUserModel;
        }
	}
    public class AuthenticatedUserModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string ImageLink { get; set; }
    }
}