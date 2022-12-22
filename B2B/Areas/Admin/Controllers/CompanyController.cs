using B2B.Areas.Admin.ViewModel;
using B2B.DAL;
using B2B.Models.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Admin.Controllers
{
    public class CompanyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private DateTime currentTime = DateTime.Now;
        private string _CatalogDirectoryBaseUrl = "/images/catalog";
        // GET: Admin/Catalog
        public ActionResult Index()
        {
            return View(db.StoreCompany.Where(u => u.IsDeleted == false).ToList());
        }

        // GET: Admin/Catalog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Catalog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Store_Company Company_model, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                Store_Company model = new Store_Company
                {
                    Name = Company_model.Name,
                };
                if (ImageFile != null)
                {
                    model.ImageUrl = UploadImage(ImageFile, _CatalogDirectoryBaseUrl);
                }

                db.StoreCompany.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Admin/Catalog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store_Company store_Company = db.StoreCompany.Find(id);
            if (store_Company == null)
            {
                return HttpNotFound();
            }
            Store_Company model = new Store_Company();
            
            model.Name = store_Company.Name;
            model.ImageUrl = store_Company.ImageUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Store_Company store_Company, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var category = db.StoreCompany.Find(store_Company.Id);
                if (category.ImageUrl != null)
                {
                    if (ImageFile != null)
                    {
                        RemoveFileFromServer(category.ImageUrl);
                        category.ImageUrl = UploadImage(ImageFile, _CatalogDirectoryBaseUrl);
                    }
                }
                category.Name = store_Company.Name;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(store_Company);
        }
        public JsonResult DeleteCatalog(int id)
        {
            var catalog = db.StoreCompany.Find(id);
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
            db.Entry(catalog).State = EntityState.Modified;
            db.SaveChanges();
            var response = new
            {
                code = 200,
                msg = ""
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