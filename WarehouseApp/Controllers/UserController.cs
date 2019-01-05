using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net.Mail;
using System.Web.UI.WebControls;
using Microsoft.Owin;
using Microsoft.Owin.BuilderProperties;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;
using EBSM.Services;
using EBSM.Entities;
using LoginViewModel = WarehouseApp.Models.ViewModels.LoginViewModel;
using RegisterViewModel = WarehouseApp.Models.ViewModels.RegisterViewModel;


namespace WarehouseApp.Controllers
{
    public class UserController : Controller
    {
        private UserService _userService = new UserService();
        public UserManager<ApplicationUser> UserManager { get; private set; }
        #region user list view
        // GET: /User/
        [Roles("Global_SupAdmin,User_Creation")]
        public ActionResult Index()
        {
            var users = db.Users.Where(x=>x.Status!=2).Include(u => u.Role);
            
            return View(users.ToList());
        }
        #endregion
        #region user profile view
        public ActionResult UserProfile(int id)
        {
            
            User userProfile = _userService.GetUserById(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }
        #endregion

        #region user profile edit
        // GET: /User/Edit/5
        public ActionResult UserProfileEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles.Where(x => x.Status == 1), "RoleId", "RoleName", user.RoleId);
            
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfileEdit(User user)
        {
            if (!String.IsNullOrEmpty(user.FullName) && user.RoleId > 0)
            {
                User u = db.Users.Find(user.UserId);
                u.FullName = user.FullName;
                u.Address = user.Address;
                u.Email = user.Email;
                u.ContactNo = user.ContactNo;
                u.Gender = user.Gender;
                u.NationalId = user.NationalId;
              

                    u.RoleId = user.RoleId;
           
                u.UpdatedBy = Membership.GetUser(User.Identity.Name, true).UserName;
                u.UpdatedDate = DateTime.Now;
                db.Entry(u).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserProfile", "User", new {id=u.UserId });
            }

            ViewBag.RoleId =new SelectList(db.Roles.Where(r => r.Status == 1 ), "RoleId", "RoleName", user.RoleId);
            return View(user);
        }
        #endregion

        #region login / logout
        [HttpGet]
        [AllowAnonymous]
       
        public ActionResult Login(string returnUrl)
        {

            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                
                // find user by username first
                //var user = await UserManager.FindByNameAsync(model.UserName);
                var user = db.Users.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower() && u.Status != 0);

                if (user != null)
                {
                    MD5 md5Hash = MD5.Create();
                    string hashPassword = GetMd5Hash(md5Hash, model.Password);
                    var validCredentials = db.Users.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower() && u.Password.Equals(hashPassword) && u.Status != 0);
                    //var validCredentials = await UserManager.FindAsync(model.UserName, model.Password);
                   if (validCredentials == null)
                    {
                        ModelState.AddModelError("", "Invalid credentials. Please try again.");
                    }
                    else
                    {

                        FormsAuthentication.SetAuthCookie(user.UserId + "|" + user.UserName.ToLower(), model.RememberMe);
                        Session["sessionid"] = System.Web.HttpContext.Current.Session.SessionID;
                        Logins login = new Logins();
                        login.UserId = model.UserName.ToLower();
                        login.SessionId = System.Web.HttpContext.Current.Session.SessionID; ;
                        login.LoggedIn = true;
                        login.LoggedInDateTime = DateTime.Now;
                        db.Logins.Add(login);
                        db.SaveChanges();


                       if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        //await SignInAsync(user, model.RememberMe);

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
                ModelState.Remove("Password");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
        #endregion
        #region User register
        [Roles("Global_SupAdmin,User_Creation")]
        public ActionResult Register()
        {
            ViewBag.Message = "";
            TempData["RegistrationSuccess"] = "";
            ViewBag.RoleId = new SelectList(db.Roles.Where(r => r.Status ==1), "RoleId", "RoleName");
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel userRegister)
        {
            User user = new User();
            MD5 md5Hash = MD5.Create();
            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                user.FullName = userRegister.FullName;
                user.Email = userRegister.Email;
                user.ContactNo = userRegister.ContactNo;
                user.Address = userRegister.Address;
                user.Gender = userRegister.Gender;
                user.NationalId = userRegister.NationalId;
                user.RoleId = userRegister.RoleId;
              
                user.UserName = userRegister.UserName.ToLower();
                //string password = Membership.GeneratePassword(10, 1);
                string hashPassword = GetMd5Hash(md5Hash, userRegister.Password);
                user.Password = hashPassword;
                user.Status = 1;
               user.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                user.CreatedDate = DateTime.Now;
                user.LastPassChangeDate = DateTime.Now.Date;
                user.PasswordChangedCount = 0;
                db.Users.Add(user);
                TempData["RegistrationSuccess"] = "New user registration successfully complete! Username and Password sent to user by Email.";
                db.SaveChanges();
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.Message = "Something went wrong! please try again";
            }
            ViewBag.RoleId = new SelectList(db.Roles.Where(r => r.RoleId != 1 && r.Status ==1), "RoleId", "RoleName", userRegister.RoleId);
            return View(userRegister);
        }
        #endregion
        #region password change
        // GET: /User/ChangePassword
        public ActionResult ChangePassword(string userName)
        {
            //ViewBag.PasswordAged = "";
            ChangePsswordViewModel model = new ChangePsswordViewModel();
            if (userName != null)
            {
                model.UserName = userName;
            }
            else
            {
                MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);

                model.UserName = currentUser.UserName;
            }

