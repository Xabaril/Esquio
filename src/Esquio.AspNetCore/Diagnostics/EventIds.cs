using Microsoft.Extensions.Logging;

namespace Esquio.AspNetCore.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId FeatureSwitchBeginProcess = new EventId(300, nameof(FeatureSwitchBeginProcess));
        public static readonly EventId FeatureSwitchSuccess = new EventId(300, nameof(FeatureSwitchSuccess));
        public static readonly EventId FeatureSwitchThrow = new EventId(301, nameof(FeatureSwitchThrow));
        public static readonly EventId FeatureFilterBeginProcess = new EventId(302, nameof(FeatureFilterBeginProcess));
        public static readonly EventId FeatureFilterExecutingAction = new EventId(302, nameof(FeatureFilterExecutingAction));
        public static readonly EventId FeatureFilterNonExcuteAction = new EventId(303, nameof(FeatureFilterNonExcuteAction));
        public static readonly EventId FallbackServiceIsNotConfigured = new EventId(304, nameof(FallbackServiceIsNotConfigured));
        public static readonly EventId FeatureTagHelperBeginProcess = new EventId(305, nameof(FeatureTagHelperBeginProcess));
        public static readonly EventId FeatureTagHelperClearContent = new EventId(305, nameof(FeatureTagHelperClearContent));
        public static readonly EventId EsquioMiddlewareThrow = new EventId(401, nameof(EsquioMiddlewareThrow));
        public static readonly EventId EsquioMiddlewareEvaluateFeature = new EventId(402, nameof(EsquioMiddlewareEvaluateFeature));
        public static readonly EventId EsquioMiddlewareSuccess = new EventId(403, nameof(EsquioMiddlewareSuccess));
    }
}
