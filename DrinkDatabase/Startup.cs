using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DrinkDatabase.Startup))]
namespace DrinkDatabase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
