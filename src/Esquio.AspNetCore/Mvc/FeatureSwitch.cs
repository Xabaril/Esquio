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
    public class FeatureSwitch
        : Attribute, IActionConstraintFactory
    {
        public string FeatureName { get; set; }

        public string ProductName { get; set; }

        public bool IsReusable => false;

        public IActionConstraint CreateInstance(IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            return new FeatureSwitchConstraint(scopeFactory, FeatureName, ProductName);
        }
    }
    internal class FeatureSwitchConstraint
        : IActionConstraint
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _productName;
        private readonly string _featureName;

        public int Order =>-1000;

        public FeatureSwitchConstraint(IServiceScopeFactory serviceScopeFactory, string featureName, string productName)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _featureName = featureName;
            _productName = productName;
        }
        public bool Accept(ActionConstraintContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<FeatureSwitch>>();
                var featureservice = scope.ServiceProvider.GetRequiredService<IFeatureService>();

                Log.FeatureSwitchBegin(logger, _featureName, _productName);

                var executingTask = featureservice.IsEnabledAsync(_featureName, _productName);
                executingTask.Wait();

                if (executingTask.IsCompletedSuccessfully)
                {
                    Log.FeatureSwitchSuccess(logger, _featureName, _productName);
                    return executingTask.Result;
                }
                else
                {
                    Log.FeatureSwitchThrow(logger, _featureName, _productName, executingTask.Exception);
                    return false;
                }
            }
        }
    }
}
