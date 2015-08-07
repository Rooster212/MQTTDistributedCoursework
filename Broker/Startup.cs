using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

[assembly: OwinStartup(typeof(BrokerApp.Startup))]
namespace BrokerApp
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.UseFileServer(new FileServerOptions()
            {
                RequestPath = PathString.Empty,
                FileSystem = new PhysicalFileSystem(ConfigurationManager.AppSettings["webFileSystemLocation"])
            });

            var hubConfig = new HubConfiguration 
            {
                EnableDetailedErrors = true, 
                EnableJSONP = true
            };
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR(hubConfig);
        }
    }
}
