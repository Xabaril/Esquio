using Microsoft.Extensions.Logging;
using System;

namespace Esquio.AspNetCore.Diagnostics
{
    public class EsquioAspNetCoreDiagnostics
    {
        private readonly ILogger _logger;

        public EsquioAspNetCoreDiagnostics(ILoggerFactory loggerFactory)
        {
            _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger("Esquio.AspNetCore");
        }

        public void FeatureMatcherPolicyCanBeAppliedToEndpoint(string endpointDisplayName)
        {
            Log.FeatureMatcherPolicyCanBeAppliedToEndpoint(_logger, endpointDisplayName);
        }

        public void FeatureMatcherPolicyEvaluatingFeatures(string endpointDisplayName, string names)
        {
            Log.FeatureMatcherPolicyEvaluatingFeatures(_logger, endpointDisplayName, names);
        }
        public void FeatureMatcherPolicyEndpointIsNotValid(string endpointDisplayName)
        {
            Log.FeatureMatcherPolicyEndpointIsNotValid(_logger, endpointDisplayName);
        }

        public void FeatureMatcherPolicyEndpointIsValid(string endpointDisplayName)
        {
            Log.FeatureMatcherPolicyEndpointIsValid(_logger, endpointDisplayName);
        }

        public void FeatureMatcherPolicyExecutingFallbackEndpoint(string requestPath)
        {
            Log.FeatureMatcherPolicyExecutingFallbackEndpoint(_logger, requestPath);
        }

        public void FeatureTagHelperBegin(string featureName)
        {
            Log.FeatureTagHelperBegin(_logger, featureName);
        }

        public void FeatureTagHelperClearContent(string featureName)
        {
            Log.FeatureTagHelperClearContent(_logger, featureName);
        }

        public void EsquioMiddlewareThrow(string featureName, Exception exception)
        {
            Log.EsquioMiddlewareThrow(_logger, featureName, exception);
        }

        public void EsquioMiddlewareEvaluatingFeature(string featureName)
        {
            Log.EsquioMiddlewareEvaluatingFeature(_logger, featureName);
        }

        public void EsquioMiddlewareSuccess()
        {
            Log.EsquioMiddlewareSuccess(_logger);
        }
    }
}
