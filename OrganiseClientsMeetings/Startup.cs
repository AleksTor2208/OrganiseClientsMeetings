using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OrganiseClientsMeetings.Startup))]
namespace OrganiseClientsMeetings
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
