using B2B.Middleware;
using B2B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Api.Controllers
{
    [AuthorizeFilter]
    [AllowAnonymous]
    public class AccountController : CommonController
    {
        
        public ActionResult Payments()
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
                var respose = new
                {
                    TotalPayment = 0,
                    PaidAmount=0,
                    Payable=0
                };
                return JsonResponse(200, "", respose);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
          public ActionResult DeliveryBoyCompletedOrder(DateTime startDate, DateTime endDate)
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
                if (loginUser.UserType == UserTypes.DeliveryTeam)
                {
                    var model = db.OrderHeader.Where(mu => mu.OrderAssignedToUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered).Count();
                    return JsonResponse(200, "", model);
                }
                else
                {
                    return JsonResponse(400, "Bad Request! only delivery team can access this.", null);
                }
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public ActionResult DeliveryBoyCompletedOrderGraph(DateTime startDate, DateTime endDate)
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
                if (loginUser.UserType == UserTypes.DeliveryTeam)
                {
                    var difference = endDate.Subtract(startDate).TotalDays;
                    List<object> res = new List<object>();
                    for (int i = 1; i <= difference; i++)
                    {
                        var temp = new
                        {
                            Days = i,
                            CompletedOrders = i
                        };
                        res.Add(temp);
                    }
                    return JsonResponse(200, "", res);
                }
                else
                {
                    return JsonResponse(400, "Bad Request! only delivery team can access this.", null);
                }
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
    }
}