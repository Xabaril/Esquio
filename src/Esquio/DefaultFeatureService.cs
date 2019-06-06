using Esquio.Abstractions;
using Esquio.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio
{
    internal sealed class DefaultFeatureService
        : IFeatureService
    {
        private readonly IRuntimeFeatureStore _featureStore;
        private readonly IToggleTypeActivator _toggleActivator;
        private readonly ILogger<DefaultFeatureService> _logger;

        public DefaultFeatureService(IRuntimeFeatureStore store, IToggleTypeActivator toggleActivator, ILogger<DefaultFeatureService> logger)
        {
            _featureStore = store ?? throw new ArgumentNullException(nameof(store));
            _toggleActivator = toggleActivator ?? throw new ArgumentNullException(nameof(toggleActivator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> IsEnabledAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            try
            {
                Log.FeatureServiceProcessingBegin(_logger, featureName, productName);

                var feature = await _featureStore
                    .FindFeatureAsync(featureName, productName, cancellationToken);

                if (feature == null)
                {
                    Log.FeatureServiceNotFoundFeature(_logger, featureName, productName);
                    return false;
                }

                if (!feature.IsEnabled)
                {
                    Log.FeatureServiceDisabledFeature(_logger, featureName, productName);
                    return false;
                }

                var toggles = feature.GetToggles();

                foreach (var toggle in toggles)
                {
                    var toggleInstance = _toggleActivator.CreateInstance(toggle.Type);

                    if (!await toggleInstance.IsActiveAsync(featureName, productName))
                    {
                        Log.FeatureServiceToggleIsNotActive(_logger, featureName, productName);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.FeatureServiceProcessingFail(_logger, featureName, productName, exception);
                return false;
            }
        }
    }
}
