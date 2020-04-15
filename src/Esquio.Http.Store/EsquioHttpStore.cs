using Esquio.Abstractions;
using Esquio.Http.Store.DependencyInjection;
using Esquio.Http.Store.Diagnostics;
using Esquio.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Http.Store
{
    internal class EsquioHttpStore
        : IRuntimeFeatureStore
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDistributedCache _cache;
        private readonly HttpStoreOptions _options;
        private readonly EsquioHttpStoreDiagnostics _diagnostics;

        public EsquioHttpStore(IHttpClientFactory httpClientFactory, IDistributedCache cache, IOptions<HttpStoreOptions> options, EsquioHttpStoreDiagnostics diagnostics)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _ = options ?? throw new ArgumentNullException(nameof(options));
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));

            _options = options.Value;
        }

        public async Task<Feature> FindFeatureAsync(string featureName, string productName, string ringName, CancellationToken cancellationToken = default)
        {
            _ = featureName ?? throw new ArgumentNullException(nameof(featureName));
            _ = productName ?? throw new ArgumentNullException(nameof(productName));
            _ = ringName ?? throw new ArgumentNullException(nameof(ringName));

            _diagnostics.FindFeature(featureName, productName, ringName);

            var featureConfiguration = await GetFeatureConfiguration(featureName, productName, ringName);

            return featureConfiguration?
                .ToFeature();
        }

        private async Task<string> GetFeatureConfiguration(string featureName, string productName, string ringName, CancellationToken cancellationToken = default)
        {
            var key = CacheKeyCreator
                .GetCacheKey(productName, featureName, ringName, "3.0");

            var featureConfiguration = await _cache
               .GetStringAsync(key, cancellationToken);

            if (featureConfiguration == null)
            {
                var httpClient = _httpClientFactory
                    .CreateClient(EsquioConstants.ESQUIO);

                var response = await httpClient
                    .GetAsync($"api/configuration/product/{productName}/feature/{featureName}?ringName={ringName}&api-version=3.0", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    featureConfiguration = await response.Content.ReadAsStringAsync();

                    await _cache.SetStringAsync(key, featureConfiguration, new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = _options.AbsoluteExpirationRelativeToNow,
                        SlidingExpiration = _options.SlidingExpiration
                    }, cancellationToken);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _diagnostics.FeatureNotExist(featureName, productName, ringName);
                    return null;
                }
                else
                {
                    _diagnostics.StoreRequestFailed(response.RequestMessage.RequestUri, response.StatusCode);
                    throw new InvalidOperationException("Http store response is not success status code.");
                }
            }

            return featureConfiguration;
        }
    }

    internal static class CacheKeyCreator
    {
        public static string GetCacheKey(string productName, string featureName, string ringName, string version)
            => $"esquio:{version}:product:{productName}:ring:{ringName}:feature:{featureName}";
    }
}
