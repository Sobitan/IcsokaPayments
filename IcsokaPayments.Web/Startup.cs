using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IcsokaPayments.Web.Startup))]
namespace IcsokaPayments.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
