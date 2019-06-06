using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
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
        public string Product { get; set; }

        /// <inheritdoc />
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            return new FeatureResourceFilter(scopeFactory, Names, Product);
        }
    }
    internal class FeatureResourceFilter
        : IAsyncResourceFilter
    {
        private static readonly char[] FeatureSeparator = new[] { ',' };

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _featureNames;
        private readonly string _productName;

        public FeatureResourceFilter(IServiceScopeFactory serviceScopeFactory, string featureNames, string productName)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _featureNames = featureNames ?? throw new ArgumentNullException(nameof(featureNames));
            _productName = productName ?? string.Empty;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var logger = scope.ServiceProvider
                    .GetRequiredService<ILogger<FeatureFilter>>();

                var featureService = scope.ServiceProvider
                    .GetRequiredService<IFeatureService>();

                var fallbackService = scope.ServiceProvider
                    .GetService<IMvcFallbackService>();

                Log.FeatureFilterBegin(logger, _featureNames, _productName);

                var tokenizer = new StringTokenizer(_featureNames, FeatureSeparator);

                foreach (var item in tokenizer)
                {
                    var featureName = item.Trim();

                    if (featureName.HasValue && featureName.Length > 0)
                    {
                        if (!await featureService.IsEnabledAsync(featureName.Value, _productName))
                        {
                            Log.FeatureFilterNonExecuteAction(logger, item.Value, _productName);
                            context.Result = fallbackService.GetFallbackActionResult(context);

                            return;
                        }
                    }
                }

                Log.FeatureFilterExecutingAction(logger, _featureNames, _productName);
                await next();
            }
        }
    }
}
