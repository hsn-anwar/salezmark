using B2B.Areas.Merchant.ViewModel;
using B2B.DAL;
using B2B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Admin.Controllers
{
    public class MarketplaceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        DateTime currentTime = DateTime.Now;
        // GET: Admin/Marketplace
        public ActionResult Index(int? flag)
        {
            ViewBag.Total = db.StoreProducts.Count();
            ViewBag.Active = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active).Count();
            ViewBag.Deleted = db.StoreProducts.Where(prd => prd.Status == UserStatus.Deleted).Count();
            ViewBag.Block = db.StoreProducts.Where(prd => prd.Status == UserStatus.Blocked).Count();
            ViewBag.New = db.StoreProducts.Where(prd => prd.Status == UserStatus.Blocked&&prd.CreatedOn.Month==currentTime.Month&&prd.CreatedOn.Year==currentTime.Year).Count();
            ViewBag.Complain = 0;
            List<ProductsVM> models = new List<ProductsVM>();
            if (flag == null)
            {
                models = db.StoreProducts.Select(u => new ProductsVM
                {
                    SalePrice = u.SalePrice,
                    Specification = u.UserProxy.ProfileImageUrl,
                    Description = u.UserProxy.UserName,
                    ActualPrice = u.ActualPrice,
                    Cost = u.Cost,
                    Discount = u.Discount,
                    Name = u.Name,
                    FeatureImageUrl = u.FeatureImageUrl,
                    Tax = u.Tax,
                    StoreMeasurementUnitName = u.StoreMeasurementUnitProxy.Name,
                    Date = u.CreatedOn,
                    Id = u.Id,
                    userStatus = u.Status
                }).ToList();

            }
            //Active
            else if (flag == 1)
            {
                models = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active).Select(u => new ProductsVM
                {
                    SalePrice = u.SalePrice,
                    Specification = u.UserProxy.ProfileImageUrl,
                    Description = u.UserProxy.UserName,
                    ActualPrice = u.ActualPrice,
                    Cost = u.Cost,
                    Discount = u.Discount,
                    Name = u.Name,
                    FeatureImageUrl = u.FeatureImageUrl,
                    Tax = u.Tax,
                    StoreMeasurementUnitName = u.StoreMeasurementUnitProxy.Name,
                    Date = u.CreatedOn,
                    Id = u.Id,
                    userStatus = u.Status
                }).ToList();
            }
            //Deleted
            else if (flag == 2)
            {
                models = db.StoreProducts.Where(prd => prd.Status == UserStatus.Deleted).Select(u => new ProductsVM
                {
                    SalePrice = u.SalePrice,
                    Specification = u.UserProxy.ProfileImageUrl,
                    Description = u.UserProxy.UserName,
                    ActualPrice = u.ActualPrice,
                    Cost = u.Cost,
                    Discount = u.Discount,
                    Name = u.Name,
                    FeatureImageUrl = u.FeatureImageUrl,
                    Tax = u.Tax,
                    StoreMeasurementUnitName = u.StoreMeasurementUnitProxy.Name,
                    Date = u.CreatedOn,
                    Id = u.Id,
                    userStatus = u.Status
                }).ToList();
            }
            //Blocked
            else if (flag == 3)
            {
                models = db.StoreProducts.Where(prd => prd.Status == UserStatus.Blocked).Select(u => new ProductsVM
                {
                    SalePrice = u.SalePrice,
                    Specification = u.UserProxy.ProfileImageUrl,
                    Description = u.UserProxy.UserName,
                    ActualPrice = u.ActualPrice,
                    Cost = u.Cost,
                    Discount = u.Discount,
                    Name = u.Name,
                    FeatureImageUrl = u.FeatureImageUrl,
                    Tax = u.Tax,
                    StoreMeasurementUnitName = u.StoreMeasurementUnitProxy.Name,
                    Date = u.CreatedOn,
                    Id = u.Id,
                    userStatus = u.Status
                }).ToList();
            }
            //New
            else if (flag == 4)
            {
                models = db.StoreProducts.Where(prd => prd.Status == UserStatus.Blocked&& prd.CreatedOn.Month == currentTime.Month && prd.CreatedOn.Year == currentTime.Year).Select(u => new ProductsVM
                {
                    SalePrice = u.SalePrice,
                    Specification = u.UserProxy.ProfileImageUrl,
                    Description = u.UserProxy.UserName,
                    ActualPrice = u.ActualPrice,
                    Cost = u.Cost,
                    Discount = u.Discount,
                    Name = u.Name,
                    FeatureImageUrl = u.FeatureImageUrl,
                    Tax = u.Tax,
                    StoreMeasurementUnitName = u.StoreMeasurementUnitProxy.Name,
                    Date = u.CreatedOn,
                    Id = u.Id,
                    userStatus = u.Status
                }).ToList();
            }
            return View(models);
        }
        public ActionResult Orders(int? flag)
        {
            ViewBag.Total = db.OrderHeader.Count();
            ViewBag.Pending = db.OrderHeader.Where(mu => mu.Status == orderStatusEnum.Pening).Count();
            ViewBag.Inprocess = db.OrderHeader.Where(mu => mu.Status == orderStatusEnum.InProcess).Count();
            ViewBag.Cancelled = db.OrderHeader.Where(mu => mu.Status == orderStatusEnum.Canceled).Count();
            ViewBag.Delivered = db.OrderHeader.Where(mu => mu.Status == orderStatusEnum.Delivered).Count();
            ViewBag.New = db.OrderHeader.Where(mu => mu.CreatedOn.Month == currentTime.Month && mu.CreatedOn.Year == currentTime.Year).Count();
            List<OrdersVM> model = new List<OrdersVM>();
            //Total
            if (flag == null)
            {
                model = db.OrderHeader.Select(u => new OrdersVM
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
            //Pending
            else if (flag == 1)
            {
                model = db.OrderHeader.Where(mu => mu.Status == orderStatusEnum.Pening).Select(u => new OrdersVM
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
            //Inprocess
            else if (flag == 2)
            {
                model = db.OrderHeader.Where(mu => mu.Status == orderStatusEnum.InProcess).Select(u => new OrdersVM
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
            else if (flag == 4)
            {
                model = db.OrderHeader.Where(mu => mu.Status == orderStatusEnum.Delivered).Select(u => new OrdersVM
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
            else if (flag == 3)
            {
                model = db.OrderHeader.Where(mu =>mu.Status == orderStatusEnum.Canceled).Select(u => new OrdersVM
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
                model = db.OrderHeader.Where(mu => mu.CreatedOn.Month == currentTime.Month && mu.CreatedOn.Year == currentTime.Year).Select(u => new OrdersVM
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
        public ActionResult Inquiries()
        {
            return View();
        }
        public ActionResult ChangeProductState(int Id)
        {
            var pro = db.StoreProducts.Find(Id);
            if(pro.Status== UserStatus.Active)
            {
                pro.Status = UserStatus.Blocked;
            }
            else if(pro.Status== UserStatus.Blocked)
            {
                pro.Status = UserStatus.Active;
            }
            db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var Res = new
            {
                Status = true,
                Msg = "OK"
            };

            return Json(Res, JsonRequestBehavior.AllowGet);
        }
    }
}