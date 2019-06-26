using Microsoft.Extensions.Logging;

namespace Esquio.UI.Api.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId ApiKeyAuthenticationBegin = new EventId(500, nameof(ApiKeyAuthenticationBegin));
        public static readonly EventId ApiKeyAuthenticationSuccess = new EventId(502, nameof(ApiKeyAuthenticationSuccess));
        public static readonly EventId ApiKeyAuthenticationFail = new EventId(503, nameof(ApiKeyAuthenticationFail));
        public static readonly EventId ApiKeyAuthenticationNotFound = new EventId(504, nameof(ApiKeyAuthenticationNotFound));
        public static readonly EventId ApiKeyAuthenticationDoesNotExist = new EventId(505, nameof(ApiKeyAuthenticationDoesNotExist));
        public static readonly EventId ApiKeyStoreValidating = new EventId(506, nameof(ApiKeyStoreValidating));
        public static readonly EventId ApiKeyStoreKeyExist = new EventId(506, nameof(ApiKeyStoreKeyExist));
    }
}
