using Microsoft.Extensions.Logging;
using System;

namespace Esquio.AspNetCore.ApplicationInsightProcessor.Diagnostics
{

    public class EsquioAspNetCoreApplicationInsightDiagnostics
    {
        private readonly ILogger _logger;

        public EsquioAspNetCoreApplicationInsightDiagnostics(ILoggerFactory loggerFactory)
        {
            _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger("Esquio.AspNetCore.ApplicationInsightProcessor");
        }

        public void FeatureMatcherPolicyCanBeAppliedToEndpoint(string httpContextItemEntry)
        {
            Log.HttpContextItemObserverCantAddItem(_logger, httpContextItemEntry);
        }
    }
}
