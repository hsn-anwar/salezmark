using B2B.Areas.Admin.ViewModel;
using B2B.Areas.Api.Dtos;
using B2B.Areas.Supermarket.ViewModel;
using B2B.DAL;
using B2B.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Supermarket.Controllers
{
    public class SupervisorController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        DateTime currentTime = DateTime.Now;
        private string _UsersDirectoryBaseUrl = "/images/users";
        private ApplicationDbContext db = new ApplicationDbContext();

        public SupervisorController()
        {
        }

        public SupervisorController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationUser getLoginUser()
        {

            //Get Login UserId
            string LoginAccountantId = User.Identity.GetUserId();

            //Get User Instant
            if (LoginAccountantId != null)
            {
                ApplicationUser LoginUser = db.Users.Find(LoginAccountantId);
                return LoginUser;
            }

            return null;
        }

        // GET: Supermarket/Supervisor
        public ActionResult Index(int? flag)
        {
            var loginUser = getLoginUser();
            ViewBag.Total = db.Users.Where(ur => ur.UserType == UserTypes.Supervisor && ur.CreatedByUserId == loginUser.Id).Count();
            ViewBag.Active = db.Users.Where(ur => ur.UserType == UserTypes.Supervisor && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Active).Count();
            ViewBag.Block = db.Users.Where(ur => ur.UserType == UserTypes.Supervisor && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Blocked).Count();
            ViewBag.Deleted = db.Users.Where(ur => ur.UserType == UserTypes.Supervisor && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Deleted).Count();
            List<RegisterUserDtos> salesteam = new List<RegisterUserDtos>();
            if (flag == null)
            {
                salesteam = db.Users.Where(ur => ur.UserType == UserTypes.Supervisor && ur.CreatedByUserId == loginUser.Id).OrderByDescending(ur => ur.CreatedOn).Select(u => new RegisterUserDtos
                {
                    Id = u.Id,
                    Email = u.Email,
                    Country = u.LocationProxy.Country,
                    City = u.LocationProxy.City,
                    State = u.LocationProxy.State,
                    Address = u.LocationProxy.AddressLine,
                    Gender = u.Gender,
                    EnableLiveMode = u.LocationProxy.LiveModeEnable,
                    Latitude = u.LocationProxy.Latitude,
                    Longitude = u.LocationProxy.Longitude,
                    PhoneNumber = u.PhoneNumber,
                    ProfileImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    BranchId = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? db.Branch.Where(ur => ur.AssignedToUserId == u.Id).Select(ur => ur.Id).FirstOrDefault() : 0,
                    IsBranchAssigned = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? true : false,
                    BranchName = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? db.Branch.Where(ur => ur.AssignedToUserId == u.Id).Select(ur => ur.Name).FirstOrDefault() : "Not assigned yet",
                    Status=u.Status
                }).ToList();
            }
            else if (flag == 1)
            {
                salesteam = db.Users.Where(ur => ur.UserType == UserTypes.Supervisor && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Active).OrderByDescending(ur => ur.CreatedOn).Select(u => new RegisterUserDtos
                {
                    Id = u.Id,
                    Email = u.Email,
                    Country = u.LocationProxy.Country,
                    City = u.LocationProxy.City,
                    State = u.LocationProxy.State,
                    Address = u.LocationProxy.AddressLine,
                    Gender = u.Gender,
                    EnableLiveMode = u.LocationProxy.LiveModeEnable,
                    Latitude = u.LocationProxy.Latitude,
                    Longitude = u.LocationProxy.Longitude,
                    PhoneNumber = u.PhoneNumber,
                    ProfileImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    BranchId = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? db.Branch.Where(ur => ur.AssignedToUserId == u.Id).Select(ur => ur.Id).FirstOrDefault() : 0,
                    IsBranchAssigned = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? true : false,
                    BranchName = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? db.Branch.Where(ur => ur.AssignedToUserId == u.Id).Select(ur => ur.Name).FirstOrDefault() : "Not assigned yet",
                    Status = u.Status
                }).ToList();

            }
            else if (flag == 2)
            {
                salesteam = db.Users.Where(ur => ur.UserType == UserTypes.Supervisor && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Blocked).OrderByDescending(ur => ur.CreatedOn).Select(u => new RegisterUserDtos
                {
                    Id = u.Id,
                    Email = u.Email,
                    Country = u.LocationProxy.Country,
                    City = u.LocationProxy.City,
                    State = u.LocationProxy.State,
                    Address = u.LocationProxy.AddressLine,
                    Gender = u.Gender,
                    EnableLiveMode = u.LocationProxy.LiveModeEnable,
                    Latitude = u.LocationProxy.Latitude,
                    Longitude = u.LocationProxy.Longitude,
                    PhoneNumber = u.PhoneNumber,
                    ProfileImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    BranchId = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? db.Branch.Where(ur => ur.AssignedToUserId == u.Id).Select(ur => ur.Id).FirstOrDefault() : 0,
                    IsBranchAssigned = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? true : false,
                    BranchName = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? db.Branch.Where(ur => ur.AssignedToUserId == u.Id).Select(ur => ur.Name).FirstOrDefault() : "Not assigned yet",
                    Status = u.Status
                }).ToList();
            }
            else if (flag == 3)
            {
                salesteam = db.Users.Where(ur => ur.UserType == UserTypes.Supervisor && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Deleted).OrderByDescending(ur => ur.CreatedOn).Select(u => new RegisterUserDtos
                {
                    Id = u.Id,
                    Email = u.Email,
                    Country = u.LocationProxy.Country,
                    City = u.LocationProxy.City,
                    State = u.LocationProxy.State,
                    Address = u.LocationProxy.AddressLine,
                    Gender = u.Gender,
                    EnableLiveMode = u.LocationProxy.LiveModeEnable,
                    Latitude = u.LocationProxy.Latitude,
                    Longitude = u.LocationProxy.Longitude,
                    PhoneNumber = u.PhoneNumber,
                    ProfileImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    BranchId = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? db.Branch.Where(ur => ur.AssignedToUserId == u.Id).Select(ur => ur.Id).FirstOrDefault() : 0,
                    IsBranchAssigned = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? true : false,
                    BranchName = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? db.Branch.Where(ur => ur.AssignedToUserId == u.Id).Select(ur => ur.Name).FirstOrDefault() : "Not assigned yet",
                    Status = u.Status
                }).ToList();
            }

            return View(salesteam);
        }
        public ActionResult Add()
        {
            return View();
        }
        internal string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Add(RegisterUserDtos SignUpUser, HttpPostedFileBase ImageFile)
        {
            var loginUser = getLoginUser();
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    SignUpUser.ProfileImageUrl = UploadImage(ImageFile, _UsersDirectoryBaseUrl);
                }

                var user = new ApplicationUser
                {
                    Email = SignUpUser.Email,
                    UserName = SignUpUser.Email,
                    PhoneNumber = SignUpUser.PhoneNumber,
                    UserType = UserTypes.Supervisor,
                    CreatedOn = currentTime,
                    LocationProxy = new Locations
                    {
                        City = SignUpUser.City,
                        Country = SignUpUser.Country,
                        State = SignUpUser.State,
                        AddressLine = SignUpUser.Address,
                        CreatedOn = currentTime,
                        Latitude = SignUpUser.Latitude,
                        Longitude = SignUpUser.Longitude,
                        LiveModeEnable = SignUpUser.EnableLiveMode,
                    },
                    Gender = SignUpUser.Gender,
                    Status = UserStatus.Active,
                    ProfileName = SignUpUser.Name,
                    OrderAuthenticationEnabled = false,
                    NotificationEnabled = false,
                    CreatedByUserId = loginUser.Id,
                    AccountSuspend = false,
                    IsEmailVerified = false,
                    PhoneNumberConfirmed = false,
                    DeviceNumber = RandomNumber(10000, 99999).ToString(),
                    DeviceToken = GenerateRefreshToken(),
                    ProfileImageUrl = SignUpUser.ProfileImageUrl,
                    UserID = "SUP-" + RandomNumber(10000, 99999),
                    IsDeleted=false,
                };
                var result = await UserManager.CreateAsync(user, "123456");
                if (result.Succeeded)
                {

                    var UserManager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(db));
                    UserManager.AddToRole(user.Id, "Supervisor");
                    return RedirectToAction("Index");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(SignUpUser);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public ActionResult ChangeUserStatus(string userId)
        {
            var user = db.Users.Find(userId);
            if (user == null)
            {
                var response = new
                {
                    Status = false,
                    Msg = "User not found"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (user.Status == UserStatus.Deleted)
            {

                user.Status = UserStatus.Active;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var sResponse = new
                {
                    Status = true,
                    Msg = "Activated."
                };
                return Json(sResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {

                user.Status = UserStatus.Deleted;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var sResponse = new
                {
                    Status = true,
                    Msg = "Delted."
                };
                return Json(sResponse, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EditProfile()
        {
            var loginUser = getLoginUser();

            ViewBag.withError = false;
            ViewBag.errorResponse = "";
            EditProfileVM model = new EditProfileVM();
            model.EnableOrderAuth = loginUser.OrderAuthenticationEnabled;
            model.NotificationEnabled = loginUser.NotificationEnabled;
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> EditProfile(EditProfileVM model)
        {
            var loginUser = getLoginUser();
            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.withError = true;
                ViewBag.errorResponse = "Password and Confirm Password do not match.";

                return View(model);
            }
            
            var user = await UserManager.FindByEmailAsync(loginUser.Email);

            var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            var result =await UserManager.ResetPasswordAsync(user.Id, token, model.Password); // use await here
            if (result.Succeeded)
            {
                loginUser.OrderAuthenticationEnabled = model.EnableOrderAuth;
                loginUser.NotificationEnabled = model.NotificationEnabled;
                db.Entry(loginUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Add", "Orders", new { area = "Supermarket" });
            }
            else
            {
                ViewBag.withError = true;
                ViewBag.errorResponse = result.Errors.ToString();
                return View(model);
            }
        }
        
        protected string UploadImage(HttpPostedFileBase file, string url)
        {
            if (file == null)
            {
                return null;
            }
            string filename = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            var ServerUrl = HttpContext.Server.MapPath(url);
            string ImagePath = ServerUrl + filename;
            file.SaveAs(ImagePath);
            return url + filename;
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

    }
}