using Esquio.Abstractions;
using Esquio.Configuration.Store.Configuration;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfigurationSection _configuration;
        private readonly ILogger<ConfigurationFeatureStore> _logger;
        private readonly EsquioConfiguration _esquio;

        public ConfigurationFeatureStore(IOptions<EsquioConfiguration> options, ILogger<ConfigurationFeatureStore> logger)
        {
            _esquio = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
        public Task<bool> AddFeatureAsync(string applicationName, string featureName, bool enabled = false)
        {
            Log.StoreIsReadOnly(_logger);
            return Task.FromResult(false);
        }
        public Task<bool> AddToggleAsync<TToggle>(string applicationName, string featureName, IDictionary<string, object> parameterValues)
            where TToggle : IToggle
        {
            Log.StoreIsReadOnly(_logger);
            return Task.FromResult(false);
        }
        public Task<Feature> FindFeatureAsync(string applicationName, string featureName)
        {
            var feature = GetFeatureFromConfiguration(applicationName, featureName);

            if (feature != null)
            {
                return Task.FromResult(feature.To());
            }
            Log.FeatureNotExist(_logger, applicationName, featureName);
            return Task.FromResult<Feature>(null);
        }
        public Task<IEnumerable<string>> FindTogglesTypesAsync(string applicationName, string featureName)
        {
            var feature = GetFeatureFromConfiguration(applicationName, featureName);

            if (feature != null)
            {
                var types = feature.Toggles
                    .Select(t => t.Type);

                return Task.FromResult(types);
            }
            Log.FeatureNotExist(_logger, applicationName, featureName);
            return Task.FromResult(Enumerable.Empty<string>());
        }
        public Task<object> GetToggleParameterValueAsync<TToggle>(string applicationName, string featureName, string parameterName) where TToggle : IToggle
        {
            var feature = GetFeatureFromConfiguration(applicationName, featureName);

            if (feature != null)
            {
                var toggle = feature.Toggles
                    .Where(t => t.Type == typeof(TToggle).FullName)
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

                Log.ParameterNotExist(_logger, applicationName, featureName, parameterName);
                return Task.FromResult<object>(null);
            }
            Log.FeatureNotExist(_logger, applicationName, featureName);
            return Task.FromResult<object>(null);
        }
        private FeatureConfiguration GetFeatureFromConfiguration(string applicationName, string featureName)
        {
            Log.FindFeature(_logger, applicationName, featureName);

            var application = _esquio
                .Applications
                .SingleOrDefault(a => a.Name.Equals(applicationName, StringComparison.InvariantCultureIgnoreCase));

            if (application != null)
            {
                return application
                .Features
                .SingleOrDefault(f => f.Name.Equals(featureName, StringComparison.InvariantCultureIgnoreCase));
            }
            return null;
        }
        private static class Log
        {
            public static void StoreIsReadOnly(ILogger logger)
            {
                _storeIsReadOnly(logger, nameof(ConfigurationFeatureStore), null);
            }

            public static void FeatureNotExist(ILogger logger, string applicationName, string featureName)
            {
                _featureNotExist(logger, featureName, applicationName, null);
            }

            public static void ParameterNotExist(ILogger logger, string applicationName, string featureName, string parameterName)
            {
                _parameterNotExist(logger, featureName, applicationName, parameterName, null);
            }

            public static void FindFeature(ILogger logger, string applicationName, string featureName)
            {
                _findFeature(logger, featureName, applicationName, null);
            }

            private static readonly Action<ILogger, string, Exception> _storeIsReadOnly = LoggerMessage.Define<string>(
                LogLevel.Warning,
                EventIds.StoreIsReadOnly,
                "Store {configurationStore} is read only, this action can't be performed.");

            private static readonly Action<ILogger, string, string, Exception> _featureNotExist = LoggerMessage.Define<string, string>(
                LogLevel.Warning,
                EventIds.FeatureNotExist,
                "The feature with name {featureName} is not configured for application {applicationName}.");

            private static readonly Action<ILogger, string, string, string, Exception> _parameterNotExist = LoggerMessage.Define<string, string, string>(
                LogLevel.Warning,
                EventIds.FeatureNotExist,
                "The feature with name {featureName} for application {applicationName} not contains a parameter with name {parameterName} for specified Toggle.");

            private static readonly Action<ILogger, string, string, Exception> _findFeature = LoggerMessage.Define<string, string>(
                LogLevel.Debug,
                EventIds.FindFeature,
                "The store is trying to find feature {featureName} for application {applicationName} on the store.");
        }
        internal static class EventIds
        {
            public static readonly EventId StoreIsReadOnly = new EventId(200, nameof(StoreIsReadOnly));
            public static readonly EventId FeatureNotExist = new EventId(201, nameof(FeatureNotExist));
            public static readonly EventId FindFeature = new EventId(220, nameof(FindFeature));
        }
    }
}
