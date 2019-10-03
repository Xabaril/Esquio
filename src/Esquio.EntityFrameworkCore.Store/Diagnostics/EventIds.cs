using Microsoft.Extensions.Logging;

namespace Esquio.EntityFrameworkCore.Store.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId FeatureNotExist = new EventId(400, nameof(FeatureNotExist));
        public static readonly EventId FindFeature = new EventId(401, nameof(FindFeature));
        public static readonly EventId FeatureExist = new EventId(403, nameof(FeatureExist));
    }
}
