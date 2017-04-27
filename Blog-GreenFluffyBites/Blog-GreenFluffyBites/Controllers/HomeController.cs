using Blog_GreenFluffyBites.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Web.Mvc;

namespace Blog_GreenFluffyBites.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("List", "Article");
        }

        public FileContentResult DisplayProfilePicture()
        {

            if (User.Identity.IsAuthenticated)
            {
                var store = new UserStore<ApplicationUser>(new BlogDBContext());
                var userManager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;

                var db = new BlogDBContext();
                var u = db.Users.Find(user.Id);

                var userImage = u.ProfilePicture;


                if (userImage == null)
                {
                    CreateDefaultProfilePic();
                }
                else
                {

                    return new FileContentResult(userImage, "image/jpeg");

                }
            }


            // Create a new default profile picture if the user is not logged in.
            return CreateDefaultProfilePic();

        }

        public FileContentResult CreateDefaultProfilePic()
        {
            string fileName = HttpContext.Server.MapPath(@"~/Content/Images/noimg.jpg");

            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(fileName);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);

            return File(imageData, "image/png");
        }

    }
}