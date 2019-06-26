using Microsoft.Extensions.Logging;

namespace Esquio.AspNetCore.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId FeatureEndpointMatcherUsingFallbackEndPoint = new EventId(200, nameof(FeatureEndpointMatcherUsingFallbackEndPoint));
        public static readonly EventId FeatureEndpointMatcherEndPointValid = new EventId(201, nameof(FeatureEndpointMatcherEndPointValid));
        public static readonly EventId FeatureEndpointMatcherEndPointNotValid = new EventId(202, nameof(FeatureEndpointMatcherEndPointNotValid));
        public static readonly EventId FeatureEndpointMatcherValidatingFeatures = new EventId(203, nameof(FeatureEndpointMatcherValidatingFeatures));
        public static readonly EventId FeatureEndpointMatcherCanBeAppliedToEndpoint = new EventId(204, nameof(FeatureEndpointMatcherCanBeAppliedToEndpoint));
        public static readonly EventId FeatureTagHelperBeginProcess = new EventId(205, nameof(FeatureTagHelperBeginProcess));
        public static readonly EventId FeatureTagHelperClearContent = new EventId(206, nameof(FeatureTagHelperClearContent));

        public static readonly EventId EsquioMiddlewareThrow = new EventId(220, nameof(EsquioMiddlewareThrow));
        public static readonly EventId EsquioMiddlewareEvaluateFeature = new EventId(221, nameof(EsquioMiddlewareEvaluateFeature));
        public static readonly EventId EsquioMiddlewareSuccess = new EventId(222, nameof(EsquioMiddlewareSuccess));
    }
}
