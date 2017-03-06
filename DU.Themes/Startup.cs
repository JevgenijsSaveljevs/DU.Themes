using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DU.Themes.Startup))]
namespace DU.Themes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
