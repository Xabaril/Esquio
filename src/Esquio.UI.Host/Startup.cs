using Esquio.UI.Api;
using Esquio.UI.Api.Infrastructure.HostedService;
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
using Microsoft.IdentityModel.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Net.Mime;

namespace Esquio.UI.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors()
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>()
                .AddSingleton<IDiscoverToggleTypesService, DiscoverToggleTypesService>()
                .AddResponseCompression(options =>
                {
                    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { MediaTypeNames.Application.Octet });
                })
                .AddSwaggerGen()
                .AddApplicationInsightsTelemetry()
                .AddAuthentication(options =>
                 {
                     options.DefaultScheme = "secured";
                     options.DefaultChallengeScheme = "secured";
                 })
                .AddApiKey()
                .AddJwtBearer(options =>
                {
                    Configuration.Bind("Security:OpenId", options);

                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                })
                .AddPolicyScheme("secured", "Authorization Bearer or ApiKey", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var bearer = context.Request
                            .Headers["Authorization"]
                            .FirstOrDefault();

                        if (bearer != null && bearer.StartsWith(JwtBearerDefaults.AuthenticationScheme))
                        {
                            return JwtBearerDefaults.AuthenticationScheme;
                        }

                        return ApiKeyAuthenticationDefaults.ApiKeyScheme;
                    };
                });

            EsquioUIApiConfiguration.ConfigureServices(services)
                .AddEntityFramework(Configuration, Environment)
                .AddHostedService<EsquioMetricsConsumer>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersion)
        {
            EsquioUIApiConfiguration.Configure(app,
                preConfigure: appBuilder =>
                 {
                     appBuilder.AddClientBlazorConfiguration();
                     appBuilder.UseResponseCompression();

                     if (env.IsDevelopment())
                     {
                         appBuilder.UseDeveloperExceptionPage();
                         appBuilder.UseWebAssemblyDebugging();
                     }

                     return appBuilder
                        .UseCors(builder=>
                         {
                             var configuredOrigins = Configuration.GetValue<string>("Cors:Origins");

                             if (!string.IsNullOrEmpty(configuredOrigins))
                             {
                                 var allowedOrigins = configuredOrigins.Split(',');

                                 builder.WithOrigins(allowedOrigins)
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                             }

                         }).UseDefaultFiles().UseStaticFiles();
                 },
                postConfigure: appBuilder =>
                 {
                     return appBuilder.UseAuthentication()
                     .UseAuthorization()
                     .UseBlazorFrameworkFiles()
                     .UseSwagger()
                     .UseSwaggerUI(setup =>
                     {
                         setup.EnableDeepLinking();

                         foreach (var description in apiVersion.ApiVersionDescriptions)
                         {
                             setup.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                         }
                     });
                 });
        }
    }


}
