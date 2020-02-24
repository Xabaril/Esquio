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
        public static readonly EventId RingAlreadyExist = new EventId(110, nameof(RingAlreadyExist));
        public static readonly EventId RingNotExist = new EventId(111, nameof(RingNotExist));
        public static readonly EventId CantDeleteDefaultRing = new EventId(111, nameof(CantDeleteDefaultRing));

        public static readonly EventId ExecutingCommand = new EventId(201, nameof(ExecutingCommand));
        public static readonly EventId ExecutedCommand = new EventId(202, nameof(ExecutedCommand));

        public static readonly EventId AuthorizationFailed = new EventId(301, nameof(AuthorizationFailed));
        public static readonly EventId AuthorizationPermissionFailed = new EventId(302, nameof(AuthorizationPermissionFailed));
        public static readonly EventId AuthorizationFailedClaimIsNotPresent = new EventId(303, nameof(AuthorizationFailedClaimIsNotPresent));

        public static readonly EventId SubjectIdAlreadyExist = new EventId(401, nameof(SubjectIdAlreadyExist));
        public static readonly EventId SubjectIdDoesNotExist = new EventId(402, nameof(SubjectIdDoesNotExist));
        public static readonly EventId MyIsNotAuthorized = new EventId(403, nameof(MyIsNotAuthorized));

        public static readonly EventId ApiKeyAuthenticationBegin = new EventId(500, nameof(ApiKeyAuthenticationBegin));
        public static readonly EventId ApiKeyAuthenticationSuccess = new EventId(502, nameof(ApiKeyAuthenticationSuccess));
        public static readonly EventId ApiKeyAuthenticationFail = new EventId(503, nameof(ApiKeyAuthenticationFail));
        public static readonly EventId ApiKeyAuthenticationNotFound = new EventId(504, nameof(ApiKeyAuthenticationNotFound));
        public static readonly EventId ApiKeyAuthenticationDoesNotExist = new EventId(505, nameof(ApiKeyAuthenticationDoesNotExist));
        public static readonly EventId ApiKeyStoreValidating = new EventId(506, nameof(ApiKeyStoreValidating));
        public static readonly EventId ApiKeyStoreKeyExist = new EventId(506, nameof(ApiKeyStoreKeyExist));
    }
}
