using Esquio;
using Esquio.Abstractions;
using Esquio.DependencyInjection;
using Esquio.Distributed.Store;
using Esquio.Distributed.Store.DependencyInjection;
using Esquio.Distributed.Store.Diagnostics;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IEsquioBuilder"/>
    /// </summary>
    public static class DistributedStoreExtensions
    {
        /// <summary>
        /// Add Esquio configuration using distributed store that connect with HTTP API on any  Esquio UI deployment.
        /// </summary>
        /// <param name="builder">The <see cref="IEsquioBuilder"/> used.</param>
        /// <param name="baseAddress">The Esquio UI HTTP API base address.</param>
        /// <param name="apiKey">The Esquio UI HTTP valid api key.</param>
        /// <param name="configurer">The action to configure default settings for internal Entity Framework Context.[Optional].</param>
        /// <returns>A new <see cref="IEsquioBuilder"/> that can be chained for register services.</returns>
        public static IEsquioBuilder AddDistributedStore(this IEsquioBuilder builder, Action<DistributedStoreOptions> setup)
        {
            const string XAPIKEYHEADERNAME = "X-API-KEY";

            builder.Services
                .Configure(setup);

            builder.Services
                .AddHttpClient(EsquioConstants.ESQUIO, (serviceProvider, httpclient) =>
                 {
                     var options = serviceProvider.GetRequiredService<IOptions<DistributedStoreOptions>>();

                     httpclient.BaseAddress = options.Value.BaseAddress;
                     httpclient.DefaultRequestHeaders.Add(XAPIKEYHEADERNAME, options.Value.ApiKey);
                     httpclient.Timeout = options.Value.Timeout;

                 }).Services
                .AddDistributedMemoryCache()
                .AddScoped<IRuntimeFeatureStore, EsquioDistributedStore>()
                .AddSingleton<EsquioDistributedStoreDiagnostics>();

            return builder;
        }
    }
}
