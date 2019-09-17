using Microsoft.Extensions.Logging;

namespace Esquio.AspNetCore.ApplicationInsightProcessor.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId HttpContextItemObserverCantAdd = new EventId(701, nameof(HttpContextItemObserverCantAdd));
    }
}
