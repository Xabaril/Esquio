using Esquio.Abstractions;
using Esquio.Configuration.Store.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.Configuration.Store
{
    public class ConfigurationFeatureStore : IFeatureStore
    {
        private readonly IConfigurationSection _configuration;
        private readonly ILogger<ConfigurationFeatureStore> _logger;
        private readonly EsquioSection esquio;

        public ConfigurationFeatureStore(IConfigurationSection configuration, ILogger<ConfigurationFeatureStore> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            esquio = new EsquioSection();
            _configuration.Bind(esquio);
        }

        public Task<bool> AddFeatureAsync(string applicationName, string featureName, bool enabled = false)
        {
            _logger.LogWarning("Can't add features for read-only store provider.");
            return Task.FromResult(false);
        }

        public Task<bool> AddToggleAsync<TToggle>(string applicationName, string featureName, IDictionary<string, object> parameterValues) where TToggle : IToggle
        {
            _logger.LogWarning("Can't add toggles for read-only store provider.");
            return Task.FromResult(false);
        }

        public Task<Feature> FindFeatureAsync(string applicationName, string featureName)
        {
            var application = GetApplicationOrThrow(applicationName);
            var feature = GetFeatureOrThrow(application, featureName);

            return Task.FromResult(feature.To());
        }

        public Task<IEnumerable<Type>> FindTogglesTypesAsync(string applicationName, string featureName)
        {
            var application = GetApplicationOrThrow(applicationName);
            var feature = GetFeatureOrThrow(application, featureName);
            return Task.FromResult(Enumerable.Empty<Type>());
        }

        public Task<object> GetParameterValueAsync<TToggle>(string applicationName, string featureName, string parameterName) where TToggle : IToggle
        {
            throw new NotImplementedException();
        }

        private Application GetApplicationOrThrow(string applicationName)
        {
            var application = esquio
                .Applications
                .SingleOrDefault(a => a.Name.Equals(applicationName, StringComparison.InvariantCultureIgnoreCase));

            if (application == null)
            {
                throw new ArgumentException(nameof(applicationName));
            }

            return application;
        }


        private Entities.Feature GetFeatureOrThrow(Application application, string featureName)
        {
            var feature = application
                .Features
                .SingleOrDefault(f => f.Name.Equals(featureName, StringComparison.InvariantCultureIgnoreCase));

            if (feature == null)
            {
                throw new ArgumentException(nameof(featureName));
            }

            return feature;
        }
    }
}
