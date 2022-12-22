using B2B.Areas.Api.Dtos;
using B2B.Areas.Merchant.ViewModel;
using B2B.Middleware;
using B2B.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Api.Controllers
{
    [AllowAnonymous]
    public class OpenResoController : CommonController
    {
        private string _UserDirectoryBaseUrl = "/images/users/";
        private string _ProductDirectoryBaseUrl = "/images/products/";
        private string _BranchDirectoryBaseUrl = "/images/branches/";
        private string _DefaultDirectoryBaseUrl = "/images/default/";

        // GET: Api/OpenReso
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase imageFile)
        {
            try
            {
                if (imageFile == null)
                {
                    return JsonResponse(404, "Image file not found!", null);
                }

                string rawAuthorizationHeader = Request.Headers["flag"];
                string userIdHeader = Request.Headers["UID"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var authUser = AuthenticationHeaderValue.Parse(userIdHeader);
                var flagValue = authHeader.Scheme;
                var UserValue = authUser.Scheme;
                int flag = 0;
                string userId = "";

                string ImagePath = "";

                if (flagValue == null)
                {
                    return JsonResponse(404, "Flag not found!", null);
                }

                flag = int.Parse(authHeader.Scheme);

                //User Profile Image Upload
                if (flag == 1)
                {
                    if (UserValue == null)
                    {
                        return JsonResponse(404, "UID not found!", null);
                    }
                    userId = authUser.Scheme;
                    ImagePath = _UserDirectoryBaseUrl;
                }
                //Product Image Upload
                else if (flag == 2)
                {
                    ImagePath = _ProductDirectoryBaseUrl;
                }
                //Branch Image Upload
                else if (flag == 3)
                {
                    ImagePath = _BranchDirectoryBaseUrl;
                }
                //Application Images Upload
                else
                {
                    ImagePath = _DefaultDirectoryBaseUrl;
                }
               
                string filename = Path.GetFileNameWithoutExtension(imageFile.FileName);
                string extension = Path.GetExtension(imageFile.FileName);
                filename = filename + currentTime.ToString("yymmssfff") + extension;
                string CompleteImagePath = ImagePath + filename;
                filename = Path.Combine(Server.MapPath(ImagePath), filename);
                imageFile.SaveAs(filename);
                if (flag == 1)
                {
                    var loginUser = db.Users.Find(userId);
                    loginUser.ProfileImageUrl = CompleteImagePath;
                    db.SaveChanges();
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
                        NotificationEnabled = loginUser.NotificationEnabled,
                        IsEmailVerified = loginUser.IsEmailVerified,
                        OrderAuthenticationEnabled = loginUser.OrderAuthenticationEnabled
                    };
                    return JsonResponse(200, "Image uploaded successfully", userProfile);
                }
                else
                {
                    return JsonResponse(200, "Image uploaded successfully", CompleteImagePath);
                }
            }
            catch (Exception ex)
            {
                var BaseError = ex.GetBaseException();
                return JsonResponse(504, BaseError.Message, null);
            }
        }
        [AuthorizeFilter]
        public ActionResult GetTermsandAbout(int flag)
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
                var setting = db.Setting.FirstOrDefault();
                if (setting != null)
                {
                    if (flag == 1)
                    {
                        return JsonResponse(200, "", setting.TermsandCondition);
                    }
                    else
                    {
                        return JsonResponse(200, "", setting.AboutUs);
                    }
                }
                else
                {
                    return JsonResponse(200, "", "");
                }
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, BaseMessage, null);
            }
        }
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult PostComplain(string Title, string Url, string Description, int orderId)
        {
            try
            {
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                var order = db.OrderHeader.Find(orderId);
                if (order == null)
                {
                    return JsonResponse(400, "Error! order not found.", orderId);
                }
                Complains com = new Complains
                {
                    CreatedOn=currentTime,
                    Title=Title,
                    OrderId=orderId,
                    AttachFileUrl=Url,
                    Complain=Description,
                    Status= complainStatus.Pending,
                    UserId=loginUser.Id
                };
                com=db.Complains.Add(com);
                db.SaveChanges();
                return JsonResponse(200, "Success! complain submitted.", com.Id);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public ActionResult MyComplains()
        {
            try
            {
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                var com = db.Complains.Where(u => u.UserId == loginUser.Id).OrderBy(o => o.CreatedOn).Select(u => new {
                    ComplainId=u.Id,
                    ComplainTitle= u.Title,
                    ComplainDescription= u.Complain,
                    ComplainDate= u.CreatedOn+"",
                    ComplainUrl= u.AttachFileUrl,
                    ComplainStatus=u.Status,
                    BranchName = u.OrderProxy.BranchProxy != null ? u.OrderProxy.BranchProxy.Name : "",
                    Status = u.Status,
                    IsPaid = u.OrderProxy.IsPaid,
                    ShippingCharges = u.OrderProxy.ShippingCharges,
                    PaymentStatus = u.OrderProxy.PaymentStatus,
                    CanceledComment = u.OrderProxy.CanceledComment,
                    CustomerName = u.OrderProxy.OrderForUserProxy != null ? u.OrderProxy.OrderForUserProxy.UserName : "",
                    CustomerImage = u.OrderProxy.OrderForUserProxy != null ? u.OrderProxy.OrderForUserProxy.ProfileImageUrl : "",
                    DeliveryNote = u.OrderProxy.DeliveryNote,
                    DeliveryOption = u.OrderProxy.DeliveryOption,
                    Id = u.Id,
                    Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.OrderProxy.Id).Select(n => new OrderLineVM
                    {
                        Id = n.Id,
                        LineTotal = n.LineTotal,
                        Price = n.Price,
                        ProductId = n.ProductId,
                        ProductName = n.ProductProxy.Name,
                        Qty = n.Qty,
                        ProductImage = n.ProductProxy.FeatureImageUrl,
                    }).ToList(),
                    OrderNumber = u.OrderProxy.OrderNumber,
                    PhoneNumber = u.OrderProxy.PhoneNumber,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.OrderId).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    OrderDate = u.CreatedOn + "",
                }).ToList();
                
                return JsonResponse(200, "Success", com);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public ActionResult GetNotification()
        {
            try
            {
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                var com = db.Notification.Where(u => u.NotifyToUserId == loginUser.Id).OrderBy(o => o.CreatedOn).Select(u => new {
                    id=u.Id,
                    CreatedOn = u.CreatedOn+"",
                    Title = u.Title,
                    Summary = u.Description,
                    Isseen=u.Isseen
                }).ToList();
                
                return JsonResponse(200, "Success", com);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public ActionResult GetUsersDetails()
        {
            try
            {
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                var com = db.Users.Where(u => u.UserType == UserTypes.Merchant).OrderBy(o => o.CreatedOn).Select(u => new {
                    Id = u.Id,
                    Name = u.ProfileName,
                    Email = u.Email,
                    ImageUrl = u.ProfileImageUrl,
                }).ToList();

                return JsonResponse(200, "Success", com);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public ActionResult MarkAllPendingNotificationSeen()
        {
            try
            {
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                var com = db.Notification.Where(u => u.NotifyToUserId == loginUser.Id&&u.Isseen==false).ToList();
                foreach(var line in com)
                {
                    line.Isseen = true;
                    db.Entry(line).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return JsonResponse(200, "Success", null);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public ActionResult MarkNotificationSeen(int id)
        {
            try
            {
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                var line = db.Notification.Find(id);
                if (line==null)
                {
                    return JsonResponse(400, "Bad Request! Notification not found.", null);
                }
                line.Isseen = true;
                db.Entry(line).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return JsonResponse(200, "Success", null);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public JsonResult CountCarts()
        {
            try
            {
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                var cartsResponse = db.CartHeaders.Where(lin => lin.CreatedByUserId == loginUser.Id).Count();
                return JsonResponse(200, "", cartsResponse);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public ActionResult CountNotification()
        {
            try
            {
                string rawAuthorizationHeader = Request.Headers["Authorization"];
                var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                var credentialPair = authHeader.Parameter;
                var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                var deviceToken = credentials[0];
                var deviceNumber = credentials[1];
                var loginUser = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                var com = db.Notification.Where(u => u.NotifyToUserId == loginUser.Id && u.Isseen==false).Count();

                return JsonResponse(200, "Success", com);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
    }
}