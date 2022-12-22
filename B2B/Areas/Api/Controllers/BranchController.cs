using B2B.Areas.Api.Dtos;
using B2B.Middleware;
using B2B.Models.Shopkeeper;
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
    public class BranchController : CommonController
    {
        // GET: Api/Branch
        [HttpGet]
        public JsonResult GetBranches()
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
                if (loginUser.UserType != Models.UserTypes.Supermarket)
                {
                    return JsonResponse(400, "Only admin can view branches!", null);
                }
                //////////////////GET USER BRANCHES////////////////////////////////////////////////
                var branches = db.Branch.Where(bra => bra.UserId == loginUser.Id && bra.IsDeleted == false).Select(u => new BranchDtos
                {
                    Id = u.Id,
                    AssignedTo = u.AssignedToProxy == null ? "Not assigned yet" : u.AssignedToProxy.UserName,
                    City = u.LocationProxy == null ? "" : u.LocationProxy.City,
                    Country = u.LocationProxy == null ? "" : u.LocationProxy.Country,
                    Latitude = u.LocationProxy == null ? 0.00 : u.LocationProxy.Latitude,
                    Longitude = u.LocationProxy == null ? 0.00 : u.LocationProxy.Longitude,
                    State = u.LocationProxy == null ? "" : u.LocationProxy.State,
                    ImageUrl = u.ImageUrl,
                    Name = u.Name,
                    PhoneNumber = u.PhoneNumber,
                    LiveModelEnabled=u.LocationProxy==null?false:u.LocationProxy.LiveModeEnable,
                    AddressLine=u.LocationProxy==null?"":u.LocationProxy.AddressLine,
                    IsAssigned = u.AssignedToProxy == null ? false : true,
                }).ToList();
                return JsonResponse(200, "", branches);
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }
        [HttpGet]
        public JsonResult GetBranchDetail(int? Id)
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

                //////////////////GET USER BRANCHES////////////////////////////////////////////////

                var branches = db.Branch.Where(bra => bra.Id== Id).Select(u => new BranchDtos
                {
                    Id = u.Id,
                    AssignedTo = u.AssignedToProxy == null ? "Not assigned yet" : u.AssignedToProxy.UserName,
                    City = u.LocationProxy == null ? "" : u.LocationProxy.City,
                    Country = u.LocationProxy == null ? "" : u.LocationProxy.Country,
                    Latitude = u.LocationProxy == null ? 0.00 : u.LocationProxy.Latitude,
                    Longitude = u.LocationProxy == null ? 0.00 : u.LocationProxy.Longitude,
                    State = u.LocationProxy == null ? "" : u.LocationProxy.State,
                    ImageUrl = u.ImageUrl,
                    Name = u.Name,
                    PhoneNumber = u.PhoneNumber,
                    LiveModelEnabled = u.LocationProxy == null ? false : u.LocationProxy.LiveModeEnable,
                    AddressLine = u.LocationProxy == null ? "" : u.LocationProxy.AddressLine,
                    IsAssigned = u.AssignedToProxy == null ? false : true,
                }).FirstOrDefault();
                return JsonResponse(200, "", branches);
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }
        [HttpGet]
        public JsonResult TopBranches()
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
                if (loginUser.UserType != Models.UserTypes.Supermarket)
                {
                    return JsonResponse(400, "Only admin can view branches!", null);
                }
                //////////////////GET USER BRANCHES////////////////////////////////////////////////
                var branches = db.Branch.Where(bra => bra.UserId == loginUser.Id && bra.IsDeleted == false && bra.AssignedToUserId!=null).Select(u => new BranchDtos
                {
                    Id = u.Id,
                    AssignedTo = u.AssignedToProxy == null ? "Not assigned yet" : u.AssignedToProxy.UserName,
                    City = u.LocationProxy == null ? "" : u.LocationProxy.City,
                    Country = u.LocationProxy == null ? "" : u.LocationProxy.Country,
                    Latitude = u.LocationProxy == null ? 0.00 : u.LocationProxy.Latitude,
                    Longitude = u.LocationProxy == null ? 0.00 : u.LocationProxy.Longitude,
                    State = u.LocationProxy == null ? "" : u.LocationProxy.State,
                    ImageUrl = u.ImageUrl,
                    Name = u.Name,
                    PhoneNumber = u.PhoneNumber,
                    LiveModelEnabled = u.LocationProxy == null ? false : u.LocationProxy.LiveModeEnable,
                    AddressLine = u.LocationProxy == null ? "" : u.LocationProxy.AddressLine,
                    IsAssigned= u.AssignedToProxy == null ? false : true
                }).ToList();
                return JsonResponse(200, "", branches);
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }
        [HttpPost]
        public JsonResult AddBranch(BranchDtos model)
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
                if(loginUser.UserType!= Models.UserTypes.Supermarket)
                {
                    return JsonResponse(400, "Only admin can registered new branch!", model);
                }
                //////////////////GET USER BRANCHES////////////////////////////////////////////////
                Branch branch = new Branch
                {
                    CreatedOn=currentTime,
                    ImageUrl=model.ImageUrl,
                    IsDeleted=false,
                    UserId=loginUser.Id,
                    Name=model.Name,
                    LocationProxy=new Models.Locations
                    {
                        City=model.City,
                        Country=model.Country,
                        State=model.State,
                        AddressLine=model.AddressLine,
                        Latitude=model.Latitude,
                        Longitude=model.Longitude,
                        LiveModeEnable=model.LiveModelEnabled,
                        CreatedOn=currentTime,
                    },
                    PhoneNumber=model.PhoneNumber,
                };
                db.Branch.Add(branch);
                db.SaveChanges();
                model.Id = branch.Id;
                return JsonResponse(200, "New branch has been registered successfully.", model);
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }
        [HttpPost]
        public JsonResult EditBranch(BranchDtos model)
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
                var branch = db.Branch.Find(model.Id);
                if (branch == null || branch.UserId != loginUser.Id)
                {
                    return JsonResponse(400, "Invalid branch id or branch belong to other user!", model);
                }
                if (loginUser.UserType != Models.UserTypes.Supermarket)
                {
                    return JsonResponse(400, "Only admin can update branch!", null);
                }
                if (branch.ImageUrl != null)
                {
                    RemoveFileFromServer(branch.ImageUrl);
                }
                //////////////////GET USER BRANCHES////////////////////////////////////////////////
                branch.UpdatedAt = currentTime;
                branch.ImageUrl = model.ImageUrl;
                branch.Name = model.Name;
                branch.PhoneNumber = model.PhoneNumber;

                branch.LocationProxy.City = model.City;
                branch.LocationProxy.Country = model.Country;
                branch.LocationProxy.State = model.State;
                branch.LocationProxy.AddressLine = model.AddressLine;
                branch.LocationProxy.Latitude = model.Latitude;
                branch.LocationProxy.Longitude = model.Longitude;
                branch.LocationProxy.LiveModeEnable = model.LiveModelEnabled;
                branch.LocationProxy.UpdatedAt = currentTime;

                db.Entry(branch).State=System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return JsonResponse(200, "Branch has been updated successfully.", model);
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }
        [HttpPost]
        public JsonResult DeleteBranch(int BranchId)
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
                var branch = db.Branch.Find(BranchId);
                if (branch == null || branch.UserId != loginUser.Id)
                {
                    return JsonResponse(400, "Invalid branch id or branch belong to other user!", null);
                }
                if (loginUser.UserType != Models.UserTypes.Supermarket)
                {
                    return JsonResponse(400, "Only admin can trashed branch!", null);
                }
                //////////////////GET USER BRANCHES////////////////////////////////////////////////
                branch.UpdatedAt = currentTime;
                branch.IsDeleted = true;
                db.Entry(branch).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return JsonResponse(200, "Branch has been successfully trashed.", null);
            }
            catch (Exception es)
            {
                return JsonResponse(504, es.GetBaseException().Message, null);
            }
        }
        private bool RemoveFileFromServer(string path)
        {
            if (path != null)
            {
                var fullPath = Request.MapPath(path);
                if (!System.IO.File.Exists(fullPath)) return false;

                try //Maybe error could happen like Access denied or Presses Already User used
                {
                    System.IO.File.Delete(fullPath);
                    return true;
                }
                catch (Exception e)
                {
                    //Debug.WriteLine(e.Message);
                }
            }
            return false;
        }
    }
}