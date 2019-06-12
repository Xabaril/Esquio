using Esquio.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WebApp.Services;
using WebApp.Toggles;

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
                        opts => { opts.ResourcesPath = "Resources"; })
                    .AddNewtonsoftJson()
                .Services
                .AddEsquio(setup => setup.RegisterTogglesFromAssemblyContaining<Startup>())
                    .AddAspNetCoreDefaultServices()
                    .AddConfigurationStore(Configuration, "Esquio")
                .Services
                .AddSingleton<IMatchService, MatchService>()
                .AddHttpClient()
                .AddTransient<ILocationProviderService, IPApiLocationProviderService>()
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

            app.UseCookiePolicy();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();
            app.UseCors();

            app.UseEndpoints(routes =>
            {
                routes.MapEsquio(pattern: "esquio");

                routes.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Match}/{action=Index}/{id?}");

                var convention = routes.MapControllerRoute(
                        name: "demo",
                        pattern: "demo/isworking").RequireFeature("SomeFeature");

                routes.MapRazorPages();
            });
        }
    }
}
