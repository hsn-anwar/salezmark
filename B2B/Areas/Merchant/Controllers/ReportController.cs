using B2B.Areas.Merchant.ViewModel;
using B2B.DAL;
using B2B.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Merchant.Controllers
{
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        DateTime currentTime = DateTime.Now;
        // GET: Merchant/Report
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
        public ActionResult Generator(int flag)
        {
            ViewBag.flag = flag;
            return View();
        }
        /// <summary>
        /// 1
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public ActionResult LowStock()
        {
            var loginUser = getLoginUser();
            var models = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id && prd.Status == UserStatus.Active && prd.InStock<= prd.LowStock).Select(u => new ProductsVM
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
                userStatus = u.Status,
                LowStock=u.LowStock,
                InStock=u.InStock,
            }).ToList();
            return View(models);
        }
        /// <summary>
        /// 2
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public ActionResult InStock()
        {
            var loginUser = getLoginUser();
            var models = db.StoreProducts.Where(prd => prd.UserId == loginUser.Id && prd.Status == UserStatus.Active && prd.InStock>prd.LowStock).Select(u => new ProductsVM
            {
                SalePrice = u.SalePrice,
                Specification = u.Specification,
                Description = u.Description,
                ActualPrice = u.ActualPrice,
                Cost = u.Cost,
                Discount = u.Discount,
                Name = u.Name,
                InStock=u.InStock,
                FeatureImageUrl = u.FeatureImageUrl,
                Tax = u.Tax,
                StoreMeasurementUnitName = u.StoreMeasurementUnitProxy.Name,
                Date = u.CreatedOn,
                Id = u.Id,
                userStatus = u.Status,
                LowStock = u.LowStock
            }).ToList();
            return View(models);
        }
        /// <summary>
        /// 3
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public ActionResult SaleOrders(DateTime start,DateTime end)
        {
            var loginUser = getLoginUser();
            var model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered && mu.Type== orderType.Online && DbFunctions.TruncateTime(mu.CreatedOn) >= start&& DbFunctions.TruncateTime(mu.CreatedOn) <= end).OrderBy(u => u.CreatedOn).Select(u => new OrdersVM
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
                OrderNumber = u.OrderNumber,
                PhoneNumber = u.PhoneNumber,
                RejectionComment = u.RejectionComment,
                TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                Type = u.Type
            }).ToList();
            return View(model);
        }
        /// <summary>
        /// 4
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public ActionResult Purchase(DateTime start,DateTime end)
        {
            var loginUser = getLoginUser();
            var model = db.PurchaseOrderHeader.Where(mu => mu.CreatedByUserId == loginUser.Id && DbFunctions.TruncateTime(mu.CreatedOn) >= start && DbFunctions.TruncateTime(mu.CreatedOn) <= end).OrderBy(u=>u.CreatedOn).Select(u => new OrdersVM
            {
                Date = u.CreatedOn,
                DeliveryNote = u.Description,
                Email = u.SupplierName,
                Id = u.Id,
                Lines = db.PurchaseOrderLine.Where(or => or.PurchaseOrderHeaderId == u.Id).Select(n => new OrderLineVM
                {
                    Id = n.Id,
                    LineTotal = n.Total,
                    Price = n.Price,
                    ProductId = n.ProductId,
                    ProductName = n.ProductProxy.Name,
                    Qty = n.Qty
                }).ToList(),
                OrderNumber = u.OrderNumber,
                PhoneNumber = u.PhoneNumber,
                TotalBill = db.PurchaseOrderLine.Where(or => or.PurchaseOrderHeaderId == u.Id).Sum(n => (decimal?)n.Total) ?? 0,
            }).ToList();
            return View(model);
        }
        /// <summary>
        /// 5
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public ActionResult CashSale(DateTime start,DateTime end)
        {
            var loginUser = getLoginUser();
            var model = db.OrderHeader.Where(mu => mu.OrderForUserId == loginUser.Id && mu.Status == orderStatusEnum.Delivered && mu.Type == orderType.Offline && DbFunctions.TruncateTime(mu.CreatedOn) >= start && DbFunctions.TruncateTime(mu.CreatedOn) <= end).OrderBy(u => u.CreatedOn).Select(u => new OrdersVM
            {
                Status = u.Status,
                ShippingCharges = u.ShippingCharges,
                PaymentStatus = u.PaymentStatus,
                Date = u.CreatedOn,
                CanceledComment = u.CanceledComment,
                DeliveryNote = u.DeliveryNote,
                DeliveryOption = u.DeliveryOption,
                Email = u.Email,
                Id = u.Id,
                OrderNumber = u.OrderNumber,
                PhoneNumber = u.PhoneNumber,
                RejectionComment = u.RejectionComment,
                TotalBill = db.OrderLine.Where(or => or.OrderHeaderId == u.Id).Sum(n => (decimal?)n.LineTotal) ?? 0,
                Type = u.Type
            }).ToList();
            return View(model);
        }
        /// <summary>
        /// 6
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult QtyMovement(DateTime start, DateTime end, int productId)
        {
            var model = db.StoreQtyMovements.Where(mu => mu.ProductId == productId && DbFunctions.TruncateTime(mu.CreatedOn) >= start && DbFunctions.TruncateTime(mu.CreatedOn) <= end).OrderBy(u => u.CreatedOn).Select(u => new QtyMovementReportVM
            {
                TransactionType=u.Type,
                Cost=u.Cost,
                Date=u.CreatedOn,
                In=u.InQty,
                OrderNumber=u.OrderHeaderProxy==null?u.PurchaseOrderHeaderProxy.OrderNumber:u.OrderHeaderProxy.OrderNumber,
                Out=u.OutQty,
                Price=u.Price,
                Total=u.Total
            }).ToList();
            return View(model);
        }
        /// <summary>
        /// 7
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult ProductSale(DateTime start, DateTime end, int productId)
        {
            var model = db.OrderLine.Where(mu => mu.ProductId == productId&&mu.OrderHeaderProxy.Status== orderStatusEnum.Delivered && DbFunctions.TruncateTime(mu.OrderHeaderProxy.CreatedOn) >= start && DbFunctions.TruncateTime(mu.OrderHeaderProxy.CreatedOn) <= end).OrderBy(u => u.OrderHeaderProxy.CreatedOn).Select(u => new ProductSaleVM
            {
                Price=u.Price,
                LineTotal=u.LineTotal,
                OrderNumber=u.OrderHeaderProxy.OrderNumber,
                Email=u.OrderHeaderProxy.Email,
                Phone=u.OrderHeaderProxy.PhoneNumber,
                Date=u.OrderHeaderProxy.CreatedOn,
                DeliveryOption=u.OrderHeaderProxy.DeliveryOption,
                Note=u.OrderHeaderProxy.DeliveryNote,
                Qty=u.Qty,
                Type=u.OrderHeaderProxy.Type
            }).ToList();
            return View(model);
        }
        /// <summary>
        /// 8
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult UserDelivery(DateTime start, DateTime end, string userId)
        {
            return View();
        }
        
    }
}