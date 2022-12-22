using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using B2B.Areas.Admin.ViewModel;
using B2B.DAL;
using B2B.Models.Packages;

namespace B2B.Areas.Admin.Controllers
{
    public class PackagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private DateTime currentTime = DateTime.Now;
        private string _CatalogDirectoryBaseUrl = "/images/packages";

        // GET: Admin/Packages
        public ActionResult Index()
        {
            List<PackageViewModel> model = new List<PackageViewModel>();
            var Packages = db.Package.Where(p=>p.IsActive==true).ToList();
            foreach(var line in Packages)
            {
                var feature = db.PackageFeatures.Where(p => p.PackageId == line.Id).Select(u => new FeatureViewModel
                {
                    Id = u.FeatureProxy.Id,
                    Name = u.FeatureProxy.Name
                }).ToList();
                PackageViewModel TempModel = new PackageViewModel
                {
                    Name = line.Name,
                    Description = line.Description,
                    Duration = line.Duration,
                    DurationType = line.DurationType,
                    Features = feature,
                    Id = line.Id,
                    Note = line.Note,
                    date = line.CreatedOn + "",
                    ImageUrl=line.ImageUrl,
                    Amount=line.Price
                };
                model.Add(TempModel);
            }
            return View(model);
        }
        public ActionResult MerchantPackageInvoice()
        {
            var Packages = db.MerchantPackageMeta.Select(u=>new MerchantPackageMetaVM {
                StartingFrom=u.StartingFrom,
                ValidTill=u.ValidTill,
                Id=u.Id,
                Status=u.Status,
                IsAmountPaid=u.IsAmountPaid,
                PackageName=u.PackageProxy.Name,
                PackageImage=u.PackageProxy.ImageUrl,
                TotalBill=u.Amount,
                UserName=u.UserProxy.UserName,
                UserImage=u.UserProxy.ProfileImageUrl,
                UserId=u.UserProxy.UserID,
            }).ToList();
            return View(Packages);
        }
        // GET: Admin/Packages/Create
        public ActionResult Create()
        {
            ViewBag.Feature = db.Feature.Select(u => new FeatureViewModel
            {
                Id = u.Id,
                Name = u.Name,
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PackageViewModel package,int[] FeatureIDs)
        {
            if (ModelState.IsValid)
            {
                Package model = new Package
                {
                    CreatedOn = currentTime,
                    Description = package.Description,
                    Duration = package.Duration,
                    DurationType = package.DurationType,
                    Name = package.Name,
                    Price = package.Amount,
                    IsActive=true,
                    Note = package.Note,
                };
                if (package.ImageFile != null)
                {
                    model.ImageUrl = UploadImage(package.ImageFile, _CatalogDirectoryBaseUrl);
                }
                
                db.Package.Add(model);

                if (FeatureIDs != null)
                {
                    foreach (var line in FeatureIDs)
                    {
                        PackageFeatures f = new PackageFeatures
                        {
                            FeatureId = line,
                            PackageId = model.Id,
                        };
                        db.PackageFeatures.Add(f);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Feature = db.Feature.Select(u => new FeatureViewModel
            {
                Id = u.Id,
                Name = u.Name,
            }).ToList();
            return View(package);
        }

        // GET: Admin/Packages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Package.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            PackageViewModel model = new PackageViewModel
            {
                Id=package.Id,
                Description=package.Description,
                Duration=package.Duration,
                DurationType=package.DurationType,
                Name=package.Name,
                Note=package.Note,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PackageViewModel package)
        {
            if (ModelState.IsValid)
            {
                var model = db.Package.Find(package.Id);
                model.Name = package.Name;
                model.Note = package.Note;
                model.DurationType = package.DurationType;
                model.Description = package.Description;
                model.UpdatedAt = currentTime;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(package);
        }
        public ActionResult DeletePackage(int Id)
        {
            var package = db.Package.Find(Id);
            if (package == null)
            {
                var res = new
                {
                    Status = false,
                    Msg = "Package not found."
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            if (package.IsActive)
            {
                package.IsActive = false;
                db.Entry(package).State = EntityState.Modified;
                db.SaveChanges();
                var res = new
                {
                    Status = true,
                    Msg = "Package deleted."
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                package.IsActive = true;
                db.Entry(package).State = EntityState.Modified;
                db.SaveChanges();
                var res = new
                {
                    Status = true,
                    Msg = "Package restored."
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AssigneFeature(int packageId,int FeatureId)
        {
            var package = db.Package.Find(packageId);
            var feature = db.Package.Find(FeatureId);
            if (package == null)
            {
                var res = new
                {
                    Status = false,
                    Msg = "Package not found."
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            if (feature == null)
            {
                var res = new
                {
                    Status = false,
                    Msg = "Feature not found."
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            var PackageFeature = db.PackageFeatures.Where(p => p.FeatureId == FeatureId && p.PackageId == packageId).FirstOrDefault();
            if (PackageFeature == null)
            {
                PackageFeatures pFeature = new PackageFeatures
                {
                    PackageId=packageId,
                    FeatureId=FeatureId,
                };
                db.PackageFeatures.Add(pFeature);
                db.SaveChanges();
                var res = new
                {
                    Status = true,
                    Msg = "Feature added into package."
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.PackageFeatures.Remove(PackageFeature);
                db.SaveChanges();
                var res = new
                {
                    Status = true,
                    Msg = "Feature removed from package."
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
