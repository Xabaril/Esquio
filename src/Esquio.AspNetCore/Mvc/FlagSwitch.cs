using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Esquio.AspNetCore.Mvc
{
    /// <summary>
    /// Use this attribute to filter if a MVC action can be executed depending on a configured
    /// feature activation state. If the configured feature is not active this action is not executed.
    /// Typically this attribute is used to switch between two MVC actions with the same name.
    /// </summary>
    public class FlagSwitch
        : Attribute, IActionConstraintFactory
    {
        public string FeatureName { get; set; }

        public string ApplicationName { get; set; }

        public bool IsReusable => false;

        public IActionConstraint CreateInstance(IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            return new FlagSwitchConstraint(scopeFactory, FeatureName, ApplicationName);
        }
    }
    internal class FlagSwitchConstraint
        : IActionConstraint
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _applicationName;
        private readonly string _featureName;

        public int Order => 0;

        public FlagSwitchConstraint(IServiceScopeFactory serviceScopeFactory, string featureName, string applicationName)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _featureName = featureName;
            _applicationName = applicationName;
        }
        public bool Accept(ActionConstraintContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<FlagSwitch>>();
                var featureservice = scope.ServiceProvider.GetRequiredService<IFeatureService>();

                Log.FeatureFlagConstraintBegin(logger, _featureName, _applicationName);

                var executingTask = featureservice.IsEnabledAsync(_featureName, _applicationName);
                executingTask.Wait();

                if (executingTask.IsCompletedSuccessfully)
                {
                    Log.FeatureFlagConstraintSuccess(logger, _featureName, _applicationName);
                    return executingTask.Result;
                }
                else
                {
                    Log.FeatureFlagConstraintThrow(logger, _featureName, _applicationName, executingTask.Exception);
                    return false;
                }
            }
        }
    }
}
