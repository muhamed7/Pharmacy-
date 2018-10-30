using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(pharmacy.Startup))]
namespace pharmacy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
