using B2B.Areas.Merchant.ViewModel;
using B2B.Middleware;
using B2B.Models;
using B2B.Models.Cart;
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
    public class OrderController : CommonController
    {
        // GET: Api/Order
        public ActionResult GetMyOrders(orderStatusEnum? status)
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
                List<OrdersVM> model = new List<OrdersVM>();
                if (loginUser.UserType== UserTypes.Supermarket)
                {
                    if (status == null)
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderByUserId == loginUser.Id).Select(u => new OrdersVM
                        {
                            Status = u.Status,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                ProductImage=n.ProductProxy.FeatureImageUrl,
                                Qty = n.Qty
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            Date = u.CreatedOn,
                            StringDate=u.CreatedOn+"",
                            BranchName=u.BranchProxy!=null?u.BranchProxy.Name:""
                        }).ToList();
                    }
                    else
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderByUserId == loginUser.Id && mu.Status == status).Select(u => new OrdersVM
                        {
                            Status = u.Status,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                Qty = n.Qty,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            Date = u.CreatedOn,
                            StringDate=u.CreatedOn+"",
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : ""
                        }).ToList();
                    }
                }
                else if(loginUser.UserType== UserTypes.Supervisor)
                {
                    if (status == null)
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id).Select(u => new OrdersVM
                        {
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : "",
                            Status = u.Status,
                            IsPaid=u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                Qty = n.Qty,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            Date = u.CreatedOn,
                            StringDate=u.CreatedOn+""
                        }).ToList();
                    }
                    else
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == status).Select(u => new OrdersVM
                        {
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : "",
                            Status = u.Status,
                            IsPaid=u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                Qty = n.Qty,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            StringDate = u.CreatedOn+"",
                            Date=u.CreatedOn
                        }).ToList();
                    }
                }
                return JsonResponse(200, "", model.OrderBy(u=>u.Date).ToList());
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public ActionResult ReorderList()
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
                List<OrdersVM> model = new List<OrdersVM>();
                if (loginUser.UserType == UserTypes.Supermarket)
                {
                        model = db.OrderHeader.Where(mu => mu.OrderByUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered).Select(u => new OrdersVM
                        {
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : "",
                            Status = u.Status,
                            IsPaid=u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                Qty = n.Qty,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            Date = u.CreatedOn,
                            StringDate = u.CreatedOn + ""
                        }).ToList();
                }
                else if (loginUser.UserType == UserTypes.Supervisor)
                {
                    model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered).Select(u => new OrdersVM
                        {
                        BranchName = u.BranchProxy != null ? u.BranchProxy.Name : "",
                        Status = u.Status,
                        IsPaid=u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                Qty = n.Qty,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            StringDate = u.CreatedOn + "",
                            Date = u.CreatedOn
                        }).ToList();
                    }
                return JsonResponse(200, "", model.OrderBy(u => u.Date).ToList());
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public ActionResult Reorder(int? orderId)
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
                if (orderId == null)
                {
                    return JsonResponse(400, "Bad Request! OrderId should not be null or empty.", null);
                }
                var order = db.OrderHeader.Find(orderId);
                if (order == null)
                {
                    return JsonResponse(400, "Bad Request! Order not found in database.", null);
                }
                else
                {
                    foreach(var Templine in db.OrderLine.Where(or => or.OrderHeaderId == orderId).ToList())
                    {
                        var product = db.StoreProducts.Find(Templine.ProductId);
                        var cartHeader = db.CartHeaders.Where(crt => crt.CreatedByUserId == loginUser.Id && crt.CreatedForUserId == product.UserId).FirstOrDefault();
                        if (cartHeader == null)
                        {
                            Random random = new Random();
                            int orderNumber = random.Next();

                            CartLine line = new CartLine
                            {
                                HeaderProxy = new CartHeader
                                {
                                    Number = "ORD-" + orderNumber,
                                    CreatedByUserId = loginUser.Id,
                                    CreatedForUserId = product.UserId,
                                    CreatedOn = currentTime
                                },
                                Qty = Templine.Qty,
                                ProductId = product.Id,
                            };
                            db.CartLines.Add(line);

                        }
                        else
                        {
                            var cartLine = db.CartLines.Where(CL => CL.HeaderId == cartHeader.Id && CL.ProductId == product.Id).FirstOrDefault();
                            if (cartLine == null)
                            {
                                CartLine line = new CartLine
                                {
                                    HeaderId = cartHeader.Id,
                                    Qty = Templine.Qty,
                                    ProductId = product.Id,
                                };
                                db.CartLines.Add(line);
                            }
                            else
                            {
                                cartLine.Qty = cartLine.Qty + Templine.Qty;
                                db.Entry(cartLine).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }
                    db.SaveChanges();
                }
                return JsonResponse(200, "Cheer up! Cart created successfully", null);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpPost]
        public JsonResult ApprovedPendingOrder(int? OrderId)
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
                if(loginUser.UserType != UserTypes.Supermarket)
                {
                    return JsonResponse(400, "Bad Request! Please login as supermarket.", null);
                }
                if (OrderId == null)
                {
                    return JsonResponse(400, "Bad Request! OrderId should not be null or empty.", null);
                }
                var order = db.OrderHeader.Find(OrderId);
                if (order.OrderByUserId != loginUser.Id)
                {
                    return JsonResponse(400, "Bad Request! Invalid order.", order.Id);
                }
                if(order.Status == orderStatusEnum.NeedApproval)
                {
                    order.Status = orderStatusEnum.Pening;
                    db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                    Notification notify2 = new Notification
                    {
                        Isseen = false,
                        Title = "Order Approved",
                        Description = "Your order having order#"+ order.OrderNumber + " has been approved by admin",
                        CreatedOn = currentTime,
                        NotifyByUserId = loginUser.Id,
                        NotifyToUserId = order.OrderCreatedByUserId,
                    };
                    db.Notification.Add(notify2);
                    db.SaveChanges();

                    return JsonResponse(200, "Success! Order approved.", order.Id);
                }
                else
                {
                    return JsonResponse(400, "Order already in queue.", null);
                }
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpPost]
        public JsonResult CancelOrder(int? OrderId)
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
                if (OrderId == null)
                {
                    return JsonResponse(400, "Bad Request! OrderId should not be null or empty.", null);
                }
                var order = db.OrderHeader.Find(OrderId);
                if (order.OrderByUserId != loginUser.Id)
                {
                    return JsonResponse(400, "Bad Request! Invalid order.", order.Id);
                }
                if (order.Status == orderStatusEnum.Enroute || order.Status == orderStatusEnum.Delivered)
                {
                    return JsonResponse(400, "OOps! Order is en-route or delivered.", order.Id);
                }
                else
                {
                    foreach (var line in db.OrderLine.Where(lin => lin.OrderHeaderId == order.Id).ToList())
                    {
                        var Pro = db.StoreProducts.Find(line.ProductId);
                        Pro.InStock = Pro.InStock + line.Qty;
                        db.Entry(Pro).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.StoreQtyMovements.RemoveRange(db.StoreQtyMovements.Where(o => o.OrderHeaderId == order.Id).ToList());
                    order.Status = orderStatusEnum.Canceled;
                    db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                    Notification notify2 = new Notification
                    {
                        Isseen = false,
                        Title = "Order Cancelled",
                        Description = "You have successfully cancelled your order order# "+order.OrderNumber,
                        CreatedOn = currentTime,
                        NotifyByUserId = loginUser.Id,
                        NotifyToUserId = loginUser.Id,
                    };
                    db.Notification.Add(notify2);
                    db.SaveChanges();
                    return JsonResponse(200, "Success! Order cancelled.", order.Id);
                }
                
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }

        public ActionResult DeliveryBoyPendingOrder()
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
                    var model = db.OrderHeader.Where(mu => mu.OrderAssignedToUserId == loginUser.Id && mu.Status== orderStatusEnum.Enroute).Select(u => new OrdersVM
                    {
                        Status = u.Status,
                        IsPaid=u.IsPaid,
                        ShippingCharges = u.ShippingCharges,
                        PaymentStatus = u.PaymentStatus,
                        CanceledComment = u.CanceledComment,
                        CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                        CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                        DeliveryNote = u.DeliveryNote,
                        DeliveryOption = u.DeliveryOption,
                        Email = u.Email,
                        Id = u.Id,
                        Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                        {
                            Id = n.Id,
                            LineTotal = n.LineTotal,
                            Price = n.Price,
                            ProductId = n.ProductId,
                            ProductName = n.ProductProxy.Name,
                            ProductImage = n.ProductProxy.FeatureImageUrl,
                            Qty = n.Qty
                        }).ToList(),
                        OrderNumber = u.OrderNumber,
                        PhoneNumber = u.PhoneNumber,
                        RejectionComment = u.RejectionComment,
                        TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                        Type = u.Type,
                        Date = u.CreatedOn,
                        StringDate = u.CreatedOn + "",
                        BranchName = u.BranchProxy != null ? u.BranchProxy.Name : ""
                    }).ToList();
                    return JsonResponse(200, "", model.OrderBy(u => u.Date).ToList());
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
        public ActionResult CompletedOrderByDeliveryBoy()
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
                    var model = db.OrderHeader.Where(mu => mu.OrderAssignedToUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered).Select(u => new OrdersVM
                    {
                        Status = u.Status,
                        IsPaid=u.IsPaid,
                        ShippingCharges = u.ShippingCharges,
                        PaymentStatus = u.PaymentStatus,
                        CanceledComment = u.CanceledComment,
                        CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                        CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                        DeliveryNote = u.DeliveryNote,
                        DeliveryOption = u.DeliveryOption,
                        Email = u.Email,
                        Id = u.Id,
                        Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                        {
                            Id = n.Id,
                            LineTotal = n.LineTotal,
                            Price = n.Price,
                            ProductId = n.ProductId,
                            ProductName = n.ProductProxy.Name,
                            ProductImage = n.ProductProxy.FeatureImageUrl,
                            Qty = n.Qty
                        }).ToList(),
                        OrderNumber = u.OrderNumber,
                        PhoneNumber = u.PhoneNumber,
                        RejectionComment = u.RejectionComment,
                        TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                        Type = u.Type,
                        Date = u.CreatedOn,
                        StringDate = u.CreatedOn + "",
                        BranchName = u.BranchProxy != null ? u.BranchProxy.Name : ""
                    }).ToList();
                    return JsonResponse(200, "", model.OrderBy(u => u.Date).ToList());
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
        public ActionResult DeliveryBoyDashboardCounter() {
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
                    var PendingDelivery = db.OrderHeader.Where(mu => mu.OrderAssignedToUserId == loginUser.Id && mu.Status == orderStatusEnum.Enroute).Count();
                    var CompletedDelivery = db.OrderHeader.Where(mu => mu.OrderAssignedToUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered).Count();
                    var response = new
                    {
                        TotalPendingOrder = PendingDelivery,
                        TotalCompletedOrder = CompletedDelivery
                    };
                    return JsonResponse(200, "", response);
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
        public ActionResult DeliveryBoyLastDelivery() {
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
                    var CompletedDelivery = db.OrderHeader.Where(mu => mu.OrderAssignedToUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered).OrderByDescending(u=>u.CreatedOn).Select(u => new OrdersVM
                    {
                        Status = u.Status,
                        ShippingCharges = u.ShippingCharges,
                        PaymentStatus = u.PaymentStatus,
                        CanceledComment = u.CanceledComment,
                        CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                        CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                        DeliveryNote = u.DeliveryNote,
                        DeliveryOption = u.DeliveryOption,
                        Email = u.Email,
                        Id = u.Id,
                        Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                        {
                            Id = n.Id,
                            LineTotal = n.LineTotal,
                            Price = n.Price,
                            ProductId = n.ProductId,
                            ProductName = n.ProductProxy.Name,
                            ProductImage = n.ProductProxy.FeatureImageUrl,
                            Qty = n.Qty
                        }).ToList(),
                        OrderNumber = u.OrderNumber,
                        PhoneNumber = u.PhoneNumber,
                        RejectionComment = u.RejectionComment,
                        TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                        Type = u.Type,
                        Date = u.CreatedOn,
                        StringDate = u.CreatedOn + "",
                        BranchName = u.BranchProxy != null ? u.BranchProxy.Name : ""
                    }).FirstOrDefault();
                    
                    return JsonResponse(200, "", CompletedDelivery);
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
        public ActionResult GetOrdersByPayment(bool? Paid)
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
                List<OrdersVM> model = new List<OrdersVM>();
                if (loginUser.UserType == UserTypes.Supermarket)
                {
                    if (Paid == null)
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderByUserId == loginUser.Id).Select(u => new OrdersVM
                        {
                            Status = u.Status,
                            IsPaid=u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                                Qty = n.Qty
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            Date = u.CreatedOn,
                            StringDate = u.CreatedOn + "",
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : ""
                        }).ToList();
                    }
                    else
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderByUserId == loginUser.Id && mu.IsPaid==Paid).Select(u => new OrdersVM
                        {
                            Status = u.Status,
                            IsPaid=u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                Qty = n.Qty,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            Date = u.CreatedOn,
                            StringDate = u.CreatedOn + "",
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : ""
                        }).ToList();
                    }
                }
                else if (loginUser.UserType == UserTypes.Supervisor)
                {
                    if (Paid == null)
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id).Select(u => new OrdersVM
                        {
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : "",
                            Status = u.Status,
                            IsPaid=u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                Qty = n.Qty,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            Date = u.CreatedOn,
                            StringDate = u.CreatedOn + ""
                        }).ToList();
                    }
                    else
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.IsPaid == Paid).Select(u => new OrdersVM
                        {
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : "",
                            Status = u.Status,
                            IsPaid=u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                Qty = n.Qty,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            StringDate = u.CreatedOn + "",
                            Date = u.CreatedOn
                        }).ToList();
                    }
                }
                return JsonResponse(200, "", model.OrderBy(u => u.Date).ToList());
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public ActionResult GetOrdersByBranchPayment(bool? Paid,int? BranchId)
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
                List<OrdersVM> model = new List<OrdersVM>();
                if (loginUser.UserType == UserTypes.Supermarket)
                {
                    if (Paid == null)
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderByUserId == loginUser.Id && mu.BranchId==BranchId).Select(u => new OrdersVM
                        {
                            Status = u.Status,
                            IsPaid = u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                                Qty = n.Qty
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            Date = u.CreatedOn,
                            StringDate = u.CreatedOn + "",
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : ""
                        }).ToList();
                    }
                    else
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderByUserId == loginUser.Id && mu.IsPaid == Paid && mu.BranchId == BranchId).Select(u => new OrdersVM
                        {
                            Status = u.Status,
                            IsPaid = u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                Qty = n.Qty,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            Date = u.CreatedOn,
                            StringDate = u.CreatedOn + "",
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : ""
                        }).ToList();
                    }
                }
                else if (loginUser.UserType == UserTypes.Supervisor)
                {
                    if (Paid == null)
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.BranchId == BranchId).Select(u => new OrdersVM
                        {
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : "",
                            Status = u.Status,
                            IsPaid = u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                Qty = n.Qty,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            Date = u.CreatedOn,
                            StringDate = u.CreatedOn + ""
                        }).ToList();
                    }
                    else
                    {
                        model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.IsPaid == Paid && mu.BranchId == BranchId).Select(u => new OrdersVM
                        {
                            BranchName = u.BranchProxy != null ? u.BranchProxy.Name : "",
                            Status = u.Status,
                            IsPaid = u.IsPaid,
                            ShippingCharges = u.ShippingCharges,
                            PaymentStatus = u.PaymentStatus,
                            CanceledComment = u.CanceledComment,
                            CustomerName = u.OrderForUserProxy != null ? u.OrderForUserProxy.UserName : "",
                            CustomerImage = u.OrderForUserProxy != null ? u.OrderForUserProxy.ProfileImageUrl : "",
                            DeliveryNote = u.DeliveryNote,
                            DeliveryOption = u.DeliveryOption,
                            Email = u.Email,
                            Id = u.Id,
                            Lines = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Select(n => new OrderLineVM
                            {
                                Id = n.Id,
                                LineTotal = n.LineTotal,
                                Price = n.Price,
                                ProductId = n.ProductId,
                                ProductName = n.ProductProxy.Name,
                                Qty = n.Qty,
                                ProductImage = n.ProductProxy.FeatureImageUrl,
                            }).ToList(),
                            OrderNumber = u.OrderNumber,
                            PhoneNumber = u.PhoneNumber,
                            RejectionComment = u.RejectionComment,
                            TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                            Type = u.Type,
                            StringDate = u.CreatedOn + "",
                            Date = u.CreatedOn
                        }).ToList();
                    }
                }
                return JsonResponse(200, "", model.OrderBy(u => u.Date).ToList());
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        public ActionResult MarkDeliveredOrder(int Id)
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
                var pro = db.OrderHeader.Where(p => p.Id == Id).FirstOrDefault();
                if (pro == null)
                {
                    return JsonResponse(400, "Invalid QRCode.", null);
                }
                else
                {
                    pro.Status = orderStatusEnum.Delivered;
                    pro.UpdatedAt = currentTime;
                    db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return JsonResponse(200, "Order Received.", null);
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