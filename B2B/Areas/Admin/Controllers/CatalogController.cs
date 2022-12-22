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
using B2B.Models.Store;

namespace B2B.Areas.Admin.Controllers
{
    public class CatalogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private DateTime currentTime = DateTime.Now;
        private string _CatalogDirectoryBaseUrl = "/images/catalog";
        // GET: Admin/Catalog
        public ActionResult Index()
        {
            return View(db.StoreCategory.Where(u=>u.IsDeleted==false).Select(u=>new CategoryVM {
                Id=u.Id,
                Name=u.Name,
                ImageUrl=u.ImageUrl,
                ParentCategoryName=u.ParentCategoryProxy==null?"-":u.ParentCategoryProxy.Name,
            }).ToList());
        }

        // GET: Admin/Catalog/Create
        public ActionResult Create()
        {
            CategoryVM model = new CategoryVM();
            model.categoryList = db.StoreCategory.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }).ToList();
            return View(model);
        }

        // POST: Admin/Catalog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryVM store_Category)
        {
            if (ModelState.IsValid)
            {
                Store_Category model = new Store_Category
                {
                    CreatedOn=currentTime,
                    Name=store_Category.Name,
                    ParentCategoryId=store_Category.ParentCategoryId,
                };
                if (store_Category.ImageFile != null)
                {
                    model.ImageUrl = UploadImage(store_Category.ImageFile, _CatalogDirectoryBaseUrl);
                }

                db.StoreCategory.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            store_Category.categoryList = db.StoreCategory.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }).ToList();
            return View(store_Category);
        }

        // GET: Admin/Catalog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store_Category store_Category = db.StoreCategory.Find(id);
            if (store_Category == null)
            {
                return HttpNotFound();
            }
            CategoryVM model = new CategoryVM();
            model.categoryList = db.StoreCategory.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }).ToList();
            model.Name = store_Category.Name;
            model.ParentCategoryId = store_Category.ParentCategoryId;
            model.ImageUrl = store_Category.ImageUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryVM store_Category)
        {
            if (ModelState.IsValid)
            {
                var category = db.StoreCategory.Find(store_Category.Id);
                if (category.ImageUrl != null)
                {
                    if (store_Category.ImageFile != null)
                    {
                        RemoveFileFromServer(category.ImageUrl);
                        category.ImageUrl = UploadImage(store_Category.ImageFile, _CatalogDirectoryBaseUrl);
                    }
                }
                category.Name = store_Category.Name;
                category.ParentCategoryId = store_Category.ParentCategoryId;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            store_Category.categoryList = db.StoreCategory.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }).ToList();
            return View(store_Category);
        }
        public JsonResult DeleteCatalog(int id)
        {
            var catalog = db.StoreCategory.Find(id);
            if (catalog == null)
            {
                var ErrorResponse = new
                {
                    code = 400,
                    msg = "No catalog found!"
                };
                return Json(ErrorResponse, JsonRequestBehavior.AllowGet);
            }
            catalog.IsDeleted = true;
            catalog.UpdatedAt = currentTime;
            db.Entry(catalog).State = EntityState.Modified;
            db.SaveChanges();
            var response = new
            {
                code = 200,
                msg=""
            };
            return Json(response, JsonRequestBehavior.AllowGet);
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
