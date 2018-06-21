using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SelectorComapat.Startup))]
namespace SelectorComapat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
