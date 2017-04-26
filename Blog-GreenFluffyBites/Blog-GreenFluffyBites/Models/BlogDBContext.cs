using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Blog_GreenFluffyBites.Models
{

    public class BlogDBContext : IdentityDbContext<ApplicationUser>
    {
        public BlogDBContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Article> Articles { get; set; }

        public static BlogDBContext Create()
        {
            return new BlogDBContext();
        }
    }
}