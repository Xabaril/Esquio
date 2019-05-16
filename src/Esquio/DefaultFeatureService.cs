using Esquio.Abstractions;
using Esquio.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Esquio
{
    internal sealed class DefaultFeatureService
        : IFeatureService
    {
        private readonly IRuntimeFeatureStore _featureStore;
        private readonly IToggleTypeActivator _toggleActivator;
        private readonly ILogger<DefaultFeatureService> _logger;
        public DefaultFeatureService(IRuntimeFeatureStore store, IToggleTypeActivator toggeActivator, ILogger<DefaultFeatureService> logger)
        {
            _featureStore = store ?? throw new ArgumentNullException(nameof(store));
            _toggleActivator = toggeActivator ?? throw new ArgumentNullException(nameof(toggeActivator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> IsEnabledAsync(string featureName, string applicationName = null)
        {
            try
            {
                Log.FeatureServiceProcessingBegin(_logger, featureName, applicationName);

                var feature = await _featureStore.FindFeatureAsync(featureName, applicationName);

                if (feature == null)
                {
                    Log.FeatureServiceNotFoundFeature(_logger, featureName, applicationName);
                    return false;
                }

                if (!feature.IsEnabled)
                {
                    Log.FeatureServiceDisabledFeature(_logger, featureName, applicationName);
                    return false;
                }

                var toggles = feature.GetToggles();

                foreach (var toggle in toggles)
                {
                    var toggleInstance = _toggleActivator.CreateInstance(toggle.Type);

                    if (!await toggleInstance.IsActiveAsync(featureName, applicationName))
                    {
                        Log.FeatureServiceToggleIsNotActive(_logger, featureName, applicationName);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.FeatureServiceProcessingFail(_logger, featureName, applicationName, exception);
                return false;
            }
        }
    }
}
