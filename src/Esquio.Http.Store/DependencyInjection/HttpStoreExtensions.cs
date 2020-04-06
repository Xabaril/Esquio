using Esquio;
using Esquio.Abstractions;
using Esquio.DependencyInjection;
using Esquio.Http.Store;
using Esquio.Http.Store.DependencyInjection;
using Esquio.Http.Store.Diagnostics;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IEsquioBuilder"/>
    /// </summary>
    public static class HttpStoreExtensions
    {
        /// <summary>
        /// Add Esquio configuration using  store that connect with HTTP API on any  Esquio UI deployment.
        /// </summary>
        /// <param name="builder">The <see cref="IEsquioBuilder"/> used.</param>
        /// <param name="baseAddress">The Esquio UI HTTP API base address.</param>
        /// <param name="apiKey">The Esquio UI HTTP valid api key.</param>
        /// <param name="configurer">The action to configure default settings for internal Entity Framework Context.[Optional].</param>
        /// <returns>A new <see cref="IEsquioBuilder"/> that can be chained for register services.</returns>
        public static IEsquioBuilder AddHttpStore(this IEsquioBuilder builder, Action<HttpStoreOptions> setup)
        {
            const string XAPIKEYHEADERNAME = "X-API-KEY";

            var options = new HttpStoreOptions();
            setup.Invoke(options);

            builder.Services
                .Configure<HttpStoreOptions>(setup=>
                {
                    setup.BaseAddress = options.BaseAddress;
                    setup.ApiKey = options.ApiKey;
                    setup.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;
                    setup.SlidingExpiration = options.SlidingExpiration;
                    setup.Timeout = options.Timeout;
                    setup.CacheEnabled = options.CacheEnabled;
                });

            builder.Services
                .AddHttpClient(EsquioConstants.ESQUIO, (serviceProvider, httpclient) =>
                 {
                     var options = serviceProvider.GetRequiredService<IOptions<HttpStoreOptions>>();

                     httpclient.BaseAddress = options.Value.BaseAddress;
                     httpclient.DefaultRequestHeaders.Add(XAPIKEYHEADERNAME, options.Value.ApiKey);
                     httpclient.Timeout = options.Value.Timeout;

                 })
                .AddHttpMessageHandler<CacheDelegatingHandler>(options.CacheEnabled)
                .Services
                .AddDistributedMemoryCache()
                .AddScoped<IRuntimeFeatureStore, EsquioHttpStore>()
                .AddSingleton<EsquioHttpStoreDiagnostics>()
                .AddSingleton<CacheDelegatingHandler>();

            return builder;
        }
    }
}
