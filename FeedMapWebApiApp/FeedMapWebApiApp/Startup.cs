using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FeedMapDAL;
using AutoMapper;

namespace FeedMapWebApiApp
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
            //Enable AutoMapper with AutoMapperCongif Obj for configuration.
            var mapConfig = new AutoMapperConfig();

            //Prepare DI Container.
            DIConfig dIConfig = new DIConfig(Configuration, mapConfig);
            dIConfig.ImplDI(services);

            //Dependency Inject ClientAuthConfigObj that takes data from appsettings and 
            //gets wraped by IOptions.
            services.AddOptions();
            services.Configure<ClientAuthConfigObj>(Configuration.GetSection("ClientKeyAuth"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}
