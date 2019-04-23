using Microsoft.Extensions.Logging;

namespace Esquio.AspNetCore.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId FeatureFlagConstraintBeginProcess = new EventId(300, nameof(FeatureFlagConstraintBeginProcess));
        public static readonly EventId FeatureFlagConstraintSuccess = new EventId(300, nameof(FeatureFlagConstraintSuccess));
        public static readonly EventId FeatureFlagConstraintThrow = new EventId(301, nameof(FeatureFlagConstraintThrow));
    }
}
