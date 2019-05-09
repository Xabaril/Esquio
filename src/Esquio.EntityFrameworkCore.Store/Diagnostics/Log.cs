using Microsoft.Extensions.Logging;
using System;

namespace Esquio.EntityFrameworkCore.Store.Diagnostics
{
    static class Log
    {
        public static void ProductNotExist(ILogger logger, string productName)
        {
            _productNotExist(logger, productName ?? "(default product)", null);
        }

        public static void FeatureNotExist(ILogger logger, string featureName, string productName)
        {
            _featureNotExist(logger, featureName, productName ?? "(default product)", null);
        }

        public static void ParameterNotExist(ILogger logger, string featureName, string productName, string parameterName)
        {
            _parameterNotExist(logger, featureName, productName ?? "(default product)", parameterName, null);
        }

        public static void FindFeature(ILogger logger, string featureName, string productName)
        {
            _findFeature(logger, featureName, productName ?? "(default product)", null);
        }
        private static readonly Action<ILogger, string, Exception> _productNotExist = LoggerMessage.Define<string>(
           LogLevel.Warning,
           EventIds.ProductNotExist,
           "The product with name {productName} not exists.");

        private static readonly Action<ILogger, string, string, Exception> _featureNotExist = LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "The feature with name {featureName} is not configured for product {productName}.");

        private static readonly Action<ILogger, string, string, string, Exception> _parameterNotExist = LoggerMessage.Define<string, string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "The feature with name {featureName} for product {productName} not contains a parameter with name {parameterName} for specified Toggle.");

        private static readonly Action<ILogger, string, string, Exception> _findFeature = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FindFeature,
            "The store is trying to find feature {featureName} for product {productName} on the store.");
    }
}
