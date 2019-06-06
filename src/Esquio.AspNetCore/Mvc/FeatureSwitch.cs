using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;

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
        /// <summary>
        /// A coma separated list of features names to be evaluated.
        /// </summary>
        /// <remarks>
        /// The feature name are compared case insensitively with the name on the store.
        /// </remarks>
        public string Names { get; set; }

        /// <summary>
        /// The product name when the features are configured. If null a default product is used.
        /// </summary>
        public string Product { get; set; }

        /// <inheritdoc />
        public bool IsReusable => false;

        /// <inheritdoc />
        public IActionConstraint CreateInstance(IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            return new FeatureSwitchConstraint(scopeFactory, Names, Product);
        }
    }
    internal class FeatureSwitchConstraint
        : IActionConstraint
    {
        private static readonly char[] FeatureSeparator = new[] { ',' };

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _productName;
        private readonly string _featureNames;

        public int Order => -1000;

        public FeatureSwitchConstraint(IServiceScopeFactory serviceScopeFactory, string featureNames, string productName)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _featureNames = featureNames;
            _productName = productName;
        }
        public bool Accept(ActionConstraintContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<FeatureSwitch>>();
                var featureService = scope.ServiceProvider.GetRequiredService<IFeatureService>();

                Log.FeatureSwitchBegin(logger, _featureNames, _productName);

                var tokenizer = new StringTokenizer(_featureNames, FeatureSeparator);

                foreach (var item in tokenizer)
                {
                    var featureName = item.Trim();

                    if (featureName.HasValue && featureName.Length > 0)
                    {
                        var cancellationToken = context.RouteContext.HttpContext?.RequestAborted ?? CancellationToken.None;

                        if (!featureService.IsEnabledAsync(featureName.Value, _productName, cancellationToken).Result)
                        {
                            Log.FeatureSwitchFail(logger, _featureNames, _productName);
                            return false;
                        }
                    }
                }

                Log.FeatureSwitchSuccess(logger, _featureNames, _productName);
                return true;
            }
        }
    }
}
