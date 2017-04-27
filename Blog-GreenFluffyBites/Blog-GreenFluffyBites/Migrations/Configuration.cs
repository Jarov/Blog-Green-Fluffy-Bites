namespace Blog_GreenFluffyBites.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Blog_GreenFluffyBites.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<Blog_GreenFluffyBites.Models.BlogDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
        protected override void Seed(Blog_GreenFluffyBites.Models.BlogDBContext context)
        {
           /* var adminEmail = "admin@admin.com";
            var adminFullName = "Admin";
            var adminPassword = "1234";
            string adminRole = "Administrator";

            CreateAdminUser(context, adminEmail, adminFullName, adminPassword, adminRole); */
        }


            /* ContextKey = "Blog_GreenFluffyBites.Models.BlogDBContext";
         }

         protected override void Seed(Blog_GreenFluffyBites.Models.BlogDBContext context)
         {

             if (!context.Roles.Any())
             {
                 this.CreateRole("Admin", context);
                 this.CreateRole("User", context);

             }

             if (!context.Users.Any())
             {

                 this.CreateUser("admin@admin.com", "Admin", "1234", context);
                 this.SetUserRole("admin@admin.com", "Admin", context);

             }

         }
         private void CreateRole(string roleName, BlogDBContext context)
         {
             var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

             var result = roleManager.Create(new IdentityRole(roleName));

             if (!result.Succeeded)
             {
                 throw new Exception(string.Join(";", result.Errors));

             }

         }

         private void SetUserRole(string email, string role, BlogDBContext context)
         {
             var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

             var userId = context.Users.FirstOrDefault(u => u.Email.Equals(email)).Id;
             var result = userManager.AddToRole(userId, role);

             if (!result.Succeeded)
             {
                 throw new Exception(string.Join(";", result.Errors));

             }


         }

         private void CreateUser(string email, string fullName, string password, BlogDBContext context)
         {

             var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
             userManager.PasswordValidator = new PasswordValidator()
             {
                 RequireDigit = false,
                 RequiredLength = 4,
                 RequireLowercase = false,
                 RequireNonLetterOrDigit = false,
                 RequireUppercase = false
             };
             var user = new ApplicationUser()
             {
                 Email = email,
                 FullName = fullName,
                 UserName = email
             };

             // Setting the password from the UserManager rather than storing it in the db as plain string
             var result = userManager.Create(user, password);

             if (!result.Succeeded)
             {
                 throw new Exception(string.Join(";", result.Errors));
             }
         } */
        }
}


