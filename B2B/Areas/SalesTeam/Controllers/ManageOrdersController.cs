using B2B.Areas.Merchant.ViewModel;
using B2B.DAL;
using B2B.Models;
using B2B.Models.Orders;
using B2B.Models.Store;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.SalesTeam.Controllers
{
    public class ManageOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        DateTime currentTime = DateTime.Now;
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

        // GET: SalesTeam/ManageOrders
        public ActionResult Orders(int? flag)
        {
            var getUser = getLoginUser();
            var loginUser = db.Users.Where(usr => usr.Id == getUser.CreatedByUserId).FirstOrDefault();
            ViewBag.Total = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id).Count();
            ViewBag.Pending = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Pening).Count();
            ViewBag.Inprocess = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.InProcess).Count();
            ViewBag.Enroute = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Enroute).Count();
            ViewBag.Delivered = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered).Count();
            ViewBag.Cancelled = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Canceled).Count();
            ViewBag.Rejected = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Rejected).Count();
            ViewBag.CashOrder = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Type == orderType.Offline).Count();
            ViewBag.ReceivedOrder = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Type == orderType.Online).Count();
            ViewBag.ReceivedSale = db.OrderLine.Where(mu => mu.OrderHeaderProxy.OrderForUserId == loginUser.Id && mu.OrderHeaderProxy.Type == orderType.Online&& (mu.OrderHeaderProxy.Status == orderStatusEnum.Delivered || mu.OrderHeaderProxy.Status == orderStatusEnum.InProcess)).Sum(u => (decimal?)u.LineTotal) ?? 0;
            ViewBag.CashSale = db.OrderLine.Where(mu => mu.OrderHeaderProxy.OrderForUserId == loginUser.Id && mu.OrderHeaderProxy.Type == orderType.Offline && (mu.OrderHeaderProxy.Status == orderStatusEnum.Delivered || mu.OrderHeaderProxy.Status == orderStatusEnum.InProcess)).Sum(u => (decimal?)u.LineTotal) ?? 0;
            ViewBag.New = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.CreatedOn.Month == currentTime.Month && mu.CreatedOn.Year == currentTime.Year).Count();

            List<OrdersVM> model = new List<OrdersVM>();
            if (flag == null)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id).Select(u => new OrdersVM
                {
                    Status = u.Status,
                    ShippingCharges = u.ShippingCharges,
                    PaymentStatus = u.PaymentStatus,
                    CanceledComment = u.CanceledComment,
                    CustomerName = u.OrderByUserProxy != null ? u.OrderByUserProxy.UserName : "",
                    CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
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
                        Qty = n.Qty
                    }).ToList(),
                    OrderNumber = u.OrderNumber,
                    PhoneNumber = u.PhoneNumber,
                    RejectionComment = u.RejectionComment,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    Type = u.Type,
                    Date = u.CreatedOn
                }).ToList();
            }
            else if (flag == 1)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Pening).Select(u => new OrdersVM
                {
                    Status = u.Status,
                    Date = u.CreatedOn,
                    ShippingCharges = u.ShippingCharges,
                    PaymentStatus = u.PaymentStatus,
                    CanceledComment = u.CanceledComment,
                    CustomerName = u.OrderByUserProxy != null ? u.OrderByUserProxy.UserName : "",
                    CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
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
                        Qty = n.Qty
                    }).ToList(),
                    OrderNumber = u.OrderNumber,
                    PhoneNumber = u.PhoneNumber,
                    RejectionComment = u.RejectionComment,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    Type = u.Type
                }).ToList();
            }
            else if (flag == 2)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.InProcess).Select(u => new OrdersVM
                {
                    Status = u.Status,
                    Date = u.CreatedOn,
                    ShippingCharges = u.ShippingCharges,
                    PaymentStatus = u.PaymentStatus,
                    CanceledComment = u.CanceledComment,
                    CustomerName = u.OrderByUserProxy != null ? u.OrderByUserProxy.UserName : "",
                    CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
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
                        Qty = n.Qty
                    }).ToList(),
                    OrderNumber = u.OrderNumber,
                    PhoneNumber = u.PhoneNumber,
                    RejectionComment = u.RejectionComment,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    Type = u.Type
                }).ToList();
            }
            else if (flag == 3)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Enroute).Select(u => new OrdersVM
                {
                    Status = u.Status,
                    ShippingCharges = u.ShippingCharges,
                    Date = u.CreatedOn,
                    PaymentStatus = u.PaymentStatus,
                    CanceledComment = u.CanceledComment,
                    CustomerName = u.OrderByUserProxy != null ? u.OrderByUserProxy.UserName : "",
                    CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
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
                        Qty = n.Qty
                    }).ToList(),
                    OrderNumber = u.OrderNumber,
                    PhoneNumber = u.PhoneNumber,
                    RejectionComment = u.RejectionComment,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    Type = u.Type
                }).ToList();
            }
            else if (flag == 4)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered).Select(u => new OrdersVM
                {
                    Status = u.Status,
                    ShippingCharges = u.ShippingCharges,
                    PaymentStatus = u.PaymentStatus,
                    Date = u.CreatedOn,
                    CanceledComment = u.CanceledComment,
                    CustomerName = u.OrderByUserProxy != null ? u.OrderByUserProxy.UserName : "",
                    CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
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
                        Qty = n.Qty
                    }).ToList(),
                    OrderNumber = u.OrderNumber,
                    PhoneNumber = u.PhoneNumber,
                    RejectionComment = u.RejectionComment,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    Type = u.Type
                }).ToList();
            }
            else if (flag == 5)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Canceled).Select(u => new OrdersVM
                {
                    Status = u.Status,
                    ShippingCharges = u.ShippingCharges,
                    PaymentStatus = u.PaymentStatus,
                    Date = u.CreatedOn,
                    CanceledComment = u.CanceledComment,
                    CustomerName = u.OrderByUserProxy != null ? u.OrderByUserProxy.UserName : "",
                    CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
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
                        Qty = n.Qty
                    }).ToList(),
                    OrderNumber = u.OrderNumber,
                    PhoneNumber = u.PhoneNumber,
                    RejectionComment = u.RejectionComment,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    Type = u.Type
                }).ToList();
            }
            else if (flag == 6)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Rejected).Select(u => new OrdersVM
                {
                    Status = u.Status,
                    ShippingCharges = u.ShippingCharges,
                    PaymentStatus = u.PaymentStatus,
                    Date = u.CreatedOn,
                    CanceledComment = u.CanceledComment,
                    CustomerName = u.OrderByUserProxy != null ? u.OrderByUserProxy.UserName : "",
                    CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
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
                        Qty = n.Qty
                    }).ToList(),
                    OrderNumber = u.OrderNumber,
                    PhoneNumber = u.PhoneNumber,
                    RejectionComment = u.RejectionComment,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    Type = u.Type
                }).ToList();
            }
            else if (flag == 7)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Type == orderType.Offline).Select(u => new OrdersVM
                {
                    Status = u.Status,
                    ShippingCharges = u.ShippingCharges,
                    PaymentStatus = u.PaymentStatus,
                    Date = u.CreatedOn,
                    CanceledComment = u.CanceledComment,
                    CustomerName = u.OrderByUserProxy != null ? u.OrderByUserProxy.UserName : "",
                    CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
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
                        Qty = n.Qty
                    }).ToList(),
                    OrderNumber = u.OrderNumber,
                    PhoneNumber = u.PhoneNumber,
                    RejectionComment = u.RejectionComment,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    Type = u.Type
                }).ToList();
            }
            else if (flag == 8)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Type == orderType.Online).Select(u => new OrdersVM
                {
                    Status = u.Status,
                    ShippingCharges = u.ShippingCharges,
                    PaymentStatus = u.PaymentStatus,
                    Date = u.CreatedOn,
                    CanceledComment = u.CanceledComment,
                    CustomerName = u.OrderByUserProxy != null ? u.OrderByUserProxy.UserName : "",
                    CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
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
                        Qty = n.Qty
                    }).ToList(),
                    OrderNumber = u.OrderNumber,
                    PhoneNumber = u.PhoneNumber,
                    RejectionComment = u.RejectionComment,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    Type = u.Type
                }).ToList();
            }
            else if (flag == 9)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.CreatedOn.Month == currentTime.Month && mu.CreatedOn.Year == currentTime.Year).Select(u => new OrdersVM
                {
                    Status = u.Status,
                    ShippingCharges = u.ShippingCharges,
                    PaymentStatus = u.PaymentStatus,
                    Date = u.CreatedOn,
                    CanceledComment = u.CanceledComment,
                    CustomerName = u.OrderByUserProxy != null ? u.OrderByUserProxy.UserName : "",
                    CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
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
                        Qty = n.Qty
                    }).ToList(),
                    OrderNumber = u.OrderNumber,
                    PhoneNumber = u.PhoneNumber,
                    RejectionComment = u.RejectionComment,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    Type = u.Type
                }).ToList();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult InprocessOrder(int IOrderId,string userID)
        {
            var loginUser = getLoginUser();
            var DeliveryBoy = db.Users.Find(userID);
            if(DeliveryBoy==null||DeliveryBoy.UserType!= UserTypes.DeliveryTeam || DeliveryBoy.CreatedByUserId != loginUser.CreatedByUserId)
            {

                var Res = new
                {
                    Status = false,
                    Data = "Delivery User not found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
            var pro = db.OrderHeader.Where(p => p.Id == IOrderId && p.OrderForUserId == loginUser.CreatedByUserId).FirstOrDefault();

            if (pro == null)
            {
                var Res = new
                {
                    Status = false,
                    Data = "No order found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (pro.Status == orderStatusEnum.Pening)
                {
                    pro.Status = orderStatusEnum.InProcess;
                    pro.OrderAssignedToUserId = userID;
                    pro.UpdatedAt = currentTime;
                    db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    foreach (var line in db.OrderLine.Where(u => u.OrderHeaderId == pro.Id).ToList())
                    {
                        var prod = db.StoreProducts.Find(line.ProductId);
                        prod.InStock = prod.InStock - line.Qty;
                        db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
                        Store_Qty_Movement movement = new Store_Qty_Movement
                        {
                            Type = transactionType.Sale,
                            Cost = prod.Cost,
                            Price = line.Price,
                            Total = line.LineTotal,
                            OutQty = line.Qty,
                            InQty = 0,
                            OrderHeaderId = pro.Id,
                            ProductId = line.ProductId,
                            CreatedOn = currentTime,
                        };
                        db.StoreQtyMovements.Add(movement);
                    }
                    db.SaveChanges();
                    var Res = new
                    {
                        Status = true,
                        Data = "Now order is inprocess."
                    };
                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Res = new
                    {
                        Status = false,
                        Data = "Sorry! invalid operation"
                    };
                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        public ActionResult CancelledOrder(int OrderId, string reason)
        {
            var loginUser = getLoginUser();
            var pro = db.OrderHeader.Where(p => p.Id == OrderId && p.OrderForUserId == loginUser.CreatedByUserId).FirstOrDefault();

            if (pro == null)
            {
                var Res = new
                {
                    Status = false,
                    Data = "No order found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (pro.Status == orderStatusEnum.InProcess)
                {
                    pro.Status = orderStatusEnum.Canceled;
                    pro.UpdatedAt = currentTime;
                    pro.CanceledComment = reason;
                    db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    foreach (var line in db.OrderLine.Where(u => u.OrderHeaderId == pro.Id).ToList())
                    {
                        var prod = db.StoreProducts.Find(line.ProductId);
                        prod.InStock = prod.InStock + line.Qty;
                        db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.StoreQtyMovements.RemoveRange(db.StoreQtyMovements.Where(u => u.OrderHeaderId == pro.Id).ToList());
                    db.SaveChanges();

                    var Res = new
                    {
                        Status = true,
                        Data = "Order cancelled."
                    };

                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var Res = new
                    {
                        Status = false,
                        Data = "Sorry! invalid operation"
                    };
                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        public ActionResult RejectOrder(int OrderId, string reason)
        {
            var loginUser = getLoginUser();
            var pro = db.OrderHeader.Where(p => p.Id == OrderId && p.OrderForUserId == loginUser.CreatedByUserId).FirstOrDefault();

            if (pro == null)
            {
                var Res = new
                {
                    Status = false,
                    Data = "No order found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (pro.Status == orderStatusEnum.Pening)
                {
                    pro.Status = orderStatusEnum.Rejected;
                    pro.UpdatedAt = currentTime;
                    pro.RejectionComment = reason;
                    db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    var Res = new
                    {
                        Status = true,
                        Data = "Order cancelled."
                    };

                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var Res = new
                    {
                        Status = false,
                        Data = "Sorry! invalid operation"
                    };
                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult EnrouteOrder(int Id)
        {
            var loginUser = getLoginUser();

            var pro = db.OrderHeader.Where(p => p.Id == Id && p.OrderForUserId == loginUser.CreatedByUserId).FirstOrDefault();

            if (pro == null)
            {
                var Res = new
                {
                    Status = false,
                    Data = "No order found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (pro.Status == orderStatusEnum.InProcess)
                {
                    pro.Status = orderStatusEnum.Enroute;
                    pro.UpdatedAt = currentTime;
                    db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    var Res = new
                    {
                        Status = true,
                        Data = "Now order is inprocess."
                    };
                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Res = new
                    {
                        Status = false,
                        Data = "Sorry! invalid operation"
                    };
                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult DeliveredOrder(int Id)
        {
            var loginUser = getLoginUser();

            var pro = db.OrderHeader.Where(p => p.Id == Id && p.OrderForUserId == loginUser.CreatedByUserId).FirstOrDefault();

            if (pro == null)
            {
                var Res = new
                {
                    Status = false,
                    Data = "No order found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (pro.Status == orderStatusEnum.Enroute)
                {
                    pro.Status = orderStatusEnum.Delivered;
                    pro.UpdatedAt = currentTime;
                    db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    var Res = new
                    {
                        Status = true,
                        Data = "Now order is inprocess."
                    };
                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Res = new
                    {
                        Status = false,
                        Data = "Sorry! invalid operation"
                    };
                    return Json(Res, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult GetOrderLineDetail(int Id)
        {
            var pro = db.OrderLine.Where(p => p.OrderHeaderId == Id).Select(u => new OrderLineVM
            {
                Id = u.Id,
                LineTotal = u.LineTotal,
                Price = u.Price,
                ProductName = u.ProductProxy.Name,
                Qty = u.Qty,
            }).ToList();

            if (pro == null)
            {
                var Res = new
                {
                    Status = false,
                    Data = "Not found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(pro, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetProductsApi()
        {
            var loginUser = getLoginUser();
            var models = db.StoreProducts.Where(prd => prd.UserId == loginUser.CreatedByUserId && prd.Status == UserStatus.Active).Select(u => new ProductsVM
            {
                Name = u.Name,
                Id = u.Id,
            }).ToList();
            return Json(models, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserApi()
        {
            var loginUser = getLoginUser();
            var models = db.Users.Where(prd => prd.CreatedByUserId == loginUser.CreatedByUserId && prd.Status == UserStatus.Active &&prd.UserType== UserTypes.DeliveryTeam).Select(u => new ProductsVM
            {
                Name = u.UserName,
                Description=u.Id
            }).ToList();
            return Json(models, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateOfflineOrder()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateOfflineOrder(OrdersVM model, bool orderDelivered)
        {
            var loginUser = getLoginUser();
            string CustomerId = null;
            if (model.UserNumber != null)
            {
                var shopkeeper = db.Users.Where(Ur => Ur.UserID == model.UserNumber).FirstOrDefault();
                if (shopkeeper == null)
                {
                    ModelState.AddModelError("", "Customer number is invalid");
                    return View(model);
                }
                else
                {
                    CustomerId = shopkeeper.Id;
                }
            }
           
            if (ModelState.IsValid)
            {
                if (model.Lines == null)
                {
                    ModelState.AddModelError("", "Order line should not be empty");
                    return View(model);
                }

                OrderHeader header = new OrderHeader
                {
                    CreatedOn = currentTime,
                    ShippingCharges = model.ShippingCharges,
                    Status = orderStatusEnum.InProcess,
                    PaymentStatus = paymentMethodEnum.COD,
                    DeliveryOption = model.DeliveryOption,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Type = orderType.Offline,
                    OrderForUserId = loginUser.CreatedByUserId,
                    OrderNumber = "ORD-" + RandomNumber(1000, 100000),
                    DeliveryNote = model.DeliveryNote,
                    OrderByUserId = CustomerId,
                    OrderAssignedToUserId = model.DeliveryById
                };
                if (orderDelivered)
                {
                    header.Status = orderStatusEnum.Delivered;
                }
                var DbHeader = db.OrderHeader.Add(header);
                foreach (var line in model.Lines)
                {

                    var Pro = db.StoreProducts.Find(line.ProductId);
                    if (Pro == null)
                    {
                        ModelState.AddModelError("", "Product# " + line.ProductId + " not found.");
                        return View(model);
                    }
                    if (line.Qty <= 0 || line.Price <= 0)
                    {
                        ModelState.AddModelError("", "Qty & Price should not be empty.");
                        return View(model);
                    }
                    OrderLine Templine = new OrderLine
                    {
                        LineTotal = line.LineTotal,
                        OrderHeaderId = DbHeader.Id,
                        Price = line.Price,
                        ProductId = line.ProductId,
                        Qty = line.Qty,
                    };
                    db.OrderLine.Add(Templine);
                    Store_Qty_Movement movement = new Store_Qty_Movement
                    {
                        Type = transactionType.Sale,
                        Cost = Pro.Cost,
                        Price = line.Price,
                        Total = line.LineTotal,
                        OutQty = line.Qty,
                        InQty = 0,
                        OrderHeaderId = DbHeader.Id,
                        ProductId = line.ProductId,
                        CreatedOn = currentTime,
                    };
                    db.StoreQtyMovements.Add(movement);
                    Pro.InStock = Pro.InStock - line.Qty;
                    db.Entry(Pro).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Orders");
            }
            return View(model);
        }
        public ActionResult GetProductDetail(int ProductId)
        {
            var pro = db.StoreProducts.Find(ProductId);
            if (pro == null)
            {
                var Res = new
                {
                    Status = false,
                    Data = "Not found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Res = new
                {
                    Status = true,
                    Data = new
                    {
                        ProductId = pro.Id,
                        Price = pro.SalePrice,
                        ProductName = pro.Name,
                        Unit = pro.StoreMeasurementUnitProxy.Name,
                        Qty = pro.InStock,
                        Cost = pro.Cost
                    }
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

    }
}