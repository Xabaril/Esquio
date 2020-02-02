using Microsoft.Extensions.Logging;

namespace Esquio.Distributed.Store.Diagnostics
{
    internal static class EventIds
    {
        public static EventId FindFeature = new EventId(800, nameof(FindFeature));
        public static EventId FeatureNotExist = new EventId(801, nameof(FeatureNotExist));

        public static EventId GetThrow = new EventId(810, nameof(GetThrow));
    }
}
