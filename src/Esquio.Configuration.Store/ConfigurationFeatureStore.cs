using Esquio.Abstractions;
using Esquio.Configuration.Store.Configuration;
using Esquio.Configuration.Store.Diagnostics;
using Esquio.Model;
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
        private readonly EsquioConfigurationStoreDiagnostics _diagnostics;
        private readonly IOptionsSnapshot<EsquioConfiguration> _options;

        public ConfigurationFeatureStore(IOptionsSnapshot<EsquioConfiguration> options, EsquioConfigurationStoreDiagnostics diagnostics)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }

        public Task<Feature> FindFeatureAsync(string featureName, string productName, CancellationToken cancellationToken = default)
        {
            _diagnostics.BeginFindFeature(featureName, productName);
            var feature = GetFeatureFromConfiguration(featureName, productName);

            if (feature != null)
            {
                _diagnostics.FeatureExist(featureName, productName);
                return Task.FromResult(feature.To());
            }

            _diagnostics.FeatureNotExist(featureName, productName);
            return Task.FromResult<Feature>(null);
        }

        private FeatureConfiguration GetFeatureFromConfiguration(string featureName, string productName)
        {
          

            var product = _options?.Value
                .Products
                .FirstOrDefault(a => a.Name.Equals(productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, StringComparison.InvariantCultureIgnoreCase));

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
