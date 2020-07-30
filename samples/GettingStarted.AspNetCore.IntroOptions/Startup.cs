using Esquio.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GettingStarted.AspNetCore.IntroOptions
{
    public class Startup
    {
        IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEsquio(options =>
            {
                options.ConfigureDefaultProductName("default");
                options.ConfigureNotFoundBehavior(NotFoundBehavior.SetDisabled);
                options.ConfigureOnErrorBehavior(OnErrorBehavior.SetDisabled);
            })
            .AddAspNetCoreDefaultServices()
            .AddEndpointFallback(requestPath => new RequestDelegate(async context =>
            {
                //requestPath is "/"
                //you can use EndpointFallbackAction.NotFound or EndpointFallbackAction.RedirectAnyToAction, RedirectAnyTo instead create the request delegate directly
                await context.Response.WriteAsync("Hello World!, the feature is disabled and endpoint fallback is executed!");
            })) 
            .AddConfigurationStore(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                }).RequireFeature("NonExistingFeature");
            });
        }
    }
}
