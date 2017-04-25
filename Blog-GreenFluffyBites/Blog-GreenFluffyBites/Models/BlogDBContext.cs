using Microsoft.AspNet.Identity.EntityFramework;

namespace Blog_GreenFluffyBites.Models
{

    public class BlogDBContext : IdentityDbContext<ApplicationUser>
    {
        public BlogDBContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static BlogDBContext Create()
        {
            return new BlogDBContext();
        }
    }
}