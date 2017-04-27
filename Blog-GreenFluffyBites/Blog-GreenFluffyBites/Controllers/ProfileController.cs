using Blog_GreenFluffyBites.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Blog_GreenFluffyBites.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            using (var database = new BlogDBContext())
            {
                var authorId = database.Users.Where(u => u.UserName == this.User.Identity.Name).First().Id;

                var articles = database.Articles.Include(a => a.Author).Where(a => a.Author.Id == authorId).ToList();

                return View(articles);
            }
        }
    }
}