using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KeeleSystem.Startup))]
namespace KeeleSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
