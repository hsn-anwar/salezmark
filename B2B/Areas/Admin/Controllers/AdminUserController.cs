using B2B.Areas.Admin.ViewModel;
using B2B.DAL;
using B2B.Models;
using B2B.Models.Packages;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Admin.Controllers
{
    public class AdminUserController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        DateTime currentTime = DateTime.Now;
        private string _UsersDirectoryBaseUrl = "/images/users";
        private ApplicationDbContext db = new ApplicationDbContext();

        public AdminUserController()
        {
        }

        public AdminUserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        public ActionResult Index(int? flag)
        {
            List<AdminUser> merchants = new List<AdminUser>();
            ViewBag.Users = db.Users.Where(ur => ur.UserType != UserTypes.Admin).Count();
            ViewBag.Active = db.Users.Where(ur => ur.UserType != UserTypes.Admin && ur.Status == UserStatus.Active).Count();
            ViewBag.Pending = db.Users.Where(ur => ur.UserType != UserTypes.Admin && ur.Status == UserStatus.Pending).Count();
            ViewBag.Block = db.Users.Where(ur => ur.UserType != UserTypes.Admin && ur.Status == UserStatus.Blocked).Count();
            ViewBag.New = db.Users.Where(ur => ur.UserType != UserTypes.Admin && ur.Status == UserStatus.Active && ur.CreatedOn.Month == currentTime.Month && ur.CreatedOn.Year == currentTime.Year).Count();
            ViewBag.Delivery = db.Users.Where(ur => ur.UserType == UserTypes.DeliveryTeam).Count();
            ViewBag.Sales = db.Users.Where(ur => ur.UserType == UserTypes.SalesTeam).Count();
            ViewBag.Merchants = db.Users.Where(ur => ur.UserType == UserTypes.Merchant).Count();
            ViewBag.Supermarket = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket).Count();
            ViewBag.Supervisor = db.Users.Where(ur => ur.UserType == UserTypes.Supervisor).Count();
            ViewBag.Expired = 0;

            //Users
            if (flag==null) {
                merchants = db.Users.Where(ur => ur.UserType != UserTypes.Admin).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            //Active
            else if (flag==1) {
                merchants = db.Users.Where(ur => ur.UserType != UserTypes.Admin && ur.Status == UserStatus.Active).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            //Pending
            else if (flag==2) {
                merchants = db.Users.Where(ur => ur.UserType != UserTypes.Admin && ur.Status == UserStatus.Pending).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            //Block
            else if (flag==3) {
                merchants = db.Users.Where(ur => ur.UserType != UserTypes.Admin && ur.Status == UserStatus.Blocked).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            //New
            else if (flag==4) {
                merchants = db.Users.Where(ur => ur.UserType != UserTypes.Admin && ur.Status == UserStatus.Active && ur.CreatedOn.Month == currentTime.Month && ur.CreatedOn.Year == currentTime.Year).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            //Delivery
            else if (flag==5) {
                merchants = db.Users.Where(ur => ur.UserType == UserTypes.DeliveryTeam).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            //Sales
            else if (flag==6) {
                merchants = db.Users.Where(ur => ur.UserType == UserTypes.SalesTeam).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            //Merchants
            else if (flag==7) {
                merchants = db.Users.Where(ur => ur.UserType == UserTypes.Merchant).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            //Supermarket
            else if (flag==8) {
                merchants = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            //Supervisor
            else if (flag==9) {
                merchants = db.Users.Where(ur => ur.UserType == UserTypes.Supervisor).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
        public ActionResult Merchants(int? flag)
        {
            List<AdminUser> merchants = new List<AdminUser>();
            ViewBag.Total = db.Users.Where(ur => ur.UserType == UserTypes.Merchant).Count();
            ViewBag.Active = db.Users.Where(ur => ur.UserType == UserTypes.Merchant&&ur.Status==UserStatus.Active).Count();
            ViewBag.Block = db.Users.Where(ur => ur.UserType == UserTypes.Merchant&&ur.Status==UserStatus.Blocked).Count();
            ViewBag.Pending = db.Users.Where(ur => ur.UserType == UserTypes.Merchant&&ur.Status==UserStatus.Pending).Count();
            ViewBag.New = db.Users.Where(ur => ur.UserType == UserTypes.Merchant&&ur.Status==UserStatus.Active&& ur.CreatedOn.Month==currentTime.Month&& ur.CreatedOn.Year == currentTime.Year).Count();
            ViewBag.Expired = 0;
            if (flag == null)
            {
                merchants = db.Users.Where(ur => ur.UserType == UserTypes.Merchant).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            else if(flag == 1) {
                merchants = db.Users.Where(ur => ur.UserType == UserTypes.Merchant&&ur.Status== UserStatus.Active).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            else if(flag == 2) {
                merchants = db.Users.Where(ur => ur.UserType == UserTypes.Merchant && ur.Status == UserStatus.Blocked).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            else if(flag == 3) {
                merchants = db.Users.Where(ur => ur.UserType == UserTypes.Merchant && ur.Status == UserStatus.Pending).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            else if(flag == 4) {
                merchants = db.Users.Where(ur => ur.UserType == UserTypes.Merchant && ur.Status == UserStatus.Active && ur.CreatedOn.Month == currentTime.Month && ur.CreatedOn.Year == currentTime.Year).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
        public ActionResult Supermarkets(int? flag)
        {
            List<AdminUser> supermarkets = new List<AdminUser>(); 
            ViewBag.Total = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket).Count();
            ViewBag.Active = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket && ur.Status == UserStatus.Active).Count();
            ViewBag.Block = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket && ur.Status == UserStatus.Blocked).Count();
            ViewBag.Pending = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket && ur.Status == UserStatus.Pending).Count();
            ViewBag.New = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket && ur.Status == UserStatus.Active && ur.CreatedOn.Month == currentTime.Month && ur.CreatedOn.Year == currentTime.Year).Count();
            ViewBag.Expired = 0;
            if (flag == null)
            {
                supermarkets = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
                supermarkets = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket && ur.Status == UserStatus.Active).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
                supermarkets = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket && ur.Status == UserStatus.Blocked).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
                supermarkets = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket && ur.Status == UserStatus.Pending).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            else if (flag == 4)
            {
                supermarkets = db.Users.Where(ur => ur.UserType == UserTypes.Supermarket && ur.Status == UserStatus.Active && ur.CreatedOn.Month == currentTime.Month && ur.CreatedOn.Year == currentTime.Year).OrderByDescending(ur => ur.CreatedOn).Select(u => new AdminUser
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
            return View(supermarkets);
        }
        // GET: Admin/AdminUser
        public ActionResult NewMerchant()
        {
            AdminUser user = new AdminUser();
            user.UserType = Models.UserTypes.Merchant;
            ViewBag.Feature = db.Package.Where(p => p.IsActive == true).Select(u => new FeatureViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Other=u.Price.ToString(),

            }).ToList();
            return View(user);
        }
        public ActionResult NewSupermarket()
        {
            AdminUser user = new AdminUser();
            user.UserType = Models.UserTypes.Supermarket;
            return View(user);
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
        public async System.Threading.Tasks.Task<ActionResult> NewMerchant(AdminUser model,int PackageId)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    model.ImageUrl = UploadImage(model.ImageFile, _UsersDirectoryBaseUrl);
                }

                var user = new ApplicationUser {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    ProfileName = model.Name,
                    Status=UserStatus.Active,
                    UserType=model.UserType,
                    LocationProxy=new Locations
                    {
                        State=model.State,
                        AddressLine=model.AddressLine,
                        City=model.City,
                        Country=model.Country,
                        CreatedOn= currentTime,
                        LiveModeEnable=model.EnableLiveLocation,
                        Latitude=model.Latitude,
                        Longitude=model.Longitude,
                    },
                    CreatedOn=currentTime,
                    ProfileImageUrl=model.ImageUrl,
                    Gender=Genders.Others,
                    UserID="MC-"+ RandomNumber(10000,99999),
                    DeviceToken=GenerateRefreshToken(),
                    DeviceNumber= RandomNumber(10000, 99999).ToString(),
                };
               
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var UserManager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(db));
                    UserManager.AddToRole(user.Id, "Merchant");
                    var pack = db.Package.Find(PackageId);
                    if (pack != null)
                    {
                        MerchantPackageMeta meta = new MerchantPackageMeta
                        {
                            Amount = pack.Price,
                            Status = packageStatus.Active,
                            StartingFrom = currentTime,
                            IsAmountPaid = false,
                            PackageId = pack.Id,
                            UserId = user.Id,
                        };
                        if(pack.DurationType== PackageDuration.Days)
                        {
                            meta.ValidTill= currentTime.AddDays(pack.Duration);
                        }else if (pack.DurationType == PackageDuration.Weeks)
                        {
                            int days = pack.Duration * 7;
                            meta.ValidTill = currentTime.AddDays(days);
                        }
                        else if (pack.DurationType == PackageDuration.Months)
                        {
                            meta.ValidTill = currentTime.AddMonths(pack.Duration);
                        }
                        else if (pack.DurationType == PackageDuration.Years)
                        {
                            meta.ValidTill = currentTime.AddYears(pack.Duration);
                        }
                        db.MerchantPackageMeta.Add(meta);
                        db.SaveChanges();
                    }
                    
                    return RedirectToAction("Merchants");
                }

                AddErrors(result);
            }

            ViewBag.Feature = db.Package.Select(u => new FeatureViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Other = u.Price.ToString(),

            }).ToList();
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> NewSupermarket(AdminUser model)
        {
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
                    UserType = model.UserType,
                    Status = UserStatus.Active,
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
                    UserID = "SM-" + RandomNumber(10000, 99999),
                    DeviceToken = GenerateRefreshToken(),
                    DeviceNumber = RandomNumber(10000, 99999).ToString(),
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    var UserManager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(db));
                    UserManager.AddToRole(user.Id, "Supermarket");
                    return RedirectToAction("Supermarkets");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult ChangeUserStatus(string userId) {
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
            if (user.Status == UserStatus.Blocked)
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

                user.Status = UserStatus.Blocked;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var sResponse = new
                {
                    Status = true,
                    Msg = "Blocked."
                };
                return Json(sResponse, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ApproveUser(string userId)
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

            user.Status = UserStatus.Active;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var sResponse = new
            {
                Status = true,
                Msg = "Blocked."
            };
            return Json(sResponse, JsonRequestBehavior.AllowGet);
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
    }
}