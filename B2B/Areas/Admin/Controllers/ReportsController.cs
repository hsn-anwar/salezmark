using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Admin.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Admin/Reports
        public ActionResult OrdersHistory()
        {
            return View();
        }
        public ActionResult CatalogReport()
        {
            return View();
        }
        public ActionResult ProductReport()
        {
            return View();
        }
        public ActionResult MerchantHistory()
        {
            return View();
        }
        public ActionResult SupermarketHistory()
        {
            return View();
        }
    }
}