            //ViewBag.PasswordAged = TempData["PasswordAgedMessage"];
            return View(model);
        }




        [HttpPost]
        public ActionResult ChangePassword(ChangePsswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //ViewBag.PasswordHistryAlert = "";
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;

                if (model.NewPassword == model.OldPassword)
                {
                    changePasswordSucceeded = false;
                }
                else
                {
                    MD5 md5Hash = MD5.Create();
                    string hashOldPassword = GetMd5Hash(md5Hash, model.OldPassword);
                    string hashNewPassword = GetMd5Hash(md5Hash, model.NewPassword);

                    //changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                    var user = db.Users.FirstOrDefault(u => u.UserName.Equals(model.UserName) && u.Password.Equals(hashOldPassword) && u.Status != 0);

                    if (user == null)
                    {
                        return HttpNotFound();
                    }


                    if (hashNewPassword != user.Password)
                    {

                        user.Password = hashNewPassword;
                        user.LastPassChangeDate = DateTime.Now.Date;
                        user.PasswordChangedCount += 1;

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges(user.UserId.ToString());
                        changePasswordSucceeded = true;

                    }
                    else
                    {
                        //ViewBag.PasswordHistryAlert = "You can not use previously used password!";
                        changePasswordSucceeded = false;
                    }
                }


                if (changePasswordSucceeded)
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        return ChangePasswordSuccess();
                    }

                }
                else
                {

                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");


                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        private ActionResult ChangePasswordSuccess()
        {
            return View("../User/ChangePasswordSuccess");
        }
        #endregion
        #region reset password
        [Roles("Global_SupAdmin,User_Creation")]

        public ActionResult ResetPassword(int id)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.UserId = id;
            return View(model);
        }


        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool resetPasswordSucceeded;
                try
                {
                    MD5 md5Hash = MD5.Create();
                    string hashNewPassword = GetMd5Hash(md5Hash, model.NewPassword);
                    var user = db.Users.FirstOrDefault(u => u.UserId == model.UserId);

                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    user.Password = hashNewPassword;
                    user.LastPassChangeDate = DateTime.Now;
                    user.PasswordChangedCount += 1;
                    user.UpdatedBy = Membership.GetUser(User.Identity.Name, true).UserName;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                    resetPasswordSucceeded = true;
                }
                catch (Exception)
                {
                    resetPasswordSucceeded = false;
                }

                if (!resetPasswordSucceeded)
                {
                    ModelState.AddModelError("", "New password is invalid.");
                    return View(model);

                }
                return ChangePasswordSuccess();
            }
            // If we got this far, something failed, redisplay form
            else
            {
                return View(model);
            }

        }
        #endregion
      
        #region delete user
        public ActionResult DeleteUser(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }
            user.Status = 0;
            db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
            return Json(new { Result = "OK" });
        }
        [Roles("Global_SupAdmin,User_Creation")]
        public ActionResult DeactiveActiveUser(int? id, byte? status)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }
            user.Status = status;
            db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
            return Json(new { Result = "OK" });
        }
