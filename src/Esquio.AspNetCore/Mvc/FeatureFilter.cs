using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Mvc
{
    /// <summary>
    /// Use this attribute to filter if an MVC action can be executed depending on features active state.
    /// If the configured feature is active this action can be executed, if not
    /// by default a NotFound result is the returned action result. You can modify the default action using
    /// the extension method AddMvcFallbackAction in <see cref="IEsquioBuilder"/> interface when register Esquio services.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class FeatureFilter
        : Attribute, IFilterFactory
    {
        /// <inheritdoc />
        public bool IsReusable => false;

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
        public string ProductName { get; set; }

        /// <inheritdoc />
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new FeatureResourceFilter(Names, ProductName);
        }
    }
    internal class FeatureResourceFilter
        : IAsyncResourceFilter
    {
        private static readonly char[] char_separator = new[] { ',' };

        private readonly string _names;
        private readonly string _productName;

        public FeatureResourceFilter(string names, string productName)
        {
            _names = names ?? throw new ArgumentNullException(nameof(names));
            _productName = productName ?? string.Empty;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<FeatureFilter>>();

            var featureService = context.HttpContext.RequestServices
                .GetRequiredService<IFeatureService>();

            var fallbackService = context.HttpContext.RequestServices
                .GetService<IMvcFallbackService>();

            Log.FeatureFilterBegin(logger, _names, _productName);

            var tokenizer = new StringTokenizer(_names, char_separator);

            foreach (var item in tokenizer)
            {
                var featureName = item.Trim();

                if (featureName.HasValue && featureName.Length > 0)
                {
                    var cancellationToken = context.HttpContext?.RequestAborted ?? CancellationToken.None;

                    if (!await featureService.IsEnabledAsync(featureName.Value, _productName, cancellationToken))
                    {
                        Log.FeatureFilterNonExecuteAction(logger, item.Value, _productName);
                        context.Result = fallbackService.GetFallbackActionResult(context);

                        return;
                    }
                }
            }

            Log.FeatureFilterExecutingAction(logger, _names, _productName);
            await next();
        }
    }
}
