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
        private const string DEFAULT_API_VERSION = "5.0";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDistributedCache _cache;
        private readonly HttpStoreOptions _options;
        private readonly EsquioHttpStoreDiagnostics _diagnostics;

        public EsquioHttpStore(IHttpClientFactory httpClientFactory, IOptions<HttpStoreOptions> options, EsquioHttpStoreDiagnostics diagnostics, IDistributedCache cache = null)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _ = options ?? throw new ArgumentNullException(nameof(options));
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));

            _cache = cache;
            _options = options.Value;
        }

        public async Task<Feature> FindFeatureAsync(string featureName, string productName, string deploymentName, CancellationToken cancellationToken = default)
        {
            _ = featureName ?? throw new ArgumentNullException(nameof(featureName));
            _ = productName ?? throw new ArgumentNullException(nameof(productName));
            _ = deploymentName ?? throw new ArgumentNullException(nameof(deploymentName));

            _diagnostics.FindFeature(featureName, productName, deploymentName);

            var featureConfiguration = await GetFeatureConfiguration(featureName, productName, deploymentName);

            return featureConfiguration?
                .ToFeature();
        }

        private async Task<string> GetFeatureConfiguration(string featureName, string productName, string deploymentName, CancellationToken cancellationToken = default)
        {
            if (_options.CacheEnabled)
            {
                if (_cache == null)
                {
                    _diagnostics.DistributedCacheIsNotConfigured();
                    throw new InvalidOperationException("HttpStore is configured with CacheEnabled = true but IDistributedCache is not registered.");
                }

                var cacheKey = CacheKeyCreator.GetCacheKey(productName, featureName, deploymentName, DEFAULT_API_VERSION);

                var featureConfiguration = await _cache.GetStringAsync(cacheKey, cancellationToken);

                _diagnostics.GetFeatureFromCache(cacheKey);

                if (featureConfiguration != null)
                {
                    _diagnostics.FeatureIsOnCache(cacheKey);

                    return featureConfiguration;
                }
                else
                {
                    _diagnostics.FeatureIsNotOnCache(cacheKey);

                    featureConfiguration = await GetFeatureFromServer(productName, featureName, deploymentName, cancellationToken);

                    if (featureConfiguration != null)
                    {
                        await _cache.SetStringAsync(cacheKey, featureConfiguration, new DistributedCacheEntryOptions()
                        {
                            SlidingExpiration = _options.SlidingExpiration,
                            AbsoluteExpirationRelativeToNow = _options.AbsoluteExpirationRelativeToNow
                        });
                    }

                    return featureConfiguration;
                }
            }
            else
            {
                return await GetFeatureFromServer(productName, featureName, deploymentName, cancellationToken);
            }
        }

        private async Task<string> GetFeatureFromServer(string productName, string featureName, string deploymentName, CancellationToken cancellationToken = default)
        {
            _diagnostics.GetFeatureFromStore(featureName, productName, deploymentName);

            var httpClient = _httpClientFactory
                   .CreateClient(EsquioConstants.ESQUIO);

            var response = await httpClient
                .GetAsync($"api/configuration/product/{productName}/feature/{featureName}?deployment={deploymentName}&api-version={DEFAULT_API_VERSION}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _diagnostics.FeatureNotExist(featureName, productName, deploymentName);
                return null;
            }
            else
            {
                _diagnostics.StoreRequestFailed(response.RequestMessage.RequestUri, response.StatusCode);
                throw new InvalidOperationException("Http store response is not success status code.");
            }
        }
    }

    internal static class CacheKeyCreator
    {
        public static string GetCacheKey(string productName, string featureName, string deploymentName, string version)
            => $"esquio:{version}:product:{productName}:deployment:{deploymentName}:feature:{featureName}";
    }
}
