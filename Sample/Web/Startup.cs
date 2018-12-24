using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGame;
using OddMud.Transport.SignalR;
using OddMud.Web.Game;
using OddMud.Web.Hubs;
using OddMud.Web.Game.Database;
using Microsoft.AspNetCore.Http;
using OddMud.SampleGame.GameModules;
using OddMud.ViewConverters.MudLikeHtml;
using OddMud.SampleGame.GameModules.Combat;

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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(typeof(FilePluginLoader<>));
            services.AddSingleton(typeof(GameHubProcessor));
            services.AddSingleton<IWorld, GridWorld>();
            services.AddSingleton<IViewConverter<string>, MudLikeHtmlConverter>();
            services.AddSingleton<IHostedService, GameService>();
            services.AddSingleton<ITransport, SignalRHubTransport<GameHub>>();
            services.AddSingleton<IGameModule<CombatModule>, CombatModule>();
            services.AddSingleton<IGameModule<CombatModule>, CombatModule>();
            services.AddSingleton<IStorage, GameStorage>();
            services.AddSingleton(typeof(CombatModuleSettings));
            services.AddSingleton<IGame, GridGame>();
            
            services.AddSignalR()
                .AddMessagePackProtocol();

            services.AddDbContext<GameDbContext>();



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
