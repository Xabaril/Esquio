using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Esquio.AspNetCore.Mvc
{
    public class Flag
        : Attribute, IActionConstraintFactory
    {
        public string FeatureName { get; set; }

        public string ApplicationName { get; set; }

        public int Order { get; set; }

        public bool IsReusable => false;

        public IActionConstraint CreateInstance(IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var featureService = scope.ServiceProvider.GetRequiredService<IFeatureService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Flag>>();

                return new FlagConstraint(featureService, ApplicationName, FeatureName, Order, logger);
            }
        }
    }
    internal class FlagConstraint
        : IActionConstraint
    {
        private readonly IFeatureService _featureService;
        private readonly ILogger<Flag> _logger;
        private readonly string _applicationName;
        private readonly string _featureName;

        public int Order { get; }

        public FlagConstraint(IFeatureService featureService, string applicationName, string featureName, int order, ILogger<Flag> logger)
        {
            _featureService = featureService ?? throw new ArgumentNullException(nameof(featureService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationName = applicationName ?? throw new ArgumentNullException(nameof(applicationName));
            _featureName = featureName ?? throw new ArgumentNullException(nameof(featureName));

            Order = order;
        }
        public bool Accept(ActionConstraintContext context)
        {
            Log.FeatureFlagConstraintBegin(_logger, _featureName, _applicationName);

            var executingTask = _featureService.IsEnabledAsync(_applicationName, _featureName);
            executingTask.Wait();

            if (executingTask.IsCompletedSuccessfully)
            {
                Log.FeatureFlagConstraintSuccess(_logger, _featureName, _applicationName);
                return executingTask.Result;
            }
            else
            {
                Log.FeatureFlagConstraintThrow(_logger, _featureName, _applicationName, executingTask.Exception);
                return false;
            }
        }
        private static class Log
        {
            public static void FeatureFlagConstraintBegin(ILogger logger, string featureName, string applicationName)
            {
                _featureFlagConstraintBegin(logger, featureName, applicationName, null);
            }
            public static void FeatureFlagConstraintSuccess(ILogger logger, string featureName, string applicationName)
            {
                _featureFlagConstraintSuccess(logger, featureName, applicationName, null);
            }
            public static void FeatureFlagConstraintThrow(ILogger logger, string featureName, string applicationName, Exception exception)
            {
                _featureFlagConstraintThrow(logger, featureName, applicationName, exception);
            }
            private static readonly Action<ILogger, string, string, Exception> _featureFlagConstraintBegin = LoggerMessage.Define<string, string>(
                LogLevel.Debug,
                EventIds.FeatureFlagConstraintBeginProcess,
                "FeatureFlag constraint begin check if {featureName} for {applicationName} is enabled.");
            private static readonly Action<ILogger, string, string, Exception> _featureFlagConstraintSuccess = LoggerMessage.Define<string, string>(
                LogLevel.Debug,
                EventIds.FeatureFlagConstraintSuccess,
                "FeatureFlag constraint successfully check if  {featureName} for {applicationName} is enabled.");
            private static readonly Action<ILogger, string, string, Exception> _featureFlagConstraintThrow = LoggerMessage.Define<string, string>(
                LogLevel.Error,
                EventIds.FeatureFlagConstraintThrow,
                "Feature service throw an error trying to check the feature {featureName} for application {applicationName}.");
        }
    }
}
