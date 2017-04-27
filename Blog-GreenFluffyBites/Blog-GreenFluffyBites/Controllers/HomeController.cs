using System.Web.Mvc;

namespace Blog_GreenFluffyBites.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("List", "Article");
        }

    }
}