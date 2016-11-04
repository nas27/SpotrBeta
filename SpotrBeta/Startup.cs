using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SpotrBeta.Startup))]
namespace SpotrBeta
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
