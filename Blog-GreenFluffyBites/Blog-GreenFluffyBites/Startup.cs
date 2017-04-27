using Blog_GreenFluffyBites.Migrations;
using Blog_GreenFluffyBites.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(Blog_GreenFluffyBites.Startup))]
namespace Blog_GreenFluffyBites
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlogDBContext, Configuration>());

            ConfigureAuth(app);
        }

    }
}
