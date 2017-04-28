using Blog_GreenFluffyBites.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog_GreenFluffyBites.Extensions;
using Microsoft.AspNet.Identity;

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
        public ActionResult List(string sortOrder, int? id)
        {

            ViewBag.TitleSortParm = "name_order";
            ViewBag.ScoreSortParm = "score_order";
            ViewBag.DateSortParm = "new_posts_first";
            using (var db = new BlogDBContext())
            {
                var posts = db.Articles.
                     Include(a => a.Author);

                switch (sortOrder)
                {

                    case "new_posts_first":
                        posts = posts.OrderByDescending(s => s.DatePosted);

                        break;
                    case "name_order":
                        posts = posts.OrderBy(s => s.Title);
                        break;

                    case "score_order":
                        posts = posts.OrderByDescending(s => s.Score);
                        break;
                    default:
                        posts = posts.OrderBy(s => s.DatePosted);
                        break;
                }


                var dbCategories = db.Categories.ToList();
                Dictionary<int, string> categories = new Dictionary<int, string>();

                foreach (var category in dbCategories)
                {
                    categories[category.Id] = category.Name;

                }

                ViewBag.Categories = categories;

                if (id != null)
                {
                    //The user clicked on a category, show only the posts in the same one

                    var categoryPosts = db.Articles.Where(t => t.CategoryId == id).Include(a => a.Author);

                    ViewBag.TitleSortParm = "name_order";
                    ViewBag.ScoreSortParm = "score_order";
                    ViewBag.DateSortParm = "new_posts_first";

                    switch (sortOrder)
                    {

                        case "new_posts_first":
                            categoryPosts = categoryPosts.OrderByDescending(s => s.DatePosted);

                            break;
                        case "name_order":
                            categoryPosts = categoryPosts.OrderBy(s => s.Title);
                            break;

                        case "score_order":
                            categoryPosts = categoryPosts.OrderByDescending(s => s.Score);
                            break;
                        default:
                            categoryPosts = categoryPosts.OrderBy(s => s.DatePosted);
                            break;
                    }



                    return View(categoryPosts.ToList());
                }


                return View(posts.ToList());

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
                    .Include(p => p.Comments.Select(c => c.Author))
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
            //check if we have error messages from our last attempt to create a post
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"].ToString();
            }


            using (var databse = new BlogDBContext())
            {
                var model = new ArticleViewModel();
                model.Categories = databse.Categories
                    .OrderBy(c => c.Name)
                    .ToList();
                return View(model);
            }
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
                    var author = database.Users.Where(u => u.UserName == this.User.Identity.Name).First();                  
                    var textPost = new Article(author.Id, article.Title, article.Content, article.CategoryId);

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

                    this.AddNotification("Article created", NotificationType.SUCCESS);

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
                    .Include(c => c.Category)
                    .First();

                if (!IsUserAuthorizedToEdit(article))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (article == null)
                {
                    return HttpNotFound();
                }
                this.AddNotification("Article will be deleted.", NotificationType.WARNING);
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

                this.AddNotification("Article deleted", NotificationType.SUCCESS);

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
                this.AddNotification("Article will be edited.", NotificationType.WARNING);
                var model = new ArticleViewModel();
                model.Id = article.Id;
                model.Title = article.Title;
                model.Content = article.Content;
                model.CategoryId = article.CategoryId;
                model.Categories = database.Categories.OrderBy(c => c.Name).ToList();
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
                    article.CategoryId = model.CategoryId;
                    
                    database.Entry(article).State = EntityState.Modified;
                    database.SaveChanges();

                    this.AddNotification("Article edited", NotificationType.SUCCESS);

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

                return RedirectToAction($"Details/{id}");

            }

        }

        [HttpPost]
        [Authorize]
        public ActionResult PostComment(CommentViewModel commentModel)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDBContext())
                {
                    var username = this.User.Identity.GetUserName();
                    var userId = this.User.Identity.GetUserId();

                    database.Comments.Add(new Comment()
                    {
                        AuthorId = userId,
                        content = commentModel.content,
                        ArticleId = commentModel.ArticleId,
                    });

                    database.SaveChanges();

                    return RedirectToAction("Details", new { id = commentModel.ArticleId });
                }
            }

            return RedirectToAction("Details", new { id = commentModel.ArticleId });

        }

    }
}