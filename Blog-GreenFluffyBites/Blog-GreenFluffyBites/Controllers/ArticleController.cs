using Blog_GreenFluffyBites.Models;
using Notification.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog_GreenFluffyBites.Controllers
{
    public class ArticleController : Controller
    {

        // GET Article
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: Article/List
        public ActionResult List()
        {
            using (var database = new BlogDBContext()) {

                var articles = database.Articles.Include(a => a.Author).ToList();

                return View(articles);
            }
        }

        // GET: Article/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDBContext())
            {
                // Get article from DB
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .First();

                if (article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }
        }

        // GET: Article Create
        [Authorize]
        public ActionResult Create()
        {
            return View();       
        }

        // POST: Article Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Article article, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                // Insert article in DB

                using (var database = new BlogDBContext())
                {
                    var authorId = database.Users.Where(u => u.UserName == this.User.Identity.Name).First().Id;

                    article.AuthorId = authorId;

                    article.UsersLikesIDs = "";

                    article.DatePosted = new DateTime();
                    article.DatePosted = DateTime.Now;

                    article.Score = 0;

                    if (image != null)
                    {
                        var allowedContentTypes = new[] { "image/jpg", "image/jpeg", "image/png" };

                        if (allowedContentTypes.Contains(image.ContentType))
                        {
                            var imagesPath = "/Content/Images";

                            var filename = image.FileName;

                            var uploadPath = imagesPath + filename;
                            var physicalPath = Server.MapPath(uploadPath);
                            image.SaveAs(physicalPath);

                            article.ImagePath = uploadPath;
                        }
                    }

                    database.Articles.Add(article);
                    database.SaveChanges();


                    return RedirectToAction("Index");
                }

            }


            return View(article);
        }

        //GET: Article Delete
        [Authorize]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDBContext())
            {
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .First();

                if (!IsUserAuthorizedToEdit(article))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }
        }

        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDBContext())
            {
                var article = database.Articles.Where(a => a.Id == id).Include(a => a.Author).First();

                if (article == null)
                {                 
                    return HttpNotFound();
                }

                database.Articles.Remove(article);
                database.SaveChanges();

                return RedirectToAction("Index");
            }

        }

        // GET: Article/Edit
        [Authorize]      
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDBContext())
            {
                var article = database.Articles.Where(a => a.Id == id).First();

                if (!IsUserAuthorizedToEdit(article))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (article == null)
                {
                    return HttpNotFound();
                }

                var model = new ArticleViewModel();
                model.Id = article.Id;
                model.Title = article.Title;
                model.Content = article.Content;
                return View(model);
            }
        }

        //POST: Article Edit
        [Authorize]
        [HttpPost]
        public ActionResult Edit(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDBContext())
                {
                    var article = database.Articles.FirstOrDefault(a => a.Id == model.Id);

                    article.Title = model.Title;
                    article.Content = model.Content;

                    database.Entry(article).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Details", new { id = article.Id });
                }
            }
            return View(model);
        }

        private bool IsUserAuthorizedToEdit(Article article)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = article.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }

        [HttpPost]
        public ActionResult LikeArticle(int? id, Article model)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            using (var database = new BlogDBContext())
            {
                var currentUserID = database.Users.First(u => u.UserName == this.User.Identity.Name).Id;

                var currentPostID = database.Articles.First(p => p.Id == id);

                if (!currentPostID.UsersLikesIDs.Contains(currentUserID))
                {
                    currentPostID.UsersLikesIDs += currentUserID;
                    currentPostID.Score += 1;

                    database.Entry(currentPostID).State = EntityState.Modified;
                    database.SaveChanges();

                    this.AddNotification("Post liked!", NotificationType.SUCCESS);
                }

                return RedirectToAction("List");

            }

        }

    }
}