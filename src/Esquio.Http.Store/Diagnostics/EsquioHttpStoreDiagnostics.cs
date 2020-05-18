using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Esquio.Http.Store.Diagnostics
{
    internal class EsquioHttpStoreDiagnostics
    {
        private readonly ILogger _logger;

        public EsquioHttpStoreDiagnostics(ILoggerFactory loggerFactory)
        {
            _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            _logger = loggerFactory.CreateLogger("Esquio.Http.Store");
        }

        public void FindFeature(string featureName, string productName, string deploymentName)
        {
            Log.FindFeature(_logger, featureName, productName, deploymentName);
        }

        public void GetFeatureFromCache(string cacheEntry)
        {
            Log.FindFeatureFromCache(_logger, cacheEntry);
        }

        public void FeatureIsOnCache(string cacheEntry)
        {
            Log.FeatureIsOnCache(_logger, cacheEntry);
        }

        public void FeatureIsNotOnCache(string cacheEntry)
        {
            Log.FeatureIsNotOnCache(_logger, cacheEntry);
        }

        public void GetFeatureFromStore(string featureName, string productName, string deploymentName)
        {
            Log.FindFeatureFromStore(_logger, featureName, productName, deploymentName);
        }


        public void FeatureNotExist(string featureName, string productName, string deploymentName)
        {
            Log.FeatureNotExist(_logger, featureName, productName, deploymentName);
        }

        public void StoreRequestFailed(Uri request, HttpStatusCode statusCode)
        {
            Log.StoreRequestFailed(_logger, request.ToString(), (int)statusCode);
        }

        public void DistributedCacheIsNotConfigured()
        {
            Log.DistributedCacheIsNotConfigured(_logger);
        }
    }
}
