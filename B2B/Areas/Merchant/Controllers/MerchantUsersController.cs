using B2B.Areas.Admin.ViewModel;
using B2B.Areas.Merchant.ViewModel;
using B2B.DAL;
using B2B.Models;
using B2B.Models.Social;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Merchant.Controllers
{
    public class MerchantUsersController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        DateTime currentTime = DateTime.Now;
        private string _UsersDirectoryBaseUrl = "/images/users";
        private ApplicationDbContext db = new ApplicationDbContext();

        public MerchantUsersController()
        {
        }

        public MerchantUsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        internal string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
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
        // GET: Merchant/MerchantUsers
        public ActionResult SalesTeam(int? flag)
        {
            var loginUser = getLoginUser();
            ViewBag.Total = db.Users.Where(ur => ur.UserType == UserTypes.SalesTeam && ur.CreatedByUserId == loginUser.Id).Count();
            ViewBag.Active = db.Users.Where(ur => ur.UserType == UserTypes.SalesTeam && ur.CreatedByUserId == loginUser.Id && ur.Status== UserStatus.Active).Count();
            ViewBag.Block = db.Users.Where(ur => ur.UserType == UserTypes.SalesTeam && ur.CreatedByUserId == loginUser.Id && ur.Status== UserStatus.Blocked).Count();
            ViewBag.Deleted = db.Users.Where(ur => ur.UserType == UserTypes.SalesTeam && ur.CreatedByUserId == loginUser.Id && ur.Status== UserStatus.Deleted).Count();
            ViewBag.ReceivedSale = db.OrderLine.Where(mu => mu.OrderHeaderProxy.OrderForUserId == loginUser.Id && mu.OrderHeaderProxy.Type == orderType.Online).Sum(u => (decimal?)u.LineTotal) ?? 0;
            List<AdminUser> salesteam = new List<AdminUser>();
            if (flag == null) {
                salesteam = db.Users.Where(ur => ur.UserType == UserTypes.SalesTeam && ur.CreatedByUserId == loginUser.Id).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();
            }
            else if (flag == 1) {
                salesteam = db.Users.Where(ur => ur.UserType == UserTypes.SalesTeam && ur.CreatedByUserId == loginUser.Id && ur.Status== UserStatus.Active).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();

            }
            else if (flag == 2) {
                salesteam = db.Users.Where(ur => ur.UserType == UserTypes.SalesTeam && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Blocked).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();
            }
            else if (flag == 3) {
                salesteam = db.Users.Where(ur => ur.UserType == UserTypes.SalesTeam && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Deleted).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();
            }

            return View(salesteam);
        }
        public ActionResult AddSalesman()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> AddSalesman(AdminUser model)
        {
            var loginUser = getLoginUser();
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    model.ImageUrl = UploadImage(model.ImageFile, _UsersDirectoryBaseUrl);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    ProfileName = model.Name,
                    Status = UserStatus.Active,
                    UserType = UserTypes.SalesTeam,
                    CreatedByUserId=loginUser.Id,
                    LocationProxy = new Locations
                    {
                        State = model.State,
                        AddressLine = model.AddressLine,
                        City = model.City,
                        Country = model.Country,
                        CreatedOn = currentTime,
                        LiveModeEnable = model.EnableLiveLocation,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                    },
                    CreatedOn = currentTime,
                    ProfileImageUrl = model.ImageUrl,
                    Gender = Genders.Others,
                    UserID = "ST-" + RandomNumber(10000, 99999),
                    DeviceToken = GenerateRefreshToken(),
                    DeviceNumber = RandomNumber(10000, 99999).ToString(),
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    var UserManager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(db));
                    UserManager.AddToRole(user.Id, "SalesTeam");
                    return RedirectToAction("SalesTeam");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult DeliveryTeam(int? flag)
        {
            var loginUser = getLoginUser();
            ViewBag.Total = db.Users.Where(ur => ur.UserType == UserTypes.DeliveryTeam && ur.CreatedByUserId == loginUser.Id).Count();
            ViewBag.Active = db.Users.Where(ur => ur.UserType == UserTypes.DeliveryTeam && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Active).Count();
            ViewBag.Block = db.Users.Where(ur => ur.UserType == UserTypes.DeliveryTeam && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Blocked).Count();
            ViewBag.Deleted = db.Users.Where(ur => ur.UserType == UserTypes.DeliveryTeam && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Deleted).Count();
            ViewBag.OrderDelivered = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Type == orderType.Online &&mu.DeliveryOption== deliveryType.MerchantDelivery).Count();
            List<AdminUser> deliveryteam = new List<AdminUser>();
            if (flag == null)
            {
                deliveryteam = db.Users.Where(ur => ur.UserType == UserTypes.DeliveryTeam && ur.CreatedByUserId == loginUser.Id).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();

            }
            else if (flag == 1)
            {
                deliveryteam = db.Users.Where(ur => ur.UserType == UserTypes.DeliveryTeam && ur.CreatedByUserId == loginUser.Id&& ur.Status== UserStatus.Active).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();

            }
            else if (flag == 2)
            {
                deliveryteam = db.Users.Where(ur => ur.UserType == UserTypes.DeliveryTeam && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Blocked).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();
            }
            else if (flag == 3)
            {
                deliveryteam = db.Users.Where(ur => ur.UserType == UserTypes.DeliveryTeam && ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Deleted).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();
            }
            return View(deliveryteam);
        }
        public ActionResult AddDeliveryBoy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> AddDeliveryBoy(AdminUser model)
        {
            var loginUser = getLoginUser();

            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    model.ImageUrl = UploadImage(model.ImageFile, _UsersDirectoryBaseUrl);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    ProfileName = model.Name,
                    Status = UserStatus.Active,
                    UserType = UserTypes.DeliveryTeam,
                    CreatedByUserId=loginUser.Id,
                    LocationProxy = new Locations
                    {
                        State = model.State,
                        AddressLine = model.AddressLine,
                        City = model.City,
                        Country = model.Country,
                        CreatedOn = currentTime,
                        LiveModeEnable = model.EnableLiveLocation,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                    },
                    CreatedOn = currentTime,
                    ProfileImageUrl = model.ImageUrl,
                    Gender = Genders.Others,
                    UserID = "ST-" + RandomNumber(10000, 99999),
                    DeviceToken = GenerateRefreshToken(),
                    DeviceNumber = RandomNumber(10000, 99999).ToString(),
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    var UserManager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(db));
                    UserManager.AddToRole(user.Id, "DeliveryTeam");
                    return RedirectToAction("DeliveryTeam");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult Index(int? flag)
        {
            var loginUser = getLoginUser();
            ViewBag.Total = db.Users.Where(ur => ur.CreatedByUserId == loginUser.Id).Count();
            ViewBag.Active = db.Users.Where(ur =>ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Active).Count();
            ViewBag.Pending = db.Users.Where(ur =>ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Pending).Count();
            ViewBag.Block = db.Users.Where(ur =>ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Blocked).Count();
            ViewBag.New = db.Users.Where(ur => ur.CreatedByUserId == loginUser.Id && ur.CreatedOn.Month == currentTime.Month && ur.CreatedOn.Year == currentTime.Year).Count();
            ViewBag.Deleted = db.Users.Where(ur => ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Deleted).Count();
            ViewBag.SalesTeam = db.Users.Where(ur =>ur.UserType== UserTypes.SalesTeam&& ur.CreatedByUserId == loginUser.Id && ur.CreatedOn.Month==currentTime.Month&&ur.CreatedOn.Year==currentTime.Year).Count();
            ViewBag.DeliveryTeam = db.Users.Where(ur =>ur.UserType== UserTypes.DeliveryTeam&& ur.CreatedByUserId == loginUser.Id && ur.CreatedOn.Month==currentTime.Month&&ur.CreatedOn.Year==currentTime.Year).Count();
            List<AdminUser> merchants = new List<AdminUser>();
            if (flag == null)
            {
                merchants = db.Users.Where(ur => ur.CreatedByUserId == loginUser.Id).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();
            }
            else if (flag == 1) {
                merchants = db.Users.Where(ur => ur.CreatedByUserId == loginUser.Id&&ur.Status== UserStatus.Active).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();
            }
            else if (flag == 2) {
                merchants = db.Users.Where(ur => ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Pending).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();

            }
            else if (flag == 3) {
                merchants = db.Users.Where(ur => ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Blocked).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();

            }
            else if (flag == 4) {
                merchants = db.Users.Where(ur => ur.CreatedByUserId == loginUser.Id && ur.CreatedOn.Month==currentTime.Month&&ur.CreatedOn.Year==currentTime.Year).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();

            }
            else if (flag == 5) {
                merchants = db.Users.Where(ur => ur.CreatedByUserId == loginUser.Id && ur.Status == UserStatus.Deleted).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();
            }
            else if (flag == 6) {
                merchants = db.Users.Where(ur => ur.CreatedByUserId == loginUser.Id && ur.UserType == UserTypes.SalesTeam).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();
            }
            else if (flag == 7) {
                merchants = db.Users.Where(ur => ur.CreatedByUserId == loginUser.Id && ur.UserType == UserTypes.SalesTeam).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    Name = u.ProfileName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    UserType = u.UserType,
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : 0.00,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : 0.00,
                    EnableLiveLocation = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    OutputDate = u.CreatedOn + "",
                    UserId = u.UserID,
                    Status = u.Status
                }).ToList();
            }
            
            return View(merchants);
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
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public ActionResult OpenMarket(int? flag)
        {
            var loginUser = getLoginUser();
            ViewBag.Inque = db.MerchantConnection.Where(m => m.MerchantId == loginUser.Id && m.Status== connectionStatus.Pending).Count();
            ViewBag.Con = db.MerchantConnection.Where(m => m.MerchantId == loginUser.Id && m.Status== connectionStatus.Apporved).Count();
            ViewBag.Total = db.MerchantConnection.Where(m => m.MerchantId == loginUser.Id).Count();

            List<ShopkeeperVM> merchants = new List<ShopkeeperVM>();
            merchants = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket && ur.Status == UserStatus.Active).OrderByDescending(ur => ur.CreatedOn).Select(u => new ShopkeeperVM
            {
                Id = u.Id,
                ImageUrl = u.ProfileImageUrl,
                Name = u.ProfileName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                State = u.LocationProxy != null ? u.LocationProxy.State : "",
                City = u.LocationProxy != null ? u.LocationProxy.City : "",
                AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                OutputDate = u.CreatedOn + "",
                UserId = u.UserID,
                Status = db.MerchantConnection.Where(ur => ur.MerchantId == loginUser.Id&&ur.ShopkeeperId==u.Id).FirstOrDefault() == null ? "No" : db.MerchantConnection.Where(ur => ur.MerchantId == loginUser.Id).Select(ur => ur.Status.ToString()).FirstOrDefault()
            }).ToList();
            return View(merchants);
        }
        public ActionResult SendInvitation(string userId)
        {
            var loginUser = getLoginUser();
            var user = db.Users.Find(userId);
            if (user == null || user.UserType!= UserTypes.Supermarket)
            {
                var response = new
                {
                    Status = false,
                    Msg = "User not found"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            var con = db.MerchantConnection.Where(m => m.MerchantId == loginUser.Id && m.ShopkeeperId == userId).FirstOrDefault();
            if (con==null)
            {
                MerchantConnection merchantConnection = new MerchantConnection
                {
                    MerchantId=loginUser.Id,
                    ShopkeeperId=userId,
                    CreatedOn=currentTime,
                    Status= connectionStatus.Pending,
                };
                db.MerchantConnection.Add(merchantConnection);
                db.SaveChanges();
                var sResponse = new
                {
                    Status = true,
                    Msg = "Send invitation."
                };
                return Json(sResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {

                var sResponse = new
                {
                    Status = false,
                    Msg = "Invitation already inque."
                };
                return Json(sResponse, JsonRequestBehavior.AllowGet);
            }
        }
    }
}