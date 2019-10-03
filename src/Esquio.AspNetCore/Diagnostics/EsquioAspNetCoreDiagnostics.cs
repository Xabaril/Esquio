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

        public void FeatureMatcherPolicyEvaluatingFeatures(string endpointDisplayName, string names, string productName)
        {
            Log.FeatureMatcherPolicyEvaluatingFeatures(_logger, endpointDisplayName, names, productName);
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

        public void FeatureTagHelperBegin(string featureName, string productName)
        {
            Log.FeatureTagHelperBegin(_logger, featureName, productName);
        }

        public void FeatureTagHelperClearContent(string featureName, string productName)
        {
            Log.FeatureTagHelperClearContent(_logger, featureName, productName);
        }

        public void EsquioMiddlewareThrow(string featureName, string productName, Exception exception)
        {
            Log.EsquioMiddlewareThrow(_logger, featureName, productName, exception);
        }

        public void EsquioMiddlewareEvaluatingFeature(string featureName, string productName)
        {
            Log.EsquioMiddlewareEvaluatingFeature(_logger, featureName, productName);
        }

        public void EsquioMiddlewareSuccess()
        {
            Log.EsquioMiddlewareSuccess(_logger);
        }
    }
}
