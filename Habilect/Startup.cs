using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Habilect.Startup))]
namespace Habilect
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
