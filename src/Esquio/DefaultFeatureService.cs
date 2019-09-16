using Esquio.Abstractions;
using Esquio.DependencyInjection;
using Esquio.Diagnostics;
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
        private readonly IFeatureObserver _observer;
        private readonly EsquioOptions _options;
        private readonly EsquioDiagnostics _diagnostics;

        public DefaultFeatureService(
            IRuntimeFeatureStore store,
            IToggleTypeActivator toggleActivator,
            IFeatureObserver observer,
            IOptions<EsquioOptions> options,
            EsquioDiagnostics diagnostics)
        {
            _featureStore = store ?? throw new ArgumentNullException(nameof(store));
            _toggleActivator = toggleActivator ?? throw new ArgumentNullException(nameof(toggleActivator));
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }
        public async Task<bool> IsEnabledAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var totalTime = ValueStopwatch.StartNew();

                _diagnostics.BeginFeatureEvaluation(featureName, productName);

                var feature = await _featureStore
                    .FindFeatureAsync(featureName, productName, cancellationToken);

                if (feature == null)
                {
                    _diagnostics.FeatureEvaluationNotFound(featureName, productName);
                    return _options.NotFoundBehavior == NotFoundBehavior.SetEnabled;
                }

                if (!feature.IsEnabled)
                {
                    _diagnostics.FeatureEvaluationDisabled(featureName, productName);
                    return false;
                }

                var enabled = true;
                var toggles = feature.GetToggles();

                foreach (var toggle in toggles)
                {
                    _diagnostics.BeginTogglevaluation(featureName, productName, toggle.Type);

                    var active = false;
                    var evaluationTime = ValueStopwatch.StartNew();

                    var toggleInstance = _toggleActivator
                        .CreateInstance(toggle.Type);

                    if (toggleInstance != null)
                    {
                        active = await toggleInstance?.IsActiveAsync(featureName, productName, cancellationToken);
                    }

                    _diagnostics.Togglevaluation(featureName, productName, toggle.Type, (long)evaluationTime.GetElapsedTime().TotalMilliseconds);
                    _diagnostics.EndTogglevaluation(featureName, productName, toggle.Type, active);

                    if (!active)
                    {
                        _diagnostics.ToggleNotActive(featureName, toggle.Type);

                        enabled = false;
                        break;
                    }
                }

                await _observer.OnNext(featureName, productName, enabled, cancellationToken);

                _diagnostics.EndFeatureEvaluation(featureName, productName, (long)totalTime.GetElapsedTime().TotalMilliseconds, enabled);
                
                return enabled;
            }
            catch (Exception exception)
            {
                _diagnostics.FeatureEvaluationThrow(featureName, productName, exception);

                if (_options.OnErrorBehavior == OnErrorBehavior.Throw)
                {
                    throw;
                }

                return _options.OnErrorBehavior == OnErrorBehavior.SetEnabled;
            }
        }
    }
}
