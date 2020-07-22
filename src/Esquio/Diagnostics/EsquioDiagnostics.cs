using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Esquio.Diagnostics
{
    internal class EsquioDiagnostics
    {
        private readonly ILogger _logger;
        private readonly DiagnosticListener _listener;

        public EsquioDiagnostics(ILoggerFactory loggerFactory)
        {
            _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            _logger = loggerFactory.CreateLogger("Esquio");
            _listener = new DiagnosticListener(EsquioConstants.ESQUIO_LISTENER_NAME);
        }

        public void BeginFeatureEvaluation(Guid correlationId, string featureName, string productName, string deploymentName)
        {
            Log.FeatureServiceProcessingBegin(_logger, featureName, productName, deploymentName);
            
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.FeatureEvaluationStart();
            }

            var payload = new FeatureEvaluatingEventData(correlationId, featureName, productName, deploymentName);

            if (_listener.IsEnabled(EsquioConstants.ESQUIO_BEGINFEATURE_ACTIVITY_NAME, payload))
            {
                _listener.Write(EsquioConstants.ESQUIO_BEGINFEATURE_ACTIVITY_NAME, payload);
            }
        }

        public void FeatureEvaluationFromSession(string featureName, string productName, string deploymentName)
        {
            Log.FeatureServiceFromSession(_logger, featureName, productName, deploymentName);
        }

        public void FeatureEvaluationNotFound(Guid correlationId, string featureName, string productName, string deploymentName)
        {
            Log.FeatureServiceNotFoundFeature(_logger, featureName, productName, deploymentName);

            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.FeatureEvaluationNotFound(featureName, productName, deploymentName);
                EsquioEventSource.Log.FeatureEvaluationStop();
            }

            var payload = new FeatureNotFoundEventData(correlationId, featureName, productName, deploymentName);

            if (_listener.IsEnabled(EsquioConstants.ESQUIO_NOTFOUNDFEATURE_ACTIVITY_NAME, payload))
            {
                _listener.Write(EsquioConstants.ESQUIO_NOTFOUNDFEATURE_ACTIVITY_NAME, payload);
            }
        }

        public void FeatureEvaluationDisabled(string featureName, string productName, string deploymentName)
        {
            Log.FeatureServiceDisabledFeature(_logger, featureName, productName, deploymentName);

            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.FeatureEvaluationStop();
            }
        }

        public void FeatureEvaluationThrow(Guid correlationId, string featureName, string productName, string deploymentName, Exception exception)
        {
            Log.FeatureServiceProcessingFail(_logger, featureName, productName, deploymentName, exception);

            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.FeatureEvaluationThrow(featureName, productName, deploymentName, exception.ToString());
            }

            var payload = new FeatureThrowEventData(correlationId, featureName, productName, deploymentName, exception);

            if (_listener.IsEnabled(EsquioConstants.ESQUIO_THROWFEATURE_ACTIVITY_NAME, payload))
            {
                _listener.Write(EsquioConstants.ESQUIO_THROWFEATURE_ACTIVITY_NAME, payload);
            }
        }

        public void EndFeatureEvaluation(Guid correlationId, string featureName, string productName, string deploymentName, long elapsedMilliseconds, bool enabled)
        {
            Log.FeatureServiceProcessingEnd(_logger, featureName, productName, deploymentName, enabled, elapsedMilliseconds);
            
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.FeatureEvaluated(featureName, productName, deploymentName, elapsedMilliseconds);
                EsquioEventSource.Log.FeatureEvaluationStop();
            }

            var payload = new FeatureEvaluatedEventData(correlationId, featureName, productName, deploymentName, enabled, elapsedMilliseconds);

            if (_listener.IsEnabled(EsquioConstants.ESQUIO_ENDFEATURE_ACTIVITY_NAME, payload))
            {
                _listener.Write(EsquioConstants.ESQUIO_ENDFEATURE_ACTIVITY_NAME, payload);
            }
        }

        public void BeginTogglevaluation(Guid correlationId, string featureName, string productName, string deploymentName, string toggleType)
        {
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleEvaluationStart();
            }

            var payload = new ToggleEvaluatingEventData(correlationId, featureName, productName, deploymentName, toggleType);

            if (_listener.IsEnabled(EsquioConstants.ESQUIO_BEGINTOGGLE_ACTIVITY_NAME, payload))
            {
                _listener.Write(EsquioConstants.ESQUIO_BEGINTOGGLE_ACTIVITY_NAME, payload);
            }
        }

        public void Togglevaluation(string featureName, string productName, string deploymentName, string toggle, long elapsedMilliseconds)
        {
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleEvaluated(featureName, productName, deploymentName, toggle, elapsedMilliseconds);
            }
        }

        public void ToggleNotActive(string featureName, string productName, string deploymentName, string toggle)
        {
            Log.FeatureServiceToggleIsNotActive(_logger, featureName, productName, deploymentName, toggle);
        }

        public void EndTogglevaluation(Guid correlationId, string featureName, string productName, string deploymentName, string toggleType, bool active)
        {
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleEvaluationStop();
            }

            var payload = new ToggleEvaluatedEventData(correlationId, featureName, productName, deploymentName, toggleType, active);

            if (_listener.IsEnabled(EsquioConstants.ESQUIO_ENDTOGGLE_ACTIVITY_NAME, payload))
            {
                _listener.Write(EsquioConstants.ESQUIO_ENDTOGGLE_ACTIVITY_NAME, payload);
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
                EsquioEventSource.Log.ToggleActivationStop(toggleTypeName);
            }
        }

        public void ToggleActivationResolveType(string toggleTypeName)
        {
            Log.DefaultToggleTypeActivatorTypeIsResolved(_logger, toggleTypeName);
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleActivationStop(toggleTypeName);
            }
        }

        public void ToggleActivationCantResolveType(string toggleTypeName)
        {
            Log.DefaultToggleTypeActivatorTypeCantResolved(_logger, toggleTypeName);
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleActivationCantCreateInstance(toggleTypeName);
                EsquioEventSource.Log.ToggleActivationStop(toggleTypeName);
            }
        }

        public void EndToggleActivation(string toggleTypeName)
        {
            if (EsquioEventSource.Log.IsEnabled())
            {
                EsquioEventSource.Log.ToggleActivationStop(toggleTypeName);
            }
        }
    }
}
