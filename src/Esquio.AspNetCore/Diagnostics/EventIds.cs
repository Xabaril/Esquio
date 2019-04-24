using Microsoft.Extensions.Logging;

namespace Esquio.AspNetCore.Diagnostics
{
    internal static class EventIds
    {
        public static readonly EventId FlagSwitchBeginProcess = new EventId(300, nameof(FlagSwitchBeginProcess));
        public static readonly EventId FlagSwitchSuccess = new EventId(300, nameof(FlagSwitchSuccess));
        public static readonly EventId FlagSwitchThrow = new EventId(301, nameof(FlagSwitchThrow));
        public static readonly EventId FlagBeginProcess = new EventId(302, nameof(FlagBeginProcess));
        public static readonly EventId FlagExecutingAction = new EventId(302, nameof(FlagExecutingAction));
        public static readonly EventId FlagNonExcuteAction = new EventId(303, nameof(FlagNonExcuteAction));
        public static readonly EventId FallbackServiceIsNotConfigured = new EventId(304, nameof(FallbackServiceIsNotConfigured));
        public static readonly EventId FlagTagHelperBeginProcess = new EventId(305, nameof(FlagTagHelperBeginProcess));
        public static readonly EventId FlagTagHelperClearContent = new EventId(305, nameof(FlagTagHelperClearContent));
    }
}
