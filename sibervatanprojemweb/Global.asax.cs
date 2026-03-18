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
            
            AreaRegistration.RegisterAllAreas();

            
            GlobalConfiguration.Configure(WebApiConfig.Register);

            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}