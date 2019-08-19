using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Esquio.Diagnostics
{
    internal class EsquioDiagnostics
    {
        private readonly ILogger<Esquio> _logger;
        private readonly DiagnosticListener _listener;

        public EsquioDiagnostics(DiagnosticListener listener, ILogger<Esquio> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
        }

        public void BeginFeatureEvaluation(string featureName, string productName)
        {
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.FeatureEvaluationStart();
            }

            Log.FeatureServiceProcessingBegin(_logger, featureName, productName);

            var payload = new { Feature = featureName, Product = productName };

            if (_listener.IsEnabled(EsquioConstants.ESQUIO_BEGINFEATURE_ACTIVITY_NAME, payload))
            {
                _listener.Write(EsquioConstants.ESQUIO_BEGINFEATURE_ACTIVITY_NAME, payload);
            }
        }

        public void FeatureEvaluationNotFound(string featureName, string productName)
        {
            Log.FeatureServiceNotFoundFeature(_logger, featureName, productName);

            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.FeatureEvaluationNotFound(featureName, productName);
                EsquioEventSource.Log.FeatureEvaluationStop();
            }
        }

        public void FeatureEvaluationDisabled(string featureName, string productName)
        {
            Log.FeatureServiceDisabledFeature(_logger, featureName, productName);

            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.FeatureEvaluationStop();
            }
        }

        public void FeatureEvaluationThrow(string featureName, string productName, Exception exception)
        {
            Log.FeatureServiceProcessingFail(_logger, featureName, productName, exception);

            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.FeatureEvaluationThrow(featureName, productName, exception.ToString());
            }
        }

        public void EndFeatureEvaluation(string featureName, string productName, long elapsedMilliseconds, bool enabled)
        {
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.FeatureEvaluated(featureName, productName, elapsedMilliseconds);
                EsquioEventSource.Log.FeatureEvaluationStop();
            }

            Log.FeatureServiceProcessingEnd(_logger, featureName, productName, enabled, elapsedMilliseconds);

            var payload = new { Feature = featureName, Product = productName, Enabled = enabled, Elapsed = elapsedMilliseconds };

            if (_listener.IsEnabled(EsquioConstants.ESQUIO_ENDFEATURE_ACTIVITY_NAME, payload))
            {
                _listener.Write(EsquioConstants.ESQUIO_ENDFEATURE_ACTIVITY_NAME, payload);
            }
        }

        public void BeginTogglevaluation()
        {
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleEvaluationStart();
            }
        }

        public void Togglevaluation(string featureName, string productName, string toggle, long elapsedMilliseconds)
        {
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleEvaluated(featureName, productName, toggle, elapsedMilliseconds);
            }
        }

        public void ToggleNotActive(string featureName, string toggle)
        {
            Log.FeatureServiceToggleIsNotActive(_logger, toggle, featureName);
        }

        public void EndTogglevaluation()
        {
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleEvaluationStop();
            }
        }

        public void BeginToggleActivation(string toggleTypeName)
        {
            Log.DefaultToggleTypeActivatorResolveTypeBegin(_logger, toggleTypeName);

            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleActivationStart();
            }
        }

        public void ToggleActivationResolveTypeFromCache(string toggleTypeName)
        {
            Log.DefaultToggleTypeActivatorTypeIsResolvedFromCache(_logger, toggleTypeName);
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleActivationStop();
            }
        }

        public void ToggleActivationResolveType(string toggleTypeName)
        {
            Log.DefaultToggleTypeActivatorTypeIsResolved(_logger, toggleTypeName);
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleActivationStop();
            }
        }

        public void ToggleActivationCantResolveType(string toggleTypeName)
        {
            Log.DefaultToggleTypeActivatorTypeCantResolved(_logger, toggleTypeName);

            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleActivationCantCreateInstance(toggleTypeName);
                EsquioEventSource.Log.ToggleActivationStop();
            }
        }

        public void EndToggleActivation(string toggleTypeName)
        {
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleActivationStop();
            }
        }
    }
}
