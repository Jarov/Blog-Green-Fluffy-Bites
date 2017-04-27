using Blog_GreenFluffyBites.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Blog_GreenFluffyBites.Controllers.Admin
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        

        //GET: User/Delete
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new BlogDBContext())
            {

                var user = db.Users.Where(u => u.Id.Equals(id)).First();

                if (user == null)
                {
                    return HttpNotFound();
                }


                return View(user);
            }

        }

        //POST: User/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new BlogDBContext())
            {

                var user = db.Users.Where(u => u.Id.Equals(id)).First();

                var userPosts = db.Articles.Where(a => a.AuthorId.Equals(user.Id));

                var userComments = db.Comments.Where(c => c.AuthorId.Equals(user.Id));

                foreach (var post in userPosts)
                {
                    db.Articles.Remove(post);
                }
                foreach (var comment in userComments)
                {
                    db.Comments.Remove(comment);
                }

                db.Users.Remove(user);
                db.SaveChanges();

                return RedirectToAction("List");
            }

        }
    }
}