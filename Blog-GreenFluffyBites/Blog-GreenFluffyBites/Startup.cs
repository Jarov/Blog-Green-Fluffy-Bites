using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Blog_GreenFluffyBites.Startup))]
namespace Blog_GreenFluffyBites
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
