using Microsoft.Extensions.Logging;

namespace Esquio.Configuration.Store.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId StoreIsReadOnly = new EventId(300, nameof(StoreIsReadOnly));
        public static readonly EventId FeatureNotExist = new EventId(301, nameof(FeatureNotExist));

        public static readonly EventId FindFeature = new EventId(320, nameof(FindFeature));
        public static readonly EventId FindAllPreviewFeatures = new EventId(321, nameof(FindAllPreviewFeatures));
    }
}
