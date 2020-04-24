using Esquio.UI.Host.Infrastructure.Middleware;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApoBuilderExtensions
    {
        public static IApplicationBuilder AddClientBlazorConfiguration(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<BlazorClientConfigurationMiddleware>();
        }
    }
}
