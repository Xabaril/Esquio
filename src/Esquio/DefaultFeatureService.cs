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
        private readonly IEvaluationSession _session;
        private readonly EsquioOptions _options;
        private readonly EsquioDiagnostics _diagnostics;

        public DefaultFeatureService(
            IRuntimeFeatureStore store,
            IToggleTypeActivator toggleActivator,
            IEvaluationSession session,
            IOptions<EsquioOptions> options,
            EsquioDiagnostics diagnostics)
        {
            _featureStore = store ?? throw new ArgumentNullException(nameof(store));
            _toggleActivator = toggleActivator ?? throw new ArgumentNullException(nameof(toggleActivator));
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }
        public async Task<bool> IsEnabledAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var featureCorrelationId = Guid.NewGuid();
            var totalTime = ValueStopwatch.StartNew();
            productName ??= _options.DefaultProductName;

            try
            {
                var enabled = true;

                if (!await _session.TryGetAsync(featureName, productName, out var sessionResult))
                {
                    _diagnostics.BeginFeatureEvaluation(featureCorrelationId, featureName, productName);

                    var feature = await _featureStore
                        .FindFeatureAsync(featureName, productName, cancellationToken);

                    if (feature == null)
                    {
                        _diagnostics.FeatureEvaluationNotFound(featureCorrelationId, featureName, productName);
                        return _options.NotFoundBehavior == NotFoundBehavior.SetEnabled;
                    }

                    if (!feature.IsEnabled)
                    {
                        _diagnostics.FeatureEvaluationDisabled(featureName, productName);
                        enabled = false;
                    }
                    else
                    {
                        var toggles = feature.GetToggles();

                        foreach (var toggle in toggles)
                        {
                            var toggleCorrelationId = Guid.NewGuid();

                            _diagnostics.BeginTogglevaluation(toggleCorrelationId, featureName, productName, toggle.Type);

                            var active = false;
                            var evaluationTime = ValueStopwatch.StartNew();

                            var toggleInstance = _toggleActivator
                                .CreateInstance(toggle.Type);

                            if (toggleInstance != null)
                            {
                                active = await toggleInstance?.IsActiveAsync(featureName, productName, cancellationToken);
                            }

                            _diagnostics.Togglevaluation(featureName, productName, toggle.Type, (long)evaluationTime.GetElapsedTime().TotalMilliseconds);
                            _diagnostics.EndTogglevaluation(toggleCorrelationId, featureName, productName, toggle.Type, active);

                            if (!active)
                            {
                                _diagnostics.ToggleNotActive(featureName, toggle.Type);

                                enabled = false;
                                break;
                            }
                        }
                    }

                    await _session.SetAsync(featureName, productName, enabled);

                    _diagnostics.EndFeatureEvaluation(featureCorrelationId, featureName, productName, (long)totalTime.GetElapsedTime().TotalMilliseconds, enabled);
                }
                else
                {
                    _diagnostics.FeatureEvaluationFromSession(featureName, productName);
                    enabled = sessionResult;
                }

                return enabled;
            }
            catch (Exception exception)
            {
                _diagnostics.FeatureEvaluationThrow(featureCorrelationId, featureName, productName, exception);

                if (_options.OnErrorBehavior == OnErrorBehavior.Throw)
                {
                    throw;
                }

                return _options.OnErrorBehavior == OnErrorBehavior.SetEnabled;
            }
        }
    }
}
