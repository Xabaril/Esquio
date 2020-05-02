using Esquio.Blazor.WebAssembly;
using Esquio.Blazor.WebAssembly.DependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEsquioClient(this IServiceCollection services, Action<BlazorFeatureServiceOptions> configure)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            return services
                .Configure(configure)
                .AddSingleton<IBlazorFeatureServiceClient, BlazorFeatureServiceClient>();
        }

        public static IServiceCollection AddEsquioClient(this IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            return services
                .Configure<BlazorFeatureServiceOptions>(_ => { })
                .AddSingleton<IBlazorFeatureServiceClient, BlazorFeatureServiceClient>();
        }
    }
}
