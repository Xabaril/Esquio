using Esquio.Abstractions;
using Esquio.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Esquio
{
    public sealed class DefaultFeatureService
        : IFeatureService
    {
        private readonly IFeatureStore _featureStore;
        private readonly IToggleTypeActivator _toggleActivator;
        private readonly ILogger<DefaultFeatureService> _logger;
        public DefaultFeatureService(IFeatureStore store, IToggleTypeActivator toggeActivator, ILogger<DefaultFeatureService> logger)
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

                if (feature != null)
                {
                    var togglesTypes = await _featureStore.FindTogglesTypesAsync(featureName, applicationName);

                    foreach (var type in togglesTypes)
                    {
                        var toggle = _toggleActivator.CreateInstance(type);

                        if (!await toggle.IsActiveAsync(featureName, applicationName))
                        {
                            Log.FeatureServiceToggleIsNotActive(_logger, featureName, applicationName);
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    Log.FeatureServiceNotFoundFeature(_logger, featureName, applicationName);
                    return false;
                }
            }
            catch (Exception exception)
            {
                Log.FeatureServiceProcessingFail(_logger, featureName, applicationName, exception);
                return false;
            }
        }
    }
}
