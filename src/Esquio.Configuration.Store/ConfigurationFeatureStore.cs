using Esquio.Abstractions;
using Esquio.Configuration.Store.Configuration;
using Esquio.Configuration.Store.Diagnostics;
using Esquio.Toggles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
        public Task<bool> AddFeatureAsync(string featureName, string applicationName, bool enabled = false)
        {
            Log.StoreIsReadOnly(_logger);
            return Task.FromResult(false);
        }
        public Task<bool> AddFeatureAsync(string applicationName, Feature feature)
        {
            Log.StoreIsReadOnly(_logger);
            return Task.FromResult(false);
        }
        public Task<bool> AddToggleAsync<TToggle>(string featureName, string applicationName, IDictionary<string, object> parameterValues)
            where TToggle : IToggle
        {
            Log.StoreIsReadOnly(_logger);
            return Task.FromResult(false);
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
        public Task<IEnumerable<string>> FindTogglesTypesAsync(string featureName, string applicationName)
        {
            var feature = GetFeatureFromConfiguration(featureName, applicationName);

            if (feature != null)
            {
                var types = feature.Toggles
                    .Select(t => t.Type);

                return Task.FromResult(types);
            }
            Log.FeatureNotExist(_logger, featureName, applicationName);
            return Task.FromResult(Enumerable.Empty<string>());
        }
        public Task<object> GetToggleParameterValueAsync<TToggle>(string featureName, string applicationName, string parameterName) where TToggle : IToggle
        {
            var feature = GetFeatureFromConfiguration(featureName, applicationName);

            if (feature != null)
            {
                var toggle = feature.Toggles
                    .Where(t => t.Type.Contains(typeof(TToggle).FullName))
                    .SingleOrDefault();

                if (toggle != null)
                {
                    var parameterValue = toggle.Parameters
                        .SingleOrDefault(t => t.Name.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase));

                    if (parameterName != null)
                    {
                        return Task.FromResult<object>(parameterValue.Value);
                    }
                }

                Log.ParameterNotExist(_logger, featureName, applicationName, parameterName);
                return Task.FromResult<object>(null);
            }
            Log.FeatureNotExist(_logger, featureName, applicationName);
            return Task.FromResult<object>(null);
        }
        private FeatureConfiguration GetFeatureFromConfiguration(string featureName, string applicationName)
        {
            Log.FindFeature(_logger, featureName, applicationName);

            var application = _options?.Value
                .Products
                .FirstOrDefault(a => a.Name.Equals(applicationName, StringComparison.InvariantCultureIgnoreCase) || String.IsNullOrEmpty(applicationName));

            if (application != null)
            {
                return application
                    .Features
                    .SingleOrDefault(f => f.Name.Equals(featureName, StringComparison.InvariantCultureIgnoreCase));
            }
            return null;
        }
    }
}
