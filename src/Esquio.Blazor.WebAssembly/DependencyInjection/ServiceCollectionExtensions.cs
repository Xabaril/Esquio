using Esquio.Blazor.WebAssembly;
using Esquio.Blazor.WebAssembly.DependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEsquioClient(this IServiceCollection services, Action<BlazorFeatureServiceOptions> configure)
        {
            return services
                .Configure(configure)
                .AddSingleton<IBlazorFeatureServiceClient, BlazorFeatureServiceClient>();
        }
    }
}
