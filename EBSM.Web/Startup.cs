using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EBSM.Web.Startup))]
namespace EBSM.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
