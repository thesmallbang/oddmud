using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.BasicGame;
using OddMud.Transport.SignalR;
using OddMud.Web.Game;
using OddMud.Web.Hubs;

namespace OddMud.Web
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public static IServiceCollection ServiceCollection { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ServiceCollection = services;
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSingleton(typeof(FilePluginLoader<>));
            services.AddSingleton(typeof(GameHubProcessor));
            services.AddSingleton<IWorld, BasicWorld>();
            services.AddSingleton<IHostedService, GameService>();
            services.AddSingleton<ITransport, SignalRHubTransport<GameHub>>();
            services.AddSingleton(typeof(BasicGame.Game));
            

            services.AddSignalR()
                .AddMessagePackProtocol();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();


            app.UseSignalR(routes =>
            {
                routes.MapHub<GameHub>("/gameHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
