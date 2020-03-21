using Esquio.UI.Api;
using Esquio.UI.Api.Infrastructure.Services;
using Esquio.UI.Host.Infrastructure.OpenApi;
using Esquio.UI.Host.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Net.Mime;

namespace Esquio.UI.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>()
                .AddSingleton<IDiscoverToggleTypesService, DiscoverToggleTypesService>()
                .AddResponseCompression(options =>
                {
                    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { MediaTypeNames.Application.Octet });
                });

            services
                 .AddSwaggerGen()
                 .AddAuthentication(options =>
                 {
                     options.DefaultScheme = "secured";
                     options.DefaultChallengeScheme = "secured";
                 })
                .AddApiKey()
                .AddJwtBearer(options =>
                {
                    Configuration.Bind("Security:Jwt", options);
                })
                .AddPolicyScheme("secured", "Authorization Bearer or ApiKey", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var bearer = context.Request.Headers["Authorization"].FirstOrDefault();

                        if (bearer != null && bearer.StartsWith(JwtBearerDefaults.AuthenticationScheme))
                        {
                            return JwtBearerDefaults.AuthenticationScheme;
                        }

                        return ApiKeyAuthenticationDefaults.ApiKeyScheme;
                    };
                });

            EsquioUIApiConfiguration.ConfigureServices(services, Environment)
                .AddEntityFramework(Configuration["ConnectionStrings:Esquio"]);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersion)
        {
         
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseBlazorFrameworkFiles();

            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.EnableDeepLinking();

                foreach (var description in apiVersion.ApiVersionDescriptions)
                {
                    setup.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
