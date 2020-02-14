using Esquio.Abstractions;
using Esquio.Distributed.Store.DependencyInjection;
using Esquio.Distributed.Store.Diagnostics;
using Esquio.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Distributed.Store
{
    internal class EsquioDistributedStore
        : IRuntimeFeatureStore
    {
        public const string HTTP_CLIENT_NAME = "Esquio";

        public static JsonSerializerOptions _serializationOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            IgnoreNullValues = false,
            ReadCommentHandling = JsonCommentHandling.Disallow
        };

        private readonly IDistributedCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly EsquioDistributedStoreDiagnostics _diagnostics;
        private readonly DistributedStoreOptions _options;

        public EsquioDistributedStore(IDistributedCache cache, IHttpClientFactory httpClientFactory, IOptions<DistributedStoreOptions> options, EsquioDistributedStoreDiagnostics diagnostics)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }

        public async Task<Feature> FindFeatureAsync(string featureName, string productName, string ringName, CancellationToken cancellationToken = default)
        {
            _ = featureName ?? throw new ArgumentNullException(nameof(featureName));
            _ = productName ?? throw new ArgumentNullException(nameof(productName));
            _ = ringName ?? throw new ArgumentNullException(nameof(ringName));

            _diagnostics.FindFeature(featureName, productName, ringName);

            string featureConfiguration = null;

            if (_options.CacheEnabled)
            {
                var cacheKey = GetCacheKey(featureName, productName, ringName);

                _diagnostics.GetFeatureFromCache(cacheKey);

                featureConfiguration = await _cache
                    .GetStringAsync(cacheKey, cancellationToken);

                if (!String.IsNullOrEmpty(featureConfiguration))
                {
                    return ConvertFromSerializedString(featureConfiguration);
                }
                else
                {
                    featureConfiguration = await GetFeatureConfiguration(featureName, productName, ringName);
                    await _cache.SetStringAsync(cacheKey, featureConfiguration, cancellationToken);
                }
            }
            else
            {
                _diagnostics.GetFeatureFromStore(featureName, productName, ringName);
                featureConfiguration = await GetFeatureConfiguration(featureName, productName, ringName);
            }

            return ConvertFromSerializedString(featureConfiguration);

            string GetCacheKey(string featureName, string productName, string ringName) 
                =>
                $"esquio:product:{productName}:ring:{ringName}:feature:{featureName}";
        }


        private async Task<string> GetFeatureConfiguration(string featureName, string productName, string ringName, CancellationToken cancellationToken = default)
        {
            var httpClient = _httpClientFactory
                .CreateClient(HTTP_CLIENT_NAME);

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

        private Feature ConvertFromSerializedString(string featureConfiguration)
        {
            var content = JsonSerializer
                .Deserialize<DistributedStoreResponse>(featureConfiguration, _serializationOptions);

            var feature = new Feature(content.FeatureName);

            if (content.Enabled)
            {
                feature.Enabled();
            }
            else
            {
                feature.Disabled();
            }

            foreach (var toggleConfiguration in content.Toggles)
            {
                var toggle = new Toggle(toggleConfiguration.Key);

                toggle.AddParameters(
                    toggleConfiguration.Value.Select(p => new Parameter(p.Key, p.Value)));

                feature.AddToggle(toggle);
            }

            return feature;
        }

        private class DistributedStoreResponse
        {
            public string FeatureName { get; set; }

            public bool Enabled { get; set; }

            public Dictionary<string, Dictionary<string, string>> Toggles { get; set; } = new Dictionary<string, Dictionary<string, string>>();
        }
    }
}
