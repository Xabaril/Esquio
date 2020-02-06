using Microsoft.Extensions.Logging;

namespace Esquio.Database.Store.Diagnostics
{
    internal static class EventIds
    {
        public static EventId FindFeature = new EventId(900, nameof(FindFeature));
        public static EventId FeatureNotExist = new EventId(901, nameof(FeatureNotExist));

        public static EventId GetThrow = new EventId(910, nameof(GetThrow));
    }
}
