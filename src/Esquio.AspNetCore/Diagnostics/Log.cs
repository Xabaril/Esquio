using Microsoft.Extensions.Logging;
using System;

namespace Esquio.AspNetCore.Diagnostics
{
    static class Log
    {
        public static void FeatureSwitchBegin(ILogger logger, string featureName, string productName)
        {
            _featureSwitchBegin(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FeatureSwitchSuccess(ILogger logger, string featureName, string productName)
        {
            _featureSwitchSuccess(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FeatureSwitchThrow(ILogger logger, string featureName, string productName, Exception exception)
        {
            _featureSwitchThrow(logger, featureName, productName ?? "(default product)", exception);
        }
        public static void FeatureFilterBegin(ILogger logger, string featureName, string productName)
        {
            _featureFilterBegin(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FeatureFilterExecutingAction(ILogger logger, string featureName, string productName)
        {
            _featureFilterActionExecuted(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FeatureFilterNonExecuteAction(ILogger logger, string featureName, string productName)
        {
            _featureFilterNonExecutedAction(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FlagFallbackServiceIsNotConfigured(ILogger logger)
        {
            _flagFallbackServiceIsNotConfigured(logger, null);
        }
        public static void FeatureTagHelperBegin(ILogger logger, string featureName, string productName)
        {
            _featureTagHelperBegin(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FeatureTagHelperClearContent(ILogger logger, string featureName, string productName)
        {
            _featureTagHelperClearContent(logger, featureName, productName ?? "(default product)", null);
        }
        private static readonly Action<ILogger, string, string, Exception> _featureSwitchBegin = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureSwitchBeginProcess,
            "FeatureSwitch constraint begin check if {featureName} for product {productName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _featureSwitchSuccess = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureSwitchSuccess,
            "FeatureSwitch constraint successfully check if  {featureName} for product {productName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _featureSwitchThrow = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            EventIds.FeatureSwitchThrow,
            "FeatureSwitch throw an error trying to check the feature {featureName} for application {productName}.");
        private static readonly Action<ILogger, string, string, Exception> _featureFilterBegin = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.FeatureFilterBeginProcess,
           "FeatureFilter begin check if {featureName} features for product {productName} is enabled.");
        private static readonly Action<ILogger, string, string, Exception> _featureFilterActionExecuted = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.FeatureFilterExecutingAction,
           "FeatureFilter check that {featureName} features for product {productName} are active and execute action.");
        private static readonly Action<ILogger, string, string, Exception> _featureFilterNonExecutedAction = LoggerMessage.Define<string, string>(
           LogLevel.Debug,
           EventIds.FeatureFilterNonExcuteAction,
           "FeatureFilter check that {featureName} feature for product {productName} is not active, action is not executed.");
        private static readonly Action<ILogger, Exception> _flagFallbackServiceIsNotConfigured = LoggerMessage.Define(
           LogLevel.Warning,
           EventIds.FallbackServiceIsNotConfigured,
           "MVCFallbackService is not configured, setting default action result as NotFoundResult.");
        private static readonly Action<ILogger, string, string, Exception> _featureTagHelperBegin = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureTagHelperBeginProcess,
            "FeatureTagHelper begin check if {featureName} for product {productName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _featureTagHelperClearContent = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureTagHelperClearContent,
            "FeatureTagHelper is clearing inner content because {featureName} for product {productName} is not active.");
    }
}
