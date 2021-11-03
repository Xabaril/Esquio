using Esquio.UI.Api.Infrastructure.Settings;
using Esquio.UI.Api.Shared.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Esquio.UI.Deployment.Infrastructure.Middleware
{
    internal class BlazorClientConfigurationMiddleware
    {
        private readonly RequestDelegate _next;

        public BlazorClientConfigurationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, IOptions<SecuritySettings> settings)
        {
            if (context.Request.Path.StartsWithSegments(new PathString("/appsettings.json")))
            {
                var securityOptions = new BlazorClientSettings();

                securityOptions.Security = new ClientOpenIdSecurity() {
                    IsAzureAd = settings.Value.IsAzureAd,
                    ClientId = settings.Value.OpenId.ClientId,
                    Authority = settings.Value.OpenId.Authority,
                    Scope = settings.Value.OpenId.Scope,
                    Audience = settings.Value.OpenId.Audience,
                    ResponseType = settings.Value.OpenId.ResponseType,
                };

                context.Response.ContentType = MediaTypeNames.Application.Json;

                var aa = JsonSerializer.Serialize(securityOptions);

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(securityOptions));

                return;
            }

            await _next(context);
        }
    }
}
