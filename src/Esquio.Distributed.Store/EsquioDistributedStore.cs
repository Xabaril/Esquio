using Esquio.Abstractions;
using Esquio.Distributed.Store.DependencyInjection;
using Esquio.Distributed.Store.Diagnostics;
using Esquio.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Distributed.Store
{
    internal class EsquioDistributedStore
        : IRuntimeFeatureStore
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly EsquioDistributedStoreDiagnostics _diagnostics;
        private readonly DistributedStoreOptions _options;

        public EsquioDistributedStore(IHttpClientFactory httpClientFactory, IOptions<DistributedStoreOptions> options, EsquioDistributedStoreDiagnostics diagnostics, IDistributedCache cache = null)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
            _cache = cache;
        }

        public async Task<Feature> FindFeatureAsync(string featureName, string productName, string ringName, CancellationToken cancellationToken = default)
        {
            _ = featureName ?? throw new ArgumentNullException(nameof(featureName));
            _ = productName ?? throw new ArgumentNullException(nameof(productName));
            _ = ringName ?? throw new ArgumentNullException(nameof(ringName));

            _diagnostics.FindFeature(featureName, productName, ringName);

            string featureConfiguration;

            if (_options.CacheEnabled)
            {
                if(_cache == null )
                {
                    _diagnostics.DistributedCacheIsNotConfigured();
                    throw new InvalidOperationException("Store is configured to use cache (CacheEnabled is true) but IDistributedCache is not configured.");
                }

                var cacheKey = CacheKeyCreator.GetCacheKey(productName, featureName, ringName);

                _diagnostics.GetFeatureFromCache(cacheKey);

                featureConfiguration = await _cache
                    .GetStringAsync(cacheKey, cancellationToken);

                if (String.IsNullOrEmpty(featureConfiguration))
                {
                    _diagnostics.GetFeatureFromStore(featureName, productName, ringName);
                    featureConfiguration = await GetFeatureConfiguration(featureName, productName, ringName);

                    await _cache.SetStringAsync(
                        cacheKey, 
                        featureConfiguration,
                        new DistributedCacheEntryOptions()
                        {
                            AbsoluteExpirationRelativeToNow = _options.AbsoluteExpirationRelativeToNow,
                            SlidingExpiration = _options.SlidingExpiration,
                        },
                        cancellationToken);
                }
            }
            else
            {
                _diagnostics.GetFeatureFromStore(featureName, productName, ringName);
                featureConfiguration = await GetFeatureConfiguration(featureName, productName, ringName);
            }

            return featureConfiguration?
                      .ToFeature();
        }

        private async Task<string> GetFeatureConfiguration(string featureName, string productName, string ringName, CancellationToken cancellationToken = default)
        {
            var httpClient = _httpClientFactory
                .CreateClient(EsquioConstants.ESQUIO);

            var response = await httpClient
                .GetAsync($"api/store/product/{productName}/feature/{featureName}?ringName={ringName}&api-version=3.0", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response
                    .Content
                    .ReadAsStringAsync();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _diagnostics.FeatureNotExist(featureName, productName, ringName);
                return null;
            }

            _diagnostics.StoreRequestFailed(response.RequestMessage.RequestUri, response.StatusCode);
            throw new InvalidOperationException("Distributed store response is not success status code.");
        }
    }

    internal static class CacheKeyCreator
    {
        public static string GetCacheKey(string productName, string featureName, string ringName)
            => $"esquio:products:{productName}:rings:{ringName}:features:{featureName}";
    }
}
