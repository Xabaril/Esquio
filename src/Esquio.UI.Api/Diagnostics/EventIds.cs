using Microsoft.Extensions.Logging;

namespace Esquio.UI.Api.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId ApiKeyAlreadyExist = new EventId(100, nameof(ApiKeyAlreadyExist));
        public static readonly EventId ApiKeyNotExist = new EventId(101, nameof(ApiKeyNotExist));
        public static readonly EventId ProductNotExist = new EventId(102, nameof(ApiKeyNotExist));
        public static readonly EventId ProductAlreadyExist = new EventId(103, nameof(ProductAlreadyExist));
        public static readonly EventId FeatureNotExist = new EventId(104, nameof(FeatureNotExist));
        public static readonly EventId FeatureAlreadyExist = new EventId(105, nameof(FeatureAlreadyExist));
        public static readonly EventId ToggleAlreadyExist = new EventId(106, nameof(ToggleAlreadyExist));
        public static readonly EventId ToggleNotExist = new EventId(107, nameof(ToggleNotExist));
        public static readonly EventId FeatureTagAlreadyExist = new EventId(108, nameof(FeatureTagAlreadyExist));
        public static readonly EventId FeatureTagNotExist = new EventId(109, nameof(FeatureTagNotExist));

        public static readonly EventId ExecutingCommand = new EventId(201, nameof(ExecutingCommand));
        public static readonly EventId ExecutedCommand = new EventId(202, nameof(ExecutedCommand));

        public static readonly EventId AuthorizationFailed = new EventId(301, nameof(AuthorizationFailed));
        public static readonly EventId AuthorizationPermissionFailed = new EventId(302, nameof(AuthorizationPermissionFailed));
        public static readonly EventId AuthorizationFailedClaimIsNotPresent = new EventId(303, nameof(AuthorizationFailedClaimIsNotPresent));
    }
}
