using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MFD_Thingy.APP.Services;

namespace MFD_Thingy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider;
        public App()
        {
            LoadDepedencies();
            Init();
        }

        private void Init()
        {

            var serverService = App.ServiceProvider.GetService(typeof(ServerService)) as ServerService;
            //serverService.RestartServer();
        }
        public static ServiceCollection Services { get; set; } = new ServiceCollection();
        private void LoadDepedencies()
        {


            Services.AddSingleton<ServerService>();
            Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            Services.AddSingleton<MessageBusService>();

            App.ServiceProvider = Services.BuildServiceProvider();

        }
    }

    public static class G_GlobalSettings
    {
        public static bool ShowStatic = false;
    }
}
