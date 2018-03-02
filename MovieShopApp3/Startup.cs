using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieShopApp3.Startup))]
namespace MovieShopApp3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