#endregion
        //user defined roles =======================================
        #region user defined roles
        [Roles("Global_SupAdmin,Role_Creation")]
        public ActionResult Roles()
        {
           
            return View("Roles", db.Roles.Where(x=>x.Status!=2).ToList());
        }
        [Roles("Global_SupAdmin,Role_Creation")]
        public ActionResult CeateRole()
        {
            var model = new RoleViewModel();
            model.RoleTaskCheckBoxList = RoleTaskCheckBoxModel.TaskNames.OrderBy(x => x.TaskNameDisplay).ToList();

            return View("CreateRole",model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CeateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Role role = new Role
                {
                    RoleName = model.RoleName,
                    Status = 1,
                    CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                    CreatedDate = DateTime.Now,
                };
                role.RoleTasks=new List<RoleTask>();
                if (model.RoleTaskCheckBoxList.Any())
                {
                    List<RoleTaskCheckBoxModel> roleTaskCheckBoxList = model.RoleTaskCheckBoxList.Where(x => x.IsChecked).ToList();
                    foreach (var task in roleTaskCheckBoxList)
                    {
                        var roleTask = new RoleTask
                        {
                            Task = task.TaskName
                        };
                        role.RoleTasks.Add(roleTask);
                    }

                }
                db.Roles.Add(role);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Roles", "User");
            }
            
            return View("CreateRole", model);
        }
        [Roles("Global_SupAdmin,Role_Creation")]
        public ActionResult EditRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            RoleViewModel model=new RoleViewModel
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Status = role.Status,
            };
            model.RoleTaskCheckBoxList=new List<RoleTaskCheckBoxModel>();
            foreach (var item in RoleTaskCheckBoxModel.TaskNames.OrderBy(x=>x.TaskNameDisplay))
            {
                item.IsChecked = role.RoleTasks.Any(x => x.Task.Equals(item.TaskName));
                model.RoleTaskCheckBoxList.Add(item);
            }
            
            return View("EditRole", model);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = db.Roles.Find(model.RoleId);

                role.RoleName = model.RoleName;
                role.Status = model.Status;
                role.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                role.UpdatedDate = DateTime.Now;
                //db.Entry(role).State = EntityState.Modified;
                //db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                var roleTasks = role.RoleTasks.ToList();
                foreach (var removeTask in roleTasks)
                {
                    db.RoleTasks.Remove(removeTask);
                }
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                
                role.RoleTasks = new List<RoleTask>();
                if (model.RoleTaskCheckBoxList.Any())
                {
                    List<RoleTaskCheckBoxModel> roleTaskCheckBoxList = model.RoleTaskCheckBoxList.Where(x => x.IsChecked).ToList();
                    foreach (var task in roleTaskCheckBoxList)
                    {
                        var roleTask = new RoleTask
                        {
                            Task = task.TaskName
                        };
                        role.RoleTasks.Add(roleTask);
                    }

                }
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Roles", "User");
            }
            // ViewBag.CategoryParentId =00. new SelectList(CategoryTree().Where(x => x.Status > 0).OrderBy(x => x.CategoryName), "ItemCategoryId", "CategoryName", category.CategoryParentId);
            return View("EditRole", model);
        }
        #endregion

        // Module ============================================================================================================
        #region helper modules

        //public ActionResult GetRoleByDepartmentId(int? departmentId)
        //{
        //    var roles = db.DeptRoleConfigs.Where(x => x.DepartmentId == departmentId && x.RoleId > 1 && x.Status == 1).Select(x => x.Role).ToList();
        //    StringBuilder selectListString = new StringBuilder();
        //    selectListString.Append("<option value=' '>--Select--</option>");
        //    foreach (var item in roles)
        //    {
        //        selectListString.Append("<option value='" + item.RoleId + "'>" + item.RoleName + "</option>");
        //    }
        //    var data = new { selectListString = selectListString.ToString() };
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
       
       
        private async Task<bool> IsFirstLogin(int? userId)
        {
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user.PasswordChangedCount > 0 || user.PasswordChangedCount == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public bool CheckPasswordStrength(string userName, string fullName, string password)
        {
            List<string> fullNameParts = fullName.Split(' ').ToList();
            var count = 0;
            if (password.ToLower().Contains(userName.ToLower()))
            {
                count += 1;
            }

            foreach (var namePart in fullNameParts)
            {
                if (password.ToLower().Contains(namePart.ToLower()))
                {
                    count += 1;
                }
            }
            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //name or email exist checker

         [AllowAnonymous]
        public JsonResult CheckUserNameExist(string userName)
        {
            var isUserNameExist = db.Users.Any(u => u.UserName.ToLower() == userName.ToLower());
            if (!isUserNameExist)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

         [AllowAnonymous]
        public JsonResult CheckEmailExist(string email)
        {
            var isEmailExist = db.Users.Any(u => u.Email.ToLower() == email.ToLower() && u.Status != 0);
            if (!isEmailExist)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public bool EmailNotExist(string email)
        {
            var isEmailExist = db.Users.Any(u => u.Email.ToLower() == email.ToLower() && u.Status!=0);
            if (!isEmailExist)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: /Account/Manage
        public ActionResult Manage(UserController.ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == UserController.ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == UserController.ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == UserController.ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == UserController.ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }
        public IEnumerable<string> AllPartsOfLength(string value, int length)
        {
            for (int startPos = 0; startPos <= value.Length - length; startPos++)
            {
                yield return value.Substring(startPos, length);
            }
            yield break;
        }

        public bool IsSupUser(string userName)
        {
            if (userName == "lc_admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
      
        //private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //    var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        //    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        //}
           #endregion
        #region dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
         #endregion
    }
    #region login controller
    public class LoginsController : Controller
    {
        private static WmsDbContext db = new WmsDbContext();
        //
        // GET: /Logins/
        public static bool IsYourLoginStillTrue(string userId, string sid)
        {


            //IEnumerable<Logins> logins = (from i in context.Logins
            //                              where i.LoggedIn == true &&
            //                              i.UserId == userId && i.SessionId == sid
            //                              select i).AsEnumerable();
            var logins = db.Logins.Where(i => i.LoggedIn == true && i.UserId == userId && i.SessionId == sid);
            return logins.Any();
        }

        public static bool IsUserLoggedOnElsewhere(string userId, string sid)
        {


            //IEnumerable<Logins> logins = (from i in context.Logins
            //                              where i.LoggedIn == true &&
            //                              i.UserId == userId && i.SessionId != sid
            //                              select i).AsEnumerable();
            var logins = db.Logins.Where(i => i.LoggedIn == true && i.UserId == userId && i.SessionId != sid);
            return logins.Any();
        }

        public static void LogEveryoneElseOut(string userId, string sid)
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


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
       
    }
     #endregion
}
