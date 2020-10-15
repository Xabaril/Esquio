using Esquio.Blazor.WebAssembly;
using Esquio.Blazor.WebAssembly.DependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Service collection extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register Esquio client on the dependencies container.
        /// </summary>
        /// <param name="services">The dependencies container.</param>
        /// <param name="configure">The configure action.</param>
        /// <returns>The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> to be chained.</returns>
        public static IServiceCollection AddEsquioClient(this IServiceCollection services, Action<BlazorFeatureServiceOptions> configure)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            return services
                .Configure(configure)
                .AddScoped<IBlazorFeatureServiceClient, BlazorFeatureServiceClient>();
        }

        /// <summary>
        /// Register Esquio client on the dependencies container.
        /// </summary>
        /// <param name="services">The dependencies container.</param>
        /// <returns>The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> to be chained.</returns>
        public static IServiceCollection AddEsquioClient(this IServiceCollection services)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            return services
                .Configure<BlazorFeatureServiceOptions>(_ => { })
                .AddScoped<IBlazorFeatureServiceClient, BlazorFeatureServiceClient>();
        }
    }
}
