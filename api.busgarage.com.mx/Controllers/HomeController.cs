using System.IO;
using System.Web.Mvc;
using api.busgarage.com.mx.Entity;

namespace api.busgarage.com.mx.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
