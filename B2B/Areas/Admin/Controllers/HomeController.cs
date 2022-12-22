using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public PartialViewResult ViewSidebar()
        {
            return PartialView("_HeaderPartial");
        }
        public ActionResult Index()
        {
            return View();
        }
        
    }
}