using Esquio.Abstractions;
using Esquio.Configuration.Store.Configuration;
using Esquio.Configuration.Store.Diagnostics;
using Esquio.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Configuration.Store
{
    internal class ConfigurationFeatureStore
        : IRuntimeFeatureStore
    {
        private readonly ILogger<ConfigurationFeatureStore> _logger;
        private readonly IOptionsSnapshot<EsquioConfiguration> _options;

        public ConfigurationFeatureStore(IOptionsSnapshot<EsquioConfiguration> options, ILogger<ConfigurationFeatureStore> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<Feature> FindFeatureAsync(string featureName, string productName, CancellationToken cancellationToken = default)
        {
            var feature = GetFeatureFromConfiguration(featureName, productName);

            if (feature != null)
            {
                return Task.FromResult(feature.To());
            }

            Log.FeatureNotExist(_logger, featureName, productName);
            return Task.FromResult<Feature>(null);
        }

        private FeatureConfiguration GetFeatureFromConfiguration(string featureName, string productName)
        {
            Log.FindFeature(_logger, featureName, productName);

            var product = _options?.Value
                .Products
                .FirstOrDefault(a => a.Name.Equals(productName, StringComparison.InvariantCultureIgnoreCase) || String.IsNullOrEmpty(productName));

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
