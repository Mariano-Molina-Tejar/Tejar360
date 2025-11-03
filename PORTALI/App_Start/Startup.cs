using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PORTALI.App_Start.Startup))]
namespace PORTALI.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configuración de SignalR
            app.MapSignalR();
        }
    }
}