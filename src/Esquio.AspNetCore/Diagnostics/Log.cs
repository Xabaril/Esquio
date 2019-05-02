using Microsoft.Extensions.Logging;
using System;

namespace Esquio.AspNetCore.Diagnostics
{
    static class Log
    {
        public static void FeatureSwitchBegin(ILogger logger, string featureName, string applicationName)
        {
            _featureSwitchBegin(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FeatureSwitchSuccess(ILogger logger, string featureName, string applicationName)
        {
            _featureSwitchSuccess(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FeatureSwitchThrow(ILogger logger, string featureName, string applicationName, Exception exception)
        {
            _featureSwitchThrow(logger, featureName, applicationName ?? "(default application)", exception);
        }
        public static void FeatureFilterBegin(ILogger logger, string featureName, string applicationName)
        {
            _featureFilterBegin(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FeatureFilterExecutingAction(ILogger logger, string featureName, string applicationName)
        {
            _featureFilterActionExecuted(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FeatureFilterNonExecuteAction(ILogger logger, string featureName, string applicationName)
        {
            _featureFilterNonExecutedAction(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FlagFallbackServiceIsNotConfigured(ILogger logger)
        {
            _flagFallbackServiceIsNotConfigured(logger, null);
        }
        public static void FeatureTagHelperBegin(ILogger logger, string featureName, string applicationName)
        {
            _featureTagHelperBegin(logger, featureName, applicationName ?? "(default application)", null);
        }
        public static void FeatureTagHelperClearContent(ILogger logger, string featureName, string applicationName)
        {
            _featureTagHelperClearContent(logger, featureName, applicationName ?? "(default application)", null);
        }
        private static readonly Action<ILogger, string, string, Exception> _featureSwitchBegin = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureSwitchBeginProcess,
            "FeatureSwitch constraint begin check if {featureName} for application {applicationName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _featureSwitchSuccess = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureSwitchSuccess,
            "FeatureSwitch constraint successfully check if  {featureName} for application {applicationName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _featureSwitchThrow = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            EventIds.FeatureSwitchThrow,
            "FeatureSwitch throw an error trying to check the feature {featureName} for application {applicationName}.");
        private static readonly Action<ILogger, string, string, Exception> _featureFilterBegin = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.FeatureFilterBeginProcess,
           "FeatureFilter begin check if {featureName} for application {applicationName} is enabled.");
        private static readonly Action<ILogger, string, string, Exception> _featureFilterActionExecuted = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.FeatureFilterExecutingAction,
           "FeatureFilter check feature {featureName} for application {applicationName} is active and execute action.");
        private static readonly Action<ILogger, string, string, Exception> _featureFilterNonExecutedAction = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.FeatureFilterNonExcuteAction,
           "FeatureFilter check feature {featureName} for application {applicationName} is not active, action is not executed.");
        private static readonly Action<ILogger, Exception> _flagFallbackServiceIsNotConfigured = LoggerMessage.Define(
           LogLevel.Warning,
           EventIds.FallbackServiceIsNotConfigured,
           "MVCFallbackService is not configured, setting default action result as NotFoundResult.");
        private static readonly Action<ILogger, string, string, Exception> _featureTagHelperBegin = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureTagHelperBeginProcess,
            "FeatureTagHelper begin check if {featureName} for application {applicationName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _featureTagHelperClearContent = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureTagHelperClearContent,
            "FeatureTagHelper is clearing inner content because {featureName} for application {applicationName} is not active.");
    }
}
