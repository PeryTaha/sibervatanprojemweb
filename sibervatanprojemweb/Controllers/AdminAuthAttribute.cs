using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace sibervatanprojemweb.Controllers
{
    public class AdminAuthAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var token = actionContext.Request.Headers.Contains("X-Admin-Token")
                ? actionContext.Request.Headers.GetValues("X-Admin-Token").FirstOrDefault()
                : null;

            if (!LoginController.TokenGecerliMi(token))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized,
                    "Yonetim islemi icin gecerli oturum gerekli."
                );
            }
        }
    }
}
