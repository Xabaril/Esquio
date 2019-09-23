using Esquio;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddLocalization(options => options.ResourcesPath = "Resources")
                .AddMvc()
                    .AddViewLocalization(
                        LanguageViewLocationExpanderFormat.Suffix,
                        opts => { opts.ResourcesPath = "Resources"; });

            if (Configuration["EFStore"] != null)
            {
                //Use EF store

                services
                    .AddEsquio(setup => setup.RegisterTogglesFromAssemblyContaining<Startup>())
                        .AddAspNetCoreDefaultServices()
                        .AddEntityFrameworkCoreStore(options =>
                        {
                            options.ConfigureDbContext = (builder) =>
                            {
                                builder.UseSqlServer(Configuration.GetConnectionString("Esquio"));
                            };
                        })
                        .AddApplicationInsightProcessor();
            }
            else
            {
                //Use configuration store (appsettings.json| env var etc.. )

                services
                    .AddEsquio(setup => setup.RegisterTogglesFromAssemblyContaining<Startup>())
                        .AddAspNetCoreDefaultServices()
                        .AddConfigurationStore(Configuration, "Esquio")
                        .AddApplicationInsightProcessor();
            }

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
            //used to test Esquio DiagnosticSourceEvents
            //listener.Subscribe(new EsquioObserver());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCookiePolicy();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();
            app.UseCors();

            app.UseEndpoints(routes =>
            {
                routes.MapEsquio(pattern: "esquio").RequireFeature("HiddenGem");

                routes.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Match}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// Observer used to test Esquio DiagnosticSource events
        /// </summary>
        private class EsquioObserver
            : IObserver<KeyValuePair<string, object>>
        {
            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnNext(KeyValuePair<string, object> item)
            {
                var isEndEvent = item.Key.Contains(EsquioConstants.ESQUIO_BEGINFEATURE_ACTIVITY_NAME);

                if (isEndEvent)
                {
                    var value = item.Value;
                }
            }
        }
    }
}
