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
        public static void FeatureSwitchFail(ILogger logger, string featureName, string productName)
        {
            _featureSwitchFail(logger, featureName, productName ?? "(default product)", null);
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
        public static void EsquioMiddlewareThrow(ILogger logger, string featureName, string productName, Exception exception)
        {
            _esquioMiddlewareThrow(logger, featureName, productName ?? "(default product)", exception);
        }
        public static void EsquioMiddlewareEvaluatingFeature(ILogger logger, string featureName, string productName)
        {
            _esquioMiddlewareEvaluateFeature(logger, featureName, productName ?? "(default product)", null);
        }
        public static void EsquioMiddlewareSuccess(ILogger logger)
        {
            _esquioMiddlewareSuccess(logger, null);
        }
        private static readonly Action<ILogger, string, string, Exception> _featureSwitchBegin = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureSwitchBeginProcess,
            "FeatureSwitch constraint begin check if {featureName} for product {productName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _featureSwitchSuccess = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FeatureSwitchSuccess,
            "FeatureSwitch constraint successfully check if  {featureName} for product {productName} is active.");
        private static readonly Action<ILogger, string, string, Exception> _featureSwitchFail = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            EventIds.FeatureSwitchThrow,
            "FeatureSwitch is not active for {featureName} and  product {productName}.");
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
        private static readonly Action<ILogger, string, string, Exception> _esquioMiddlewareThrow = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            EventIds.EsquioMiddlewareThrow,
            "Esquio middleware throw exception when evaluating {featureName} for product {productName}.");
        private static readonly Action<ILogger, string, string, Exception> _esquioMiddlewareEvaluateFeature = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.EsquioMiddlewareEvaluateFeature,
            "Evaluating {featureName} for product {productName} on Esquio middleware.");
        private static readonly Action<ILogger, Exception> _esquioMiddlewareSuccess = LoggerMessage.Define(
            LogLevel.Debug,
            EventIds.EsquioMiddlewareSuccess,
            "Esquio middleware perform feature evaluation succesfully.");

    }
}

