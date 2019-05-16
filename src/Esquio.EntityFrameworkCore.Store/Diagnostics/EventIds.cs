using Microsoft.Extensions.Logging;

namespace Esquio.EntityFrameworkCore.Store.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId FeatureNotExist = new EventId(201, nameof(FeatureNotExist));
        public static readonly EventId FeatureIsNotPreviewOrNotExist = new EventId(201, nameof(FeatureNotExist));
        public static readonly EventId FindFeature = new EventId(202, nameof(FindFeature));
        public static readonly EventId FindUserPreviewFeatures = new EventId(203, nameof(FindUserPreviewFeatures));
        public static readonly EventId EnablingUserOnPreviewFeature = new EventId(204, nameof(EnablingUserOnPreviewFeature));
    }
}
