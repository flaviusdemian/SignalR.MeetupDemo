using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using SignalRDemo;
using SignalRDemo.Infrastructure;

[assembly: OwinStartup(typeof(SignalRDemo.Startup))]
namespace SignalRDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.HubPipeline.AddModule(new LoggingPipelineModule());
            GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());
            app.MapSignalR();
        }
    }
}