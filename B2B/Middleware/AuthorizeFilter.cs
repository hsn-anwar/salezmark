using B2B.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace B2B.Middleware
{
    public class AuthorizeFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var header = filterContext.RequestContext.HttpContext.Request.Headers["Authorization"];
            if (!Authenticate(header))
                filterContext.Result = new HttpUnauthorizedResult();
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        Code = "1100",
                        ShortMessage = "This is a secure api, Please provide valid token to proceed.",
                        Result = "",
                    }
                };
            }
        }

        private bool Authenticate(string rawAuthorizationHeader)
        {
            try
            {
                if (rawAuthorizationHeader != null)
                {
                    var authHeader = AuthenticationHeaderValue.Parse(rawAuthorizationHeader);
                    if (authHeader != null
                        && authHeader.Scheme.Equals("token", StringComparison.OrdinalIgnoreCase)
                        && authHeader.Parameter != null)
                    {
                        var credentialPair = authHeader.Parameter;
                        var credentials = credentialPair.Split(new[] { ":" }, StringSplitOptions.None);
                        if (credentials == null || credentials.Length <= 0)
                        {
                            return false;
                        }
                        var deviceToken = credentials[0];
                        var deviceNumber = credentials[1];
                        var user = db.Users.Where(u => u.DeviceNumber == deviceNumber && u.DeviceToken == deviceToken).FirstOrDefault();
                        if (user != null)
                        {
                            return true;
                        }

                        return false;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}