using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B2B.Controllers
{
    [AllowAnonymous]
    public class SmoovPayController : Controller
    {
        // GET: SmoovPay
        public ActionResult Index()
        {
            return View();
        }
    }
}