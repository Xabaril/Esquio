using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GettingStarted.AspNetCore.Mvc
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
            services
                .AddEsquio(setup=>
                {
                    // Use request cache to store feature evaluation results and re-use this results on the scope
                    // In this sample ( privacy feature is evaluated on _layout.cshtml ( using a taghelper ) and HomeController
                    // Privacy action with FeatureFilter attribute, when useScopedEvaluation is true, feature evaluation only happens once!

                    setup.UseScopedEvaluation(useScopedEvaluation: true);
                })
                .AddAspNetCoreDefaultServices()
                .AddConfigurationStore(Configuration)
                .Services
                .AddControllersWithViews();
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
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}").RequireFeature("HiddenGem");
            });
        }
    }
}
