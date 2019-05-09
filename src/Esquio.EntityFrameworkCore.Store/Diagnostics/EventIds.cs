using Microsoft.Extensions.Logging;

namespace Esquio.EntityFrameworkCore.Store.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId ProductNotExist = new EventId(201, nameof(ProductNotExist));
        public static readonly EventId FeatureNotExist = new EventId(202, nameof(FeatureNotExist));
        public static readonly EventId FindFeature = new EventId(220, nameof(FindFeature));
    }
}
