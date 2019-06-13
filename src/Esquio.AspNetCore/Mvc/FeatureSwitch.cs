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
            return new FeatureSwitchConstraint(Names, Product);
        }
    }
    internal class FeatureSwitchConstraint
        : IActionConstraint
    {
        private static readonly char[] char_separator = new[] { ',' };

        private readonly string _productName;
        private readonly string _featureNames;

        public int Order => -1000;

        public FeatureSwitchConstraint(string names, string productName)
        {
            _featureNames = names;
            _productName = productName;
        }
        public bool Accept(ActionConstraintContext context)
        {
            var logger = context.RouteContext
                .HttpContext
                .RequestServices
                .GetRequiredService<ILogger<FeatureSwitch>>();

            var featureService = context.RouteContext
                .HttpContext
                .RequestServices
                .GetRequiredService<IFeatureService>();

            Log.FeatureSwitchBegin(logger, _featureNames, _productName);

            var tokenizer = new StringTokenizer(_featureNames, char_separator);

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
