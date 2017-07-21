using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FourthMLInsights.Startup))]
namespace FourthMLInsights
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
