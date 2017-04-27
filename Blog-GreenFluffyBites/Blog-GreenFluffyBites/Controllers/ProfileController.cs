using Blog_GreenFluffyBites.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
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

        //GET
        [HttpGet]
        public ActionResult ChangeProfilePicture()
        {
            return View();
        }

        //POST 
        [HttpPost]
        public ActionResult ChnageProfilePicture()
        {
            //This works only if you are logged in

            if (User.Identity.IsAuthenticated)
            {
                // To convert the user uploaded Photo as Byte Array before save to DB
                byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["ProfilePicture"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);
                    }
                }

                var store = new UserStore<ApplicationUser>(new BlogDBContext());
                var userManager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;


                var db = new BlogDBContext();
                var u = db.Users.Find(user.Id);

                if (u != null)
                {
                    u.ProfilePicture = imageData;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Profile");

        }
    }
}