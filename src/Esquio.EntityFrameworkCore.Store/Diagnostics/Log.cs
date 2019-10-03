using Microsoft.Extensions.Logging;
using System;

namespace Esquio.EntityFrameworkCore.Store.Diagnostics
{
    static class Log
    {
        public static void FindFeature(ILogger logger, string featureName, string productName)
        {
            _findFeature(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }
        public static void FeatureNotExist(ILogger logger, string featureName, string productName)
        {
            _featureNotExist(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }
        public static void FeatureExist(ILogger logger, string featureName, string productName)
        {
            _featureExist(logger, featureName, productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME, null);
        }
        
        private static readonly Action<ILogger, string, string, Exception> _featureNotExist = LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "The feature with name {featureName} is not configured for product {productName}.");

        private static readonly Action<ILogger, string, string, Exception> _featureExist = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.FeatureExist,
           "The feature with name {featureName} is configured for product {productName}.");

        private static readonly Action<ILogger, string, string, Exception> _findFeature = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FindFeature,
            "Finding feature with name {featureName} for product {productName}.");

    }
}
