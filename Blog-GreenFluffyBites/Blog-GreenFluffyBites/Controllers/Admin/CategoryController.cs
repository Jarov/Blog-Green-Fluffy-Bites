using Blog_GreenFluffyBites.Extensions;
using Blog_GreenFluffyBites.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Blog_GreenFluffyBites.Controllers.Admin
{
    public class CategoryController : Controller
    {
        //
        // GET: Category
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        //GET: Category/List
        public ActionResult List()
        {
            using (var database = new BlogDBContext())
            {
                var categories = database.Categories
                    .ToList();
                return View(categories);
            }
        }

        //
        //GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: Category/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDBContext())
                {
                    database.Categories.Add(category);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            this.AddNotification("You have created the Category!", NotificationType.SUCCESS);

            return View(category);
        }

        //
        // GET: Category/Edit
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDBContext())
            {

                var category = database.Categories
                    .FirstOrDefault(c => c.Id == id);
                if (category == null)
                {
                    return HttpNotFound();
                }

                this.AddNotification("You are about to edit the Category!", NotificationType.WARNING);

                return View(category);
            }
        }

        //
        // POST: Category/Edit
        [HttpPost]
        public ActionResult Edit(Category category)
        {

            if (ModelState.IsValid)
            {
                using (var database = new BlogDBContext())
                {

                    database.Entry(category).State = EntityState.Modified;
                    database.SaveChanges();

                    this.AddNotification("You have edited the Category!", NotificationType.SUCCESS);

                    return RedirectToAction("Index");
                }
            }

            return View(category);
        }

        //
        // GET: Category/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDBContext())
            {
                var category = database.Categories
                  .FirstOrDefault(c => c.Id == id);
                if (category == null)
                {
                    return HttpNotFound();
                }

                this.AddNotification("You are about to delete the Category!", NotificationType.WARNING);

                return View(category);
            }
        }

        //
        // POST: Category/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {


            using (var database = new BlogDBContext())
            {

                var category = database.Categories
                   .FirstOrDefault(c => c.Id == id);

                var categoryArticles = category.Articles.ToList();

                foreach (var article in categoryArticles)
                {
                    database.Articles.Remove(article);
                }


                database.Categories.Remove(category);
                database.SaveChanges();

                this.AddNotification("You have deleted the Category!", NotificationType.SUCCESS);

                return RedirectToAction("Index");
            }
        }
    }
}