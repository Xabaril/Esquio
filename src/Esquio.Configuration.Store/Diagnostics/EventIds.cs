using Microsoft.Extensions.Logging;

namespace Esquio.Configuration.Store.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId StoreIsReadOnly = new EventId(200, nameof(StoreIsReadOnly));
        public static readonly EventId FeatureNotExist = new EventId(201, nameof(FeatureNotExist));
        public static readonly EventId FindFeature = new EventId(220, nameof(FindFeature));
    }
}
