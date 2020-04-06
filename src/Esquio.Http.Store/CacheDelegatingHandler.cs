using Esquio.Http.Store.DependencyInjection;
using Esquio.Http.Store.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Http.Store
{
    class CacheDelegatingHandler
        : DelegatingHandler
    {
        private readonly IDistributedCache _cache;
        private readonly EsquioHttpStoreDiagnostics _diagnostics;
        private readonly HttpStoreOptions _options;

        public CacheDelegatingHandler(IDistributedCache cache, IOptions<HttpStoreOptions> options, EsquioHttpStoreDiagnostics diagnostics)
        {
            _cache = cache ?? throw new InvalidOperationException("Cache is enabled but IDistributedCache is not registered");
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
            _ = options ?? throw new ArgumentNullException(nameof(options));

            _options = options.Value;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            var key = CacheKeyCreator
                .FromUri(request.RequestUri);

            _diagnostics.GetFeatureFromCache(key);

            var featureConfiguration = await _cache
                .GetStringAsync(key, cancellationToken);

            if (featureConfiguration != null)
            {
                _diagnostics.FeatureExistOnCache(key);
            }
            else
            {
                var response = await base.SendAsync(request, cancellationToken);
                featureConfiguration = await response.Content.ReadAsStringAsync();

                await _cache.SetStringAsync(key, featureConfiguration, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = _options.AbsoluteExpirationRelativeToNow,
                    SlidingExpiration = _options.SlidingExpiration
                }, cancellationToken);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(featureConfiguration)
            };
        }
    }

    internal static class CacheKeyCreator
    {
        public static string FromUri(Uri requestUri)
        {
            var queryString = requestUri
                       .Query
                       .Substring(1, requestUri.Query.Length - 1)
                       .Split('&');

            var ringName = queryString[0].Split('=')[1];
            var version = queryString[1].Split('=')[1];
            var productName = requestUri.Segments[4];
            var featureName = requestUri.Segments[6];

            return GetCacheKey(productName, featureName, ringName, version);
        }
        static string GetCacheKey(string productName, string featureName, string ringName, string version)
            => $"esquio:{version}:product:{productName}:ring:{ringName}:feature:{featureName}";
    }
}
