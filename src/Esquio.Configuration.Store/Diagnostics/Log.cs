using Microsoft.Extensions.Logging;
using System;

namespace Esquio.Configuration.Store.Diagnostics
{
    static class Log
    {
        public static void StoreIsReadOnly(ILogger logger)
        {
            _storeIsReadOnly(logger, nameof(ConfigurationFeatureStore), null);
        }

        public static void FeatureNotExist(ILogger logger, string featureName, string productName)
        {
            _featureNotExist(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }

        public static void ParameterNotExist(ILogger logger, string featureName, string productName, string parameterName)
        {
            _parameterNotExist(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, parameterName, null);
        }

        public static void FindFeature(ILogger logger, string featureName, string productName)
        {
            _findFeature(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }

        public static void FindAllPreviewFeatures(ILogger logger, string productName)
        {
            _findAllPreviewFeatures(logger, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }


        private static readonly Action<ILogger, string, Exception> _storeIsReadOnly = LoggerMessage.Define<string>(
            LogLevel.Warning,
            EventIds.StoreIsReadOnly,
            "Store {configurationStore} is read only, this action can't be performed.");

        private static readonly Action<ILogger, string, string, Exception> _featureNotExist = LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "The feature with name {featureName} is not configured for application {product}.");

        private static readonly Action<ILogger, string, string, string, Exception> _parameterNotExist = LoggerMessage.Define<string, string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "The feature with name {featureName} for product {product} not contains a parameter with name {parameterName} for specified Toggle.");

        private static readonly Action<ILogger, string, string, Exception> _findFeature = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FindFeature,
            "The store is trying to find feature {featureName} for product {product} on the store.");

        private static readonly Action<ILogger, string, Exception> _findAllPreviewFeatures = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.FindAllPreviewFeatures,
            "The store is trying to find all configured preview features for product {product} on the store.");
    }
}
