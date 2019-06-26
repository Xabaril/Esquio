using Microsoft.Extensions.Logging;
using System;

namespace Esquio.AspNetCore.Diagnostics
{
    static class Log
    {
        public static void FeatureMatcherPolicyCanBeAppliedToEndpoint(ILogger logger, string endpointDisplayName)
        {
            _featureMatcherPolicyEndpointCanBeApplied(logger, endpointDisplayName, null);
        }
        public static void FeatureMatcherPolicyEvaluatingFeatures(ILogger logger, string endpointDisplayName, string names, string productName)
        {
            _featureMathcherPolicyValidatingMetadata(logger, endpointDisplayName, names, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }
        public static void FeatureMatcherPolicyEndpointIsNotValid(ILogger logger, string endpointDisplayName)
        {
            _featureMatcherPolicyEndpointIsNotValid(logger, endpointDisplayName, null);
        }
        public static void FeatureMatcherPolicyEndpointIsValid(ILogger logger, string endpointDisplayName)
        {
            _featureMatcherPolicyEndpointIsValid(logger, endpointDisplayName, null);
        }
        public static void FeatureMatcherPolicyExecutingFallbackEndpoint(ILogger logger, string requestPath)
        {
            _featureMatcherPolicyExecutingFallbackEndpoint(logger, requestPath, null);
        }
        public static void FeatureTagHelperBegin(ILogger logger, string featureName, string productName)
        {
            _featureTagHelperBegin(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }
        public static void FeatureTagHelperClearContent(ILogger logger, string featureName, string productName)
        {
            _featureTagHelperClearContent(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }
        public static void EsquioMiddlewareThrow(ILogger logger, string featureName, string productName, Exception exception)
        {
            _esquioMiddlewareThrow(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, exception);
        }
        public static void EsquioMiddlewareEvaluatingFeature(ILogger logger, string featureName, string productName)
        {
            _esquioMiddlewareEvaluateFeature(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }
        public static void EsquioMiddlewareSuccess(ILogger logger)
        {
            _esquioMiddlewareSuccess(logger, null);
        }
        private static readonly Action<ILogger, string, Exception> _featureMatcherPolicyEndpointCanBeApplied = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.FeatureEndpointMatcherCanBeAppliedToEndpoint,
            "FeatureMatcherPolicy can be applied to endpoint {endpointDisplayName}.");
        private static readonly Action<ILogger, string, string, string, Exception> _featureMathcherPolicyValidatingMetadata = LoggerMessage.Define<string, string, string>(
            LogLevel.Debug,
            EventIds.FeatureEndpointMatcherValidatingFeatures,
            "FeatureMatcherPolicy is validating features for endpoint {endpointDisplayName} for features {names} on product {productName}.");
        private static readonly Action<ILogger, string, Exception> _featureMatcherPolicyEndpointIsNotValid = LoggerMessage.Define<string>(
           LogLevel.Debug,
           EventIds.FeatureEndpointMatcherEndPointNotValid,
           "FeatureMatcherPolicy set validity FALSE to endpoint {endpointDisplayName}.");
        private static readonly Action<ILogger, string, Exception> _featureMatcherPolicyEndpointIsValid = LoggerMessage.Define<string>(
           LogLevel.Debug,
           EventIds.FeatureEndpointMatcherEndPointValid,
           "FeatureMatcherPolicy set validity TRUE to endpoint {endpointDisplayName}.");
        private static readonly Action<ILogger, string, Exception> _featureMatcherPolicyExecutingFallbackEndpoint = LoggerMessage.Define<string>(
          LogLevel.Debug,
          EventIds.FeatureEndpointMatcherUsingFallbackEndPoint,
          "FeatureMatcherPolicy use fallback enpoint configured service because the endpoint for request {requestPath} does not have valid candidates to use.");
        private static readonly Action<ILogger, string, string, Exception> _featureTagHelperBegin = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureTagHelperBeginProcess,
            "FeatureTagHelper begin check if {featureName} for product {productName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _featureTagHelperClearContent = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureTagHelperClearContent,
            "FeatureTagHelper is clearing inner content because {featureName} for product {productName} is not active.");
        private static readonly Action<ILogger, string, string, Exception> _esquioMiddlewareThrow = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            EventIds.EsquioMiddlewareThrow,
            "Esquio middleware throw exception when evaluating {featureName} for product {productName}.");
        private static readonly Action<ILogger, string, string, Exception> _esquioMiddlewareEvaluateFeature = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.EsquioMiddlewareEvaluateFeature,
            "Evaluating {featureName} for product {productName} on Esquio middleware.");
        private static readonly Action<ILogger, Exception> _esquioMiddlewareSuccess = LoggerMessage.Define(
            LogLevel.Debug,
            EventIds.EsquioMiddlewareSuccess,
            "Esquio middleware perform feature evaluation succesfully.");

    }
}

