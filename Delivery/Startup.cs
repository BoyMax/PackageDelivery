using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Delivery.Startup))]
namespace Delivery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
