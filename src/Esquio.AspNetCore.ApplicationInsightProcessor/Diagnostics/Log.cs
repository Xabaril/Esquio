using Microsoft.Extensions.Logging;
using System;

namespace Esquio.AspNetCore.ApplicationInsightProcessor.Diagnostics
{
    static class Log
    {
        public static void HttpContextItemObserverCantAddItem(ILogger logger, string entry)
        {
            _httpContextItemObserverCantAddItem(logger, entry, null);
        }
       
        private static readonly Action<ILogger, string, Exception> _httpContextItemObserverCantAddItem = LoggerMessage.Define<string>(
           LogLevel.Warning,
           EventIds.HttpContextItemObserverCantAdd,
           "HttpContextItemObserver can't add entry {item} into dictionary.");
    }
}
