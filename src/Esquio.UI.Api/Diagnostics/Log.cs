using Microsoft.Extensions.Logging;
using System;

namespace Esquio.UI.Api.Diagnostics
{
    static class Log
    {
        public static void ApiKeyAlreadyExist(ILogger logger, string apiKeyName)
        {
            _apiKeyAlreadyExist(logger, apiKeyName, null);
        }
        public static void ApiKeyNotExist(ILogger logger, string apiKeyId)
        {
            _apiKeyNotExist(logger, apiKeyId, null);
        }

        public static void ExecutingCommand(ILogger logger, string commandName)
        {
            _executingCommand(logger, commandName, null);
        }
        public static void ExecutedCommand(ILogger logger, string commandName)
        {
            _executedCommand(logger, commandName, null);
        }

        private static readonly Action<ILogger, string, Exception> _apiKeyAlreadyExist = LoggerMessage.Define<string>(
            LogLevel.Warning,
            EventIds.ApiKeyAlreadyExist,
            "The ApiKey with name {apiKeyName} already exist in the store and can't be created.");

        private static readonly Action<ILogger, string, Exception> _apiKeyNotExist = LoggerMessage.Define<string>(
            LogLevel.Warning,
            EventIds.ApiKeyNotExist,
            "The ApiKey with identifier {apiKeyId} does not exist in database and can't be deleted.");

        private static readonly Action<ILogger, string, Exception> _executingCommand = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.ExecutingCommand,
            "Executing command with name {commandName}.");

        private static readonly Action<ILogger, string, Exception> _executedCommand = LoggerMessage.Define<string>(
            LogLevel.Debug,
            EventIds.ExecutedCommand,
            "Executed  command with name {commandName}.");
    }
}
