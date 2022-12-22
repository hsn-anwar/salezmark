using B2B.Areas.Api.Dtos;
using B2B.Areas.Merchant.ViewModel;
using B2B.Areas.Supermarket.ViewModel;
using B2B.DAL;
using B2B.Models;
using B2B.Models.Orders;
using B2B.Models.Store;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Merchant.Controllers
{
    public class MerchantOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        DateTime currentTime = DateTime.Now;
        private string _ProductsDirectoryBaseUrl = "/images/products";
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
        // GET: Merchant/MerchantOrders
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Products(int? flag)
        {
            var loginUser = getLoginUser();
            ViewBag.Total = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id).Count();
            ViewBag.Active = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id && prd.Status == UserStatus.Active).Count();
            ViewBag.Deleted = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id && prd.Status == UserStatus.Deleted).Count();
            ViewBag.Block = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id && prd.Status == UserStatus.Blocked).Count();
            var All = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id).ToList();
            decimal NValue = 0;
            foreach (var line in All)
            {
                decimal Temp = line.InStock * line.Cost;
                NValue = NValue + Temp;
            }
            ViewBag.NetValue = NValue;
            List<ProductsVM> models = new List<ProductsVM>();
            if (flag == null)
            {
                models = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id).Select(u => new ProductsVM
                {
                    SalePrice = u.SalePrice,
                    LowStock=u.LowStock,
                    Specification = u.Specification,
                    Description = u.Description,
                    ActualPrice = u.ActualPrice,
                    Cost = u.Cost,
                    Discount = u.Discount,
                    Name = u.Name,
                    FeatureImageUrl = u.FeatureImageUrl,
                    Tax = u.Tax,
                    StoreMeasurementUnitName = u.StoreMeasurementUnitProxy.Name,
                    Date = u.CreatedOn,
                    Id = u.Id,
                    InStock=u.InStock,
                    userStatus = u.Status,
                    CategoryName=u.CategoryProxy.Name
                }).ToList();

            }
            else if (flag == 1)
            {
                models = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id && prd.Status == UserStatus.Active).Select(u => new ProductsVM
                {
                    SalePrice = u.SalePrice,
                    Specification = u.Specification,
                    Description = u.Description,
                    ActualPrice = u.ActualPrice,
                    Cost = u.Cost,
                    LowStock = u.LowStock,
                    Discount = u.Discount,
                    Name = u.Name,
                    FeatureImageUrl = u.FeatureImageUrl,
                    Tax = u.Tax,
                    StoreMeasurementUnitName = u.StoreMeasurementUnitProxy.Name,
                    Date = u.CreatedOn,
                    Id = u.Id,
                    InStock = u.InStock,
                    userStatus = u.Status,
                    CategoryName = u.CategoryProxy.Name
                }).ToList();
            }
            else if (flag == 2)
            {
                models = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id && prd.Status == UserStatus.Deleted).Select(u => new ProductsVM
                {
                    SalePrice = u.SalePrice,
                    Specification = u.Specification,
                    Description = u.Description,
                    ActualPrice = u.ActualPrice,
                    Cost = u.Cost,
                    LowStock = u.LowStock,
                    Discount = u.Discount,
                    Name = u.Name,
                    FeatureImageUrl = u.FeatureImageUrl,
                    Tax = u.Tax,
                    StoreMeasurementUnitName = u.StoreMeasurementUnitProxy.Name,
                    Date = u.CreatedOn,
                    Id = u.Id,
                    userStatus = u.Status,
                    InStock = u.InStock,
                    CategoryName = u.CategoryProxy.Name
                }).ToList();
            }
            else if (flag == 3)
            {
                models = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id && prd.Status == UserStatus.Blocked).Select(u => new ProductsVM
                {
                    SalePrice = u.SalePrice,
                    Specification = u.Specification,
                    Description = u.Description,
                    ActualPrice = u.ActualPrice,
                    Cost = u.Cost,
                    LowStock = u.LowStock,
                    Discount = u.Discount,
                    Name = u.Name,
                    FeatureImageUrl = u.FeatureImageUrl,
                    Tax = u.Tax,
                    StoreMeasurementUnitName = u.StoreMeasurementUnitProxy.Name,
                    Date = u.CreatedOn,
                    Id = u.Id,
                    userStatus = u.Status,
                    InStock = u.InStock,
                    CategoryName = u.CategoryProxy.Name
                }).ToList();
            }
            return View(models);
        }
        public ActionResult AddProducts()
        {
            var loginUser = getLoginUser();
            ViewBag.StoreMeasurementUnitList = new SelectList(db.StoreMeasurementUnits.Where(pr => pr.CreatedByUserId == loginUser.Id&&pr.IsDeleted==false).ToList(), "Id", "Name");
            ViewBag.CategoryList = new SelectList(db.StoreCategory.Where(c=>c.IsDeleted==false).ToList(), "Id", "Name");
            ViewBag.CompanyList = new SelectList(db.StoreCompany.Where(c=>c.IsDeleted==false).ToList(), "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProducts(ProductsVM model)
        {
            var loginUser = getLoginUser();

            ViewBag.StoreMeasurementUnitList = new SelectList(db.StoreMeasurementUnits.Where(pr => pr.CreatedByUserId == loginUser.Id && pr.IsDeleted == false).ToList(), "Id", "Name");
            ViewBag.CategoryList = new SelectList(db.StoreCategory.Where(c => c.IsDeleted == false).ToList(), "Id", "Name");
            ViewBag.CompanyList = new SelectList(db.StoreCompany.Where(c => c.IsDeleted == false).ToList(), "Id", "Name");

            if (ModelState.IsValid)
            {
                Store_Products pro = new Store_Products
                {
                    ActualPrice = model.ActualPrice,
                    SalePrice = model.SalePrice,
                    Specification = model.Specification,
                    StoreMeasurementUnitId = model.StoreMeasurementUnitId,
                    Name = model.Name,
                    Description = model.Description,
                    CreatedOn = currentTime,
                    Cost = model.Cost,
                    Tax = model.Tax,
                    UserId = loginUser.Id,
                    Discount = model.Discount,
                    FeatureImageUrl = model.FeatureImageUrl,
                    LowStock=model.LowStock,
                    Status= UserStatus.Active,
                    CategoryId=model.CategoryId,
                    CompanyId=model.CompanyId
                };
                if (model.ImageFile != null)
                {
                    pro.FeatureImageUrl = UploadImage(model.ImageFile, _ProductsDirectoryBaseUrl);
                }
                db.StoreProducts.Add(pro);
                db.SaveChanges();
                return RedirectToAction("Products");
            }
            return View(model);
        }
        /// <summary>
        /// //////////////////////////////////////////Measurement Units
        /// </summary>
        /// <returns></returns>
        public ActionResult MeasurementUnit()
        {
            var loginUser = getLoginUser();
            var units = db.StoreMeasurementUnits.Where(mu => mu.CreatedByUserId == loginUser.Id&&mu.IsDeleted==false).ToList();
            return View(units);
        }
        public ActionResult AddUnit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUnit(Store_Measurement_Units model)
        {
            var loginUser = getLoginUser();
            if (ModelState.IsValid)
            {
                model.CreatedByUserId = loginUser.Id;
                model.CreatedOn = currentTime;
                db.StoreMeasurementUnits.Add(model);
                db.SaveChanges();
                var response = new
                {
                    Status=true,
                    Msg=""
                };
                return Json(response,JsonRequestBehavior.AllowGet);
            }
            var Errorresponse = new
            {
                Status = false,
                Msg = "some error occure."
            };
            return Json(Errorresponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeUnitStatus(int Id)
        {
            var loginUser = getLoginUser();
            var user = db.StoreMeasurementUnits.Where(mu=>mu.CreatedByUserId==loginUser.Id&& mu.Id==Id).FirstOrDefault();
            if (user == null)
            {
                var response = new
                {
                    Status = false,
                    Msg = "Unit not found"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (user.IsDeleted)
            {

                user.IsDeleted = false;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var sResponse = new
                {
                    Status = true,
                    Msg = "Restore."
                };
                return Json(sResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {

                user.IsDeleted = true;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var sResponse = new
                {
                    Status = true,
                    Msg = "Deleted."
                };
                return Json(sResponse, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ChangeProductStatus(int Id)
        {
            var loginUser = getLoginUser();
            var user = db.StoreProducts.Where(mu => mu.UserId == loginUser.Id && mu.Id == Id).FirstOrDefault();
            if (user == null)
            {
                var response = new
                {
                    Status = false,
                    Msg = "Product not found"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (user.Status== UserStatus.Active)
            {

                user.Status = UserStatus.Deleted;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var sResponse = new
                {
                    Status = true,
                    Msg = "Deleted."
                };
                return Json(sResponse, JsonRequestBehavior.AllowGet);
            }
            else if(user.Status== UserStatus.Deleted)
            {
                user.Status = UserStatus.Active;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var sResponse = new
                {
                    Status = true,
                    Msg = "Restore."
                };
                return Json(sResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var sResponse = new
                {
                    Status = false,
                    Msg = "Invalid operation."
                };
                return Json(sResponse, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// /////////////////////////////////////////////Orders
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult GetProductsApi()
        {
            var loginUser = getLoginUser();
            var models = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id && prd.Status == UserStatus.Active).Select(u => new ProductsVM
            {
                Name = u.Name,
                Id = u.Id,
            }).ToList();
            return Json(models,JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateOfflineOrder()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateOfflineOrder(OrdersVM model,bool orderDelivered)
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
                    OrderForUserId = loginUser.Id,
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
                return RedirectToAction("ReceivedOrders");
            }
            return View(model);
        }
        public ActionResult AddPurchaseOrder()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPurchaseOrder(OrdersVM model)
        {
            var loginUser = getLoginUser();
            if (ModelState.IsValid)
            {
                if (model.Lines == null)
                {
                    ModelState.AddModelError("", "Purchase Line should not be empty");
                    return View(model);
                }
                PurchaseOrderHeader header = new PurchaseOrderHeader
                {
                    CreatedOn = currentTime,
                    OrderNumber = "PRO-" + RandomNumber(1000, 100000),
                    Description = model.DeliveryNote,
                    CreatedByUserId=loginUser.Id,
                    SupplierName=model.Email,
                    PhoneNumber=model.PhoneNumber
                };
                var DbHeader = db.PurchaseOrderHeader.Add(header);
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
                    PurchaseOrderLine Templine = new PurchaseOrderLine
                    {
                        Total = line.LineTotal,
                        PurchaseOrderHeaderId = DbHeader.Id,
                        Price = line.Price,
                        ProductId = line.ProductId,
                        Qty = line.Qty,
                    };
                    db.PurchaseOrderLine.Add(Templine);
                    Store_Qty_Movement movement = new Store_Qty_Movement
                    {
                        Type = transactionType.Purchase,
                        Cost = Pro.Cost,
                        Price = line.Price,
                        Total = line.LineTotal,
                        OutQty = 0,
                        InQty = line.Qty,
                        PurchaseOrderHeaderId = DbHeader.Id,
                        ProductId = line.ProductId,
                        CreatedOn = currentTime,
                    };
                    db.StoreQtyMovements.Add(movement);
                    Pro.InStock = Pro.InStock + line.Qty;
                    db.Entry(Pro).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("PurchaseOrders");
            }
            return View(model);
        }
        public ActionResult ReceivedOrders(int? flag)
        {
            var loginUser = getLoginUser();
            ViewBag.Total = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status!= orderStatusEnum.NeedApproval).Count();
            ViewBag.Pending = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status== orderStatusEnum.Pening).Count();
            ViewBag.Inprocess = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status== orderStatusEnum.InProcess).Count();
            ViewBag.Enroute = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status== orderStatusEnum.Enroute).Count();
            ViewBag.Delivered = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status== orderStatusEnum.Delivered).Count();
            ViewBag.Cancelled = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status== orderStatusEnum.Canceled).Count();
            ViewBag.Rejected = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status== orderStatusEnum.Rejected).Count();
            ViewBag.CashOrder = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Type== orderType.Offline).Count();
            ViewBag.ReceivedOrder = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Type== orderType.Online).Count();
            ViewBag.ReceivedSale = db.OrderLine.Where(mu => mu.OrderHeaderProxy.OrderForUserId == loginUser.Id && mu.OrderHeaderProxy.Type== orderType.Online && (mu.OrderHeaderProxy.Status == orderStatusEnum.Delivered || mu.OrderHeaderProxy.Status == orderStatusEnum.InProcess)).Sum(u=>(decimal?) u.LineTotal)??0;
            ViewBag.CashSale = db.OrderLine.Where(mu => mu.OrderHeaderProxy.OrderForUserId == loginUser.Id && mu.OrderHeaderProxy.Type== orderType.Offline && (mu.OrderHeaderProxy.Status== orderStatusEnum.Delivered|| mu.OrderHeaderProxy.Status == orderStatusEnum.InProcess)).Sum(u=>(decimal?) u.LineTotal)??0;
            ViewBag.New = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.CreatedOn.Month==currentTime.Month&&mu.CreatedOn.Year==currentTime.Year).Count();

            List<OrdersVM> model = new List<OrdersVM>();
            if (flag == null)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status != orderStatusEnum.NeedApproval).Select(u => new OrdersVM
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
                    Date=u.CreatedOn
                }).ToList();
            }
            else if (flag == 1)
            {
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status== orderStatusEnum.Pening).Select(u => new OrdersVM
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
                model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Type== orderType.Online).Select(u => new OrdersVM
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
        public ActionResult PurchaseOrders()
        {
            var loginUser = getLoginUser();
            var units = db.PurchaseOrderHeader.Where(mu => mu.CreatedByUserId == loginUser.Id).Select(u=> new OrdersVM {
                DeliveryNote=u.Description,
                Date=u.CreatedOn,
                Id=u.Id,
                Email=u.SupplierName,
                PhoneNumber=u.PhoneNumber,
                Lines=db.PurchaseOrderLine.Where(or=>or.PurchaseOrderHeaderId == u.Id).Select(n=>new OrderLineVM {
                    Id=n.Id,
                    LineTotal=n.Total,
                    Price=n.Price,
                    ProductId=n.ProductId,
                    ProductName=n.ProductProxy.Name,
                    Qty=n.Qty
                }).ToList(),
                OrderNumber=u.OrderNumber,
                TotalBill=db.PurchaseOrderLine.Where(or => or.PurchaseOrderHeaderId == u.Id).Sum(n=> (decimal?)n.Total)??0,
            }).ToList();
            return View(units);
        }
        public ActionResult DeletePurchaseOrder(int orderId)
        {
            var order = db.PurchaseOrderHeader.Find(orderId);
            if (order == null)
            {
                var Res = new
                {
                    Status = false,
                    Data = "Order not found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var lines = db.PurchaseOrderLine.Where(u => u.PurchaseOrderHeaderId == orderId).ToList();
                foreach(var line in lines)
                {
                    var prod = db.StoreProducts.Find(line.ProductId);
                    prod.InStock = prod.InStock - line.Qty;
                    db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
                    db.PurchaseOrderLine.Remove(line);
                }
                db.StoreQtyMovements.RemoveRange(db.StoreQtyMovements.Where(u => u.PurchaseOrderHeaderId == orderId).ToList());
                db.PurchaseOrderHeader.Remove(order);
                db.SaveChanges();
                var Res = new
                {
                    Status = true,
                    Data = "Order not found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteSaleOrder(int orderId)
        {
            var order = db.OrderHeader.Find(orderId);
            if (order == null)
            {
                var Res = new
                {
                    Status = false,
                    Data = "Order not found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var lines = db.OrderLine.Where(u => u.OrderHeaderId == orderId).ToList();
                foreach (var line in lines)
                {
                    var prod = db.StoreProducts.Find(line.ProductId);
                    prod.InStock = prod.InStock + line.Qty;
                    db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
                    db.OrderLine.Remove(line);
                }
                db.StoreQtyMovements.RemoveRange(db.StoreQtyMovements.Where(u => u.OrderHeaderId == orderId).ToList());
                db.OrderHeader.Remove(order);
                db.SaveChanges();
                var Res = new
                {
                    Status = true,
                    Data = "Order not found"
                };
                return Json(Res, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetProductDetail(int ProductId) {
            var pro = db.StoreProducts.Find(ProductId);
            if (pro == null)
            {
                var Res = new
                {
                    Status = false,
                    Data = "Not found"
                };
                return Json(Res,JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Res = new
                {
                    Status = true,
                    Data = new
                    {
                        ProductId=pro.Id,
                        Price=pro.SalePrice,
                        ProductName=pro.Name,
                        Unit=pro.StoreMeasurementUnitProxy.Name,
                        Qty=pro.InStock,
                        Cost=pro.Cost
                    }
                };
                return Json(Res,JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetPurchaseOrderLineDetail(int Id) {
            var pro = db.PurchaseOrderLine.Where(p=>p.PurchaseOrderHeaderId==Id).Select(u=>new OrderLineVM{
                Id=u.Id,
                LineTotal=u.Total,
                Price=u.Price,
                ProductName=u.ProductProxy.Name,
                Qty=u.Qty,
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

        /// <summary>
        /// //////////////////////////////////////Order Operation
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult InprocessOrder(int IOrderId, string userID)
        {
            var loginUser = getLoginUser();

            var pro = db.OrderHeader.Where(p => p.Id == IOrderId && p.OrderForUserId == loginUser.Id).FirstOrDefault();

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
                if(pro.Status== orderStatusEnum.Pening)
                {
                    pro.Status = orderStatusEnum.InProcess;
                    pro.UpdatedAt = currentTime;
                    pro.OrderAssignedToUserId = userID;
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
            var pro = db.OrderHeader.Where(p => p.Id == OrderId&&p.OrderForUserId==loginUser.Id).FirstOrDefault();

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
                    foreach(var line in db.OrderLine.Where(u => u.OrderHeaderId == pro.Id).ToList())
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
            var pro = db.OrderHeader.Where(p => p.Id == OrderId && p.OrderForUserId == loginUser.Id).FirstOrDefault();

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

            var pro = db.OrderHeader.Where(p => p.Id == Id && p.OrderForUserId == loginUser.Id).FirstOrDefault();

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

            var pro = db.OrderHeader.Where(p => p.Id == Id && p.OrderForUserId == loginUser.Id).FirstOrDefault();

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
        /// <summary>
        /// //////////////////////////Private
        /// </summary>
        /// <param name="file"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        /// 
        public ActionResult GetUserApi()
        {
            var loginUser = getLoginUser();
            var models = db.Users.Where(prd => prd.CreatedByUserId == loginUser.Id && prd.Status == UserStatus.Active && prd.UserType == UserTypes.DeliveryTeam).Select(u => new ProductsVM
            {
                Name = u.UserName,
                Description = u.Id
            }).ToList();
            return Json(models, JsonRequestBehavior.AllowGet);
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
        public ActionResult PrintInvoice(int orderID)
        {
            var orderHeader = db.OrderHeader.Find(orderID);

            ViewBag.lines = db.OrderLine.Where(or => or.OrderHeaderId == orderID).Select(u => new InvoiceLine
            {
                ProductImageUrl = u.ProductProxy.FeatureImageUrl,
                CategoryName = u.ProductProxy.CompanyProxy.Name,
                LineTotal = u.LineTotal,
                Qty = u.Qty,
                Price = u.Price,
                ProductName = u.ProductProxy.Name,
                Tax = u.ProductProxy.Tax ?? 0,
                Discount = u.ProductProxy.Discount ?? 0,
                ActuallPrice = u.ProductProxy.ActualPrice,
            }).ToList();
            ViewBag.forUser = db.Users.Where(yu => yu.Id == orderHeader.OrderForUserId).Select(u => new UserProfileDtos
            {
                Address = u.LocationProxy.AddressLine,
                Email = u.Email,
                Username = u.ProfileName,
                PhoneNumber = u.PhoneNumber,
                DeviceFCM = u.LocationProxy.State,
                DeviceNumber = u.LocationProxy.City,
            }).FirstOrDefault();

            ViewBag.byUser = db.Branch.Where(yu => yu.Id == orderHeader.BranchId).Select(u => new UserProfileDtos
            {
                Address = u.LocationProxy.AddressLine,
                Username = u.Name,
                PhoneNumber = u.PhoneNumber,
                Email = u.AssignedToProxy != null ? u.AssignedToProxy.Email : null,
                DeviceFCM = u.LocationProxy.State,
                DeviceNumber = u.LocationProxy.City,
            }).FirstOrDefault();

            ViewBag.orderNumber = orderHeader.OrderNumber;
            ViewBag.note = orderHeader.DeliveryNote;
            ViewBag.doption = orderHeader.DeliveryOption;
            ViewBag.deliveryTime = orderHeader.CustomDeliveryTime ?? null;
            ViewBag.date = orderHeader.CreatedOn;
            return View();
        }

    }
}