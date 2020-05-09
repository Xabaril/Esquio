using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
            //add application default services
            services
                .AddApplicationInsightsTelemetry()
                .AddLocalization(options => options.ResourcesPath = "Resources")
                .AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = "Resources"; })
                .Services
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

                }).AddEsquioProfiler();

            //add and configure esquio
            services
                .AddEsquio(setup=>
                {
                    setup.UseScopedEvaluation(useScopedEvaluation: true);
                })
                .AddHttpStore(setup =>
                {
                    setup
                         .UseBaseAddress(Configuration["EsquioHttpStore:BaseAddress"])
                         .UseApiKey(Configuration["EsquioHttpStore:ApiKey"])
                         .UseCache(enabled: true, absoluteExpirationRelativeToNow: TimeSpan.FromSeconds(10));
                })
                .AddAspNetCoreDefaultServices()
                .AddApplicationInsightProcessor();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app
                .UseMiniProfiler()
                .UseCors()
                .UseCookiePolicy()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(routes =>
                {
                    routes.MapEsquio().RequireFeature("HiddenGem");

                    routes.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Match}/{action=Index}/{id?}");
                });
        }
    }
}
