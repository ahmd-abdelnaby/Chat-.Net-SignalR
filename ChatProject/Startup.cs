using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChatProject.Startup))]
namespace ChatProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration { EnableDetailedErrors = true };
            
            ConfigureAuth(app);
            app.MapSignalR(hubConfiguration);
        }
    }
}
