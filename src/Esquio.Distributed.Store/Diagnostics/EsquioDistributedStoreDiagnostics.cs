using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Esquio.Distributed.Store.Diagnostics
{
    internal class EsquioDistributedStoreDiagnostics
    {
        private readonly ILogger _logger;

        public EsquioDistributedStoreDiagnostics(ILoggerFactory loggerFactory)
        {
            _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger("Esquio.Distributed.Store");
        }

        public void FindFeature(string featureName, string productName, string ringName)
        {
            Log.FindFeature(_logger, featureName, productName, ringName);
        }

        public void FeatureNotExist(string featureName, string productName, string ringName)
        {
            Log.FeatureNotExist(_logger, featureName, productName, ringName);
        }

        public void RequestFailed(Uri request, HttpStatusCode statusCode)
        {
            Log.GetThrow(_logger, request.ToString(), (int)statusCode);
        }
    }
}
