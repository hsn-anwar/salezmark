using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.SalesTeam.Controllers
{
    public class HomeController : Controller
    {
        // GET: SalesTeam/Home
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ViewSidebar()
        {
            return PartialView("_LayoutHeader");
        }
    }
}