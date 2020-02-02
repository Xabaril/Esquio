using Microsoft.Extensions.Logging;
using System;

namespace Esquio.Distributed.Store.Diagnostics
{
    static class Log
    {
        public static void FindFeature(ILogger logger, string featureName, string productName, string ringName)
        {
            _findFeature(logger, featureName, productName, ringName, null);
        }

        public static void FeatureNotExist(ILogger logger, string featureName, string productName, string ringName)
        {
            _findFeature(logger, featureName, productName, ringName, null);
        }

        public static void GetThrow(ILogger logger, string request, int statusCode)
        {
            _getThrow(logger, request, statusCode, null);
        }

        private static readonly Action<ILogger, string, string, string, Exception> _findFeature = LoggerMessage.Define<string, string, string>(
            LogLevel.Information,
            EventIds.FindFeature,
            "Finding feature with name {featureName} for product {productName}({ringName}).");

        private static readonly Action<ILogger, string, string, string, Exception> _featureNotExist = LoggerMessage.Define<string, string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "Feature with name {featureName} for product {productName}({ringName}) does not exist on the store.");

        private static readonly Action<ILogger, string, int, Exception> _getThrow = LoggerMessage.Define<string, int>(
          LogLevel.Error,
          EventIds.GetThrow,
          "Request GET to distributed store {request} throw with status code {statusCode}.");

    }
}
