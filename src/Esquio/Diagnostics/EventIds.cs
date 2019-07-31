using Microsoft.Extensions.Logging;

namespace Esquio.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId DefaultFeatureServiceBegin = new EventId(100, nameof(DefaultFeatureServiceBegin));
        public static readonly EventId FeatureNotFound = new EventId(101, nameof(FeatureNotFound));
        public static readonly EventId FeatureDisabled = new EventId(102, nameof(FeatureDisabled));
        public static readonly EventId ToggleNotActive = new EventId(103, nameof(ToggleNotActive));
        public static readonly EventId DefaultFeatureServiceThrows = new EventId(104, nameof(DefaultFeatureServiceThrows));

        public static readonly EventId DefaultToggleTypeActivatorResolveTypeBegin = new EventId(120, nameof(DefaultToggleTypeActivatorResolveTypeBegin));
        public static readonly EventId DefaultToggleTypeActivatorCantResolve = new EventId(121, nameof(DefaultToggleTypeActivatorCantResolve));
        public static readonly EventId DefaultToggleTypeActivatorTypeIsResolved = new EventId(122, nameof(DefaultToggleTypeActivatorTypeIsResolved));
    }
}
