using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using UserAgentToggles;
using WebApp.Services;
namespace WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //add MVC 
            services.AddLocalization(options => options.ResourcesPath = "Resources")
                .AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = "Resources"; });

            //add mini-profiler and esquio profiling
            services
                .AddMiniProfiler(options =>
                {
                    options.RouteBasePath = "/profiler";
                    options.EnableServerTimingHeader = true;

                    options.ResultsAuthorize = (_) => true;
                    options.ShouldProfile = _ => true;

                    options.IgnoredPaths.Add("/lib");
                    options.IgnoredPaths.Add("/css");
                    options.IgnoredPaths.Add("/js");
                    options.IgnoredPaths.Add("/assets");

                }).AddEsquio();

            if (Configuration["EFStore"] != null)
            {
                //Use EF store
                AddEsquioWithEntityFrameworkCoreStore(services);
            }
            else
            {
                //Use configuration store (appsettings.json| env var etc.. )
                AddEsquioWithConfigurationStore(services);
            }

            //Add custom services and authentication
            services
                .AddSingleton<IMatchService, MatchService>()
                .AddAuthentication(setup =>
                {
                    setup.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    setup.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    setup.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    setup.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, setup =>
                {
                    setup.LoginPath = "/account/login";
                });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DiagnosticListener listener)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiniProfiler()
                .UseCookiePolicy()
                .UseStaticFiles()
                .UseAuthentication()
                .UseRouting()
                .UseAuthorization()
                .UseCors();

            app.UseEndpoints(routes =>
            {
                routes.MapEsquio(pattern: "esquio").RequireFeature("HiddenGem");

                routes.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Match}/{action=Index}/{id?}");
            });
        }

        private IServiceCollection AddEsquioWithEntityFrameworkCoreStore(IServiceCollection services)
        {
            return services
                .AddEsquio(setup =>
                {
                    //esquio constribution toggles on https://github.com/xabaril/esquio.contrib 
                    setup.RegisterTogglesFromAssemblyContaining<UserAgentBrowserToggle>();
                })
                .AddAspNetCoreDefaultServices()
                .AddEntityFrameworkCoreStore(options =>
                {
                    options.ConfigureDbContext = (builder) =>
                    {
                        builder.UseSqlServer(Configuration.GetConnectionString("Esquio"));
                    };
                })
                .AddApplicationInsightProcessor()
                .Services;
        }

        private IServiceCollection AddEsquioWithConfigurationStore(IServiceCollection services)
        {
            return services
                .AddEsquio(setup =>
                {
                    //esquio constribution toggles on https://github.com/xabaril/esquio.contrib 
                    setup.RegisterTogglesFromAssemblyContaining<UserAgentBrowserToggle>();
                })
                .AddAspNetCoreDefaultServices()
                .AddConfigurationStore(Configuration, "Esquio")
                .AddApplicationInsightProcessor()
                .Services;
        }
    }
}
