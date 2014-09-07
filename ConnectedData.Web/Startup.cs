using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ConnectedData.Web.Startup))]
namespace ConnectedData.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
