using Esquio.Abstractions;
using Esquio.Http.Store.Diagnostics;
using Esquio.Model;
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
        private readonly EsquioHttpStoreDiagnostics _diagnostics;

        public EsquioHttpStore(IHttpClientFactory httpClientFactory, EsquioHttpStoreDiagnostics diagnostics)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
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
            var httpClient = _httpClientFactory
                .CreateClient(EsquioConstants.ESQUIO);

            var response = await httpClient
                .GetAsync($"api/configuration/product/{productName}/feature/{featureName}?ringName={ringName}&api-version=3.0", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _diagnostics.FeatureNotExist(featureName, productName, ringName);
                return null;
            }

            _diagnostics.StoreRequestFailed(response.RequestMessage.RequestUri, response.StatusCode);
            throw new InvalidOperationException("Http store response is not success status code.");
        }
    }
}
