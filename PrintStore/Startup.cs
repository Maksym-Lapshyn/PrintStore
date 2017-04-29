using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PrintStore.Startup))]
namespace PrintStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
