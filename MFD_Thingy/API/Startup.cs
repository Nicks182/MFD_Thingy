using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Routing;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;

namespace MFD_Thingy.API
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddResponseCompression()
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(null, true));
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

        }


        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            EmbeddedFileProvider L_FileProvider = _Get_FileProvider();

            app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = L_FileProvider });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = L_FileProvider,
            });


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        private EmbeddedFileProvider _Get_FileProvider()
        {
            return new EmbeddedFileProvider(
                assembly: typeof(Startup).Assembly,
                baseNamespace: _Get_FP_Namespace());
        }

        private string _Get_FP_Namespace()
        {
            // This is so the File Provider knows where to get the Embedded files.
            // It's the namespace of your app with the folder path.
            // You can see this folder is specified with some wild cards when you Edit the Project File.
            return "MFD_Thingy.API.wwwroot";
        }

    }
}