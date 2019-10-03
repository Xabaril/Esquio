using Microsoft.Extensions.Logging;
using System;

namespace Esquio.EntityFrameworkCore.Store.Diagnostics
{
    internal class EsquioEntityFrameworkCoreStoreDiagnostics
    {
        private readonly ILogger _logger;

        public EsquioEntityFrameworkCoreStoreDiagnostics(ILoggerFactory loggerFactory)
        {
            _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger("Esquio.EntityFrameworkCore.Store");
        }
        public void FindFeature(string featureName, string productName)
        {
            Log.FindFeature(_logger, featureName, productName);
        }
        public void FeatureNotExist(string featureName, string productName)
        {
            Log.FeatureNotExist(_logger, featureName, productName);
        }
        public void FeatureExist(string featureName, string productName)
        {
            Log.FeatureExist(_logger, featureName, productName);
        }
    }
}
