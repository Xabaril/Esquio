using Microsoft.Extensions.Logging;

namespace Esquio.UI.Api.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId ApiKeyAlreadyExist = new EventId(100, nameof(ApiKeyAlreadyExist));
        public static readonly EventId ApiKeyNotExist = new EventId(101, nameof(ApiKeyNotExist));

        public static readonly EventId ExecutingCommand = new EventId(201, nameof(ExecutingCommand));
        public static readonly EventId ExecutedCommand = new EventId(202, nameof(ExecutedCommand));
    }
}
