using B2B.Areas.Api.Dtos;
using B2B.Areas.Merchant.ViewModel;
using B2B.Areas.Supermarket.ViewModel;
using B2B.DAL;
using B2B.Models;
using B2B.Models.Orders;
using B2B.Models.Store;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Supervisor.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Supermarket/Orders
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
        public ActionResult ManageOrders(int? flag)
        {
            var loginUser = getLoginUser();
            ViewBag.Total = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status != orderStatusEnum.NeedApproval).Count();
            ViewBag.Pending = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.Pening).Count();
            ViewBag.Inprocess = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.InProcess).Count();
            ViewBag.Enroute = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.Enroute).Count();
            ViewBag.Delivered = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered).Count();
            ViewBag.Cancelled = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.Canceled).Count();
            ViewBag.Rejected = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.Rejected).Count();

            List<OrdersVM> model = new List<OrdersVM>();
            if (flag == null)
            {
                model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status != orderStatusEnum.NeedApproval).Select(u => new OrdersVM
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
                model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.Pening).Select(u => new OrdersVM
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
                model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.InProcess).Select(u => new OrdersVM
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
                model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.Enroute).Select(u => new OrdersVM
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
                model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered).Select(u => new OrdersVM
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
                model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.Canceled).Select(u => new OrdersVM
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
                model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.Rejected).Select(u => new OrdersVM
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
        public ActionResult AuthOrders()
        {
            var loginUser = getLoginUser();

            List<OrdersVM> model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && mu.Status == orderStatusEnum.NeedApproval).Select(u => new OrdersVM
            {
                Status = u.Status,
                ShippingCharges = u.ShippingCharges,
                PaymentStatus = u.PaymentStatus,
                CanceledComment = u.CanceledComment,
                CustomerName = u.OrderCreatedByUserProxy.UserID,
                CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
                DeliveryNote = u.DeliveryNote,
                DeliveryOption = u.DeliveryOption,
                Email = u.OrderCreatedByUserProxy.Email,
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
                PhoneNumber = u.OrderCreatedByUserProxy.PhoneNumber,
                RejectionComment = u.RejectionComment,
                TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                Type = u.Type,
                Date = u.CreatedOn
            }).ToList();
            return View(model);
        }
        public ActionResult Add(int? categoryId, decimal? minPrice, decimal? maxPrice, string search)
        {
            ViewBag.categoryId = new SelectList(db.StoreCategory.ToList(), "Id", "Name");
            if (search == "")
            {
                search = null;
            }
            if (categoryId == null && search == null && (minPrice == null || maxPrice == null))
            {
                var models = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active && prd.InStock > 0).Select(u => new MarketProductDtos
                {
                    SalePrice = u.SalePrice,
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
                    OnSale = u.Discount == null ? false : true,
                    CategoryName = u.CategoryProxy.Name,
                    InStock = u.InStock,
                    TotalStock = db.StoreQtyMovements.Where(pr => pr.ProductId == u.Id).Sum(pr => (decimal?)pr.InQty) ?? 0,
                    IsNew = (u.CreatedOn.Month == currentTime.Month && u.CreatedOn.Year == currentTime.Year) ? true : false,
                }).ToList();
                return View(models);
            }
            else if (categoryId != null && search == null && (minPrice == null || maxPrice == null))
            {
                var models = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active && prd.InStock > 0 && prd.CategoryId == categoryId).Select(u => new MarketProductDtos
                {
                    SalePrice = u.SalePrice,
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
                    OnSale = u.Discount == null ? false : true,
                    CategoryName = u.CategoryProxy.Name,
                    InStock = u.InStock,
                    TotalStock = db.StoreQtyMovements.Where(pr => pr.ProductId == u.Id).Sum(pr => (decimal?)pr.InQty) ?? 0,
                    IsNew = (u.CreatedOn.Month == currentTime.Month && u.CreatedOn.Year == currentTime.Year) ? true : false
                }).ToList();
                return View(models);
            }
            else if (categoryId != null && search != null && search != "" && (minPrice == null || maxPrice == null))
            {
                var models = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active && prd.InStock > 0 && prd.CategoryId == categoryId && prd.Name.ToLower().Contains(search.ToLower())).Select(u => new MarketProductDtos
                {
                    SalePrice = u.SalePrice,
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
                    OnSale = u.Discount == null ? false : true,
                    CategoryName = u.CategoryProxy.Name,
                    InStock = u.InStock,
                    TotalStock = db.StoreQtyMovements.Where(pr => pr.ProductId == u.Id).Sum(pr => (decimal?)pr.InQty) ?? 0,
                    IsNew = (u.CreatedOn.Month == currentTime.Month && u.CreatedOn.Year == currentTime.Year) ? true : false
                }).ToList();
                return View(models);
            }
            else if (categoryId != null && search != null && minPrice != null && maxPrice != null)
            {
                var models = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active && prd.InStock > 0 && prd.CategoryId == categoryId && prd.Name.ToLower().Contains(search.ToLower()) && prd.SalePrice >= minPrice && prd.SalePrice <= maxPrice).Select(u => new MarketProductDtos
                {
                    SalePrice = u.SalePrice,
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
                    OnSale = u.Discount == null ? false : true,
                    CategoryName = u.CategoryProxy.Name,
                    InStock = u.InStock,
                    TotalStock = db.StoreQtyMovements.Where(pr => pr.ProductId == u.Id).Sum(pr => (decimal?)pr.InQty) ?? 0,
                    IsNew = (u.CreatedOn.Month == currentTime.Month && u.CreatedOn.Year == currentTime.Year) ? true : false
                }).ToList();
                return View(models);
            }
            else if (categoryId != null && search == null && minPrice != null && maxPrice != null)
            {
                var models = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active && prd.InStock > 0 && prd.SalePrice >= minPrice && prd.SalePrice <= maxPrice).Select(u => new MarketProductDtos
                {
                    SalePrice = u.SalePrice,
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
                    OnSale = u.Discount == null ? false : true,
                    CategoryName = u.CategoryProxy.Name,
                    InStock = u.InStock,
                    TotalStock = db.StoreQtyMovements.Where(pr => pr.ProductId == u.Id).Sum(pr => (decimal?)pr.InQty) ?? 0,
                    IsNew = (u.CreatedOn.Month == currentTime.Month && u.CreatedOn.Year == currentTime.Year) ? true : false
                }).ToList();
                return View(models);
            }
            else
            {
                var models = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active && prd.InStock > 0).Select(u => new MarketProductDtos
                {
                    SalePrice = u.SalePrice,
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
                    OnSale = u.Discount == null ? false : true,
                    CategoryName = u.CategoryProxy.Name,
                    InStock = u.InStock,
                    TotalStock = db.StoreQtyMovements.Where(pr => pr.ProductId == u.Id).Sum(pr => (decimal?)pr.InQty) ?? 0,
                    IsNew = (u.CreatedOn.Month == currentTime.Month && u.CreatedOn.Year == currentTime.Year) ? true : false
                }).ToList();
                return View(models);
            }
        }
        public ActionResult ViewCarts()
        {
            List<CartSessionHeader> header = new List<CartSessionHeader>();

            if (Session["cartHeaders"] != null)
            {
                header = (List<CartSessionHeader>)Session["cartHeaders"];
            }

            return View(header);
        }
        [HttpPost]
        public JsonResult addtocart(int productId, int qty)
        {
            var product = db.StoreProducts.Find(productId);
            if (product == null)
            {
                var response = new
                {
                    code = 400,
                    msg = "product not found!"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (Session["cartHeaders"] == null && Session["cartLines"] == null)
                {
                    CartSessionLine line = new CartSessionLine
                    {
                        ProductName = product.Name,
                        LineId = product.Id,
                        LineTotal = product.SalePrice * qty,
                        MeasurementUnit = product.StoreMeasurementUnitProxy.Name,
                        Price = product.SalePrice,
                        ProductImageUrl = product.FeatureImageUrl,
                        Qty = qty,
                        CategoryName = product.CategoryProxy.Name,
                        HeaderId = product.UserId,
                    };
                    CartSessionHeader header = new CartSessionHeader
                    {
                        Id = product.UserId,
                        Count = 1,
                        Email = product.UserProxy.Email,
                        ImageUrl = product.UserProxy.ProfileImageUrl,
                        Name = product.UserProxy.ProfileName
                    };
                    List<CartSessionLine> lines = new List<CartSessionLine>();
                    List<CartSessionHeader> headers = new List<CartSessionHeader>();
                    lines.Add(line);
                    headers.Add(header);
                    Session["cartLines"] = lines;
                    Session["cartHeaders"] = headers;
                    Session["count"] = headers.Count();
                }
                else
                {
                    List<CartSessionLine> lines = (List<CartSessionLine>)Session["cartLines"];
                    List<CartSessionHeader> headers = (List<CartSessionHeader>)Session["cartHeaders"];
                    var tempLine = lines.Where(k => k.LineId == productId).FirstOrDefault();
                    var headerLine = headers.Where(k => k.Id == product.UserId).FirstOrDefault();
                    if (tempLine != null)
                    {
                        CartSessionLine Newline = new CartSessionLine
                        {
                            ProductName = product.Name,
                            LineId = product.Id,
                            LineTotal = product.SalePrice * (tempLine.Qty + qty),
                            MeasurementUnit = product.StoreMeasurementUnitProxy.Name,
                            Price = product.SalePrice,
                            ProductImageUrl = product.FeatureImageUrl,
                            Qty = tempLine.Qty + qty,
                            CategoryName = product.CategoryProxy.Name,
                            HeaderId = product.UserId,
                        };
                        lines.Remove(tempLine);
                        lines.Add(Newline);
                    }
                    else
                    {
                        CartSessionLine Newline = new CartSessionLine
                        {
                            ProductName = product.Name,
                            LineId = product.Id,
                            LineTotal = product.SalePrice * qty,
                            MeasurementUnit = product.StoreMeasurementUnitProxy.Name,
                            Price = product.SalePrice,
                            ProductImageUrl = product.FeatureImageUrl,
                            Qty = qty,
                            CategoryName = product.CategoryProxy.Name,
                            HeaderId = product.UserId,
                        };
                        lines.Add(Newline);
                    }
                    if (headerLine == null)
                    {
                        CartSessionHeader header = new CartSessionHeader
                        {
                            Id = product.UserId,
                            Count = 1,
                            Email = product.UserProxy.Email,
                            ImageUrl = product.UserProxy.ProfileImageUrl,
                            Name = product.UserProxy.ProfileName
                        };
                        headers.Add(header);
                    }
                    else
                    {
                        headerLine.Count = headers.Count + 1;
                        headers.Remove(headerLine);
                        headers.Add(headerLine);
                    }
                    Session["cartLines"] = lines;
                    Session["cartHeaders"] = headers;
                    Session["count"] = headers.Count();
                }
                var response = new
                {
                    code = 200,
                    msg = "successfully added."
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    code = 400,
                    msg = ex.GetBaseException().Message
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult buyNow(int bproductId, int bqty)
        {
            var product = db.StoreProducts.Find(bproductId);
            if (product == null)
            {
                var response = new
                {
                    code = 400,
                    msg = "product not found!"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (Session["cartHeaders"] == null && Session["cartLines"] == null)
                {
                    CartSessionLine line = new CartSessionLine
                    {
                        ProductName = product.Name,
                        LineId = product.Id,
                        LineTotal = product.SalePrice * bqty,
                        MeasurementUnit = product.StoreMeasurementUnitProxy.Name,
                        Price = product.SalePrice,
                        ProductImageUrl = product.FeatureImageUrl,
                        Qty = bqty,
                        CategoryName = product.CategoryProxy.Name,
                        HeaderId = product.UserId,
                    };
                    CartSessionHeader header = new CartSessionHeader
                    {
                        Id = product.UserId,
                        Count = 1,
                        Email = product.UserProxy.Email,
                        ImageUrl = product.UserProxy.ProfileImageUrl,
                        Name = product.UserProxy.ProfileName
                    };
                    List<CartSessionLine> lines = new List<CartSessionLine>();
                    List<CartSessionHeader> headers = new List<CartSessionHeader>();
                    lines.Add(line);
                    headers.Add(header);
                    Session["cartLines"] = lines;
                    Session["cartHeaders"] = headers;
                    Session["count"] = headers.Count();
                }
                else
                {
                    List<CartSessionLine> lines = (List<CartSessionLine>)Session["cartLines"];
                    List<CartSessionHeader> headers = (List<CartSessionHeader>)Session["cartHeaders"];
                    var tempLine = lines.Where(k => k.LineId == bproductId).FirstOrDefault();
                    var headerLine = headers.Where(k => k.Id == product.UserId).FirstOrDefault();
                    if (tempLine != null)
                    {
                        CartSessionLine Newline = new CartSessionLine
                        {
                            ProductName = product.Name,
                            LineId = product.Id,
                            LineTotal = product.SalePrice * (tempLine.Qty + bqty),
                            MeasurementUnit = product.StoreMeasurementUnitProxy.Name,
                            Price = product.SalePrice,
                            ProductImageUrl = product.FeatureImageUrl,
                            Qty = tempLine.Qty + bqty,
                            CategoryName = product.CategoryProxy.Name,
                            HeaderId = product.UserId,
                        };
                        lines.Remove(tempLine);
                        lines.Add(Newline);
                    }
                    else
                    {
                        CartSessionLine Newline = new CartSessionLine
                        {
                            ProductName = product.Name,
                            LineId = product.Id,
                            LineTotal = product.SalePrice * bqty,
                            MeasurementUnit = product.StoreMeasurementUnitProxy.Name,
                            Price = product.SalePrice,
                            ProductImageUrl = product.FeatureImageUrl,
                            Qty = bqty,
                            CategoryName = product.CategoryProxy.Name,
                            HeaderId = product.UserId,
                        };
                        lines.Add(Newline);
                    }
                    if (headerLine == null)
                    {
                        CartSessionHeader header = new CartSessionHeader
                        {
                            Id = product.UserId,
                            Count = 1,
                            Email = product.UserProxy.Email,
                            ImageUrl = product.UserProxy.ProfileImageUrl,
                            Name = product.UserProxy.ProfileName
                        };
                        headers.Add(header);
                    }
                    else
                    {
                        headerLine.Count = headers.Count + 1;
                        headers.Remove(headerLine);
                        headers.Add(headerLine);
                    }
                    Session["cartLines"] = lines;
                    Session["cartHeaders"] = headers;
                    Session["count"] = headers.Count();
                }
                var response = new
                {
                    code = 200,
                    msg = product.UserId
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    code = 400,
                    msg = ex.GetBaseException().Message
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Remove(int productId)
        {
            try
            {
                var product = db.StoreProducts.Find(productId);
                if (Session["cartLines"] != null)
                {
                    List<CartSessionLine> lines = (List<CartSessionLine>)Session["cartLines"];
                    List<CartSessionHeader> headers = (List<CartSessionHeader>)Session["cartHeaders"];
                    var TempHeader = headers.Where(k => k.Id == product.UserId).FirstOrDefault();

                    lines.RemoveAll(x => x.LineId == productId);
                    Session["cartLines"] = lines;
                    if (lines.Where(h => h.HeaderId == product.UserId).FirstOrDefault() == null)
                    {
                        headers.RemoveAll(x => x.Id == product.UserId);
                    }
                    else
                    {
                        TempHeader.Count = TempHeader.Count - 1;
                        headers.Remove(TempHeader);
                        headers.Add(TempHeader);
                    }
                    Session["cartHeaders"] = headers;
                    Session["count"] = headers.Count();
                    var response = new
                    {
                        code = 200,
                        msg = "successfully removed."
                    };
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new
                    {
                        code = 200,
                        msg = "already removed."
                    };
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    code = 200,
                    msg = ex.GetBaseException().Message
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult ViewCart(string id)
        {
            ViewBag.CartId = id;
            var loginUser = getLoginUser();
            if (loginUser.UserType == UserTypes.Supervisor)
            {
                ViewBag.BranchId = new SelectList(db.Branch.Where(b => b.IsDeleted == false && b.UserId == loginUser.CreatedByUserId).ToList(), "Id", "Name");
            }
            else
            {
                ViewBag.BranchId = new SelectList(db.Branch.Where(b => b.IsDeleted == false && b.UserId == loginUser.Id).ToList(), "Id", "Name");
            }
            if (Session["cartLines"] == null)
            {
                List<CartSessionLine> lines = new List<CartSessionLine>();
                ViewBag.Name = "";
                ViewBag.Email = "";
                ViewBag.TotalBill = 0;
                return View(lines);
            }
            else
            {
                List<CartSessionLine> li = (List<CartSessionLine>)Session["cartLines"];
                if (Session["cartHeaders"] != null)
                {
                    List<CartSessionHeader> headers = (List<CartSessionHeader>)Session["cartHeaders"];
                    var tempHeader = headers.Where(hr => hr.Id == id).FirstOrDefault();
                    if (tempHeader != null)
                    {
                        ViewBag.Name = tempHeader.Name;
                        ViewBag.Email = tempHeader.Email;
                        ViewBag.TotalBill = li.Sum(k => (decimal?)k.LineTotal) ?? 0;
                    }
                    else
                    {
                        ViewBag.Name = "";
                        ViewBag.Email = "";
                        ViewBag.TotalBill = 0;

                    }
                    return View(li.Where(i => i.HeaderId == id).ToList());
                }
                return View(li.Where(i => i.HeaderId == id).ToList());

            }
        }
        public JsonResult Addqtyincart(int productId, decimal qty)
        {
            if (qty <= 0)
            {
                var response = new
                {
                    code = 400,
                    msg = "invalid qty! qty must be greater than zero."
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (Session["cartLines"] == null)
            {
                var response = new
                {
                    code = 400,
                    msg = "no cart found!"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<CartSessionLine> lines = (List<CartSessionLine>)Session["cartLines"];
                var tempLine = lines.Where(k => k.LineId == productId).FirstOrDefault();
                if (tempLine != null)
                {
                    tempLine.Qty = qty;
                    tempLine.LineTotal = qty * tempLine.Price;
                    lines.Remove(tempLine);
                    lines.Add(tempLine);

                }
                else
                {
                    var errResponse = new
                    {
                        code = 400,
                        msg = "no product found!"
                    };
                    return Json(errResponse, JsonRequestBehavior.AllowGet);
                }
                Session["cartLines"] = lines;
                var response = new
                {
                    code = 200,
                    msg = "already removed."
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ConfirmOrder(string CartId, paymentMethodEnum PaymentMethod, string DeliveryNote, int BranchId, bool? withCustomDelivery, DateTime? CustomDeliveryTime)
        {
            try
            {
                if (withCustomDelivery == null)
                {
                    withCustomDelivery = false;
                }
                if (Session["cartLines"] == null || Session["cartHeaders"] == null)
                {
                    var ErrorResponse = new
                    {
                        code = 400,
                        msg = "no cart found!"
                    };
                    return Json(ErrorResponse, JsonRequestBehavior.AllowGet);
                }
                var loginUser = getLoginUser();
                List<CartSessionLine> lines = (List<CartSessionLine>)Session["cartLines"];
                List<CartSessionHeader> headers = (List<CartSessionHeader>)Session["cartHeaders"];
                var tempLines = lines.Where(k => k.HeaderId == CartId).ToList();
                var headerLine = headers.Where(k => k.Id == CartId).FirstOrDefault();
                if (tempLines == null || headerLine == null)
                {
                    var ErrorResponse = new
                    {
                        code = 400,
                        msg = "no cart found!"
                    };
                    return Json(ErrorResponse, JsonRequestBehavior.AllowGet);
                }
                var Branch = db.Branch.Find(BranchId);
                Random random = new Random();
                int orderNumber = random.Next();
                OrderHeader header = new OrderHeader
                {
                    CreatedOn = currentTime,
                    ShippingCharges = 0.00m,
                    Status = orderStatusEnum.Pening,
                    PaymentStatus = PaymentMethod,
                    DeliveryOption = deliveryType.MerchantDelivery,
                    Email = loginUser.Email,
                    PhoneNumber = Branch.PhoneNumber,
                    Type = orderType.Online,
                    OrderForUserId = CartId,
                    OrderNumber = "ORD-" + orderNumber,
                    DeliveryNote = DeliveryNote,
                    OrderCreatedByUserId = loginUser.Id,
                    BranchId = Branch.Id,
                    WithCustomDelivery = withCustomDelivery.Value,
                    CustomDeliveryTime = CustomDeliveryTime
                };
                if (loginUser.UserType == UserTypes.Supervisor)
                {
                    var supermarket = db.Users.Find(loginUser.CreatedByUserId);
                    if (supermarket.OrderAuthenticationEnabled)
                    {
                        header.Status = orderStatusEnum.NeedApproval;
                    }
                    else
                    {
                        header.Status = orderStatusEnum.Pening;
                    }
                    header.OrderByUserId = supermarket.Id;
                }
                else
                {
                    header.OrderByUserId = loginUser.Id;
                }
                foreach (var line in tempLines)
                {

                    var Pro = db.StoreProducts.Find(line.LineId);
                    decimal LineTotal = line.Qty * Pro.SalePrice;
                    OrderLine Templine = new OrderLine
                    {
                        LineTotal = LineTotal,
                        OrderHeaderProxy = header,
                        Price = Pro.SalePrice,
                        ProductId = line.LineId,
                        Qty = line.Qty,
                    };
                    db.OrderLine.Add(Templine);
                    Store_Qty_Movement movement = new Store_Qty_Movement
                    {
                        Type = transactionType.Sale,
                        Cost = Pro.Cost,
                        Price = Pro.SalePrice,
                        Total = LineTotal,
                        OutQty = line.Qty,
                        InQty = 0,
                        OrderHeaderProxy = header,
                        ProductId = Pro.Id,
                        CreatedOn = currentTime,
                    };
                    db.StoreQtyMovements.Add(movement);
                    Pro.InStock = Pro.InStock - line.Qty;
                    db.Entry(Pro).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                lines.RemoveAll(li => li.HeaderId == CartId);
                headers.RemoveAll(li => li.Id == CartId);
                Session["cartLines"] = lines;
                Session["cartHeaders"] = headers;
                var response = new
                {
                    code = 200,
                    msg = header.Id
                };
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                var response = new
                {
                    code = 400,
                    msg = BaseMessage
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PrintInvoice(int orderID)
        {
            var orderHeader = db.OrderHeader.Find(orderID);

            ViewBag.lines = db.OrderLine.Where(or => or.OrderHeaderId == orderID).Select(u => new InvoiceLine
            {
                ProductImageUrl = u.ProductProxy.FeatureImageUrl,
                CategoryName = u.ProductProxy.CategoryProxy.Name,
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
        public ActionResult OrdersReport(DateTime? startDate, DateTime? endDate)
        {
            var loginUser = getLoginUser();
            ViewBag.supervisorId = new SelectList(db.Users.Where(u => u.CreatedByUserId == loginUser.Id && u.UserType == UserTypes.Supervisor).ToList(), "Id", "ProfileName");

            if (startDate == null || endDate == null)
            {
                List<OrdersVM> model = new List<OrdersVM>();
                return View(model);
            }
            else
            {
                List<OrdersVM> model = db.OrderHeader.Where(mu => mu.OrderCreatedByUserId == loginUser.Id && DbFunctions.TruncateTime(mu.CreatedOn) <= endDate && DbFunctions.TruncateTime(mu.CreatedOn) >= startDate).Select(u => new OrdersVM
                {
                    Status = u.Status,
                    ShippingCharges = u.ShippingCharges,
                    PaymentStatus = u.PaymentStatus,
                    CanceledComment = u.CanceledComment,
                    CustomerName = u.OrderCreatedByUserProxy.UserID,
                    CustomerImage = u.OrderByUserProxy != null ? u.OrderByUserProxy.ProfileImageUrl : "",
                    DeliveryNote = u.DeliveryNote,
                    DeliveryOption = u.DeliveryOption,
                    Email = u.OrderCreatedByUserProxy.Email,
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
                    PhoneNumber = u.OrderCreatedByUserProxy.PhoneNumber,
                    RejectionComment = u.RejectionComment,
                    TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                    Type = u.Type,
                    Date = u.CreatedOn
                }).ToList();
                return View(model);
            }
        }
    }
}