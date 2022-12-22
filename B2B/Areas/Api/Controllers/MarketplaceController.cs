using B2B.Areas.Admin.ViewModel;
using B2B.Areas.Api.Dtos;
using B2B.Middleware;
using B2B.Models;
using B2B.Models.Cart;
using B2B.Models.Orders;
using B2B.Models.Social;
using B2B.Models.Store;
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
    public class MarketplaceController : CommonController
    {
        // GET: Api/Marketplace
        [HttpGet]
        public JsonResult GetCategories()
        {
            try
            {
                var categories = db.StoreCategory.Select(u => new CategoryDtos
                {
                    Id = u.Id,
                    Name = u.Name,
                    ImageUrl = u.ImageUrl,
                }).ToList();
                return JsonResponse(200, "", categories);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpGet]
        public JsonResult GetProducts(int? CategoryId)
        {
            try
            {
                if (CategoryId == null)
                {
                    var products = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active && prd.UserProxy.AccountSuspend == false && prd.UserProxy.IsDeleted == false && prd.UserProxy.Status== UserStatus.Active).Select(u => new ProductDtos
                    {
                        MerchantId = u.UserId,
                        Id = u.Id,
                        SalePrice = u.SalePrice,
                        Specification = u.Specification,
                        StoreMeasurementUnitName = u.StoreMeasurementUnitProxy != null ? u.StoreMeasurementUnitProxy.Name : "-",
                        MerchantName = u.UserProxy != null ? u.UserProxy.ProfileName : "-",
                        CategoryName = u.CategoryProxy != null ? u.CategoryProxy.Name : "-",
                        Description = u.Description,
                        Discount = u.Discount == null ? 0.00m : u.Discount,
                        InStock = u.InStock,
                        ActualPrice = u.ActualPrice,
                        Cost = u.Cost,
                        FeatureImageUrl = u.FeatureImageUrl,
                        Images = db.StoreProductGallery.Where(ur => ur.StoreProductId == u.Id).Select(ur => ur.ImageUrl).ToList(),
                        Name = u.Name,
                        CategoryId=u.CategoryId,
                        Tax = u.Tax == null ? 0.00m : u.Tax,
                        TotalPurchaseQty = (decimal?)db.StoreQtyMovements.Where(prd => prd.ProductId == u.Id && prd.Type == transactionType.Purchase).Sum(prd => (decimal?)prd.InQty ?? 0.00m) ??0.00m,
                        IsNew = u.CreatedOn.Month == currentTime.Month && u.CreatedOn.Year == currentTime.Year ? true : false,
                    }).ToList();
                    return JsonResponse(200, "", products);
                }
                else
                {
                    var category = db.StoreCategory.Find(CategoryId);
                    if (category == null)
                    {
                        return JsonResponse(400, "Bad Request! Category not found.", CategoryId);
                    }
                    var products = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active && prd.UserProxy.AccountSuspend == false && prd.UserProxy.IsDeleted == false && prd.CategoryId == CategoryId && prd.UserProxy.Status == UserStatus.Active).Select(u => new ProductDtos
                    {
                        MerchantId = u.UserId,
                        Id = u.Id,
                        CategoryId = u.CategoryId,
                        SalePrice = u.SalePrice,
                        Specification = u.Specification,
                        StoreMeasurementUnitName = u.StoreMeasurementUnitProxy != null ? u.StoreMeasurementUnitProxy.Name : "-",
                        MerchantName = u.UserProxy != null ? u.UserProxy.ProfileName : "-",
                        CategoryName = u.CategoryProxy != null ? u.CategoryProxy.Name : "-",
                        Description = u.Description,
                        Discount = u.Discount == null ? 0.00m : u.Discount,
                        InStock = u.InStock,
                        ActualPrice = u.ActualPrice,
                        Cost = u.Cost,
                        FeatureImageUrl = u.FeatureImageUrl,
                        Images = db.StoreProductGallery.Where(ur => ur.StoreProductId == u.Id).Select(ur => ur.ImageUrl).ToList(),
                        Name = u.Name,
                        Tax = u.Tax == null ? 0.00m : u.Tax,
                        IsNew= u.CreatedOn.Month==currentTime.Month && u.CreatedOn.Year==currentTime.Year?true:false,
                        TotalPurchaseQty=(decimal?)db.StoreQtyMovements.Where(prd=>prd.ProductId==u.Id&&prd.Type== transactionType.Purchase).Sum(prd=>(decimal?)prd.InQty??0.00m)??0.00m
                    }).ToList();
                    return JsonResponse(200, "", products);
                }
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpGet]
        public JsonResult ScanBarcode(string Code)
        {
            try
            {
                var products = db.StoreProducts.Where(prd => prd.Code == Code).FirstOrDefault();
                if (products.Status != UserStatus.Active)
                {
                    return JsonResponse(400, "Oops! Product is not active.", null);
                }
                else if (products.UserProxy.AccountSuspend)
                {
                    return JsonResponse(400, "Oops! Merchant account is suspended.", null);
                }
                else if (products.UserProxy.IsDeleted)
                {
                    return JsonResponse(400, "Oops! Merchant deleted his account.", null);
                }
                else if (products.UserProxy.Status != UserStatus.Active)
                {
                    return JsonResponse(400, "Oops! Merchant account is not active.", null);
                }
                else {

                    var Finalproducts = db.StoreProducts.Where(prd => prd.Code == Code).Select(u => new ProductDtos
                    {
                        MerchantId = u.UserId,
                        Id = u.Id,
                        CategoryId = u.CategoryId,
                        SalePrice = u.SalePrice,
                        Specification = u.Specification,
                        StoreMeasurementUnitName = u.StoreMeasurementUnitProxy != null ? u.StoreMeasurementUnitProxy.Name : "-",
                        MerchantName = u.UserProxy != null ? u.UserProxy.ProfileName : "-",
                        CategoryName = u.CategoryProxy != null ? u.CategoryProxy.Name : "-",
                        Description = u.Description,
                        Discount = u.Discount == null ? 0.00m : u.Discount,
                        InStock = u.InStock,
                        ActualPrice = u.ActualPrice,
                        Cost = u.Cost,
                        FeatureImageUrl = u.FeatureImageUrl,
                        Images = db.StoreProductGallery.Where(ur => ur.StoreProductId == u.Id).Select(ur => ur.ImageUrl).ToList(),
                        Name = u.Name,
                        Tax = u.Tax == null ? 0.00m : u.Tax,
                        IsNew = u.CreatedOn.Month == currentTime.Month && u.CreatedOn.Year == currentTime.Year ? true : false,
                        TotalPurchaseQty = (decimal?)db.StoreQtyMovements.Where(prd => prd.ProductId == u.Id && prd.Type == transactionType.Purchase).Sum(prd => (decimal?)prd.InQty ?? 0.00m) ?? 0.00m
                    }).FirstOrDefault();
                    return JsonResponse(200, "", Finalproducts);
                }

            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpGet]
        public JsonResult GetMerchantProducts(int? CategoryId, string MerchantId)
        {
            try
            {
                var user = db.Users.Find(MerchantId);
                if (MerchantId == null||user==null)
                {
                    return JsonResponse(400, "Bad Request! Merhcant not found or null.", MerchantId);
                }

                if (CategoryId == null)
                {
                    var products = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active && prd.UserProxy.AccountSuspend == false && prd.UserProxy.IsDeleted == false && prd.UserProxy.Status == UserStatus.Active && prd.UserId==MerchantId).Select(u => new ProductDtos
                    {
                        MerchantId = u.UserId,
                        Id = u.Id,
                        CategoryId = u.CategoryId,
                        SalePrice = u.SalePrice,
                        Specification = u.Specification,
                        StoreMeasurementUnitName = u.StoreMeasurementUnitProxy != null ? u.StoreMeasurementUnitProxy.Name : "-",
                        MerchantName = u.UserProxy != null ? u.UserProxy.ProfileName : "-",
                        CategoryName = u.CategoryProxy != null ? u.CategoryProxy.Name : "-",
                        Description = u.Description,
                        Discount = u.Discount == null ? 0.00m : u.Discount,
                        InStock = u.InStock,
                        ActualPrice = u.ActualPrice,
                        Cost = u.Cost,
                        FeatureImageUrl = u.FeatureImageUrl,
                        Images = db.StoreProductGallery.Where(ur => ur.StoreProductId == u.Id).Select(ur => ur.ImageUrl).ToList(),
                        Name = u.Name,
                        Tax = u.Tax == null ? 0.00m : u.Tax,
                        TotalPurchaseQty = (decimal?)db.StoreQtyMovements.Where(prd => prd.ProductId == u.Id && prd.Type == transactionType.Purchase).Sum(prd => (decimal?)prd.InQty ?? 0.00m) ?? 0.00m,
                        IsNew = u.CreatedOn.Month == currentTime.Month && u.CreatedOn.Year == currentTime.Year ? true : false,
                    }).ToList();
                    return JsonResponse(200, "", products);
                }
                else
                {
                    var category = db.StoreCategory.Find(CategoryId);
                    if (category == null)
                    {
                        return JsonResponse(400, "Bad Request! Category not found.", CategoryId);
                    }
                    var products = db.StoreProducts.Where(prd => prd.Status == UserStatus.Active && prd.UserProxy.AccountSuspend == false && prd.UserProxy.IsDeleted == false && prd.CategoryId == CategoryId && prd.UserProxy.Status == UserStatus.Active && prd.UserId == MerchantId).Select(u => new ProductDtos
                    {
                        MerchantId = u.UserId,
                        Id = u.Id,
                        CategoryId = u.CategoryId,
                        SalePrice = u.SalePrice,
                        Specification = u.Specification,
                        StoreMeasurementUnitName = u.StoreMeasurementUnitProxy != null ? u.StoreMeasurementUnitProxy.Name : "-",
                        MerchantName = u.UserProxy != null ? u.UserProxy.ProfileName : "-",
                        CategoryName = u.CategoryProxy != null ? u.CategoryProxy.Name : "-",
                        Description = u.Description,
                        Discount = u.Discount == null ? 0.00m : u.Discount,
                        InStock = u.InStock,
                        ActualPrice = u.ActualPrice,
                        Cost = u.Cost,
                        FeatureImageUrl = u.FeatureImageUrl,
                        Images = db.StoreProductGallery.Where(ur => ur.StoreProductId == u.Id).Select(ur => ur.ImageUrl).ToList(),
                        Name = u.Name,
                        Tax = u.Tax == null ? 0.00m : u.Tax,
                        IsNew = u.CreatedOn.Month == currentTime.Month && u.CreatedOn.Year == currentTime.Year ? true : false,
                        TotalPurchaseQty = (decimal?)db.StoreQtyMovements.Where(prd => prd.ProductId == u.Id && prd.Type == transactionType.Purchase).Sum(prd => (decimal?)prd.InQty ?? 0.00m) ?? 0.00m
                    }).ToList();
                    return JsonResponse(200, "", products);
                }
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        /// <summary>
        /// ////////////////////////////////////////////////Cart Detail
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public JsonResult GetCarts()
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

                List<CartHeaderDtos> cartsResponse = db.CartHeaders.Where(cr => cr.CreatedByUserId == loginUser.Id).Select(u => new CartHeaderDtos
                {
                    Number = u.Number,
                    MerchantEmail = u.CreatedForUserProxy.Email,
                    MerchantName = u.CreatedForUserProxy.ProfileName,
                    Date = u.CreatedOn + "",
                    CartId = u.Id,
                    MerchantProfileImageUrl = u.CreatedForUserProxy.ProfileImageUrl,
                    LineDetails = db.CartLines.Where(lin => lin.HeaderId == u.Id).Select(li => new CartLineDtos
                    {
                        LineId = li.Id,
                        ProductImageUrl = li.ProductProxy.FeatureImageUrl,
                        LineTotal = li.ProductProxy.SalePrice * li.Qty,
                        Qty = li.Qty,
                        Price = li.ProductProxy.SalePrice,
                        ProductName = li.ProductProxy.Name,
                        MeasurementUnit = li.ProductProxy.StoreMeasurementUnitProxy != null ? li.ProductProxy.StoreMeasurementUnitProxy.Name : "",
                    }).ToList(),
                }).ToList();
                return JsonResponse(200, "", cartsResponse);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error!" + BaseMessage, null);
            }
        }
        [HttpGet]
        public JsonResult GetCartDetail(int? CartId)
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
                if (CartId == null)
                {
                    return JsonResponse(400, "Bad Request! Cart number(#) should not be null or empty.", null);
                }
                var cart = db.CartHeaders.Find(CartId);
                if (cart == null || cart.CreatedByUserId != loginUser.Id)
                {
                    return JsonResponse(400, "Oops! Cart not found try again.", null);
                }
                CartHeaderDtos cartsResponse = new CartHeaderDtos
                {
                    Number = cart.Number,
                    MerchantEmail = cart.CreatedForUserProxy.Email,
                    MerchantName = cart.CreatedForUserProxy.ProfileName,
                    Date = cart.CreatedOn + "",
                    CartId = cart.Id,
                    MerchantProfileImageUrl = cart.CreatedForUserProxy.ProfileImageUrl,
                    LineDetails = db.CartLines.Where(lin => lin.HeaderId == cart.Id).Select(li => new CartLineDtos
                    {
                        LineId = li.Id,
                        ProductImageUrl = li.ProductProxy.FeatureImageUrl,
                        LineTotal = li.ProductProxy.SalePrice * li.Qty,
                        Qty = li.Qty,
                        Price = li.ProductProxy.SalePrice,
                        ProductName = li.ProductProxy.Name,
                        MeasurementUnit = li.ProductProxy.StoreMeasurementUnitProxy != null ? li.ProductProxy.StoreMeasurementUnitProxy.Name : "",
                    }).ToList(),
                };
                return JsonResponse(200, "", cartsResponse);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpPost]
        public JsonResult AddCartLine(int? ProductId,decimal? Qty)
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

                if (ProductId == null)
                {
                    return JsonResponse(400, "Bad Request! Product should not be null or empty.", null);
                }
                if (Qty == null)
                {
                    return JsonResponse(400, "Bad Request! Qty should not be null or empty.", null);
                }
                var product = db.StoreProducts.Find(ProductId);
                if (ProductId == null)
                {
                    return JsonResponse(400, "Bad Request! No product found.", ProductId);
                }
                var cartHeader = db.CartHeaders.Where(crt => crt.CreatedByUserId == loginUser.Id && crt.CreatedForUserId == product.UserId).FirstOrDefault();
                if (cartHeader == null)
                {
                    Random random = new Random();
                    int orderNumber = random.Next();

                    CartLine line = new CartLine
                    {
                        HeaderProxy=new CartHeader
                        {
                            Number="ORD-"+orderNumber,
                            CreatedByUserId=loginUser.Id,
                            CreatedForUserId=product.UserId,
                            CreatedOn=currentTime
                        },
                        Qty=Qty.Value,
                        ProductId=product.Id,
                    };
                    db.CartLines.Add(line);

                }
                else
                {
                    var cartLine = db.CartLines.Where(CL => CL.HeaderId == cartHeader.Id && CL.ProductId == ProductId).FirstOrDefault();
                    if (cartLine == null)
                    {
                        CartLine line = new CartLine
                        {
                            HeaderId= cartHeader.Id,
                            Qty= Qty.Value,
                            ProductId=product.Id,
                        };
                        db.CartLines.Add(line);
                    }
                    else
                    {
                        cartLine.Qty = cartLine.Qty + Qty.Value;
                        db.Entry(cartLine).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                db.SaveChanges();
                List<CartHeaderDtos> cartsResponse = db.CartHeaders.Where(cr => cr.CreatedByUserId == loginUser.Id).Select(u => new CartHeaderDtos
                {
                    Number = u.Number,
                    MerchantEmail = u.CreatedForUserProxy.Email,
                    MerchantName = u.CreatedForUserProxy.ProfileName,
                    Date = u.CreatedOn + "",
                    CartId = u.Id,
                    MerchantProfileImageUrl = u.CreatedForUserProxy.ProfileImageUrl,
                    LineDetails = db.CartLines.Where(lin => lin.HeaderId == u.Id).Select(li => new CartLineDtos
                    {
                        LineId = li.Id,
                        ProductImageUrl = li.ProductProxy.FeatureImageUrl,
                        LineTotal = li.ProductProxy.SalePrice * li.Qty,
                        Qty = li.Qty,
                        Price = li.ProductProxy.SalePrice,
                        ProductName = li.ProductProxy.Name,
                        MeasurementUnit = li.ProductProxy.StoreMeasurementUnitProxy != null ? li.ProductProxy.StoreMeasurementUnitProxy.Name : "",
                    }).ToList(),
                }).ToList();
                return JsonResponse(200, "Cheer Up! Product successfully added.", cartsResponse);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpPost]
        public JsonResult DeleteCart(int? CartId)
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

                if (CartId == null)
                {
                    return JsonResponse(400, "Bad Request! Cart number(#) should not be null or empty.", null);
                }
                var cart = db.CartHeaders.Find(CartId);
                if (cart == null || cart.CreatedByUserId != loginUser.Id)
                {
                    return JsonResponse(400, "Oops! Cart not found try again.", null);
                }
                db.CartLines.RemoveRange(db.CartLines.Where(line => line.HeaderId == cart.Id).ToList());
                db.CartHeaders.Remove(cart);
                db.SaveChanges();
                return JsonResponse(200, "Cheer up! Cart successfully deleted.", null);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpPost]
        public JsonResult DeleteCartLine(int? LineId)
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

                if (LineId == null)
                {
                    return JsonResponse(400, "Bad Request! Line number(#) should not be null or empty.", null);
                }
                var cart = db.CartLines.Include("HeaderProxy").Where(l => l.Id == LineId).FirstOrDefault();
                if (cart == null || cart.HeaderProxy.CreatedByUserId != loginUser.Id)
                {
                    return JsonResponse(400, "Oops! Line not found try again.", null);
                }
                var cartHeaderCount = db.CartLines.Where(cr => cr.HeaderId == cart.HeaderId && cr.Id != LineId).Count();
                
                db.CartLines.Remove(cart);
                if (cartHeaderCount <= 0)
                {
                    db.CartHeaders.Remove(db.CartHeaders.Find(cart.HeaderId));
                }
                db.SaveChanges();
                
                return JsonResponse(200, "Cheer up! Line successfully deleted.", null);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpPost]
        public JsonResult DecrementLineQty(int? LineId,int? Qty)
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

                if (LineId == null)
                {
                    return JsonResponse(400, "Bad Request! Cart line number(#) should not be null or empty.", null);
                }
                if (Qty == null)
                {
                    return JsonResponse(400, "Bad Request! Qty should not be null or empty.", null);
                }
                var cartLine = db.CartLines.Include("HeaderProxy").Where(l=>l.Id==LineId).FirstOrDefault();
                if (cartLine == null || cartLine.HeaderProxy.CreatedByUserId != loginUser.Id)
                {
                    return JsonResponse(400, "Bad Request! Cart line not found.", null);
                }
                if (Qty.Value > cartLine.Qty)
                {
                    return JsonResponse(400, "Bad Request! Qty must be smaller than or equall to line qty.", null);
                }
                else if(Qty.Value == cartLine.Qty)
                {
                    var cartHeaderCount = db.CartLines.Where(cr => cr.HeaderId == cartLine.HeaderId && cr.Id != cartLine.Id).Count();
                    db.CartLines.Remove(cartLine);
                    if (cartHeaderCount <= 0)
                    {
                        db.CartHeaders.Remove(db.CartHeaders.Find(cartLine.HeaderId));
                    }
                }
                else
                {
                    cartLine.Qty = cartLine.Qty - Qty.Value;
                    db.Entry(cartLine).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                List<CartLineDtos> cartsResponse = db.CartLines.Where(lin => lin.HeaderId == cartLine.HeaderId).Select(li => new CartLineDtos
                {
                    LineId = li.Id,
                    ProductImageUrl = li.ProductProxy.FeatureImageUrl,
                    LineTotal = li.ProductProxy.SalePrice * li.Qty,
                    Qty = li.Qty,
                    Price = li.ProductProxy.SalePrice,
                    ProductName = li.ProductProxy.Name,
                    MeasurementUnit = li.ProductProxy.StoreMeasurementUnitProxy != null ? li.ProductProxy.StoreMeasurementUnitProxy.Name : "",
                }).ToList();
                return JsonResponse(200, "Cheer Up! Subtracted successfully.", cartsResponse);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpPost]
        public JsonResult ConfirmOrder(int? CartId, paymentMethodEnum? PaymentMethod,string DeliveryNote,int? BranchId,bool withCustomDelivery,DateTime? CustomDeliveryTime)
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
                if (PaymentMethod == null)
                {
                    return JsonResponse(400, "Bad Request! Payment method should not be null or empty.", null);
                }
                if (BranchId == null)
                {
                    return JsonResponse(400, "Bad Request! Branch number(#) should not be null or empty.", null);
                }
                if (CartId == null)
                {
                    return JsonResponse(400, "Bad Request! Cart number(#) should not be null or empty.", null);
                }
                var cart = db.CartHeaders.Find(CartId);
                if (cart == null || cart.CreatedByUserId != loginUser.Id)
                {
                    return JsonResponse(400, "Oops! Cart not found try again.", null);
                }
                var Branch = db.Branch.Find(BranchId);
                if (Branch == null)
                {
                    return JsonResponse(400, "Bad Request! Branch not found.", null);
                }

                var merchant = db.Users.Find(cart.CreatedForUserId);
                Notification notify = new Notification
                {
                    Isseen = false,
                    Title = "Order Placed",
                    Description = "Your order has been place successfully to " + merchant.ProfileName,
                    CreatedOn = currentTime,
                    NotifyByUserId = loginUser.Id,
                    NotifyToUserId = loginUser.Id,
                };
                Notification notify2 = new Notification
                {
                    Isseen = false,
                    Title = "Order Received",
                    Description = "You have received an order from " + loginUser.ProfileName,
                    CreatedOn = currentTime,
                    NotifyByUserId = loginUser.Id,
                    NotifyToUserId = merchant.Id,
                };
                db.Notification.Add(notify);
                db.Notification.Add(notify2);
                OrderHeader header = new OrderHeader
                {
                    CreatedOn = currentTime,
                    ShippingCharges = 0.00m,
                    Status = orderStatusEnum.Pening,
                    PaymentStatus = PaymentMethod.Value,
                    DeliveryOption = deliveryType.MerchantDelivery,
                    Email = cart.CreatedByUserProxy.Email,
                    PhoneNumber = Branch.PhoneNumber,
                    Type = orderType.Online,
                    OrderForUserId = cart.CreatedForUserId,
                    OrderNumber = cart.Number,
                    DeliveryNote = DeliveryNote,
                    OrderCreatedByUserId = cart.CreatedByUserId,
                    BranchId=Branch.Id,
                    WithCustomDelivery=withCustomDelivery,
                    CustomDeliveryTime=CustomDeliveryTime
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
                foreach (var line in db.CartLines.Where(lin=>lin.HeaderId==cart.Id).ToList())
                {

                    var Pro = db.StoreProducts.Find(line.ProductId);
                    decimal LineTotal = line.Qty * Pro.SalePrice;
                    OrderLine Templine = new OrderLine
                    {
                        LineTotal = LineTotal,
                        OrderHeaderProxy= header,
                        Price = Pro.SalePrice,
                        ProductId = line.ProductId,
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
                return JsonResponse(200, "Cheer Up! Order confirmed.", null);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpGet]
        public JsonResult GetBranch()
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
                if(loginUser.UserType== UserTypes.Supermarket)
                {
                    var branch = db.Branch.Where(br => br.UserId == loginUser.Id).Select(u=>new BranchDtos {
                        Id=u.Id,
                        ImageUrl=u.ImageUrl,
                        State=u.LocationProxy!=null?u.LocationProxy.State:"",
                        Country=u.LocationProxy!=null?u.LocationProxy.Country:"",
                        City=u.LocationProxy!=null?u.LocationProxy.City:"",
                        AddressLine=u.LocationProxy!=null?u.LocationProxy.AddressLine:"",
                        Latitude=u.LocationProxy!=null?u.LocationProxy.Latitude:null,
                        Longitude=u.LocationProxy!=null?u.LocationProxy.Longitude:null,
                        LiveModelEnabled=u.LocationProxy!=null?u.LocationProxy.LiveModeEnable:false,
                        Name=u.Name,
                        PhoneNumber=u.PhoneNumber,
                    }).FirstOrDefault();
                    return JsonResponse(200, "Supermarket branch", branch);
                }
                else if(loginUser.UserType== UserTypes.Supervisor)
                {
                    var branch = db.Branch.Where(br => br.AssignedToUserId == loginUser.Id).Select(u => new BranchDtos
                    {
                        Id = u.Id,
                        ImageUrl = u.ImageUrl,
                        State = u.LocationProxy != null ? u.LocationProxy.State : "",
                        Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                        City = u.LocationProxy != null ? u.LocationProxy.City : "",
                        AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                        Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : null,
                        Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : null,
                        LiveModelEnabled = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                        Name = u.Name,
                        PhoneNumber = u.PhoneNumber,
                    }).FirstOrDefault();
                    return JsonResponse(200, "Supervisor branch.", branch);
                }
                else
                {
                    return JsonResponse(400, "Bad Request! Access denied.", null);
                }
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpGet]
        public JsonResult GetSupermarketBranches()
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
                if (loginUser.UserType != UserTypes.Supermarket)
                {
                    return JsonResponse(400, "Bad Request! Access denied.", null);
                }
                var branch = db.Branch.Where(br => br.UserId == loginUser.Id).Select(u => new BranchDtos
                {
                    Id = u.Id,
                    ImageUrl = u.ImageUrl,
                    State = u.LocationProxy != null ? u.LocationProxy.State : "",
                    Country = u.LocationProxy != null ? u.LocationProxy.Country : "",
                    City = u.LocationProxy != null ? u.LocationProxy.City : "",
                    AddressLine = u.LocationProxy != null ? u.LocationProxy.AddressLine : "",
                    Latitude = u.LocationProxy != null ? u.LocationProxy.Latitude : null,
                    Longitude = u.LocationProxy != null ? u.LocationProxy.Longitude : null,
                    LiveModelEnabled = u.LocationProxy != null ? u.LocationProxy.LiveModeEnable : false,
                    Name = u.Name,
                    PhoneNumber = u.PhoneNumber,
                }).ToList();
                return JsonResponse(200, "Supermarket branchs.", branch);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        /////////////////////////////////////////////////////////////////////////////
        ///
        [HttpPost]
        public JsonResult ChangeMerchantFavouriteStatus(string MerchantId)
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
                var Merchant = db.Users.Where(u => u.Id == MerchantId).FirstOrDefault();

                if (Merchant == null)
                {
                    return JsonResponse(400, "Bad Request! MerchantId should not be null or empty.", null);
                }
                var CheckMerchantStatus = db.FavouriteMerchant.Where(u => u.UserID == loginUser.Id && u.MerchantId == MerchantId).FirstOrDefault();
                if (CheckMerchantStatus == null)
                {
                    FavouriteMerchants favMerchant = new FavouriteMerchants
                    {
                        CreatedOn=currentTime,
                        MerchantId=MerchantId,
                        UserID=loginUser.Id,
                    };
                    db.FavouriteMerchant.Add(favMerchant);
                    db.SaveChanges();
                    return JsonResponse(200, "Cheer Up! Merchant successfully added into favourite list.", true);

                }
                else
                {
                    db.FavouriteMerchant.Remove(CheckMerchantStatus);
                    db.SaveChanges();
                    return JsonResponse(200, "Cheer Up! Merchant successfully removed from favourite list.", false);
                }
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }
        [HttpGet]
        public JsonResult GetFavouriteMerchantList()
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
                var branch = db.FavouriteMerchant.Where(br => br.UserID == loginUser.Id).Select(u => new
                {
                    Id = u.Id,
                    ImageUrl = u.MerchantProxy.ProfileImageUrl,
                    ProfileName = u.MerchantProxy.ProfileName,
                    Username = u.MerchantProxy.UserName,
                    Email = u.MerchantProxy.Email,
                    Phone = u.MerchantProxy.PhoneNumber,
                }).ToList();
                return JsonResponse(200, "", branch);
            }
            catch (Exception ex)
            {
                var BaseMessage = ex.GetBaseException().Message;
                return JsonResponse(504, "Processing Error! " + BaseMessage, null);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////
        ///
        [HttpGet]
        public JsonResult GetMerchantDetail(string MerchantId)
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
                if (MerchantId == null)
                {
                    return JsonResponse(400, "Bad Request! MerchantId should not be null.", null);
                }
                var branch = db.Users.Where(br => br.Id == MerchantId).Select(u => new
                {
                    Id = u.Id,
                    ImageUrl = u.ProfileImageUrl,
                    ProfileName = u.ProfileName,
                    Username = u.UserName,
                    Email = u.Email,
                    Phone = u.PhoneNumber,
                    IsFavourite=db.FavouriteMerchant.Where(ur=>ur.UserID==loginUser.Id&&ur.MerchantId==MerchantId).FirstOrDefault()==null?false:true
                }).FirstOrDefault();
                if (branch == null)
                {
                    return JsonResponse(400, "Bad Request! Merchant not found.", null);
                }
                else
                {
                    return JsonResponse(200, "", branch);
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