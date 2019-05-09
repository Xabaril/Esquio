using Esquio.Abstractions;
using Esquio.Configuration.Store.Configuration;
using Esquio.Configuration.Store.Diagnostics;
using Esquio.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.Configuration.Store
{
    internal class ConfigurationFeatureStore : IFeatureStore
    {
        private readonly ILogger<ConfigurationFeatureStore> _logger;
        private readonly IOptionsSnapshot<EsquioConfiguration> _options;

        public ConfigurationFeatureStore(IOptionsSnapshot<EsquioConfiguration> options, ILogger<ConfigurationFeatureStore> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
        public Task AddFeatureAsync(string applicationName, Feature feature)
        {
            Log.StoreIsReadOnly(_logger);
            throw new EsquioException($"Store {nameof(ConfigurationFeatureStore)} is read only, this action can't be performed.");
        }
        public Task<Product> FindProductAsync(string name)
        {
            var product = _options?.Value
                .Products
                .FirstOrDefault(a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) || String.IsNullOrEmpty(name));

            return Task.FromResult(product.To());
        }

        public Task AddProductAsync(Product product)
        {
            Log.StoreIsReadOnly(_logger);
            throw new EsquioException($"Store {nameof(ConfigurationFeatureStore)} is read only, this action can't be performed.");
        }

        public Task DeleteProductAsync(Product product)
        {
            Log.StoreIsReadOnly(_logger);
            throw new EsquioException($"Store {nameof(ConfigurationFeatureStore)} is read only, this action can't be performed.");
        }
        public Task UpdateProductAsync(Product product)
        {
            Log.StoreIsReadOnly(_logger);
            throw new EsquioException($"Store {nameof(ConfigurationFeatureStore)} is read only, this action can't be performed.");
        }

        public Task<Feature> FindFeatureAsync(string featureName, string applicationName)
        {
            var feature = GetFeatureFromConfiguration(featureName, applicationName);

            if (feature != null)
            {
                return Task.FromResult(feature.To());
            }
            Log.FeatureNotExist(_logger, featureName, applicationName);
            return Task.FromResult<Feature>(null);
        }

        private FeatureConfiguration GetFeatureFromConfiguration(string featureName, string applicationName)
        {
            Log.FindFeature(_logger, featureName, applicationName);

            var product = _options?.Value
                .Products
                .FirstOrDefault(a => a.Name.Equals(applicationName, StringComparison.InvariantCultureIgnoreCase) || String.IsNullOrEmpty(applicationName));

            if (product != null)
            {
                return product
                    .Features
                    .SingleOrDefault(f => f.Name.Equals(featureName, StringComparison.InvariantCultureIgnoreCase));
            }
            return null;
        }

    }
}
