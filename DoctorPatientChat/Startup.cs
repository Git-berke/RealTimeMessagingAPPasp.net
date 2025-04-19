using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(DoctorPatientChat.Startup))]

namespace DoctorPatientChat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // SignalR'ı yapılandır
            app.MapSignalR();
        }
    }
}