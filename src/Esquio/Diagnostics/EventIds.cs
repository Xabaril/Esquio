using Microsoft.Extensions.Logging;

namespace Esquio.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId DefaultFeatureServiceBegin = new EventId(100, nameof(DefaultFeatureServiceBegin));
        public static readonly EventId FeatureNotFound = new EventId(101, nameof(FeatureNotFound));
        public static readonly EventId FeatureNotActive = new EventId(102, nameof(FeatureNotActive));
        public static readonly EventId DefaultFeatureServiceThrows = new EventId(103, nameof(DefaultFeatureServiceThrows));
        public static readonly EventId DefaultToggleTypeActivatorResolveTypeBegin = new EventId(110, nameof(DefaultToggleTypeActivatorResolveTypeBegin));
    }
}
