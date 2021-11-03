using Esquio.UI.Api;
using Esquio.UI.Api.Infrastructure.HostedService;
using Esquio.UI.Api.Infrastructure.Services;
using Esquio.UI.Api.Infrastructure.Settings;
using Esquio.UI.Deployment.Infrastructure.OpenApi;
using Esquio.UI.Deployment.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Net.Mime;

namespace Esquio.UI.Deployment.Setup
{
    public static class EsquioServiceCollectionExtensions
    {
        public static IServiceCollection AddEsquioServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services
                .AddHttpClient()
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
                    var securitySettings = new SecuritySettings();
                    configuration.Bind("Security", securitySettings);

                    options.Audience = securitySettings.OpenId.Audience;
                    options.Authority = securitySettings.OpenId.Authority;

                    options.TokenValidationParameters.ValidateIssuer = false;
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

            EsquioUIApiConfiguration.ConfigureServices(services, configuration)
                .AddEntityFramework(configuration, environment)
                .AddHostedService<EsquioMetricsConsumer>();

            return services;
        }
    }
}
