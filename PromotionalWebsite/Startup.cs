using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PromotionalWebsite.Startup))]
namespace PromotionalWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
