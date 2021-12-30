using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Logging;

using MFD_Thingy.API;

namespace MFD_Thingy.APP.Services
{
    public class ServerService
    {
        public bool G_Enabled = false;
        public MessageBusService messageBusService;

        public ServerService(MessageBusService messageBusService)
        {
            this.messageBusService = messageBusService;
        }

        

        private string serverStatus;

        private IWebHost server = null;
        public void RestartServer()
        {
            serverStatus = "Checking...";
            this.messageBusService.Emit("serverstatuschanged", serverStatus);

            StopServer();

            if (G_Enabled == true)
            {
                return;
            }


            
            this.server = WebHost.CreateDefaultBuilder()
                .UseKestrel(x =>
                {
                    x.ListenAnyIP(5000);
                    x.ListenLocalhost(5000);
                    
                })
                .UseStartup<Startup>()
                //.UseUrls("http://*:5000")
                .Build();

            

            serverStatus = "Starting";


            this.messageBusService.Emit("serverstatuschanged", serverStatus);

            Task.Run(() =>
            {
                Thread.Sleep(3000);
                server.RunAsync();
                serverStatus = "Running...";
                this.messageBusService.Emit("serverstatuschanged", serverStatus);

            });



        }



        public void StopServer()
        {
            if (this.server != null)
            {
                serverStatus = "Shutting down";
                this.messageBusService.Emit("serverstatuschanged", serverStatus);
                this.server.StopAsync().Wait();

            }
            Thread.Sleep(3000);
            serverStatus = "Down";
            this.messageBusService.Emit("serverstatuschanged", serverStatus);

        }

        public string GetServerStatus()
        {
            return serverStatus;
        }

    }
}
