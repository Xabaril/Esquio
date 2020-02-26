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


        public static void FindFeatureFromCache(ILogger logger, string cacheEntry)
        {
            _findFeatureFromCache(logger, cacheEntry, null);
        }

        public static void FindFeatureFromStore(ILogger logger, string featureName, string productName, string ringName)
        {
            _findFeatureFromStore(logger, featureName, productName, ringName, null);
        }

        public static void FeatureNotExist(ILogger logger, string featureName, string productName, string ringName)
        {
            _findFeature(logger, featureName, productName, ringName, null);
        }

        public static void StoreRequestFailed(ILogger logger, string request, int statusCode)
        {
            _storeRequestFailed(logger, request, statusCode, null);
        }

        public static void DistributedCacheIsNotConfigured(ILogger logger)
        {
            _distributedStoreIsNotConfigured(logger, null);
        }

        private static readonly Action<ILogger, string, string, string, Exception> _findFeature = LoggerMessage.Define<string, string, string>(
            LogLevel.Information,
            EventIds.FindFeature,
            "Distributed store trying to find feature with name {featureName} for product {productName}({ringName}).");

        private static readonly Action<ILogger, string, Exception> _findFeatureFromCache = LoggerMessage.Define<string>(
           LogLevel.Information,
           EventIds.FindFeatureFromCache,
           "Finding feature from configured cache using entry {cacheEntry}.");

        private static readonly Action<ILogger, string, string, string, Exception> _findFeatureFromStore = LoggerMessage.Define<string, string, string>(
          LogLevel.Information,
          EventIds.FindFeatureFromStore,
          "Finding feature from store with name {featureName} for product {productName}({ringName}).");

        private static readonly Action<ILogger, string, string, string, Exception> _featureNotExist = LoggerMessage.Define<string, string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "Feature with name {featureName} for product {productName}({ringName}) does not exist on the store.");

        private static readonly Action<ILogger, string, int, Exception> _storeRequestFailed = LoggerMessage.Define<string, int>(
          LogLevel.Error,
          EventIds.StoreRequestFailed,
          "Request GET to distributed store {request} throw with status code {statusCode}.");

        private static readonly Action<ILogger, Exception> _distributedStoreIsNotConfigured = LoggerMessage.Define(
          LogLevel.Error,
          EventIds.DistributedCacheIsNotRegistered,
          "Store is configured to use cache (CacheEnabled is true) but the service IDistributedCache is not registered.");

    }
}
