using Microsoft.Extensions.Logging;
using System;

namespace Esquio.EntityFrameworkCore.Store.Diagnostics
{
    static class Log
    {
        public static void FindFeature(ILogger logger, string featureName, string productName)
        {
            _findFeature(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FeatureNotExist(ILogger logger, string featureName, string productName)
        {
            _featureNotExist(logger, featureName, productName ?? "(default product)", null);
        }
        public static void FeatureValueConversionThrow(ILogger logger, string clrType, string value, Exception exception)
        {
            _featureValueConversionThrow(logger, clrType, value ?? "(null value)", exception);

        }
        public static void StartingFeatureValueConversion(ILogger logger, string clrType, string value)
        {
            _startingFeatureValueConversion(logger, clrType, value ?? "(null value)", null);

        }
        private static readonly Action<ILogger, string, string, Exception> _featureNotExist = LoggerMessage.Define<string, string>(
            LogLevel.Warning,
            EventIds.FeatureNotExist,
            "The feature with name {featureName} is not configured for product {productName}.");
        private static readonly Action<ILogger, string, string, Exception> _findFeature = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.FindFeature,
            "Finding feature with name {featureName} for product {productName}.");
        private static readonly Action<ILogger, string, string, Exception> _startingFeatureValueConversion = LoggerMessage.Define<string, string>(
            LogLevel.Debug,
            EventIds.StartingValueConversionFromDatabase,
            "Starting value ( {value} ) conversion of  type {clrType} from database parameter.");
        private static readonly Action<ILogger, string, string, Exception> _featureValueConversionThrow = LoggerMessage.Define<string, string>(
            LogLevel.Error,
            EventIds.ValueConversionFromDatabaseThrow,
            "Value ( {value} ) conversion of  type {clrType} from database  parameter throw on conversion.");

    }
}
