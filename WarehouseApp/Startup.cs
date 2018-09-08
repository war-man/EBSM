using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WarehouseApp.Startup))]
namespace WarehouseApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
