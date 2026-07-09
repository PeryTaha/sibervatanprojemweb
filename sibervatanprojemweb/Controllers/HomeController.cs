using System.Web.Mvc;

namespace sibervatanprojemweb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("https://localhost:44330/SiberVatanWeb/indexx.html");
        }
    }
}
