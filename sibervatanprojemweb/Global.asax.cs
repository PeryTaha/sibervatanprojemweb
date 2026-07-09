using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http; 

namespace sibervatanprojemweb
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-SourceFiles");
            Response.Headers.Remove("X-Powered-By");
        }
    }
}
