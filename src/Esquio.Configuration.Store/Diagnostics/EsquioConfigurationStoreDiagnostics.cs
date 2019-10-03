using Microsoft.Extensions.Logging;
using System;

namespace Esquio.Configuration.Store.Diagnostics
{
    internal class EsquioConfigurationStoreDiagnostics
    {
        private readonly ILogger _logger;

        public EsquioConfigurationStoreDiagnostics(ILoggerFactory loggerFactory)
        {
            _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger("Esquio.Configuration.Store");
        }

        public void FeatureNotExist(string featureName, string productName)
        {
            Log.FeatureNotExist(_logger, featureName, productName);
        }

        public void FeatureExist(string featureName, string productName)
        {
            Log.FeatureExist(_logger, featureName, productName);
        }

        public void BeginFindFeature(string featureName, string productName)
        {
            Log.BeginFindFeature(_logger, featureName, productName);
        }
    }
}
