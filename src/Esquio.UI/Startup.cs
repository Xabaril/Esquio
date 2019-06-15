using Esquio.UI.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Esquio.UI
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
            //services.AddAuthentication()
            //    .AddApiKey();

            EsquioUIApiConfiguration.ConfigureServices(services)
                .AddDbContext<StoreDbContext>(options =>
                {
                    options.UseSqlServer("");
                })
                .AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "ClientApp/dist";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            EsquioUIApiConfiguration.Configure(app, host =>
            {
                host
                    .AddIfElse(
                        env.IsDevelopment(),
                        x => x.UseDeveloperExceptionPage(),
                        x => x.UseExceptionHandler("/Error").UseHsts()
                    )
                    .UseHttpsRedirection()
                    .UseStaticFiles()
                    .UseSpaStaticFiles();

                host
                    .UseMvc(routes =>
                    {
                        routes.MapRoute(
                                  name: "default",
                                  template: "{controller}/{action=Index}/{id?}");
                    })
                    .UseSpa(spa =>
                    {
                        spa.Options.SourcePath = "ClientApp";

                        if (env.IsDevelopment())
                        {
                            spa.UseReactDevelopmentServer(npmScript: "start");
                        }
                    });

                return host;
            });
        }
    }
}
