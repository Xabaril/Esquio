using Microsoft.Extensions.Logging;

namespace Esquio.AspNetCore.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId FeatureEndpointMatcherUsingFallbackEndPoint = new EventId(300, nameof(FeatureEndpointMatcherUsingFallbackEndPoint));
        public static readonly EventId FeatureEndpointMatcherEndPointValid = new EventId(301, nameof(FeatureEndpointMatcherEndPointValid));
        public static readonly EventId FeatureEndpointMatcherEndPointNotValid = new EventId(302, nameof(FeatureEndpointMatcherEndPointNotValid));
        public static readonly EventId FeatureEndpointMatcherValidatingFeatures = new EventId(303, nameof(FeatureEndpointMatcherValidatingFeatures));
        public static readonly EventId FeatureEndpointMatcherCanBeAppliedToEndpoint = new EventId(304, nameof(FeatureEndpointMatcherCanBeAppliedToEndpoint));
        public static readonly EventId FeatureTagHelperBeginProcess = new EventId(305, nameof(FeatureTagHelperBeginProcess));
        public static readonly EventId FeatureTagHelperClearContent = new EventId(305, nameof(FeatureTagHelperClearContent));
        public static readonly EventId EsquioMiddlewareThrow = new EventId(401, nameof(EsquioMiddlewareThrow));
        public static readonly EventId EsquioMiddlewareEvaluateFeature = new EventId(402, nameof(EsquioMiddlewareEvaluateFeature));
        public static readonly EventId EsquioMiddlewareSuccess = new EventId(403, nameof(EsquioMiddlewareSuccess));
    }
}
