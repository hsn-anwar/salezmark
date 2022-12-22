using B2B.Areas.Api.Dtos;
using B2B.DAL;
using B2B.Models;
using B2B.Models.Shopkeeper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Supermarket.Controllers
{
    public class BranchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
        // GET: Supermarket/Branch
        public ActionResult Index()
        {
            ApplicationUser user = getLoginUser();

            var branches = db.Branch.Where(bra => bra.UserId == user.Id && bra.IsDeleted == false).Select(u => new BranchDtos
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
            }).ToList();

            return View(branches);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(HttpPostedFileBase ImageFile, BranchDtos model)
        {
            if (ModelState.IsValid) {

                if (ImageFile != null)
                {
                    model.ImageUrl = UploadImage(ImageFile, "/images/branches");
                }
                ApplicationUser user = getLoginUser();
                Branch branch = new Branch
                {
                    CreatedOn = DateTime.Now,
                    ImageUrl = model.ImageUrl,
                    IsDeleted = false,
                    UserId = user.Id,
                    Name = model.Name,
                    LocationProxy = new Models.Locations
                    {
                        City = model.City,
                        Country = model.Country,
                        State = model.State,
                        AddressLine = model.AddressLine,
                        LiveModeEnable = false,
                        CreatedOn = DateTime.Now,
                    },
                    PhoneNumber = model.PhoneNumber,
                };
                db.Branch.Add(branch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public JsonResult Delete(int id)
        {
            var branch = db.Branch.Find(id);
            if (branch != null)
            {
                branch.UpdatedAt = DateTime.Now;
                branch.IsDeleted = true;
                db.Entry(branch).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var response = new
                {
                    Status = true,
                    Msg = "successfully!"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            var errorresponse = new
            {
                Status = false,
                Msg = "branch not found."
            };
            return Json(errorresponse, JsonRequestBehavior.AllowGet);
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
    }
}