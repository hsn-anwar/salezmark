using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(B2B.Startup))]
namespace B2B
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
