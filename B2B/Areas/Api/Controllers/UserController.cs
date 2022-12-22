using B2B.Areas.Api.Dtos;
using B2B.Middleware;
using B2B.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Api.Controllers
{
    [AllowAnonymous]
    public class UserController : CommonController
    {
        //Init ASP.NET identity store to handle user sign-in & sign-up 
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        /// <summary>
        /// //////////////////////////////////////////////////////////////////Login & profile apis
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SignIn(UserDtos LoginUser)
        {
            try
            {
                if (LoginUser.DeviceNumber == null)
                {
                    return JsonResponse(400, "Device number is required!", null);
                }
                if (LoginUser.Email == null)
                {
                    return JsonResponse(400, "Email or username is required!", null);
                }
                if (LoginUser.Password == null)
                {
                    return JsonResponse(400, "Password is required!", null);
                }
                //Check Validations
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                    
                    return JsonResponse(400, message, null);
                }
                //GET User detail from database
                var User = db.Users.Where(usr => usr.Email == LoginUser.Email || usr.PhoneNumber == LoginUser.Email || usr.UserName == LoginUser.Email).FirstOrDefault();

                //Login Process
                if (User != null)
                {
                    if (User.AccountSuspend)
                    {
                        return JsonResponse(400, "Your account has beed suspended!", null);
                    }
                    if (User.IsDeleted)
                    {
                        return JsonResponse(400, "Your account has beed trashed!", null);
                    }
                    if (User.ParentUserId != null)
                    {
                        var parentUser = db.Users.Find(User.CreatedByUserId);
                        if (parentUser.AccountSuspend)
                        {
                            return JsonResponse(400, "Your organization account has beed suspended!", null);
                        }
                        if (parentUser.IsDeleted)
                        {
                            return JsonResponse(400, "Your organization account has beed trashed!", null);
                        }
                    }
                    var result = await SignInManager.PasswordSignInAsync(User.UserName, LoginUser.Password, false, shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            //var newToken = GenerateRefreshToken();
                            
                            UserProfileDtos userProfile = new UserProfileDtos
                            {
                                Id = User.Id,
                                ProfileImageUrl = User.ProfileImageUrl,
                                Username = User.ProfileName,
                                PhoneNumber = User.PhoneNumber,
                                UserType = User.UserType,
                                Email = User.Email,
                                DeviceNumber=User.DeviceNumber,
                                DeviceToken=User.DeviceToken,
                                Address=User.LocationProxy.AddressLine,
                                DeviceFCM=User.DeviceFCM,
                                NotificationEnabled = User.NotificationEnabled,
                                AssignBranchId= db.Branch.Where(ur => ur.AssignedToUserId == User.Id).FirstOrDefault() != null ?(int?) db.Branch.Where(ur => ur.AssignedToUserId == User.Id).Select(ur => ur.Id).FirstOrDefault() : null,
                                IsEmailVerified = User.IsEmailVerified,
                                OrderAuthenticationEnabled = User.OrderAuthenticationEnabled
                            };
                            return JsonResponse(200, "You have signed in successfully!", userProfile);
                        case SignInStatus.LockedOut:
                            return JsonResponse(400, "Account lockedout!", null);
                        case SignInStatus.RequiresVerification:
                            return JsonResponse(400, "Verification Required!", null);
                        case SignInStatus.Failure:
                        default:
                            return JsonResponse(400, "Password not correct!", null);
                    }
                }
                else
                {
                    return JsonResponse(400, "Invalid user email or username!", null);
                }
            }
            catch (Exception ex)
            {
                return JsonResponse(501, ex.GetBaseException().Message, null);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ForgetPassword(string Email)
        {
            try
            {
                ApplicationUser user = db.Users.Where(u => u.Email == Email).FirstOrDefault();

                if (user == null)
                {
                    return JsonResponse(400,"Email not found!", null);
                }

                Random random = new Random();
                string Password = RandomPassword();
                var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var result = await UserManager.ResetPasswordAsync(user.Id, token, Password.ToString());

                if (result.Succeeded)
                {
                    StringBuilder mailBody = new StringBuilder();
                    mailBody.AppendFormat("Dear (" + user.UserName + "),");
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("<p>Your new password: " + Password + " </p>");
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("Regards.");
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("SalezMark");
                    mailBody.AppendFormat("<br />");
                    mailBody.AppendFormat("<p>Need help? Contact us at help@salezmark.com</p>");
                    var IsEmailSend = SendEmail(user.Email, "SalezMark new password.", mailBody.ToString());
                    if (IsEmailSend == null)
                    {
                        return JsonResponse(200, "Please Check your email for new password.", null);
                    }
                    else
                    {
                        return JsonResponse(500, IsEmailSend, null);
                    }
                }
                else
                {
                    return JsonResponse(501, "Not implemented! unhandle exception occurred while processing your request.", null);
                }
            }
            catch (Exception ex)
            {
                return JsonResponse(504, ex.GetBaseException().Message, null);
            }
        }

        [AuthorizeFilter]
        [HttpPost]
        public async Task<JsonResult> ChangePassword(string OldPassword, string NewPassword)
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                ////////////////////////////////////////////////////////////
                ///
                
                var result = await SignInManager.PasswordSignInAsync(loginUser.UserName, OldPassword, false, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        var token = await UserManager.GeneratePasswordResetTokenAsync(loginUser.Id);
                        var changeResult = await UserManager.ResetPasswordAsync(loginUser.Id, token, NewPassword);

                        if (changeResult.Succeeded)
                        {
                            return JsonResponse(200, "Password has been changed successfully!", null);
                        }
                        List<string> errorList = new List<string>();
                        foreach (var error in changeResult.Errors)
                        {
                            errorList.Add(error);
                        }
                        
                        return JsonResponse(400, errorList.ToString(), null);

                    case SignInStatus.LockedOut:
                        return JsonResponse(400, "Account lockedout.", null);
                    case SignInStatus.RequiresVerification:
                        return JsonResponse(400, "Verification Required.", null);
                    case SignInStatus.Failure:
                    default:
                        return JsonResponse(400, "Incorrect old password.", null);
                }
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }

        [AuthorizeFilter]
        [HttpGet]
        public JsonResult GetUserProfile()
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                ////////////////////////////////////////////////////////////
                UserProfileDtos userProfile = new UserProfileDtos
                {
                    Id = loginUser.Id,
                    ProfileImageUrl = loginUser.ProfileImageUrl,
                    Username = loginUser.ProfileName,
                    PhoneNumber = loginUser.PhoneNumber,
                    UserType = loginUser.UserType,
                    Email = loginUser.Email,
                    DeviceNumber = loginUser.DeviceNumber,
                    DeviceToken = loginUser.DeviceToken,
                    Address = loginUser.LocationProxy.AddressLine,
                    AssignBranchId = db.Branch.Where(ur => ur.AssignedToUserId == loginUser.Id).FirstOrDefault() != null ? (int?)db.Branch.Where(ur => ur.AssignedToUserId == loginUser.Id).Select(ur => ur.Id).FirstOrDefault() : null,
                    DeviceFCM = loginUser.DeviceFCM,
                    NotificationEnabled=loginUser.NotificationEnabled,
                    IsEmailVerified=loginUser.IsEmailVerified,
                    OrderAuthenticationEnabled = loginUser.OrderAuthenticationEnabled
                };
                return JsonResponse(200, "", userProfile);
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }
        [AuthorizeFilter]
        [HttpGet]
        public JsonResult GetUserInfo(string UserId)
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Find(UserId);
                if (loginUser == null)
                {
                    return JsonResponse(400, "User not found!", null);
                }
                ////////////////////////////////////////////////////////////
                var userProfile = new 
                {
                    Id = loginUser.Id,
                    ProfileImageUrl = loginUser.ProfileImageUrl,
                    Username = loginUser.UserName,
                    PhoneNumber = loginUser.PhoneNumber,
                    UserType = loginUser.UserType,
                    Email = loginUser.Email,
                    DeviceFCM = loginUser.DeviceFCM,
                    NotificationEnabled = loginUser.NotificationEnabled,
                  };
                return JsonResponse(200, "", userProfile);
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }
        [AuthorizeFilter]
        [HttpPost]
        public JsonResult ChangeNotificationSetting()
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                ////////////////////////////////////////////////////////////
                if (loginUser.NotificationEnabled)
                {
                    loginUser.NotificationEnabled = false;
                    db.Entry(loginUser).State = EntityState.Modified;
                    db.SaveChanges();
                    UserProfileDtos userProfile = new UserProfileDtos
                    {
                        Id = loginUser.Id,
                        ProfileImageUrl = loginUser.ProfileImageUrl,
                        Username = loginUser.UserName,
                        PhoneNumber = loginUser.PhoneNumber,
                        UserType = loginUser.UserType,
                        Email = loginUser.Email,
                        DeviceNumber = loginUser.DeviceNumber,
                        DeviceToken = loginUser.DeviceToken,
                        Address = loginUser.LocationProxy.AddressLine,
                        DeviceFCM = loginUser.DeviceFCM,
                        NotificationEnabled = loginUser.NotificationEnabled,
                        IsEmailVerified = loginUser.IsEmailVerified,
                        OrderAuthenticationEnabled = loginUser.OrderAuthenticationEnabled
                    };
                    return JsonResponse(200, "Notification disabled.", userProfile);
                }
                else
                {
                    loginUser.NotificationEnabled = true;

                    db.Entry(loginUser).State = EntityState.Modified;
                    db.SaveChanges();
                    UserProfileDtos userProfile = new UserProfileDtos
                    {
                        Id = loginUser.Id,
                        ProfileImageUrl = loginUser.ProfileImageUrl,
                        Username = loginUser.UserName,
                        PhoneNumber = loginUser.PhoneNumber,
                        UserType = loginUser.UserType,
                        Email = loginUser.Email,
                        DeviceNumber = loginUser.DeviceNumber,
                        DeviceToken = loginUser.DeviceToken,
                        Address = loginUser.LocationProxy.AddressLine,
                        DeviceFCM = loginUser.DeviceFCM,
                        NotificationEnabled = loginUser.NotificationEnabled,
                        IsEmailVerified = loginUser.IsEmailVerified,
                        OrderAuthenticationEnabled = loginUser.OrderAuthenticationEnabled
                    };
                    return JsonResponse(200, "Notification enabled.", userProfile);
                }
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }

        [AuthorizeFilter]
        [HttpPost]
        public JsonResult ChangeOrderAuthenticationSetting()
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                ////////////////////////////////////////////////////////////
                if (loginUser.OrderAuthenticationEnabled)
                {
                    loginUser.OrderAuthenticationEnabled = false;
                    db.Entry(loginUser).State = EntityState.Modified;
                    db.SaveChanges();
                    UserProfileDtos userProfile = new UserProfileDtos
                    {
                        Id = loginUser.Id,
                        ProfileImageUrl = loginUser.ProfileImageUrl,
                        Username = loginUser.UserName,
                        PhoneNumber = loginUser.PhoneNumber,
                        UserType = loginUser.UserType,
                        Email = loginUser.Email,
                        DeviceNumber = loginUser.DeviceNumber,
                        DeviceToken = loginUser.DeviceToken,
                        Address = loginUser.LocationProxy.AddressLine,
                        DeviceFCM = loginUser.DeviceFCM,
                        NotificationEnabled = loginUser.NotificationEnabled,
                        IsEmailVerified = loginUser.IsEmailVerified,
                        OrderAuthenticationEnabled = loginUser.OrderAuthenticationEnabled
                    };
                    return JsonResponse(200, "Order authentication disabled.", userProfile);
                }
                else
                {
                    loginUser.OrderAuthenticationEnabled = true;

                    db.Entry(loginUser).State = EntityState.Modified;
                    db.SaveChanges();
                    UserProfileDtos userProfile = new UserProfileDtos
                    {
                        Id = loginUser.Id,
                        ProfileImageUrl = loginUser.ProfileImageUrl,
                        Username = loginUser.UserName,
                        PhoneNumber = loginUser.PhoneNumber,
                        UserType = loginUser.UserType,
                        Email = loginUser.Email,
                        DeviceNumber = loginUser.DeviceNumber,
                        DeviceToken = loginUser.DeviceToken,
                        Address = loginUser.LocationProxy.AddressLine,
                        DeviceFCM = loginUser.DeviceFCM,
                        NotificationEnabled = loginUser.NotificationEnabled,
                        IsEmailVerified = loginUser.IsEmailVerified,
                        OrderAuthenticationEnabled = loginUser.OrderAuthenticationEnabled
                    };
                    return JsonResponse(200, "Order authentication enabled.", userProfile);
                }
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public string SendEmail(string receiver, string subject, string message)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("moin.aziz786@gmail.com", "SalezMark");
                    mail.To.Add(receiver);
                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.EnableSsl = true;
                        //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("moin.aziz786@gmail.com", "moin&moin&0222");
                        smtp.Send(mail);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return BaseMessage;
            }
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////Supervisor apis
        /// </summary>
        /// <returns></returns>
        /// 
        [AuthorizeFilter]
        public JsonResult GetSupervisors()
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                /////////////////////////////
                ///
                if (loginUser.UserType != Models.UserTypes.Supermarket)
                {
                    return JsonResponse(400, "Only admin can view supervisors!", null);
                }
                var supervisors = db.Users.Where(ur => ur.UserType == UserTypes.Supervisor && ur.CreatedByUserId == loginUser.Id && ur.AccountSuspend==false && ur.IsDeleted==false).Select(u => new RegisterUserDtos
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
                    BranchId= db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null ? db.Branch.Where(ur => ur.AssignedToUserId == u.Id).Select(ur => ur.Id).FirstOrDefault() : 0,
                    IsBranchAssigned = db.Branch.Where(ur => ur.AssignedToUserId == u.Id).FirstOrDefault() != null?true:false,
                    BranchName = db.Branch.Where(ur=>ur.AssignedToUserId==u.Id).FirstOrDefault() != null ? db.Branch.Where(ur => ur.AssignedToUserId == u.Id).Select(ur=>ur.Name).FirstOrDefault() : "Not assigned yet",
                }).ToList();

                return JsonResponse(200, "", supervisors);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, BaseMessage, null);
            }
        }
        [AuthorizeFilter]
        [HttpPost]
        public async Task<JsonResult> AddSupervisor(RegisterUserDtos SignUpUser)
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                ///////////////////////////////////////////////////////////////////
                if (loginUser.UserType != Models.UserTypes.Supermarket)
                {
                    return JsonResponse(400, "Only admin can add new supervisor!", null);
                }
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));

                    return JsonResponse(400, message, null);
                }
                var UserEmail = db.Users.Where(ur => ur.Email == SignUpUser.Email).Any();
                var UserPhoneNumber = db.Users.Where(ur => ur.PhoneNumber == SignUpUser.PhoneNumber).Any();
                if (UserEmail)
                {
                    return JsonResponse(400, "Email already registered!", null);
                }
                if (UserPhoneNumber)
                {
                    return JsonResponse(400, "Phone number already registered!", null);
                }
                if (SignUpUser.BranchId != null)
                {
                    var branch = db.Branch.Find(SignUpUser.BranchId);
                    if (branch == null || branch.UserId != loginUser.Id)
                    {
                        return JsonResponse(400, "Associated branch not found!", null);
                    }
                }
                ApplicationUser newUser = new ApplicationUser
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
                    UserID = "SUP-" + RandomNumber(10000, 99999),
                    AccountSuspend = false,
                    IsEmailVerified = false,
                    PhoneNumberConfirmed = false,
                    DeviceToken=GenerateRefreshToken(),
                    DeviceNumber=RandomNumber(900000,1000000).ToString(),
                    ProfileImageUrl=SignUpUser.ProfileImageUrl,
                    IsDeleted=false
                };
                var result = await UserManager.CreateAsync(newUser, "1q2w3e4r");
                if (result.Succeeded)
                {
                    var UserManager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(db));
                    UserManager.AddToRole(newUser.Id, "Supervisor");
                    if (SignUpUser.BranchId != null)
                    {
                        var associateBranch = db.Branch.Find(SignUpUser.BranchId);
                        associateBranch.AssignedToUserId = newUser.Id;
                        db.Entry(associateBranch).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return JsonResponse(200, "New supervisor registered successfully.", newUser);
                }

                else
                {
                    List<string> errorList = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errorList.Add(error);
                    }

                    return JsonResponse(400, errorList.ToString(), null);
                }
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }
        [AuthorizeFilter]
        [HttpPost]
        public JsonResult DeleteSupervisor(string SupervisorId)
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                /////////////////////////////
                ///
                if (loginUser.UserType != Models.UserTypes.Supermarket)
                {
                    return JsonResponse(400, "Only admin can trashed supervisor!", null);
                }
                var supervisor = db.Users.Find(SupervisorId);
                if (supervisor == null|| supervisor.CreatedByUserId != loginUser.Id)
                {
                    return JsonResponse(400, "Supervisor not found!", SupervisorId);
                }
                supervisor.IsDeleted = true;
                db.Entry(supervisor).State = EntityState.Modified;
                db.SaveChanges();
                return JsonResponse(200, "Supervisor successfully trashed.", SupervisorId);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, BaseMessage, null);
            }
        }
        [AuthorizeFilter]
        [HttpPost]
        public JsonResult ChangeSupervisorBranch(string SupervisorId, int? BrachId)
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                /////////////////////////////
                ///
                if (BrachId == null)
                {
                    return JsonResponse(400, "Branch not found!", BrachId);
                }
                if (loginUser.UserType != Models.UserTypes.Supermarket)
                {
                    return JsonResponse(400, "Only admin can change supervisor branch!", null);
                }
                var supervisor = db.Users.Find(SupervisorId);
                if (supervisor == null || supervisor.CreatedByUserId != loginUser.Id)
                {
                    return JsonResponse(400, "Supervisor not found!", SupervisorId);
                }
                var branch = db.Branch.Find(BrachId);
                if (branch == null || branch.UserId != loginUser.Id)
                {
                    return JsonResponse(400, "Branch not found!", BrachId);
                }
                branch.AssignedToUserId = SupervisorId;
                db.Entry(branch).State = EntityState.Modified;
                db.SaveChanges();
                return JsonResponse(200, "Branch successfully changed.", SupervisorId);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, BaseMessage, null);
            }
        }
        [AuthorizeFilter]
        public ActionResult PendingInvitations()
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                /////////////////////////////
                ///

                var con = db.MerchantConnection.Where(m => m.ShopkeeperId == loginUser.Id && m.Status == connectionStatus.Pending).Select(u=> new{
                    id=u.Id,
                    MerchantId=u.MerchantId,
                    ImageUrl=u.MerchantProxy.ProfileImageUrl,
                    Username=u.MerchantProxy.UserName,
                    Email=u.MerchantProxy.Email,
                }).ToList();
                return JsonResponse(200, "", con);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, BaseMessage, null);
            }
        }
        [AuthorizeFilter]
        public ActionResult SubscribedMerchant()
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                /////////////////////////////
                ///

                var con = db.MerchantConnection.Where(m => m.ShopkeeperId == loginUser.Id && m.Status == connectionStatus.Apporved).Select(u => new {
                    id = u.Id,
                    MerchantId = u.MerchantId,
                    ImageUrl = u.MerchantProxy.ProfileImageUrl,
                    Username = u.MerchantProxy.UserName,
                    Email = u.MerchantProxy.Email,
                }).ToList();
                return JsonResponse(200, "", con);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, BaseMessage, null);
            }
        }
        [AuthorizeFilter]
        public ActionResult AcceptInvitation(int? requestId) {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                /////////////////////////////
                ///
                if (requestId==null)
                {
                    return JsonResponse(400, "requestId should not be null", null);
                }
                var con = db.MerchantConnection.Find(requestId);
                if (con==null)
                {
                    return JsonResponse(400, "RequestId not be found in database", null);
                }
                con.Status = connectionStatus.Apporved;
                db.Entry(con).State = EntityState.Modified;
                db.SaveChanges();

                return JsonResponse(200, "Invitation approved successfully.", con);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, BaseMessage, null);
            }
        }
        [AuthorizeFilter]
        public ActionResult RejectInvitation(int? requestId)
        {
            try
            {
                ////////////////Get User from auth header/////////////////////
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                /////////////////////////////
                ///
                if (requestId == null)
                {
                    return JsonResponse(400, "requestId should not be null", null);
                }
                var con = db.MerchantConnection.Find(requestId);
                if (con == null)
                {
                    return JsonResponse(400, "RequestId not be found in database", null);
                }
                con.Status = connectionStatus.Blocked;
                db.Entry(con).State = EntityState.Modified;
                db.SaveChanges();

                return JsonResponse(200, "Invitation rejected.", con);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, BaseMessage, null);
            }
        }
        /// <summary>
        /// /////////////////////////////////////////////////////////////////Private functions
        /// </summary>
        /// <returns></returns>
        private string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            return builder.ToString();
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        // Generate a random number between two numbers  
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}