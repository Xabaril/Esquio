using Microsoft.Extensions.Logging;
using System;

namespace Esquio.Configuration.Store.Diagnostics
{
    static class Log
    {
        public static void FeatureNotExist(ILogger logger, string featureName, string productName)
        {
            _featureNotExist(logger, featureName, productName, null);
        }

        public static void FeatureExist(ILogger logger, string featureName, string productName)
        {
            _featureExist(logger, featureName, productName, null);
        }

        public static void BeginFindFeature(ILogger logger, string featureName, string productName)
        {
            _beginFindFeature(logger, featureName, productName, null);
        }

        private static readonly Action<ILogger, string, string, Exception> _featureNotExist = LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "The feature with name {featureName} is not configured for application {product}.");

        private static readonly Action<ILogger, string, string, Exception> _featureExist = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureExist,
            "The feature with name {featureName} is configured for application {product}.");

        private static readonly Action<ILogger, string, string, Exception> _beginFindFeature = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FindFeature,
            "The store is trying to find feature {featureName} for product {product} on the store.");
    }
}
