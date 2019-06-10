using Esquio.Abstractions;
using Esquio.DependencyInjection;
using Esquio.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly EsquioOptions _options;
        private readonly ILogger<DefaultFeatureService> _logger;

        public DefaultFeatureService(
            IRuntimeFeatureStore store,
            IToggleTypeActivator toggleActivator,
            IOptions<EsquioOptions> options,
            ILogger<DefaultFeatureService> logger)
        {
            _featureStore = store ?? throw new ArgumentNullException(nameof(store));
            _toggleActivator = toggleActivator ?? throw new ArgumentNullException(nameof(toggleActivator));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
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


                var active = true;
                var toggles = feature.GetToggles();

                foreach (var toggle in toggles)
                {
                    var toggleInstance = _toggleActivator.CreateInstance(toggle.Type);

                    if (toggleInstance != null)
                    {
                        if (!await toggleInstance.IsActiveAsync(featureName, productName, cancellationToken))
                        {
                            Log.FeatureServiceToggleIsNotActive(_logger, featureName, productName);
                            active = false;
                            break;
                        }
                    }
                    else
                    {
                        Log.FeatureServiceToggleTypeIsNull(_logger, featureName, productName, toggle.Type);
                        active = false;
                        break;
                    }
                }

                return active;
            }
            catch (Exception exception)
            {
                Log.FeatureServiceProcessingFail(_logger, featureName, productName, exception);

                if (_options.ExceptionBehavior == ExceptionBehavior.Throw)
                {
                    throw;
                }

                return _options.ExceptionBehavior == ExceptionBehavior.SetAsActive;
            }
        }
    }
}
