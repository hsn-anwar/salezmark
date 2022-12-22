using B2B.Areas.Api.Dtos;
using B2B.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace B2B.Areas.Api.Controllers
{
    public class CommonController : Controller
    {
        internal DateTime currentTime = DateTime.Now;
        internal ApplicationDbContext db = new ApplicationDbContext();
        internal JsonResult JsonResponse(int errorCode, string message, object responseData)
        {
            ResponseDto response = new ResponseDto
            {
                Code = errorCode,
                ShortMessage = message,
                Result = responseData,
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        internal string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}