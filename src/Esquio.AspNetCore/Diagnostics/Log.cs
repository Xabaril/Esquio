using Microsoft.Extensions.Logging;
using System;

namespace Esquio.AspNetCore.Diagnostics
{
    static class Log
    {
        public static void FlagSwitchBegin(ILogger logger, string featureName, string applicationName)
        {
            _flagSwitchBegin(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FlagSwitchSuccess(ILogger logger, string featureName, string applicationName)
        {
            _flagSwitchSuccess(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FlagSwitchThrow(ILogger logger, string featureName, string applicationName, Exception exception)
        {
            _flagSwitchThrow(logger, featureName, applicationName ?? "(default application)", exception);
        }
        public static void FlagBegin(ILogger logger, string featureName, string applicationName)
        {
            _flagBegin(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FlagExecutingAction(ILogger logger, string featureName, string applicationName)
        {
            _flagActionExecuted(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FlagNonExecuteAction(ILogger logger, string featureName, string applicationName)
        {
            _flagNonExecutedAction(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FlagFallbackServiceIsNotConfigured(ILogger logger)
        {
            _flagFallbackServiceIsNotConfigured(logger, null);
        }
        public static void FlagTagHelperBegin(ILogger logger, string featureName, string applicationName)
        {
            _flagTagHelperBegin(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FlagTagHelperClearContent(ILogger logger, string featureName, string applicationName)
        {
            _flagTagHelperClearContent(logger, featureName, applicationName ?? "(default application)", null);
        }
        private static readonly Action<ILogger, string, string, Exception> _flagSwitchBegin = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FlagSwitchBeginProcess,
            "FeatureFlag constraint begin check if {featureName} for application {applicationName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _flagSwitchSuccess = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FlagSwitchSuccess,
            "FeatureFlag constraint successfully check if  {featureName} for application {applicationName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _flagSwitchThrow = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            EventIds.FlagSwitchThrow,
            "Feature service throw an error trying to check the feature {featureName} for application {applicationName}.");
        private static readonly Action<ILogger, string, string, Exception> _flagBegin = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.FlagBeginProcess,
           "FlagFilter begin check if {featureName} for application {applicationName} is enabled.");
        private static readonly Action<ILogger, string, string, Exception> _flagActionExecuted = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.FlagExecutingAction,
           "FlagFilter check feature {featureName} for application {applicationName} is active and execute action.");
        private static readonly Action<ILogger, string, string, Exception> _flagNonExecutedAction = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.FlagNonExcuteAction,
           "FlagFilter check feature {featureName} for application {applicationName} is not active, action is not executed.");
        private static readonly Action<ILogger, Exception> _flagFallbackServiceIsNotConfigured = LoggerMessage.Define(
           LogLevel.Warning,
           EventIds.FallbackServiceIsNotConfigured,
           "Esquio FallbackService is not configured, setting default action result as NotFoundResult.");
        private static readonly Action<ILogger, string, string, Exception> _flagTagHelperBegin = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FlagTagHelperBeginProcess,
            "Flag TagHelper begin check if {featureName} for application {applicationName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _flagTagHelperClearContent = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FlagTagHelperClearContent,
            "Flag TagHelper is clearing inner content because {featureName} for application {applicationName} is not active.");
    }
}
