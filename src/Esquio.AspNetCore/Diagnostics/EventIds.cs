using Microsoft.Extensions.Logging;

namespace Esquio.AspNetCore.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId FeatureSwitchBeginProcess = new EventId(200, nameof(FeatureSwitchBeginProcess));
        public static readonly EventId FeatureSwitchSuccess = new EventId(201, nameof(FeatureSwitchSuccess));
        public static readonly EventId FeatureSwitchThrow = new EventId(202, nameof(FeatureSwitchThrow));
        public static readonly EventId FeatureFilterBeginProcess = new EventId(203, nameof(FeatureFilterBeginProcess));
        public static readonly EventId FeatureFilterExecutingAction = new EventId(204, nameof(FeatureFilterExecutingAction));
        public static readonly EventId FeatureFilterNonExcuteAction = new EventId(205, nameof(FeatureFilterNonExcuteAction));
        public static readonly EventId FallbackServiceIsNotConfigured = new EventId(206, nameof(FallbackServiceIsNotConfigured));
        public static readonly EventId FeatureTagHelperBeginProcess = new EventId(207, nameof(FeatureTagHelperBeginProcess));
        public static readonly EventId FeatureTagHelperClearContent = new EventId(208, nameof(FeatureTagHelperClearContent));

        public static readonly EventId EsquioMiddlewareThrow = new EventId(220, nameof(EsquioMiddlewareThrow));
        public static readonly EventId EsquioMiddlewareEvaluateFeature = new EventId(221, nameof(EsquioMiddlewareEvaluateFeature));
        public static readonly EventId EsquioMiddlewareSuccess = new EventId(222, nameof(EsquioMiddlewareSuccess));
    }
}
